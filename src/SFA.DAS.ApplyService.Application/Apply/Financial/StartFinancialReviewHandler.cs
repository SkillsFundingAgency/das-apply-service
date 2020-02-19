using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class StartFinancialReviewHandler : IRequestHandler<StartFinancialReviewRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly ILogger<StartFinancialReviewHandler> _logger;

        public StartFinancialReviewHandler(IApplyRepository applyRepository, ILogger<StartFinancialReviewHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }
        
        public async Task<bool> Handle(StartFinancialReviewRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting financial review for application {request.ApplicationId}");
            await _applyRepository.StartFinancialReview(request.ApplicationId, request.Reviewer);   
                        
            return await Task.FromResult(true);
        }
    }
}