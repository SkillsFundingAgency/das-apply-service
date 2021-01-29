using SFA.DAS.ApplyService.Domain.Ukrlp;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Roatp;

    public interface IRoatpApiClient
    {
        Task<IEnumerable<ApplicationRoute>> GetApplicationRoutes();
        Task<OrganisationRegisterStatus> GetOrganisationRegisterStatus(int ukprn);
        Task<IEnumerable<ProviderDetails>> GetUkrlpProviderDetails(string ukprn);
    }
}