using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway.ApplicationActions
{
    public class RemoveApplicationHandler : IRequestHandler<RemoveApplicationRequest, bool>
    {
        private readonly IGatewayRepository _repository;
        private readonly ILogger<RemoveApplicationHandler> _logger;

        public RemoveApplicationHandler(IGatewayRepository repository, ILogger<RemoveApplicationHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(RemoveApplicationRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Performing Remove Application action for ApplicationId: {request.ApplicationId}");

            return await _repository.RemoveApplication(request.ApplicationId, request.Comments, request.ExternalComments, request.UserId, request.UserName);
        }
    }
}
