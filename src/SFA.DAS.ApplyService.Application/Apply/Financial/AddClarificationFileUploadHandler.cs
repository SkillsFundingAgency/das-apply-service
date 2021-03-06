﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class AddClarificationFileUploadHandler : IRequestHandler<AddClarificationFileUploadRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;

        private readonly ILogger<AddClarificationFileUploadHandler> _logger;

        public AddClarificationFileUploadHandler(IApplyRepository applyRepository, ILogger<AddClarificationFileUploadHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(AddClarificationFileUploadRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Adding clarification file [{request.FileName}] for application ID {request.ApplicationId}");
            var application = await _applyRepository.GetApplication(request.ApplicationId);

            var financialGrade = application.FinancialGrade;
            if (financialGrade.ClarificationFiles == null)
                financialGrade.ClarificationFiles = new List<ClarificationFile>();

            financialGrade.ClarificationFiles.Add(new ClarificationFile {Filename = request.FileName});
            return await _applyRepository.UpdateFinancialReviewDetails(request.ApplicationId, financialGrade);
        }
    }
}
