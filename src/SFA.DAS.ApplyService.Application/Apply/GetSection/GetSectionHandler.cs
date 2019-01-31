using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.GetPage;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.GetSection
{
    public class GetSectionHandler : IRequestHandler<GetSectionRequest, ApplicationSection> 
    {
        private readonly IApplyRepository _applyRepository;

        public GetSectionHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<ApplicationSection> Handle(GetSectionRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetSection(request.ApplicationId, request.SequenceId, request.SectionId, request.UserId);
        }
    }
}