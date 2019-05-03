namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SFA.DAS.ApplyService.InternalApi.Models.Roatp;

    public interface IRoatpApiClient
    {
        Task<IEnumerable<ProviderType>> GetProviderTypes();
        Task<DuplicateCheckResponse> DuplicateUKPRNCheck(Guid organisationId, long ukprn);
        Task<OrganisationReapplyStatus> GetOrganisationReapplyStatus(Guid organisationId);
    }
}
