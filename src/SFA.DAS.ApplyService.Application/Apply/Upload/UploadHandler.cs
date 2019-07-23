using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Upload
{
    public class UploadHandler : IRequestHandler<UploadRequest, UploadResult>
    {
        private readonly ILogger<UploadHandler> _logger;
        private readonly IStorageService _storageService;
        private readonly IEncryptionService _encryptionService;

        public UploadHandler(ILogger<UploadHandler> logger, IStorageService storageService, IEncryptionService encryptionService)
        {
            _logger = logger;
            _storageService = storageService;
            _encryptionService = encryptionService;
        }

        public async Task<UploadResult> Handle(UploadRequest request, CancellationToken cancellationToken)
        {
            var result = new UploadResult();

            foreach (var file in request.Files)
            {
                var storedFileName = string.Empty;

                try
                {
                    using (var fileStream = file.OpenReadStream())
                    {
                        using (var encryptedFileStream = await _encryptionService.Encrypt(fileStream))
                        {
                            storedFileName = await _storageService.Store(request.ApplicationId.ToString(), request.SequenceId, request.SectionId, request.PageId, file.Name, file.FileName, encryptedFileStream, file.ContentType);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Unable to store file: {file.FileName} for application: {request.ApplicationId} - question: {file.Name} ");
                    storedFileName = null;
                }

                if (!string.IsNullOrWhiteSpace(storedFileName))
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