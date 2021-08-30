using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals
{
    public class GroundsOfAppealViewModel
    {
        public const string UPLOAD_APPEALFILE_FORMACTION = "Upload";


        public Guid ApplicationId { get; set; }

        public bool AppealOnPolicyOrProcesses { get; set; }
        public bool AppealOnEvidenceSubmitted { get; set; }

        public string HowFailedOnPolicyOrProcesses { get; set; }
        public string HowFailedOnEvidenceSubmitted { get; set; }

        public string FormAction { get; set; }
        public Microsoft.AspNetCore.Http.IFormFile AppealFileToUpload { get; set; }

        public List<InternalApi.Types.Responses.Appeals.AppealFile> AppealFiles { get; set; }
    }
}
