using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class GetPageReviewOutcomeHandler : IRequestHandler<GetPageReviewOutcomeRequest, PageReviewOutcome>
    {
        private readonly IApplyRepository _repository;
        private readonly ILogger<GetPageReviewOutcomeHandler> _logger;

        public GetPageReviewOutcomeHandler(IApplyRepository repository, ILogger<GetPageReviewOutcomeHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<PageReviewOutcome> Handle(GetPageReviewOutcomeRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetPageReviewOutcome for ApplicationId '{request.ApplicationId}' - " +
                                                    $"SequenceNumber '{request.SequenceNumber}' - SectionNumber '{request.SectionNumber}' - PageId '{request.PageId}' - " +
                                                    $"AssessorType '{request.AssessorType}' - UserId '{request.UserId}'");

            var pageReviewOutcome = await _repository.GetPageReviewOutcome(request.ApplicationId,
                                                        request.SequenceNumber,
                                                        request.SectionNumber,
                                                        request.PageId,
                                                        request.AssessorType,
                                                        request.UserId);

            return pageReviewOutcome;
        }
    }
}
