using SFA.DAS.ApplyService.Domain.Roatp;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public interface IRoatpStatusValidator
    {
        bool ProviderEligibleToJoinRegister(OrganisationRegisterStatus registerStatus);
    }
}
