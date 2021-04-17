using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class RemoveSubcontractorDeclarationFileHandler : IRequestHandler<RemoveSubcontractorDeclarationFileRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IGatewayRepository _gatewayRepository;

        private readonly ILogger<RemoveSubcontractorDeclarationFileHandler> _logger;

        public RemoveSubcontractorDeclarationFileHandler(IApplyRepository applyRepository, IGatewayRepository gatewayRepository, ILogger<RemoveSubcontractorDeclarationFileHandler> logger)
        {
            _applyRepository = applyRepository;
            _gatewayRepository = gatewayRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(RemoveSubcontractorDeclarationFileRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Removing subcontractor declaration clarification file [{request.FileName}] for application ID {request.ApplicationId}");
            var application = await _applyRepository.GetApplication(request.ApplicationId);

            var gatewayReviewDetails = application.ApplyData.GatewayReviewDetails;
            if (gatewayReviewDetails.GatewaySubcontractorDeclarationClarificationUpload == request.FileName)
            {
                gatewayReviewDetails.GatewaySubcontractorDeclarationClarificationUpload = null;

                application.ApplyData.GatewayReviewDetails = gatewayReviewDetails;
                return await _gatewayRepository.UpdateGatewayApplyData(request.ApplicationId, application.ApplyData, request.UserId, request.UserName);
            }
            else
            {
                _logger.LogError($"Removing subcontractor declaration clarification file [{request.FileName}] for application ID {request.ApplicationId} failed as filename could not be matched");
                return false;
            }
        }
    }
}