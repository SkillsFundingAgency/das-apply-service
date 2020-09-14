﻿using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class SubmitModeratorPageOutcomeHandler : IRequestHandler<SubmitModeratorPageOutcomeRequest>
    {
        private readonly IAssessorRepository _repository;
        private readonly ILogger<SubmitModeratorPageOutcomeHandler> _logger;

        public SubmitModeratorPageOutcomeHandler(IAssessorRepository repository, ILogger<SubmitModeratorPageOutcomeHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(SubmitModeratorPageOutcomeRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"SubmitModeratorPageOutcome for ApplicationId '{request.ApplicationId}' - PageId '{request.PageId}' - Status '{request.Status}'");

            await _repository.SubmitModeratorPageOutcome(request.ApplicationId, 
                                                        request.SequenceNumber, 
                                                        request.SectionNumber, 
                                                        request.PageId,  
                                                        request.UserId, 
                                                        request.Status, 
                                                        request.Comment,
                                                        request.ExternalComment);

            return Unit.Value;
        }
    }
}