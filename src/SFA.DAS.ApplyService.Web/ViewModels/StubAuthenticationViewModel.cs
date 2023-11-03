using SFA.DAS.GovUK.Auth.Models;

namespace SFA.DAS.ApplyService.Web.ViewModels
{
    public class StubAuthenticationViewModel : StubAuthUserDetails
    {
        public string ReturnUrl { get; set; }
    }
}
