using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class AssignAssessorHandler : IRequestHandler<AssignAssessorRequest>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly ILogger<AssignAssessorHandler> _logger;

        public AssignAssessorHandler(IApplyRepository applyRepository, ILogger<AssignAssessorHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(AssignAssessorRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Assigning assessor for application {request.ApplicationId}");

            if (request.AssessorNumber == 1)
            {
                await _applyRepository.UpdateAssessor1(request.ApplicationId, request.AssessorUserId, request.AssessorName);
            }
            else
            {
                await _applyRepository.UpdateAssessor2(request.ApplicationId, request.AssessorUserId, request.AssessorName);
            }

            return Unit.Value;
        }
    }
}
