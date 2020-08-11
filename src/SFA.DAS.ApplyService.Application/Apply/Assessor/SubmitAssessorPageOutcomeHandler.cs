using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class SubmitAssessorPageOutcomeHandler : IRequestHandler<SubmitAssessorPageOutcomeRequest>
    {
        private readonly IAssessorRepository _repository;
        private readonly ILogger<SubmitAssessorPageOutcomeHandler> _logger;

        public SubmitAssessorPageOutcomeHandler(IAssessorRepository repository, ILogger<SubmitAssessorPageOutcomeHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(SubmitAssessorPageOutcomeRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"SubmitAssessorPageOutcome for ApplicationId '{request.ApplicationId}' - " +
                                                    $"SequenceNumber '{request.SequenceNumber}' - SectionNumber '{request.SectionNumber}' - PageId '{request.PageId}' - " +
                                                    $"AssessorType '{request.AssessorType}' - UserId '{request.UserId}' - " +
                                                    $"Status '{request.Status}' - Comment '{request.Comment}'");

            await _repository.SubmitAssessorPageOutcome(request.ApplicationId, 
                                                        request.SequenceNumber, 
                                                        request.SectionNumber, 
                                                        request.PageId, 
                                                        request.AssessorType, 
                                                        request.UserId, 
                                                        request.Status, 
                                                        request.Comment);

            return Unit.Value;
        }
    }
}
