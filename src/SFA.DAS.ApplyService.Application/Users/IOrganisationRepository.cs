using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Users
{
    public interface  IOrganisationRepository
    {
        Task<Organisation> GetUserOrganisation(Guid userId);
    }
}