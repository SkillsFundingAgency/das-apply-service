using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetContactForApplicationHandler : IRequestHandler<GetContactForApplicationRequest, Domain.Entities.Contact>
    {
        private readonly IApplyRepository _applyRepository;

        public GetContactForApplicationHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<Domain.Entities.Contact> Handle(GetContactForApplicationRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetContactForApplication(request.ApplicationId);
        }
    }
}