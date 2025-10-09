namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Roatp;
    using Models.Ukrlp;
    using Refit;
    using SFA.DAS.ApplyService.InternalApi.Models.Roatp;

    public interface IRoatpApiClient
    {
        [Get("/api/v1/lookupData/providerTypes")]
        Task<IEnumerable<ProviderType>> GetProviderTypes();

        [Get("/api/v1/organisation/register-status?&ukprn={ukprn}")]
        Task<OrganisationRegisterStatus> GetOrganisationRegisterStatus([AliasAs("ukprn")] string ukprn);

        [Get("/api/v1/ukrlp/lookup/{ukprn}")]
        Task<UkprnLookupResponse> GetUkrlpDetails([AliasAs("ukprn")] string ukprn);

        [Get("/api/v1/lookupData/organisationTypes?providerTypeId={providerTypeId}")]
        Task<IEnumerable<OrganisationType>> GetOrganisationTypes([AliasAs("providerTypeId")] int? providerTypeId);
    }
}
