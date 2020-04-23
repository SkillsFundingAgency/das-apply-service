using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class ChangeProviderRouteHandler : IRequestHandler<ChangeProviderRouteRequest, bool>
    {
        private ILogger<ChangeProviderRouteHandler> _logger;
        private readonly IApplyRepository _repository;

        public ChangeProviderRouteHandler(ILogger<ChangeProviderRouteHandler> logger, IApplyRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<bool> Handle(ChangeProviderRouteRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Changing ProviderRoute to {request.ProviderRoute} for Application ID {request.ApplicationId}");

            return await _repository.ChangeProviderRoute(request.ApplicationId, request.ProviderRoute, request.ProviderRouteName);
        }
    }
}
