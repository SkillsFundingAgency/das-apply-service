
namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TaskListViewModel
    {
        public Guid ApplicationId { get; set; }
        public string UKPRN { get; set; }
        public string OrganisationName { get; set; }
        public IEnumerable<Domain.Entities.ApplicationSequence> ApplicationSequences { get; set; }

        public string CssClass(int sequenceId, int sectionId)
        {
            var status = SectionStatus(sequenceId, sectionId);

            if (status == String.Empty)
            {
                return "hidden";
            }

            var cssClass = status.ToLower();
            cssClass = cssClass.Replace(" ", "");
            
            return cssClass;
        }

        public string SectionStatus(int sequenceId, int sectionId)
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

            var pages = section.QnAData.Pages;
            foreach (var page in pages)
            {
                var questionIds = page.Questions.Select(x => x.QuestionId);
                foreach (var questionId in questionIds)
                {
                    if (!page.PageOfAnswers.Any())
                    {
                        return "In Progress";
                    }

                    foreach (var pageOfAnswers in page.PageOfAnswers)
                    {
                        var matchedAnswer = pageOfAnswers.Answers.FirstOrDefault(y => y.QuestionId == questionId);
                        if (matchedAnswer == null || String.IsNullOrEmpty(matchedAnswer.Value))
                        {
                            return "In Progress";
                        }
                    }
                }
            }

            return "Completed";
        }
    }
}
