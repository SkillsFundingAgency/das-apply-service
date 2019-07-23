using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Download
{
    public class FileInfoHandler : IRequestHandler<FileInfoRequest, FileInfoResponse>
    {
        private readonly ILogger<FileInfoHandler> _logger;
        private readonly IStorageService _storageService;
        private readonly IEncryptionService _encryptionService;

        public FileInfoHandler(ILogger<FileInfoHandler> logger, IStorageService storageService, IEncryptionService encryptionService)
        {
            _logger = logger;
            _storageService = storageService;
            _encryptionService = encryptionService;
        }

        public async Task<FileInfoResponse> Handle(FileInfoRequest request, CancellationToken cancellationToken)
        {
            FileInfoResponse result;

            try
            {
                var encryptedResult = await _storageService.Retrieve(request.ApplicationId.ToString(), request.SequenceId, request.SectionId, request.PageId, request.QuestionId, request.Filename);
                result = new FileInfoResponse() { Filename = encryptedResult.Item1, ContentType = encryptedResult.Item3 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to retrieve file info: {request.Filename} for application: {request.ApplicationId} - question: {request.QuestionId} ");
                result = new FileInfoResponse() { Filename = request.Filename, ContentType = string.Empty };
            }

            return result;
        }
    }
}