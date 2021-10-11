using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    [Authorize]
    public class RoatpApplyControllerBase : Controller
    {
        protected const string ApplicationDetailsKey = "Roatp_Application_Details";

        private const string GetHelpSubmittedForPageKey = "Roatp_GetHelpSubmitted_{0}";
        private const string GetHelpQuestionKey = "Roatp_GetHelpQuestion_{0}";
        private const string GetHelpErrorMessageKey = "Roatp_GetHelp_ErrorMessage_{0}";

        protected ISessionService _sessionService;

        public RoatpApplyControllerBase(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        protected void PopulateGetHelpWithQuestion(IPageViewModel viewModel)
        {
            var getHelpSessionKey = string.Format(GetHelpSubmittedForPageKey, viewModel.PageId);
            var getHelpSubmitted = _sessionService.Get<bool>(getHelpSessionKey);
            viewModel.GetHelpQuerySubmitted = getHelpSubmitted;
            
            var getHelpQuestionKey = string.Format(GetHelpQuestionKey, viewModel.PageId);
            var getHelpQuestion = _sessionService.Get(getHelpQuestionKey);
            var getHelpErrorMessageKey = string.Format(GetHelpErrorMessageKey, viewModel.PageId);
            var getHelpErrorMessage = _sessionService.Get(getHelpErrorMessageKey);

            if (!string.IsNullOrWhiteSpace(getHelpErrorMessage))
            {
                viewModel.GetHelpErrorMessage = getHelpErrorMessage;
                viewModel.GetHelpQuerySubmitted = false;
                viewModel.GetHelpQuestion = getHelpQuestion;
            }
        }
    }
}
