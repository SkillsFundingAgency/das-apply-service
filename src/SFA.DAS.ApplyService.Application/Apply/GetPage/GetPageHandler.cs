using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.GetPage
{
    public class GetPageHandler : IRequestHandler<GetPageRequest, Page> 
    {
        private readonly IApplyRepository _applyRepository;

        public GetPageHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<Page> Handle(GetPageRequest request, CancellationToken cancellationToken)
        {
            var application = await _applyRepository.GetEntity(request.ApplicationId, request.UserId);
            if (application == null)
            {
                throw new BadRequestException("Application not found");
            }
            var sequence = application.QnAWorkflow.Sequences.Single(w => w.Sections.Any(s => s.Pages.Any(p => p.PageId == request.PageId)));
            var section = sequence.Sections.Single(s => s.Pages.Any(p => p.PageId == request.PageId));

            if (!sequence.Active)
            {
                throw new UnauthorisedException("Sequence not active");
            }

            var page = section.Pages.Single(p => p.PageId == request.PageId);

            return page;
        }
    }
}