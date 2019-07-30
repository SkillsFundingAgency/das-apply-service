namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Roatp;
    using Models.Ukrlp;
    using SFA.DAS.ApplyService.InternalApi.Models.Roatp;

    public interface IRoatpApiClient
    {
        Task<IEnumerable<ProviderType>> GetProviderTypes();
        Task<OrganisationRegisterStatus> GetOrganisationRegisterStatus(string ukprn);
        Task<UkprnLookupResponse> GetUkrlpDetails(string ukprn);
    }
}
