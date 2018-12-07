using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Download
{
    public class DownloadHandler : IRequestHandler<DownloadRequest, DownloadResponse>
    {
        private readonly IStorageService _storageService;
        private readonly IEncryptionService _encryptionService;

        public DownloadHandler(IStorageService storageService, IEncryptionService encryptionService)
        {
            _storageService = storageService;
            _encryptionService = encryptionService;
        }
        
        public async Task<DownloadResponse> Handle(DownloadRequest request, CancellationToken cancellationToken)
        {
            var encryptedResult = await _storageService.Retrieve(request.ApplicationId.ToString(), request.SequenceId, request.SectionId, request.PageId, request.QuestionId, request.Filename);

            var decrypted = await _encryptionService.Decrypt(encryptedResult.Item2);

            return new DownloadResponse() {Filename = encryptedResult.Item1, FileStream = decrypted, ContentType = encryptedResult.Item3};

        }
    }
}