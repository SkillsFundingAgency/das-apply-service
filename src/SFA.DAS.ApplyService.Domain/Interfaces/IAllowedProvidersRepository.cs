using SFA.DAS.ApplyService.Domain.Apply.AllowedProviders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IAllowedProvidersRepository
    {
        Task<bool> CanUkprnStartApplication(int ukprn);

        Task<List<AllowedProvider>> GetAllowedProvidersList(string sortColumn, string sortOrder);

        Task<bool> AddToAllowedProvidersList(int ukprn, DateTime startDateTime, DateTime endDateTime);

        Task<AllowedProvider> GetAllowedProviderDetails(int ukprn);
    }
}
