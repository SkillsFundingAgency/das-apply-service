using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using NPOI.POIFS.Storage;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.Configuration;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class RoatpTaskListWorkflowService: IRoatpTaskListWorkflowService
    {
        public  string SectionStatus(IEnumerable<ApplicationSequence> applicationSequences, List<NotRequiredOverrideConfiguration> notRequiredOverrides, int sequenceId, int sectionId, string applicationRouteId, bool sequential = false)
        {
            var sequence = applicationSequences?.FirstOrDefault(x => (int)x.SequenceId == sequenceId);

            var section = sequence?.Sections?.FirstOrDefault(x => x.SectionId == sectionId);
            if (section == null)
            {
                return string.Empty;
            }

            if (notRequiredOverrides!=null && notRequiredOverrides.Any(condition => condition.ConditionalCheckField == "ProviderTypeId" &&
                                                          applicationRouteId == condition.MustEqual &&
                                                          sectionId == condition.SectionId &&
                                                          sequenceId == condition.SequenceId))
            {
                return "Not required";
            }


            if (!PreviousSectionCompleted(sequence, sectionId, sequential))
            {
                return string.Empty;
            }

            var questionsCompleted = SectionHasCompletedQuestions(section);

            return SectionText(questionsCompleted, section.SectionCompleted, sequential);
        }

        public  bool PreviousSectionCompleted(ApplicationSequence sequence, int sectionId, bool sequential)
        {
            if (sequential && sectionId > 1)
            {
                var previousSection = sequence.Sections.FirstOrDefault(x => x.SectionId == (sectionId - 1));
                if (previousSection == null)
                {
                    return false;
                }

                if (previousSection.SectionCompleted)
                {
                    return true;
                }

                var previousSectionQuestionsCompleted = SectionHasCompletedQuestions(previousSection);
                var previousSectionQuestionsCount = previousSection.QnAData.Pages.Where(p => p.NotRequired == false).SelectMany(x => x.Questions)
                    .DistinctBy(q => q.QuestionId).Count();
                if (previousSectionQuestionsCompleted < previousSectionQuestionsCount)
                {
                    return false;
                }
            }

            return true;
        }

        private  int SectionHasCompletedQuestions(ApplicationSection section)
        {
            int answeredQuestions = 0;
            
            var pages = section.QnAData.Pages.Where(p => p.NotRequired == false);
            foreach (var page in pages)
            {
                var questionIds = page.Questions.Select(x => x.QuestionId);
                foreach (var questionId in questionIds)
                {
                    foreach (var pageOfAnswers in page.PageOfAnswers)
                    {
                        var matchedAnswer = pageOfAnswers.Answers.FirstOrDefault(y => y.QuestionId == questionId);
                        if (matchedAnswer != null && !String.IsNullOrEmpty(matchedAnswer.Value))
                        {
                            answeredQuestions++;
                        }
                    }
                }
            }

            return answeredQuestions;
        }

        private  string SectionText(int completedCount, bool sectionCompleted, bool sequential)
        {
            if (sectionCompleted)
            {
                return "Completed";
            }

            if (sequential && completedCount == 0)
            {
                return "Next";
            }

            if (completedCount > 0)
            {
                return "In Progress";
            }

            return string.Empty;

        }
    }
}
