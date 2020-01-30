using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetFeedbackAddedApplicationsHandler : IRequestHandler<GetFeedbackAddedApplicationsRequest, IEnumerable<Domain.Entities.Apply>>
    {
        private readonly IApplyRepository _applyRepository;
        public GetFeedbackAddedApplicationsHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<IEnumerable<Domain.Entities.Apply>> Handle(GetFeedbackAddedApplicationsRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetFeedbackAddedApplications();
        }
    }
}
