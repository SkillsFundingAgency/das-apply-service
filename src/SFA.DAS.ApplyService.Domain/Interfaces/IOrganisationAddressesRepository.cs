using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IOrganisationAddressesRepository
    {
        Task<int> CreateOrganisationAddresses(OrganisationAddresses organisationAddresses);
        Task UpdateOrganisationAddresses(OrganisationAddresses organisationAddresses);
        Task<OrganisationAddresses> GetOrganisationAddressesByOrganisationId(Guid organisationId);
    }
}
