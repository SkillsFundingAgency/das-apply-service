using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Financial.Applications
{
    public class OpenFinancialApplicationsForDownloadRequestHandler : IRequestHandler<
        OpenFinancialApplicationsForDownloadRequest, List<RoatpFinancialSummaryDownloadItem>>
    {
        private readonly IApplyRepository _repository;

        public OpenFinancialApplicationsForDownloadRequestHandler(IApplyRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RoatpFinancialSummaryDownloadItem>> Handle(OpenFinancialApplicationsForDownloadRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetOpenFinancialApplicationsForDownload();
        }
    }
}