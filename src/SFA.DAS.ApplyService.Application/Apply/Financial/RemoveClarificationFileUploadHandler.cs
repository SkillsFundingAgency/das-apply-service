﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class RemoveClarificationFileUploadHandler : IRequestHandler<RemoveClarificationFileUploadRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;

        private readonly ILogger<RemoveClarificationFileUploadHandler> _logger;

        public RemoveClarificationFileUploadHandler(IApplyRepository applyRepository, ILogger<RemoveClarificationFileUploadHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(RemoveClarificationFileUploadRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Removing clarification file [{request.FileName}] for application ID {request.ApplicationId}");
            return await _applyRepository.RemoveFinancialReviewClarificationFile(request.ApplicationId, request.FileName);
        }
    }
}