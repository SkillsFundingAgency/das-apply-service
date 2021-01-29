using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class AssignAssessorHandler : IRequestHandler<AssignAssessorRequest>
    {
        private readonly IAssessorRepository _assessorRepository;
        private readonly ILogger<AssignAssessorHandler> _logger;

        public AssignAssessorHandler(IAssessorRepository assessorRepository, ILogger<AssignAssessorHandler> logger)
        {
            _assessorRepository = assessorRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(AssignAssessorRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Assigning assessor for application {request.ApplicationId}");

            if (request.AssessorNumber == 1)
            {
                await _assessorRepository.AssignAssessor1(request.ApplicationId, request.AssessorUserId, request.AssessorName);
            }
            else
            {
                await _assessorRepository.AssignAssessor2(request.ApplicationId, request.AssessorUserId, request.AssessorName);
            }

            return Unit.Value;
        }
    }
}
