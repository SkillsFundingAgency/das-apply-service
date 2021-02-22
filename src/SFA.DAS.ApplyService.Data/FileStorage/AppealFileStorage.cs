using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Models;

namespace SFA.DAS.ApplyService.Data.FileStorage
{
    public class AppealFileStorage : FileStorage, IAppealFileStorage
    {
        public AppealFileStorage(IConfigurationService configurationService, IByteArrayEncryptionService byteArrayEncryptionService)
            : base(configurationService, byteArrayEncryptionService)
        {
        }

        protected override string ContainerName => "appeals-uploads";


        public async Task<Guid> Add(Guid applicationId, FileUpload file, CancellationToken cancellationToken)
        {
            return await AddFileToContainer($"{applicationId}", file, cancellationToken);
        }

        public async Task Remove(Guid applicationId, Guid reference, CancellationToken cancellationToken)
        {
            await RemoveFileFromContainer($"{applicationId}", reference);
        }
    }
}

