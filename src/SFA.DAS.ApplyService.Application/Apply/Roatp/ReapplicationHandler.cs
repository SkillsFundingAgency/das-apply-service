using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class ReapplicationHandler : IRequestHandler<ReapplicationRequest, bool>
    {
        private ILogger<ReapplicationHandler> _logger;
        private readonly IApplyRepository _repository;

        public ReapplicationHandler(ILogger<ReapplicationHandler> logger, IApplyRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<bool> Handle(ReapplicationRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Setting reapplication status for Application ID {request.ApplicationId}");

            return await _repository.SubmitReapplicationRequest(request.ApplicationId, request.UserId);
        }
    }
}
