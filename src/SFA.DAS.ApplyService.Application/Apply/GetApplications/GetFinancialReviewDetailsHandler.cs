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
            return await _applyRepository.GetFinancialReviewDetails(request.ApplicationId);
        }
    }
}