﻿using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class GetAllModeratorPageReviewOutcomesHandler : IRequestHandler<GetAllModeratorPageReviewOutcomesRequest, List<ModeratorPageReviewOutcome>>
    {
        private readonly IModeratorRepository _repository;
        private readonly ILogger<GetAllModeratorPageReviewOutcomesHandler> _logger;

        public GetAllModeratorPageReviewOutcomesHandler(IModeratorRepository repository, ILogger<GetAllModeratorPageReviewOutcomesHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<ModeratorPageReviewOutcome>> Handle(GetAllModeratorPageReviewOutcomesRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetModeratorAssessorPageReviewOutcomes for ApplicationId '{request.ApplicationId}'");

            var moderatorPageReviewOutcomes = await _repository.GetAllModeratorPageReviewOutcomes(request.ApplicationId);

            return moderatorPageReviewOutcomes;
        }
    }
}
