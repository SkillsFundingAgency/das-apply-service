using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class StartFinancialReviewHandler : IRequestHandler<StartFinancialReviewRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public StartFinancialReviewHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<Unit> Handle(StartFinancialReviewRequest request, CancellationToken cancellationToken)
        {
            var section = await _applyRepository.GetSection(request.ApplicationId, 1, 3, null);

            if (section.Status == SectionStatus.Submitted)
            {
                await _applyRepository.StartFinancialReview(request.ApplicationId);   
            }
            
            return Unit.Value;
        }
    }
}