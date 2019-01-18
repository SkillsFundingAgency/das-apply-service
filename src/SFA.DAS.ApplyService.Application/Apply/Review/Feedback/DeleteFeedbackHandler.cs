using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Feedback
{
    public class DeleteFeedbackHandler : IRequestHandler<DeleteFeedbackRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public DeleteFeedbackHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<Unit> Handle(DeleteFeedbackRequest request, CancellationToken cancellationToken)
        {
            var section = await _applyRepository.GetSection(request.ApplicationId, request.SequenceId, request.SectionId, null);

            var page = section.GetPage(request.PageId);

            if (page.Feedback == null)
            {
                page.Feedback = new List<Domain.Apply.Feedback>();
            }

            var feedbackToRemove = page.Feedback.Find(f => f.Id == request.FeedbackId);
           
            page.Feedback.Remove(feedbackToRemove);

            section.UpdatePage(page);
            
            await _applyRepository.UpdateSections(new List<ApplicationSection> {section});
            
            return Unit.Value;
        }
    }
}