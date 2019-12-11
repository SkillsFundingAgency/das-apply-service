using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class GetApplicationDataHandler : IRequestHandler<GetApplicationDataRequest, RoatpApplicationData>
    {
        private readonly IApplyRepository _repository;
        private readonly ILogger<GetApplicationDataHandler> _logger;

        public GetApplicationDataHandler(IApplyRepository repository, ILogger<GetApplicationDataHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<RoatpApplicationData> Handle(GetApplicationDataRequest request, CancellationToken cancellationToken)
        {
            var applicationData = await _repository.GetRoatpApplicationData(request.ApplicationId);

            return await Task.FromResult(applicationData);
        }
    }
}
