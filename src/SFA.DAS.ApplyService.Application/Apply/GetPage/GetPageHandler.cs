using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
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
            Page page = null;

            var section = await _applyRepository.GetSection(request.ApplicationId, request.SequenceId, request.SectionId, request.UserId);
            if (section != null)
            {
                page = section.QnAData.Pages.SingleOrDefault(p => p.PageId == request.PageId);

                if(page != null) page.DisplayType = section.DisplayType;
            }

            return page;
        }
    }
}