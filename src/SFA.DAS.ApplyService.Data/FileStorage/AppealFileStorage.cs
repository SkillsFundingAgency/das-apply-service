using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Models;

namespace SFA.DAS.ApplyService.Data.FileStorage
{
    public class AppealFileStorage : IAppealFileStorage
    {
        public Task<Guid> Add(Guid applicationId, FileUpload file, CancellationToken cancellationToken)
        {
            return Task.FromResult(Guid.NewGuid());
        }

        public Task Remove(Guid applicationId, Guid fileId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
