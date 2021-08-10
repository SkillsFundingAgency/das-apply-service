using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetFinancialReviewDetailsHandler : IRequestHandler<GetFinancialReviewDetailsRequest, FinancialReviewDetails>
    {
        private readonly IApplyRepository _applyRepository;

        public GetFinancialReviewDetailsHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<FinancialReviewDetails> Handle(GetFinancialReviewDetailsRequest request, CancellationToken cancellationToken)
        {
            var financialReviewDetails= await _applyRepository.GetFinancialReviewDetails(request.ApplicationId);

            if (financialReviewDetails!=null)
                financialReviewDetails.ClarificationFiles =
                    await _applyRepository.GetFinancialReviewClarificationFiles(request.ApplicationId);

            return financialReviewDetails;
        }
    }
}