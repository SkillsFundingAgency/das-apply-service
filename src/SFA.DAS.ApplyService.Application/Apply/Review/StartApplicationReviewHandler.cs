using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Review
{
    public class StartApplicationReviewHandler : IRequestHandler<StartApplicationReviewRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public StartApplicationReviewHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<Unit> Handle(StartApplicationReviewRequest request, CancellationToken cancellationToken)
        {
            await _applyRepository.UpdateSequenceStatus(request.ApplicationId, request.SequenceId, ApplicationSequenceStatus.InProgress, ApplicationStatus.InProgress);

            if (request.SequenceId == 1)
            {
                var appSections = await _applyRepository.GetSections(request.ApplicationId);
                var sequenceSections = appSections.Where(sec => sec.SequenceId == request.SequenceId);

                foreach (var section in sequenceSections)
                {
                    if (section.Status == SectionStatus.Submitted)
                    {
                        await _applyRepository.StartApplicationReview(request.ApplicationId, section.SectionId);
                    }
                }
            }

            return Unit.Value;
        }
    }
}
