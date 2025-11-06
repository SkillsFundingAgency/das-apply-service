using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using SFA.DAS.ApplyService.Domain.Roatp.Models;
using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
using SFA.DAS.ApplyService.InternalApi.Models.Ukrlp;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure;

public interface IRoatpApiClient
{
    [Get("/provider-types")]
    Task<IEnumerable<Models.Roatp.ProviderType>> GetProviderTypes();

    [Get("/organisations/{ukprn}")]
    Task<ApiResponse<OrganisationModel>> GetOrganisation([Query("ukprn")] int ukprn);

    [Get("/organisations/{ukprn}/ukrlp-data")]
    Task<UkprnLookupResponse> GetUkrlpDetails([Query("ukprn")] string ukprn);

    [Get("/api/v1/lookupData/organisationTypes?providerTypeId={providerTypeId}")]
    Task<IEnumerable<OrganisationType>> GetOrganisationTypes([AliasAs("providerTypeId")] int? providerTypeId);
}
