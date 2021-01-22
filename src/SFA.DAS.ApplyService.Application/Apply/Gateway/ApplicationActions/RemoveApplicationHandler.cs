using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway.ApplicationActions
{
    public class RemoveApplicationHandler : IRequestHandler<RemoveApplicationRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly ILogger<RemoveApplicationHandler> _logger;

        public RemoveApplicationHandler(IApplyRepository applyRepository, ILogger<RemoveApplicationHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(RemoveApplicationRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Performing Remove Application action for ApplicationId: {request.ApplicationId}");

            return await _applyRepository.RemoveApplication(request.ApplicationId, request.Comments, request.ExternalComments, request.UserId, request.UserName);
        }
    }
}
