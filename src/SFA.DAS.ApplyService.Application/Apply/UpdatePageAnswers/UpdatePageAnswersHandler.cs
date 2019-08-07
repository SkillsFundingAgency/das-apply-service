using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.Validation;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.UpdatePageAnswers
{
    public class UpdatePageAnswersHandler : IRequestHandler<UpdatePageAnswersRequest, UpdatePageAnswersResult>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IValidatorFactory _validatorFactory;

        public UpdatePageAnswersHandler(IApplyRepository applyRepository, IValidatorFactory validatorFactory)
        {
            _applyRepository = applyRepository;
            _validatorFactory = validatorFactory;
        }

        public async Task<UpdatePageAnswersResult> Handle(UpdatePageAnswersRequest request, CancellationToken cancellationToken)
        {
            var section = await _applyRepository.GetSection(request.ApplicationId, request.SequenceId, request.SectionId,
                request.UserId);

            var page = section.QnAData.Pages.Single(p => p.PageId == request.PageId);
            page.DisplayType = section.DisplayType;

            var newAnswers = new List<Answer>();
            var existingAnswers = page.PageOfAnswers.Select(poa => new PageOfAnswers { Answers = poa.Answers }).ToList();

            var qnADataObject = section.QnAData;

            PageOfAnswers pageAnswers = new PageOfAnswers() { Answers = new List<Answer>(), Id = Guid.NewGuid() }; 
            if (!page.AllowMultipleAnswers)
            {
                page.PageOfAnswers = new List<PageOfAnswers>();
                newAnswers.AddRange(request.Answers);
                page.PageOfAnswers.Add(pageAnswers);
            }
            else
            {
                if (page.PageOfAnswers == null)
                {
                    page.PageOfAnswers = new List<PageOfAnswers>();
                }
                
                if (!request.SaveNewAnswers && existingAnswers.Any())
                {
                    // work with the first pre-existing answers, answers have no order so if they are
                    // branching answers with contradictory conditions then the branch will be
                    // non-deterministic when validating existing answers
                    newAnswers.AddRange(existingAnswers.First().Answers);
                }
                else
                {
                    // work with the new answers and retain them in the first / additional page of answers
                    newAnswers.AddRange(request.Answers);
                    page.PageOfAnswers.Add(pageAnswers);
                }                
            }

            var validationPassed = true;
            var validationErrors = new List<KeyValuePair<string, string>>();

            var atLeastOneAnswerChanged = page.Questions.Any(q => q.Input.Type == "FileUpload");

            foreach (var question in page.Questions)
            {
                var answer = newAnswers.FirstOrDefault(a => a.QuestionId == question.QuestionId);
                var existingAnswer = existingAnswers.SelectMany(poa => poa.Answers).FirstOrDefault(a => a.QuestionId == question.QuestionId);

                atLeastOneAnswerChanged = atLeastOneAnswerChanged ? true : existingAnswer?.Value != answer?.Value;
                validationPassed = ProcessAnswer(answer, question, validationPassed, validationErrors, pageAnswers, existingAnswers);
                
                if (question.Input.Options != null)
                {
                    foreach (var option in question.Input.Options)
                    {
                        if (answer?.Value == option.Value.ToString())
                        {
                            if (option.FurtherQuestions != null)
                            {
                                var atLeastOneFutherQuestionAnswerChanged = page.Questions.Any(q => q.Input.Type == "FileUpload");

                                foreach (var furtherQuestion in option.FurtherQuestions)
                                {
                                    var furtherAnswer = newAnswers.FirstOrDefault(a => a.QuestionId == furtherQuestion.QuestionId);
                                    var existingFutherAnswer = existingAnswers.SelectMany(poa => poa.Answers).FirstOrDefault(a => a.QuestionId == furtherQuestion.QuestionId);
                                    validationPassed = ProcessAnswer(furtherAnswer, furtherQuestion, validationPassed, validationErrors, pageAnswers, existingAnswers);
                                    atLeastOneFutherQuestionAnswerChanged = atLeastOneFutherQuestionAnswerChanged ? true : furtherAnswer?.Value != existingFutherAnswer?.Value;
                                }

                                atLeastOneAnswerChanged = atLeastOneAnswerChanged ? true : atLeastOneFutherQuestionAnswerChanged;
                            }
                        }
                    }
                }
            }

            if (validationPassed && !atLeastOneAnswerChanged && section.Status == Domain.Entities.ApplicationSectionStatus.Evaluated)
            {
                foreach (var question in page.Questions)
                {
                    validationErrors.Add(new KeyValuePair<string, string>(question.QuestionId, "Unable to save as you have not updated your answer"));
                }
            }
            else if (validationPassed)
            {
                if (page.HasFeedback)
                {
                    page.Feedback.ForEach(f => f.IsCompleted = true);
                }

                // the page is always complete if validation passes
                page.Complete = true;

                if (page.Next.Count() > 1)
                {
                    foreach (var nextAction in page.Next)
                    {
                        nextAction.ConditionMet = false;
                    }

                    var returnIdConditionMet = string.Empty;

                    // for pages with branching conditions then next page will be activated or deactivated
                    // depending on whether the condition has been met by the answer being validated
                    foreach (var nextAction in page.Next)
                    {
                        // ignore an action with no condition as these do not activate pages
                        if (nextAction.Condition == null) continue;

                        var answerValue = newAnswers.Single(a => a.QuestionId == nextAction.Condition.QuestionId).Value;
                        if (nextAction.Condition.MustEqual == answerValue)
                        {
                            if (nextAction.Action == "NextPage")
                            {
                                var nextActionPage = qnADataObject.Pages.FirstOrDefault(p => p.PageId == nextAction.ReturnId);
                                if (nextActionPage != null)
                                {
                                    nextActionPage.Active = true;
                                }
                            }

                            returnIdConditionMet = nextAction.ReturnId;
                            nextAction.ConditionMet = true;
                        }
                        else
                        {
                            // if a condition has not been met and the next page is not the same page
                            // as a previously met condition (handles OR conditions) then deactivate the page 
                            if (nextAction.Action == "NextPage" && nextAction.ReturnId != returnIdConditionMet)
                            {
                                var nextActionPage = qnADataObject.Pages.FirstOrDefault(p => p.PageId == nextAction.ReturnId);
                                if (nextActionPage != null)
                                {
                                    nextActionPage.Active = false;
                                }
                            }
                        }
                    }

                    // when no conditional page has been met then the first non-conditional page will be met by default
                    if (!page.Next.Any(p => p.ConditionMet))
                    {
                        var nullCondition = page.Next.FirstOrDefault(p => p.Condition == null);
                        if (nullCondition != null)
                        {
                            nullCondition.ConditionMet = true;
                        }
                    }
                }
                else if (page.Next.Count() == 1)
                {
                    // assumption is that all pages have at least one condition
                    page.Next.First().ConditionMet = true;
                }

                qnADataObject.Pages.ForEach(p =>
                {
                    if (p.PageId == request.PageId)
                    {
                        p.Complete = page.Complete;
                        p.PageOfAnswers = page.PageOfAnswers;
                        p.Feedback = page.Feedback;
                    }
                });

                // remove any previous grade as it would be based on previous answers
                qnADataObject.FinancialApplicationGrade = null;

                if (qnADataObject.Pages.Any(p => p.HasNewFeedback))
                {
                    qnADataObject.RequestedFeedbackAnswered =
                        qnADataObject.Pages.All(p => p.AllFeedbackIsCompleted);
                }

                section.QnAData = qnADataObject;

                await _applyRepository.SaveSection(section, request.UserId);

                return new UpdatePageAnswersResult
                    {Page = page, ValidationPassed = true};
            }

            return new UpdatePageAnswersResult
                { Page = page, ValidationPassed = false, ValidationErrors = validationErrors };
            
        }

        private bool ProcessAnswer(Answer answer, Question question, bool validationPassed,
            List<KeyValuePair<string, string>> validationErrors,
            PageOfAnswers pageAnswers, List<PageOfAnswers> pagePageOfAnswers)
        {
            if (question.Input.Type == "FileUpload")
            {
                if (ExistingUpload(question.QuestionId, pagePageOfAnswers) && (answer == null || answer.Value == ""))
                {
                    var existingAnswer = pagePageOfAnswers.SelectMany(poa => poa.Answers).Single(a => a.QuestionId == question.QuestionId);
                    pageAnswers.Answers.Add(existingAnswer);
                }
                else
                {
                    var validators = _validatorFactory.Build(question);
                    foreach (var validator in validators)
                    {
                        var errors = validator.Validate(question, answer);

                        if (!errors.Any()) continue;

                        validationPassed = false;
                        validationErrors.AddRange(errors);
                    }

                    if (answer != null)
                    {
                        pageAnswers.Answers.Add(answer);
                    }
                }
            }
            else
            {
                var validators = _validatorFactory.Build(question);
                foreach (var validator in validators)
                {
                    var errors = validator.Validate(question, answer);

                    if (errors.Any())
                    {
                        validationPassed = false;
                        validationErrors.AddRange(errors);
                    }
                    else
                    {
                        if (question.Input.Type == "Checkbox" && answer.Value == "on")
                        {
                            answer.Value = "Yes";
                        }
                    }
                }

                if (answer != null)
                {
                    pageAnswers.Answers.Add(answer);
                }
            }

            return validationPassed;
        }

        private static bool ExistingUpload(string questionId, List<PageOfAnswers> pagePageOfAnswers)
        {
            return pagePageOfAnswers.Any(poa => poa.Answers.Any(a => a.QuestionId == questionId && a.Value != ""));
        }
    }
}