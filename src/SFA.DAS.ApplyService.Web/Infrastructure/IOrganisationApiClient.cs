using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
   public interface IOrganisationApiClient
   {
       Task<Organisation> Create(OrganisationSearchResult organisation, Guid userId);
       Task<Organisation> Create(CreateOrganisationRequest request, Guid userId);
       Task<Organisation> GetByUser(Guid userId);
   }
}
