using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class GetOversightDownloadHandler : IRequestHandler<GetOversightDownloadRequest, List<ApplicationOversightDownloadDetails>>
    {
        private readonly IApplyRepository _applyRepository;

        public GetOversightDownloadHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<List<ApplicationOversightDownloadDetails>> Handle(GetOversightDownloadRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetOversightsForDownload(request.DateFrom, request.DateTo);
        }
    }
}