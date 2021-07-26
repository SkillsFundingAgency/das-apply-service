using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Roatp;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class GetExistingApplicationStatusHandler : IRequestHandler<GetExistingApplicationStatusRequest, IEnumerable<RoatpApplicationStatus>>
    {
        private IApplyRepository _repository;
        private ILogger<GetExistingApplicationStatusHandler> _logger;

        public GetExistingApplicationStatusHandler(IApplyRepository repository, ILogger<GetExistingApplicationStatusHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<RoatpApplicationStatus>> Handle(GetExistingApplicationStatusRequest request, CancellationToken cancellationToken)
        {
            var applicationStatuses = await _repository.GetExistingApplicationStatusByUkprn(request.UKPRN);

            return await Task.FromResult(applicationStatuses);
        }
    }
}
