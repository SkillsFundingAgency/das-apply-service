
namespace SFA.DAS.ApplyService.Web.Validators
{
    using System;
    using Domain.Roatp;

    public class RoatpStatusValidator : IRoatpStatusValidator
    {
        public bool ProviderEligibleToJoinRegister(OrganisationRegisterStatus registerStatus)
        {
            if (!registerStatus.UkprnOnRegister)
            {
                return true;
            }

            if (registerStatus.StatusId == OrganisationStatus.Active ||
                registerStatus.StatusId == OrganisationStatus.ActiveNotTakingOnApprentices ||
                registerStatus.StatusId == OrganisationStatus.Onboarding)
            {
                return true;
            }

            if (registerStatus.StatusId == OrganisationStatus.Removed 
                && registerStatus.StatusDate.HasValue 
                && registerStatus.StatusDate.Value <= DateTime.Today.AddYears(-3))
            {
                return true;
            }

            if (registerStatus.StatusId == OrganisationStatus.Removed
                && registerStatus.StatusDate > DateTime.Today.AddYears(-3)
                && registerStatus.RemovedReasonId.HasValue
                && registerStatus.RemovedReasonId.Value == RemovedReason.ProviderRequest)
            {
                return true;
            }

            return false;
        }
    }
}
