using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IAppealUploadRepository
    {
        void Add(AppealUpload entity);
        void Remove(Guid entityId);
        Task<AppealUpload> GetById(Guid entityId);
    }
}
