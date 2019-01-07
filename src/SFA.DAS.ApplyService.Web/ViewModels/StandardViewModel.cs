using System;
using System.Collections.Generic;
using SFA.DAS.ApplyService.InternalApi.Models.AssessorService;

namespace SFA.DAS.ApplyService.Web.ViewModels
{
    public class StandardViewModel
    {
        public Guid ApplicationId { get; set; }

        public string StandardToFind { get; set; }

        public int StandardCode { get; set; }

        public List<StandardCollation> Results { get; set; }

        public StandardCollation SelectedStandard { get; set; }
    }
}
