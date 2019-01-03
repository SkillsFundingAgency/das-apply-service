using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Review.CompleteSection
{
    public class CompleteSectionHandler : IRequestHandler<CompleteSectionRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public CompleteSectionHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<Unit> Handle(CompleteSectionRequest request, CancellationToken cancellationToken)
        {
            var section = await _applyRepository.GetSection(request.ApplicationId, request.SequenceId, request.SectionId, null);

            if (request.IsSectionComplete)
            {
                section.Status = ApplicationSectionStatus.Completed;
            }
            else if (request.SequenceId == 1 && request.SectionId == 3)
            {
                section.Status = ApplicationSectionStatus.Graded;
            }
            else
            {
                section.Status = ApplicationSectionStatus.InProgress;
            }

            section.FeedbackComment = request.FeedbackComment;

            await _applyRepository.CompleteSection(section);

            return Unit.Value;
        }
    }
}
