using System;
using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class SequenceViewModel
    {
        public SequenceViewModel(Sequence sequence, Guid applicationId)
        {
            ApplicationId = applicationId;
            Sections = sequence.Sections;
            Title = sequence.Title;
            Active = sequence.Active;
            SequenceId = sequence.SequenceId;
            LinkTitle = sequence.LinkTitle;
        }

        public Guid ApplicationId { get; }
        public string LinkTitle { get; set; }
        public string SequenceId { get; set; }
        public bool Active { get; set; }
        public List<Section> Sections { get; set; }
        public string Title { get; set; }
    }
}