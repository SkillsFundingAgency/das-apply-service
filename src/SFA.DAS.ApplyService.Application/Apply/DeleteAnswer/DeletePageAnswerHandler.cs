using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.DeleteAnswer
{
    public class DeletePageAnswerHandler : IRequestHandler<DeletePageAnswerRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public DeletePageAnswerHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<Unit> Handle(DeletePageAnswerRequest request, CancellationToken cancellationToken)
        {
            var section = await _applyRepository.GetSection(request.ApplicationId, request.SequenceId, request.SectionId,
                request.UserId);

            var pages = section.QnADataObject.Pages;

            var page = pages.Single(p => p.PageId == request.PageId);

            var answerPage = page.PageOfAnswers.Single(poa => poa.Id == request.AnswerId);

            page.PageOfAnswers.Remove(answerPage);

            section.QnADataObject.Pages = pages;

            await _applyRepository.UpdateSections(new List<ApplicationSection> {section});
            
            return Unit.Value;
        }
    }
}