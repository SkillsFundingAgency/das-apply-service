using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Models;

namespace SFA.DAS.ApplyService.Data.FileStorage
{
    public interface IAppealFileStorage
    {
        Task<Guid> Add(Guid applicationId, FileUpload file, CancellationToken cancellationToken);
        Task Remove(Guid applicationId, Guid reference, CancellationToken cancellationToken);
    }
}
