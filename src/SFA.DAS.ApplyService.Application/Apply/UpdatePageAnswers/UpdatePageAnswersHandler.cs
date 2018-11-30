using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Apply.GetPage;
using SFA.DAS.ApplyService.Application.Apply.Validation;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

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
            
//            var entity = await _applyRepository.GetEntity(request.ApplicationId, request.UserId);
//            var workflow = entity.QnAWorkflow;
//
//            var sequence = workflow.GetSequenceContainingPage(request.PageId);
//            var section = sequence.Sections.Single(s => s.Pages.Any(p => p.PageId == request.PageId));
//
//            if (!sequence.Active)
//            {
//                throw new BadRequestException("Sequence not active");
//            }
//
            var page = section.Pages.Single(p => p.PageId == request.PageId);

            var pages = section.Pages;
            
            PageOfAnswers pageAnswers;
            if (!page.AllowMultipleAnswers)
            {
                page.PageOfAnswers = new List<PageOfAnswers>();
                pageAnswers = new PageOfAnswers(){Answers = new List<Answer>()};
                page.PageOfAnswers.Add(pageAnswers);
            }
            else
            {
                if (page.PageOfAnswers == null)
                {
                    page.PageOfAnswers = new List<PageOfAnswers>();
                }
                pageAnswers = new PageOfAnswers(){Answers = new List<Answer>(), Id = Guid.NewGuid()};
                page.PageOfAnswers.Add(pageAnswers);
            }
            
            var validationPassed = true;
            var validationErrors = new List<KeyValuePair<string, string>>();

            foreach (var question in page.Questions)
            {
                validationPassed = ProcessAnswer(request, question, validationPassed, validationErrors, pageAnswers);
                // IF Question is type ComplexRadio
                // Need to get all answers from Input.Options.FurtherQuestions

                var answer = request.Answers.FirstOrDefault(a => a.QuestionId == question.QuestionId);

                if (question.Input.Options != null)
                {
                    foreach (var option in question.Input.Options)
                    {
                        if (answer?.Value == option.Value.ToString())
                        {
                            if (option.FurtherQuestions != null)
                            {
                                foreach (var furtherQuestion in option.FurtherQuestions)
                                {
                                    var fq = JsonConvert.DeserializeObject<Question>(furtherQuestion.ToString());

                                    validationPassed = ProcessAnswer(request, fq, validationPassed, validationErrors,
                                        pageAnswers);
                                }
                            }
                        }
                    }
                }
            }

            if (validationPassed) 
            {
                if (page.HasFeedback)
                {
                    page.Feedback.ForEach(f => f.IsCompleted = true);
                }
                page.Complete = true;

                //MarkSequenceAsCompleteIfAllPagesComplete(sequence, workflow.Sequences);

                if (page.Next.Count() > 1)
                {
                    // Activate next page if necessary
                    foreach (var nextAction in page.Next)
                    {
                        if (nextAction.Condition.MustEqual == request.Answers
                                .Single(a => a.QuestionId == nextAction.Condition.QuestionId).Value)
                        {
                            pages.Single(p => p.PageId == nextAction.ReturnId).Active = true;
                            pages.Single(p => p.PageId == nextAction.ReturnId).Visible = true;
                            nextAction.ConditionMet = true;
                        }
                    }
                }
                else
                {
                    page.Next.First().ConditionMet = true;
                }

//                section.QnAData = workflow;

                pages.ForEach(p =>
                {
                    if (p.PageId == request.PageId)
                    {
                        p.Complete = page.Complete;
                        p.PageOfAnswers = page.PageOfAnswers;
                        p.Feedback = page.Feedback;
                    }
                });

                section.Pages = pages;
                
                await _applyRepository.SaveSection(section, request.UserId);
                
                return new UpdatePageAnswersResult {Page = page, ValidationPassed = validationPassed};
            }
            else
            {
                return new UpdatePageAnswersResult
                    {Page = page, ValidationPassed = validationPassed, ValidationErrors = validationErrors};
            }
        }
        
        private bool ProcessAnswer(UpdatePageAnswersRequest request, Question question, bool validationPassed,
            List<KeyValuePair<string, string>> validationErrors,
            PageOfAnswers pageAnswers)
        {
            var answer = request.Answers.FirstOrDefault(a => a.QuestionId == question.QuestionId);

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

                pageAnswers.Answers.Add(answer);
            }

            return validationPassed;
        }

//        private static void MarkSequenceAsCompleteIfAllPagesComplete(ApplicationSequence sequence, List<Sequence> workflow)
//        {
//            sequence.Complete = sequence.Sections.SelectMany(s => s.Pages).All(p => p.Complete);
//
//            if (!sequence.Complete) return;
//
//            var nextSequences = sequence.NextSequences;
//            foreach (var nextSequence in nextSequences)
//            {
//                if (nextSequence.Condition != null)
//                {
//                    var answers = sequence.Sections.SelectMany(s => s.Pages).SelectMany(p => p.PageOfAnswers[0].Answers).ToList();
//                    if (answers.Any(a =>
//                        a.QuestionId == nextSequence.Condition.QuestionId &&
//                        a.Value == nextSequence.Condition.MustEqual))
//                    {
//                        workflow.Single(w => w.SequenceId == nextSequence.NextSequenceId).Active = true;
//                    }
//                }
//                else
//                {
//                    workflow.Single(w => w.SequenceId == nextSequence.NextSequenceId).Active = true;
//                }
//            }
//        }
    }
}