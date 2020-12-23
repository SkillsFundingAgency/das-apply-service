using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{

    public class AddSubcontractorDeclarationFileUploadHandler : IRequestHandler<AddSubcontractorDeclarationFileUploadRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;

        private readonly ILogger<AddSubcontractorDeclarationFileUploadHandler> _logger;

        public AddSubcontractorDeclarationFileUploadHandler(IApplyRepository applyRepository, ILogger<AddSubcontractorDeclarationFileUploadHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(AddSubcontractorDeclarationFileUploadRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Adding subcontractor declaration clarification file [{request.FileName}] for application ID {request.ApplicationId}");
            var application = await _applyRepository.GetApplication(request.ApplicationId);

            var gatewayReviewDetails = application.ApplyData.GatewayReviewDetails;
            gatewayReviewDetails.GatewaySubcontractorDeclarationClarificationUpload = request.FileName;
            application.ApplyData.GatewayReviewDetails = gatewayReviewDetails;
            return await _applyRepository.UpdateGatewayApplyData(request.ApplicationId, application.ApplyData, request.UserId, request.UserName);
        }
    }
}
