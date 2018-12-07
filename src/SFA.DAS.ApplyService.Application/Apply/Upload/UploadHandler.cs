using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Upload
{
    public class UploadHandler : IRequestHandler<UploadRequest, UploadResult>
    {
        private readonly IStorageService _storageService;
        private readonly IEncryptionService _encryptionService;

        public UploadHandler(IStorageService storageService, IEncryptionService encryptionService)
        {
            _storageService = storageService;
            _encryptionService = encryptionService;
        }
        
        public async Task<UploadResult> Handle(UploadRequest request, CancellationToken cancellationToken)
        {
            var result = new UploadResult();
            foreach (var file in request.Files)
            {
                var encryptedFileStream = await _encryptionService.Encrypt(file.OpenReadStream());
                
                var storedFileName = await _storageService.Store(request.ApplicationId.ToString(), request.SequenceId, request.SectionId, request.PageId, file.Name, file.FileName, encryptedFileStream, file.ContentType);

                if (storedFileName != null)
                {
                    result.Files.Add(new UploadedFileResult()
                    {
                        Uploaded = true,
                        UploadedFileName = storedFileName,
                        QuestionId = file.Name
                    });                    
                }
                else
                {
                    result.Files.Add(new UploadedFileResult()
                    {
                        Uploaded = false,
                        UploadedFileName = file.FileName,
                        QuestionId = file.Name
                    });
                }
            }

            return result;
        }
    }
}