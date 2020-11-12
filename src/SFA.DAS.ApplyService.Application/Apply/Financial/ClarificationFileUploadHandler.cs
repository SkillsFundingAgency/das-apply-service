using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class ClarificationFileUploadHandler : IRequestHandler<ClarificationFileUploadRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;

        private readonly ILogger<ClarificationFileUploadHandler> _logger;

        public ClarificationFileUploadHandler(IApplyRepository applyRepository, ILogger<ClarificationFileUploadHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(ClarificationFileUploadRequest request, CancellationToken cancellationToken)
        {

            // go get the current details
            // update the financialGrade stuff
            // update the apply record
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
