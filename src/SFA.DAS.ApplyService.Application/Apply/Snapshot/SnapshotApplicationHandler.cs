using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Snapshot
{
    public class SnapshotApplicationHandler : IRequestHandler<SnapshotApplicationRequest, Guid>
    {
        private readonly IApplyRepository _applyRepository;

        public SnapshotApplicationHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<Guid> Handle(SnapshotApplicationRequest request, CancellationToken cancellationToken)
        {
            var snapshotApplicationId = Guid.Empty;

            var application = await _applyRepository.GetApplication(request.ApplicationId);

            if (application != null && request.SnapshotApplicationId != Guid.Empty)
            {
                snapshotApplicationId = await _applyRepository.SnapshotApplication(request.ApplicationId, request.SnapshotApplicationId, request.Sequences);
            }

            return snapshotApplicationId;
        }
    }
}
