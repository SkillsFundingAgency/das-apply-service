using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IAppealsQueries
    {
        Task<Appeal> GetAppeal(Guid applicationId);

        Task<AppealFile> GetAppealFile(Guid applicationId, string fileName);

        Task<List<AppealFile>> GetAppealFilesForApplication(Guid applicationId);
    }
}
