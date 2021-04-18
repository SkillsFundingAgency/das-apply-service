using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{

    public class AddSubcontractorDeclarationFileUploadHandler : IRequestHandler<AddSubcontractorDeclarationFileUploadRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IGatewayRepository _gatewayRepository;

        private readonly ILogger<AddSubcontractorDeclarationFileUploadHandler> _logger;

        public AddSubcontractorDeclarationFileUploadHandler(IApplyRepository applyRepository, IGatewayRepository gatewayRepository, ILogger<AddSubcontractorDeclarationFileUploadHandler> logger)
        {
            _applyRepository = applyRepository;
            _gatewayRepository = gatewayRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(AddSubcontractorDeclarationFileUploadRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.FileName)) return false;
            _logger.LogInformation($"Adding subcontractor declaration clarification file [{request.FileName}] for application ID {request.ApplicationId}");

            var application = await _applyRepository.GetApplication(request.ApplicationId);
            var gatewayReviewDetails = application.ApplyData.GatewayReviewDetails;
            gatewayReviewDetails.GatewaySubcontractorDeclarationClarificationUpload = request.FileName;
            application.ApplyData.GatewayReviewDetails = gatewayReviewDetails;

            return await _gatewayRepository.UpdateGatewayApplyData(request.ApplicationId, application.ApplyData, request.UserId, request.UserName);
        }
    }
}
