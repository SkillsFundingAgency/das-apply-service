using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class GetNotRequiredOverridesHandler : IRequestHandler<GetNotRequiredOverridesRequest, NotRequiredOverrideConfiguration>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly ILogger<GetNotRequiredOverridesHandler> _logger;

        public GetNotRequiredOverridesHandler(IApplyRepository applyRepository, ILogger<GetNotRequiredOverridesHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<NotRequiredOverrideConfiguration> Handle(GetNotRequiredOverridesRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Getting NotRequiredOverrides for applicationId {request.ApplicationId}");
            var configuration = await _applyRepository.GetNotRequiredOverrides(request.ApplicationId);
            _logger.LogInformation($"NotRequiredOverrides configuration returned for applicationId {request.ApplicationId} | Result count [{configuration?.NotRequiredOverrides.Count()}]");
            return configuration;
        }
    }
}
