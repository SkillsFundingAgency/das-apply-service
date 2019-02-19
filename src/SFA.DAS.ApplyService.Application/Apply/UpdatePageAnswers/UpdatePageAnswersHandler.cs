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
      var page = section.QnAData.Pages.Single(p => p.PageId == request.PageId);
      page.DisplayType = section.DisplayType;
      var existingAnswers = page.PageOfAnswers.Select(poa => new PageOfAnswers { Answers = poa.Answers }).ToList();

      //var pages = section.Pages;
      var qnADataObject = section.QnAData;

      PageOfAnswers pageAnswers;
      if (!page.AllowMultipleAnswers)
      {
        page.PageOfAnswers = new List<PageOfAnswers>();
        pageAnswers = new PageOfAnswers() { Answers = new List<Answer>() };
        page.PageOfAnswers.Add(pageAnswers);
      }
      else
      {
        if (page.PageOfAnswers == null)
        {
          page.PageOfAnswers = new List<PageOfAnswers>();
        }
        pageAnswers = new PageOfAnswers() { Answers = new List<Answer>(), Id = Guid.NewGuid() };
        page.PageOfAnswers.Add(pageAnswers);
      }

      var validationPassed = true;
      var validationErrors = new List<KeyValuePair<string, string>>();

      foreach (var question in page.Questions)
      {
        validationPassed = ProcessAnswer(request, question, validationPassed, validationErrors, pageAnswers, existingAnswers);
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
                  //var fq = JsonConvert.DeserializeObject<Question>(furtherQuestion.ToString());

                  validationPassed = ProcessAnswer(request, furtherQuestion, validationPassed, validationErrors,
                      pageAnswers, existingAnswers);
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
          foreach (var nextAction in page.Next)
          {
            nextAction.ConditionMet = false;
          }

          var aConditionMet = false;
          var returnIdMet = "";
          // Activate next page if necessary
          foreach (var nextAction in page.Next)
          {
            if (nextAction.Condition == null) continue;
            var answerValue = request.Answers.Single(a => a.QuestionId == nextAction.Condition.QuestionId).Value;
            if (nextAction.Condition.MustEqual == answerValue)
            {
              if (nextAction.Action == "NextPage")
              {
                qnADataObject.Pages.Single(p => p.PageId == nextAction.ReturnId).Active = true;
                qnADataObject.Pages.Single(p => p.PageId == nextAction.ReturnId).Visible = true;
              }

              returnIdMet = nextAction.ReturnId;
              aConditionMet = true;
              nextAction.ConditionMet = true;
            }
            else
            {
              if (nextAction.Action == "NextPage" && nextAction.ReturnId != returnIdMet)
              {
                qnADataObject.Pages.Single(p => p.PageId == nextAction.ReturnId).Active = false;
                qnADataObject.Pages.Single(p => p.PageId == nextAction.ReturnId).Visible = false;
              }
            }
          }

          // if not set elsewhere, then a null condition must be the one met
          if (!aConditionMet)
          {
            foreach (var nextAction in page.Next)
            {
              if (nextAction.Condition == null)
              {
                nextAction.ConditionMet = true;
                break;
              }
            }
          }
        }
        else
        {
          page.Next.First().ConditionMet = true;
        }


        //                section.QnAData = workflow;

        qnADataObject.Pages.ForEach(p =>
        {
          if (p.PageId == request.PageId)
          {
            p.Complete = page.Complete;
            p.PageOfAnswers = page.PageOfAnswers;
            p.Feedback = page.Feedback;
          }
        });

        qnADataObject.FinancialApplicationGrade = null; // Remove any previous grade as it doesn't reflect the new answers
        section.QnAData = qnADataObject;

        await _applyRepository.SaveSection(section, request.UserId);

        return new UpdatePageAnswersResult { Page = page, ValidationPassed = validationPassed };
      }
      else
      {
        return new UpdatePageAnswersResult
        { Page = page, ValidationPassed = validationPassed, ValidationErrors = validationErrors };
      }
    }

    private bool ProcessAnswer(UpdatePageAnswersRequest request, Question question, bool validationPassed,
        List<KeyValuePair<string, string>> validationErrors,
        PageOfAnswers pageAnswers, List<PageOfAnswers> pagePageOfAnswers)
    {

      if (question.Input.Type == "FileUpload")
      {
        var answer = request.Answers.FirstOrDefault(a => a.QuestionId == question.QuestionId);
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