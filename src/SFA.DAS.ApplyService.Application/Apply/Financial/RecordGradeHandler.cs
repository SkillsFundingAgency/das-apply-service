using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class RecordGradeHandler : IRequestHandler<RecordGradeRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;

        private readonly ILogger<RecordGradeHandler> _logger;

        public RecordGradeHandler(IApplyRepository applyRepository, ILogger<RecordGradeHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(RecordGradeRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Recording financial grade {request.FinancialReviewDetails.SelectedGrade} for application ID {request.ApplicationId}");
            return await _applyRepository.RecordFinancialGrade(request.ApplicationId, request.FinancialReviewDetails, request.FinancialReviewStatus);
        }
    }
}
