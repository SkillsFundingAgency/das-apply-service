using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Organisations.UpdateOrganisation;
using SFA.DAS.ApplyService.Domain.CompaniesHouse;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
   public interface IOrganisationApiClient
   {
       Task<Organisation> Create(OrganisationSearchResult organisation, Guid userId);
       Task<Organisation> Create(CreateOrganisationRequest request, Guid userId);
       Task<Organisation> GetByUser(Guid userId);
      //Task<Organisation> Update(Organisation organisation, Guid userId);

       Task<bool> UpdateDirectorsAndPscs(string ukprn, List<DirectorInformation> directors, List<PersonSignificantControlInformation> personsWithSignificantControl, Guid userId);
   }
}
