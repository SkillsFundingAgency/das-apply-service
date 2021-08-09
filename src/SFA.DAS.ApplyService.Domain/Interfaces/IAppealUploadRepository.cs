using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    // TODO: APPEALREVIEW - Review once appeal work starts
    public interface IAppealUploadRepository
    {
        void Add(AppealUpload entity);
        void Remove(Guid entityId);
        void Update(AppealUpload entity);
        Task<AppealUpload> GetById(Guid entityId);
        Task<IEnumerable<AppealUpload>> GetByApplicationId(Guid applicationId);
    }
}
