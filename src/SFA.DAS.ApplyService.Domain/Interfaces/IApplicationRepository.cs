using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IApplicationRepository
    {
        void Update(Entities.Apply application);
        Task<Entities.Apply> GetApplication(Guid applicationId);
    }
}
