using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class AssessorEvaluateSectionHandler : IRequestHandler<AssessorEvaluateSectionRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly ILogger<AssessorEvaluateSectionHandler> _logger;

        public AssessorEvaluateSectionHandler(IApplyRepository applyRepository, ILogger<AssessorEvaluateSectionHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(AssessorEvaluateSectionRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Assessor evaluation of complete = {request.SectionCompleted} for application {request.ApplicationId} sequence {request.SequenceId} section {request.SectionId}");

            return await _applyRepository.AssessorEvaluateSection(request.ApplicationId, request.SequenceId, request.SectionId,
                                                                  request.SectionCompleted, request.Reviewer);
        }
    }
}
