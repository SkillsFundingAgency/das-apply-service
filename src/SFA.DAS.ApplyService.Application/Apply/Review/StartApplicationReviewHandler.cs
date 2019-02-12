using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Interfaces;

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
            if (request.SequenceId == 1)
            {
                var section1 = await _applyRepository.GetSection(request.ApplicationId, 1, 1, null);
                var section2 = await _applyRepository.GetSection(request.ApplicationId, 1, 2, null);

                if (section1.Status == ApplicationSectionStatus.Submitted)
                {
                    await _applyRepository.StartApplicationReview(request.ApplicationId, section1.SectionId);
                }

                if (section2.Status == ApplicationSectionStatus.Submitted)
                {
                    await _applyRepository.StartApplicationReview(request.ApplicationId, section2.SectionId);
                }
            }

            return Unit.Value;
        }
    }
}
