using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetApplicationsByUkprnHandler : IRequestHandler<GetApplicationsByUkprnRequest, List<Domain.Entities.Apply>>
    {
        private readonly IApplyRepository _applyRepository;

        public GetApplicationsByUkprnHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<List<Domain.Entities.Apply>> Handle(GetApplicationsByUkprnRequest request, CancellationToken cancellationToken)
        {

            return await _applyRepository.GetApplicationsByUkprn(request.Ukprn);
        }
    }
}