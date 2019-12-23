using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class RoatpTaskListWorkflowService
    {
        public static string SectionStatus(IEnumerable<ApplicationSequence> applicationSequences, List<NotRequiredOverrideConfiguration> notRequiredOverrides, int sequenceId, int sectionId, string applicationRouteId)
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
                return TaskListSectionStatus.NotRequired;
            }


            if (!PreviousSectionCompleted(sequence, sectionId, sequence.Sequential))
            {
                return string.Empty;
            }

            var questionsCompleted = SectionCompletedQuestionsCount(section);
                        
            var sectionText = GetSectionText(questionsCompleted, section, sequence.Sequential);
            
            return sectionText;
        }

        public static bool PreviousSectionCompleted(ApplicationSequence sequence, int sectionId, bool sequential)
        {
            if (sequence.Sequential && sectionId > 1)
            {
                var previousSection = sequence.Sections.FirstOrDefault(x => x.SectionId == (sectionId - 1));
                if (previousSection == null)
                {
                    return false;
                }

                if (previousSection.PagesActive == previousSection.PagesComplete && previousSection.PagesComplete > 0)
                {
                    return true;
                }

                var previousSectionsCompletedCount = SectionCompletedQuestionsCount(previousSection);
                if (previousSectionsCompletedCount == 0)
                    return false;                               

                var previousSectionQuestionsCount = previousSection.QnAData.Pages.Where(p => p.NotRequired == false).SelectMany(x => x.Questions)
                    .DistinctBy(q => q.QuestionId).Count();
                if (previousSectionsCompletedCount < previousSectionQuestionsCount)
                {
                    return false;
                }
            }

            return true;
        }

        private static int SectionCompletedQuestionsCount(ApplicationSection section)
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
                            break;
                        }
                    }
                }
            }

            return answeredQuestions;
        }

        private static string GetSectionText(int completedCount, ApplicationSection section,  bool sequential)
        {
            var pagesCompleted = section.QnAData.Pages.Count(x => x.Complete);
            var pagesActive = section.QnAData.Pages.Count(x => x.Active);

            if ((section.PagesComplete == section.PagesActive && section.PagesActive > 0))
                return TaskListSectionStatus.Completed;

            if (sequential && completedCount == 0)
            {
                return TaskListSectionStatus.Next;
            }

            if (completedCount > 0)
            {
                return TaskListSectionStatus.InProgress;
            }

            return TaskListSectionStatus.Blank;

        }
    }
}
