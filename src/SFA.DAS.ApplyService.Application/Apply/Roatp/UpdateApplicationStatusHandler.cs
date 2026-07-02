using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class UpdateApplicationStatusHandler : IRequestHandler<UpdateApplicationStatusRequest, bool>
    {
        private ILogger<UpdateApplicationStatusHandler> _logger;
        private readonly IApplyRepository _repository;

        public UpdateApplicationStatusHandler(ILogger<UpdateApplicationStatusHandler> logger, IApplyRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateApplicationStatusRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating application status to {ApplicationStatus} for application ID {ApplicationId}", request.ApplicationStatus, request.ApplicationId);

            try
            {
                await _repository.UpdateApplicationStatus(request.ApplicationId, request.ApplicationStatus, request.UserId);
            }
            catch (Exception updateException)
            {
                _logger.LogError(updateException, "Updating application status failed for application ID {ApplicationId}", request.ApplicationId);
            }

            return true;
        }
    }
}
