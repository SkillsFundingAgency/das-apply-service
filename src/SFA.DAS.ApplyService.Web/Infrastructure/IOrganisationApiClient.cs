using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.CompaniesHouse;
using SFA.DAS.ApplyService.InternalApi.Types.CharityCommission;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
   public interface IOrganisationApiClient
   {
       Task<Organisation> Create(OrganisationSearchResult organisation, Guid userId);
       Task<Organisation> Create(CreateOrganisationRequest request, Guid userId);
       Task<Organisation> GetByUser(Guid userId);
       Task<Organisation> GetByApplicationId(Guid applicationId);
        Task<bool> UpdateDirectorsAndPscs(string ukprn, List<DirectorInformation> directors, List<PersonSignificantControlInformation> personsWithSignificantControl, Guid userId);
       Task<bool> UpdateTrustees(string ukprn, List<Trustee> trustees, Guid userId);
   }
}
