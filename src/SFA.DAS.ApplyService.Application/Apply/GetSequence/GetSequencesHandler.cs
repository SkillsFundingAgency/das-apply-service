using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.GetSequence
{
    public class GetSequencesHandler : IRequestHandler<GetSequencesRequest, IEnumerable<ApplicationSequence>>
    {
        private readonly IApplyRepository _applyRepository;

        public GetSequencesHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<IEnumerable<ApplicationSequence>> Handle(GetSequencesRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetSequences(request.ApplicationId);
        }
    }
}
