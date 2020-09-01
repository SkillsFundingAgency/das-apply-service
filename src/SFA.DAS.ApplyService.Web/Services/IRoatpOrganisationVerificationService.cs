using SFA.DAS.ApplyService.Domain.Roatp;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface IRoatpOrganisationVerificationService
    {
        Task<OrganisationVerificationStatus> GetOrganisationVerificationStatus(Guid applicationId);
    }
}
