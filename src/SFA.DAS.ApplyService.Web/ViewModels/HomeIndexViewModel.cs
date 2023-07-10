using SFA.DAS.ApplyService.Web.Constants;

namespace SFA.DAS.ApplyService.Web.ViewModels
{
    public record HomeIndexViewModel(bool UseGovSignIn)
    {
        public string ApplyNowLink =>
            UseGovSignIn 
                ? RouteNames.SignIn 
                : RouteNames.ExistingAccount;

        public string ApplyNowBtnText =>
            UseGovSignIn 
                ? "Start now" 
                : "Apply now";

        public string OrganisationName =>
            UseGovSignIn
                ? "DfE"
                : "ESFA";
    }
}
