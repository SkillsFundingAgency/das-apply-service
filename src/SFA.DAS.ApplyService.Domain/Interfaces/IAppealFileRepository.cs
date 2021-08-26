using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IAppealFileRepository
    {
        void Add(AppealFile entity);
        void Remove(Guid entityId);
        Task<AppealFile> Get(Guid entityId);

        Task<AppealFile> Get(Guid applicationId, string fileName);
        Task<IEnumerable<AppealFile>> GetAllForApplication(Guid applicationId);
    }
}
