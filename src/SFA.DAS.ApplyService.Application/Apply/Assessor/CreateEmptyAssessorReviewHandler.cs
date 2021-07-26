using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class CreateEmptyAssessorReviewHandler : IRequestHandler<CreateEmptyAssessorReviewRequest>
    {
        private readonly IAssessorRepository _repository;
        private readonly ILogger<CreateEmptyAssessorReviewHandler> _logger;

        public CreateEmptyAssessorReviewHandler(IAssessorRepository repository, ILogger<CreateEmptyAssessorReviewHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateEmptyAssessorReviewRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CreateEmptyAssessorReview for ApplicationId '{request.ApplicationId}'");
            await _repository.CreateEmptyAssessorReview(request.ApplicationId, request.AssessorUserId, request.AssessorUserName, request.PageReviewOutcomes);
            return Unit.Value;
        }
    }
}
