using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{

    public class GetOversightDetailsHandler : IRequestHandler<GetOversightDetailsRequest, ApplicationOversightDetails>
    {
        private readonly IApplyRepository _applyRepository;

        public GetOversightDetailsHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<ApplicationOversightDetails> Handle(GetOversightDetailsRequest request,
            CancellationToken cancellationToken)
        {
            return await _applyRepository.GetOversightDetails(request.ApplicationId);
        }
    }
}