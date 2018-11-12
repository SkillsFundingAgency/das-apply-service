using SFA.DAS.ApplyService.Domain.Entities;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Organisations
{
    public interface IOrganisationRepository
    {
        Task<Organisation> CreateOrganisation(Organisation organisation);
        Task<Organisation> GetOrganisationByContactEmail(string email);
        Task<Organisation> GetOrganisationByName(string name);
        Task<Organisation> UpdateOrganisation(Organisation organisation);
    }
}