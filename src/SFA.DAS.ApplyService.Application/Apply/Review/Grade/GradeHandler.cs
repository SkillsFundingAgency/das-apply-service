using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Grade
{
    public class GradeHandler : IRequestHandler<GradeRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public GradeHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<Unit> Handle(GradeRequest request, CancellationToken cancellationToken)
        {
            var section = await _applyRepository.GetSection(request.ApplicationId, request.SequenceId, request.SectionId, null);

            if(request.IsSectionComplete)
            {
                section.Status = ApplicationSectionStatus.Graded;
            }
            else
            {
                section.Status = ApplicationSectionStatus.InProgress;
            }

            section.FeedbackComment = request.FeedbackComment;

            await _applyRepository.GradeSection(section);

            return Unit.Value;
        }
    }
}