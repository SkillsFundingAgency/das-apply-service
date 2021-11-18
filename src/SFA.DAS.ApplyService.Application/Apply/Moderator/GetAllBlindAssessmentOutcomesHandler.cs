using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class GetAllBlindAssessmentOutcomesHandler : IRequestHandler<GetAllBlindAssessmentOutcomesRequest, List<BlindAssessmentOutcome>>
    {
        private readonly IModeratorRepository _repository;
        private readonly ILogger<GetAllBlindAssessmentOutcomesHandler> _logger;

        public GetAllBlindAssessmentOutcomesHandler(IModeratorRepository repository, ILogger<GetAllBlindAssessmentOutcomesHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<BlindAssessmentOutcome>> Handle(GetAllBlindAssessmentOutcomesRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetAllBlindAssessmentOutcome for ApplicationId '{request.ApplicationId}");

            var blindAssessmentOutcome = await _repository.GetAllBlindAssessmentOutcome(request.ApplicationId);

            return blindAssessmentOutcome;
        }
    }
}
