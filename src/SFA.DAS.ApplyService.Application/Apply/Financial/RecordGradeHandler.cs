using System;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
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

            var financialReviewStatus = GetApplicableFinancialReviewStatus(request.FinancialReviewDetails.SelectedGrade);
            var financialReviewDetails = request.FinancialReviewDetails;
            if (financialReviewDetails.SelectedGrade == FinancialApplicationSelectedGrade.Clarification)
            {
                financialReviewDetails.ClarificationRequestedOn = financialReviewDetails.GradedDateTime;
                financialReviewDetails.ClarificationRequestedBy = financialReviewDetails.GradedBy;
            }

            return await _applyRepository.RecordFinancialGrade(request.ApplicationId, financialReviewDetails, financialReviewStatus);
        }

        private static string GetApplicableFinancialReviewStatus(string financialGrade)
        {
            switch (financialGrade)
            {
                case FinancialApplicationSelectedGrade.Outstanding:
                case FinancialApplicationSelectedGrade.Good:
                case FinancialApplicationSelectedGrade.Satisfactory:
                    return FinancialReviewStatus.Pass;

                case FinancialApplicationSelectedGrade.Exempt:
                    return FinancialReviewStatus.Exempt;

                case FinancialApplicationSelectedGrade.Clarification:
                    return FinancialReviewStatus.ClarificationSent;

                default:
                    return FinancialReviewStatus.Fail;
            }
        }
    }
}
