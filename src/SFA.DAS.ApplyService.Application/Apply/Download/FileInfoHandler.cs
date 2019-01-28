using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Download
{
    public class FileInfoHandler : IRequestHandler<FileInfoRequest, FileInfoResponse>
    {
        private readonly IStorageService _storageService;
        private readonly IEncryptionService _encryptionService;

        public FileInfoHandler(IStorageService storageService, IEncryptionService encryptionService)
        {
            _storageService = storageService;
            _encryptionService = encryptionService;
        }
        
        public async Task<FileInfoResponse> Handle(FileInfoRequest request, CancellationToken cancellationToken)
        {
            var encryptedResult = await _storageService.Retrieve(request.ApplicationId.ToString(), request.SequenceId, request.SectionId, request.PageId, request.QuestionId, request.Filename);

            return new FileInfoResponse() {Filename = encryptedResult.Item1, ContentType = encryptedResult.Item3};
        }
    }
}