
namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;

    public class TaskListViewModel
    {
        public Guid ApplicationId { get; set; }
        public string UKPRN { get; set; }
        public string OrganisationName { get; set; }
        public IEnumerable<Domain.Entities.ApplicationSequence> ApplicationSequences { get; set; }

        public string CssClass(int sequenceId, int sectionId, bool sequential = false)
        {
            var status = SectionStatus(sequenceId, sectionId, sequential);

            if (status == String.Empty)
            {
                return "hidden";
            }

            var cssClass = status.ToLower();
            cssClass = cssClass.Replace(" ", "");
            
            return cssClass;
        }

        public string SectionStatus(int sequenceId, int sectionId, bool sequential = false)
        {
            var sequence = ApplicationSequences.FirstOrDefault(x => (int)x.SequenceId == sequenceId);
            if (sequence == null)
            {
                return String.Empty;
            }

            var section = sequence.Sections.FirstOrDefault(x => x.SectionId == sectionId);
            if (section == null)
            {
                return string.Empty;
            }

            if (sequential && sectionId > 1)
            {
                var previousSection = sequence.Sections.FirstOrDefault(x => x.SectionId == (sectionId - 1));
                if (previousSection == null)
                {
                    return string.Empty;
                }

                var previousSectionQuestionsCompleted = SectionHasCompletedQuestions(previousSection);
                var previousSectionQuestionsCount = previousSection.QnAData.Pages.SelectMany(x => x.Questions).Count();
                if (previousSectionQuestionsCompleted != previousSectionQuestionsCount)
                {
                    return string.Empty;
                } 
            }
            
            var questionsCompleted = SectionHasCompletedQuestions(section);
            var questionsInSection = section.QnAData.Pages.SelectMany(x => x.Questions).Count();
            return SectionText(questionsCompleted, questionsInSection, sequential);
        }

        private int SectionHasCompletedQuestions(ApplicationSection section)
        {
            int answeredQuestions = 0;
            var pages = section.QnAData.Pages;
            foreach (var page in pages)
            {
                var questionIds = page.Questions.Select(x => x.QuestionId);
                foreach (var questionId in questionIds)
                {
                    if (!page.PageOfAnswers.Any())
                    {
                        return answeredQuestions;
                    }

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

        private string SectionText(int completedCount, int questionCount, bool sequential)
        {
            if (completedCount != questionCount)
            {
                if (sequential)
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
