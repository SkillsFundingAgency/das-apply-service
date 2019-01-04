using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Evaluate
{
    public class EvaluateHandler : IRequestHandler<EvaluateRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public EvaluateHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<Unit> Handle(EvaluateRequest request, CancellationToken cancellationToken)
        {
            var section = await _applyRepository.GetSection(request.ApplicationId, request.SequenceId, request.SectionId, null);

            if (request.IsSectionComplete)
            {
                section.Status = ApplicationSectionStatus.Evaluated;
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
