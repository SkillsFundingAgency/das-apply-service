using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Organisations
{
    public interface IOrganisationRepository
    {
        Task<Organisation> CreateOrganisation(Organisation organisation, Guid userId);
        Task<Organisation> GetOrganisationByContactEmail(string email);
        Task<Organisation> GetOrganisationByName(string name);
        Task<Organisation> GetOrganisationByApplicationId(Guid applicationId);
        Task<Organisation> UpdateOrganisation(Organisation organisation, Guid userId);
        Task<Organisation> GetUserOrganisation(Guid userId);
    }
}