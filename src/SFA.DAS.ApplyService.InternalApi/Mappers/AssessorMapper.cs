using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.InternalApi.Services;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.InternalApi.Mappers
{
    internal static class AssessorMapper
    {
        public static AssessorPage ToAssessorPage(this Page qnaPage, IAssessorLookupService assessorLookupService, Guid applicationId, int sequenceNumber, int sectionNumber)
        {
            var page = new AssessorPage
            {
                ApplicationId = applicationId,
                SequenceNumber = sequenceNumber,
                SectionNumber = sectionNumber,
                PageId = qnaPage.PageId,

                DisplayType = qnaPage.DisplayType,
                Caption = assessorLookupService?.GetTitleForSequence(sequenceNumber),
                Heading = assessorLookupService?.GetTitleForPage(qnaPage.PageId) ?? qnaPage.LinkTitle,
                Title = qnaPage.Title,
                BodyText = qnaPage.BodyText
            };

            if (qnaPage.Questions != null && qnaPage.Questions.Any())
            {
                page.Questions = new List<AssessorQuestion>(qnaPage.Questions.Select(q => { return q.ToAssessorQuestion(assessorLookupService); }));
            }

            if (qnaPage.PageOfAnswers != null && qnaPage.PageOfAnswers.Any())
            {
                var qnaAnswers = qnaPage.PageOfAnswers.SelectMany(pao => pao.Answers);
                page.Answers = qnaAnswers.ToLookup(a => a.QuestionId).Select(group =>
                {
                    return new AssessorAnswer { QuestionId = group.Key, Value = group.FirstOrDefault()?.Value };
                }).ToList();
            }

            return page;
        }

        private static AssessorQuestion ToAssessorQuestion(this Question qnaQuestion, IAssessorLookupService assessorLookupService)
        {
            var question = new AssessorQuestion
            {
                QuestionId = qnaQuestion.QuestionId,
                Label = assessorLookupService?.GetLabelForQuestion(qnaQuestion.QuestionId) ?? qnaQuestion.Label,
                QuestionBodyText = qnaQuestion.QuestionBodyText,
                InputType = qnaQuestion.Input?.Type,
                InputPrefix = qnaQuestion.Input?.InputPrefix,
                InputSuffix = qnaQuestion.Input?.InputSuffix
            };

            if (qnaQuestion.Input?.Options != null && qnaQuestion.Input.Options.Any())
            {
                question.Options = new List<AssessorOption>(qnaQuestion.Input.Options.Select(opt => { return opt.ToAssessorOption(assessorLookupService); }));
            }

            return question;
        }

        private static AssessorOption ToAssessorOption(this Option qnaOption, IAssessorLookupService assessorLookupService)
        {
            var option = new AssessorOption
            {
                HintText = qnaOption.HintText,
                Label = qnaOption.Label,
                Value = qnaOption.Value
            };

            if (qnaOption.FurtherQuestions != null && qnaOption.FurtherQuestions.Any())
            {
                option.FurtherQuestions = new List<AssessorQuestion>(qnaOption.FurtherQuestions.Select(fq => { return fq.ToAssessorQuestion(assessorLookupService); }));
            }

            return option;
        }
    }
}
