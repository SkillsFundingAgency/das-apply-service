namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Roatp;
    using SFA.DAS.ApplyService.Domain.Entities;

    public interface IRoatpApiClient
    {
        Task<IEnumerable<ApplicationRoute>> GetApplicationRoutes();
        Task<OrganisationRegisterStatus> GetOrganisationRegisterStatus(long ukprn);
    }
}