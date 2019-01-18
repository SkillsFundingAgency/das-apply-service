using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Feedback
{
    public class AddFeedbackHandler : IRequestHandler<AddFeedbackRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public AddFeedbackHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<Unit> Handle(AddFeedbackRequest request, CancellationToken cancellationToken)
        {
            var section = await _applyRepository.GetSection(request.ApplicationId, request.SequenceId, request.SectionId, null);

            var page = section.GetPage(request.PageId);

            if (page.Feedback == null)
            {
                page.Feedback = new List<Domain.Apply.Feedback>();
            }

            request.Feedback.Id = Guid.NewGuid();
            request.Feedback.IsNew = true;
            request.Feedback.IsCompleted = false;

            page.Feedback.Add(request.Feedback);

            section.UpdatePage(page);
            
            await _applyRepository.UpdateSections(new List<ApplicationSection> {section});
            
            return Unit.Value;
        }
    }
}