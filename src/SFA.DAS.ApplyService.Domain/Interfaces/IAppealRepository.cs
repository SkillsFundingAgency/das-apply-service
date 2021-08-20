using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    // TODO: APPEALREVIEW - Review once appeal work starts
    public interface IAppealRepository
    {
        void Add(Appeal entity);
        void Update(Appeal entity);

        Task<Appeal> GetByApplicationId(Guid applicationId);
    }
}
