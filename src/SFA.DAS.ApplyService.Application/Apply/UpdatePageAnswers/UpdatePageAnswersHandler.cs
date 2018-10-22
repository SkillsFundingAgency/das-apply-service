using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Apply.GetPage;
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
            var entity = await _applyRepository.GetEntity(request.ApplicationId, request.UserId);
            var workflow = entity.QnAWorkflow;

            var sequence = workflow.Sequences.Single(w => w.Sections.Any(s => s.Pages.Any(p => p.PageId == request.PageId)));
            var section = sequence.Sections.Single(s => s.Pages.Any(p => p.PageId == request.PageId));

            if (!sequence.Active)
            {
                throw new BadRequestException("Sequence not active");
            }

            var page = section.Pages.Single(p => p.PageId == request.PageId);
            page.Answers = new List<Answer>();


            var validationPassed = true;
            var validationErrors = new List<KeyValuePair<string, string>>();

            foreach (var question in page.Questions)
            {
                validationPassed = ProcessAnswer(request, question, validationPassed, validationErrors, page);
                // IF Question is type ComplexRadio
                // Need to get all answers from Input.Options.FurtherQuestions

                var answer = request.Answers.FirstOrDefault(a => a.QuestionId == question.QuestionId);

                if (question.Input.Options != null)
                {
                    foreach (var option in question.Input.Options)
                    {
                        if (answer?.Value == option.Label.ToString())
                        {
                            if (option.FurtherQuestions != null)
                            {
                                foreach (var furtherQuestion in option.FurtherQuestions)
                                {
                                    var fq = JsonConvert.DeserializeObject<Question>(furtherQuestion.ToString());

                                    validationPassed = ProcessAnswer(request, fq, validationPassed, validationErrors,
                                        page);
                                }
                            }
                        }
                    }
                }
            }

            if (validationPassed)
            {
                page.Complete = true;

                MarkSequenceAsCompleteIfAllPagesComplete(sequence, workflow.Sequences);

                if (page.Next.Count() > 1)
                {
                    // Activate next page if necessary
                    foreach (var nextAction in page.Next)
                    {
                        if (nextAction.Condition.MustEqual == request.Answers
                                .Single(a => a.QuestionId == nextAction.Condition.QuestionId).Value)
                        {
                            section.Pages.Single(p => p.PageId == nextAction.ReturnId).Active = true;
                        }
                    }
                }

                entity.QnAWorkflow = workflow;

                await _applyRepository.SaveEntity(entity, request.ApplicationId, request.UserId);
                
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
            Page page)
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

                page.Answers.Add(answer);
            }

            return validationPassed;
        }

        private static void MarkSequenceAsCompleteIfAllPagesComplete(Sequence sequence, List<Sequence> workflow)
        {
            sequence.Complete = sequence.Sections.SelectMany(s => s.Pages).All(p => p.Complete);

            if (!sequence.Complete) return;

            var nextSequences = sequence.NextSequences;
            foreach (var nextSequence in nextSequences)
            {
                if (nextSequence.Condition != null)
                {
                    var answers = sequence.Sections.SelectMany(s => s.Pages).SelectMany(p => p.Answers).ToList();
                    if (answers.Any(a =>
                        a.QuestionId == nextSequence.Condition.QuestionId &&
                        a.Value == nextSequence.Condition.MustEqual))
                    {
                        workflow.Single(w => w.SequenceId == nextSequence.NextSequenceId).Active = true;
                    }
                }
                else
                {
                    workflow.Single(w => w.SequenceId == nextSequence.NextSequenceId).Active = true;
                }
            }
        }
    }
}