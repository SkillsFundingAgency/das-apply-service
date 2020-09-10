using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class UpdateNotRequiredOverridesHandler : IRequestHandler<UpdateNotRequiredOverridesRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly ILogger<UpdateNotRequiredOverridesHandler> _logger;

        public UpdateNotRequiredOverridesHandler(IApplyRepository applyRepository, ILogger<UpdateNotRequiredOverridesHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateNotRequiredOverridesRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Updating NotRequiredOverrides for applicationId {request.ApplicationId} | Count [{request.NotRequiredOverrides?.NotRequiredOverrides.Count()}]");
            var result = await _applyRepository.SaveNotRequiredOverrides(request.ApplicationId, request.NotRequiredOverrides);
            _logger.LogInformation($"Result of updating NotRequiredOverrides for applicationId {request.ApplicationId} | Result {result}");

            return result;
        }
    }
}
