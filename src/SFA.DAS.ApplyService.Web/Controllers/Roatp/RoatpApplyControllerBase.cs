using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.ViewModels;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using System;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    [Authorize]
    public class RoatpApplyControllerBase : Controller
    {
        protected const string ApplicationDetailsKey = "Roatp_Application_Details";
        protected const string GetHelpSubmittedForPageKey = "Roatp_GetHelpSubmitted_{0}";
        protected const string GetHelpQuestionKey = "Roatp_GetHelpQuestion_{0}";
        protected const string GetHelpErrorMessageKey = "Roatp_GetHelp_ErrorMessage_{0}";

        protected ISessionService _sessionService;

        public RoatpApplyControllerBase(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        protected void PopulateGetHelpWithQuestion(IPageViewModel viewModel, string pageId)
        {
            viewModel.PageId = pageId;
            var getHelpSessionKey = string.Format(GetHelpSubmittedForPageKey, pageId);
            var getHelpSubmitted = _sessionService.Get<bool>(getHelpSessionKey);
            viewModel.GetHelpQuerySubmitted = getHelpSubmitted;
            
            var getHelpQuestionKey = string.Format(GetHelpQuestionKey, pageId);
            var getHelpQuestion = _sessionService.Get(getHelpQuestionKey);
            var getHelpErrorMessageKey = string.Format(GetHelpErrorMessageKey, pageId);
            var getHelpErrorMessage = _sessionService.Get(getHelpErrorMessageKey);

            if (!String.IsNullOrWhiteSpace(getHelpErrorMessage))
            {
                viewModel.GetHelpErrorMessage = getHelpErrorMessage;
                viewModel.GetHelpQuerySubmitted = false;
                viewModel.GetHelpQuestion = getHelpQuestion;
            }
        }
    }
}
