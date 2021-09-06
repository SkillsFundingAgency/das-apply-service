using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals
{
    public class GroundsOfAppealViewModel
    {
        public const char FORMACTION_SEPERATOR = '|';
        public const string UPLOAD_APPEALFILE_FORMACTION = "UploadAppealFile";
        public const string DELETE_APPEALFILE_FORMACTION = "DeleteAppealFile";
        public const string SUBMIT_APPEAL_FORMACTION = "SubmitAppeal";

        public Guid ApplicationId { get; set; }

        public bool AppealOnPolicyOrProcesses { get; set; }
        public bool AppealOnEvidenceSubmitted { get; set; }

        public string HowFailedOnPolicyOrProcesses { get; set; }
        public string HowFailedOnEvidenceSubmitted { get; set; }

        public string FormAction { get; set; }

        public string RequestedFormAction
        { 
            get
            {
                if (FormAction?.StartsWith(DELETE_APPEALFILE_FORMACTION) is true)
                { 
                    return DELETE_APPEALFILE_FORMACTION;
                }
                else 
                {
                    return FormAction;
                }
            } 
        }

        public string RequestedFileToDelete
        {
            get
            {
                if (RequestedFormAction == DELETE_APPEALFILE_FORMACTION)
                {
                    var index = $"{DELETE_APPEALFILE_FORMACTION}{FORMACTION_SEPERATOR}".Length;

                    if (FormAction.Length > index)
                    {
                        var fileToDelete = FormAction.Remove(0, index);
                        return fileToDelete;
                    }
                }

                return null;
            }
        }

        public Microsoft.AspNetCore.Http.IFormFile AppealFileToUpload { get; set; }

        public List<InternalApi.Types.Responses.Appeals.AppealFile> AppealFiles { get; set; }
    }
}
