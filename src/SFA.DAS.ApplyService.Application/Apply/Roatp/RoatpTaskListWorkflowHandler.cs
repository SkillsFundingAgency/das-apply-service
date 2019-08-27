
namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using MoreLinq;
    using SFA.DAS.ApplyService.Domain.Entities;

    public static class RoatpTaskListWorkflowHandler
    {
        public static string SectionStatus(IEnumerable<ApplicationSequence> applicationSequences, int sequenceId, int sectionId, bool sequential = false)
        {
            var sequence = applicationSequences.FirstOrDefault(x => (int)x.SequenceId == sequenceId);
            if (sequence == null)
            {
                return String.Empty;
            }

            var section = sequence.Sections.FirstOrDefault(x => x.SectionId == sectionId);
            if (section == null)
            {
                return string.Empty;
            }

            if (!PreviousSectionCompleted(sequence, sectionId, sequential))
            {
                return string.Empty;
            }

            var questionsCompleted = SectionHasCompletedQuestions(section);
            var questionsInSection = section.QnAData.Pages.Where(p => p.NotRequired == false).SelectMany(x => x.Questions).DistinctBy(q => q.QuestionId).Count();
            return SectionText(questionsCompleted, questionsInSection, sequential);
        }

        public static bool PreviousSectionCompleted(ApplicationSequence sequence, int sectionId, bool sequential)
        {
            if (sequential && sectionId > 1)
            {
                var previousSection = sequence.Sections.FirstOrDefault(x => x.SectionId == (sectionId - 1));
                if (previousSection == null)
                {
                    return false;
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

        private static int SectionHasCompletedQuestions(ApplicationSection section)
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

        private static string SectionText(int completedCount, int questionCount, bool sequential)
        {
            if (completedCount < questionCount)
            {
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

            return "Completed";
        }
    }
}
