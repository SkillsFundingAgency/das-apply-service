using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Interfaces;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Download
{
    public class DownloadHandler : IRequestHandler<DownloadRequest, DownloadResponse>
    {
        private readonly ILogger<DownloadHandler> _logger;
        private readonly IStorageService _storageService;
        private readonly IEncryptionService _encryptionService;

        public DownloadHandler(ILogger<DownloadHandler> logger, IStorageService storageService, IEncryptionService encryptionService)
        {
            _logger = logger;
            _storageService = storageService;
            _encryptionService = encryptionService;
        }

        public async Task<DownloadResponse> Handle(DownloadRequest request, CancellationToken cancellationToken)
        {
            DownloadResponse result;

            try
            {
                var encryptedResult = await _storageService.Retrieve(request.ApplicationId.ToString(), request.SequenceId, request.SectionId, request.PageId, request.QuestionId, request.Filename);

                var decrypted = await _encryptionService.Decrypt(encryptedResult.Item2);

                result = new DownloadResponse() { Filename = encryptedResult.Item1, FileStream = decrypted, ContentType = encryptedResult.Item3 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to retrieve file: {request.Filename} for application: {request.ApplicationId} - question: {request.QuestionId} ");
                result = new DownloadResponse() { Filename = request.Filename, FileStream = new MemoryStream(), ContentType = string.Empty };
            }

            return result;
        }
    }
}