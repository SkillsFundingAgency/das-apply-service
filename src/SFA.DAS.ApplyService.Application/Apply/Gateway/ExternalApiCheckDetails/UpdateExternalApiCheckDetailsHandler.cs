using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway.ExternalApiCheckDetails
{
    public class UpdateExternalApiCheckDetailsHandler : IRequestHandler<UpdateExternalApiCheckDetailsRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;

        public UpdateExternalApiCheckDetailsHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<bool> Handle(UpdateExternalApiCheckDetailsRequest request, CancellationToken cancellationToken)
        {
            var application = await _applyRepository.GetApplication(request.ApplicationId);

            if (application is null || request.ApplyGatewayDetails is null) return false;

            if (application.ApplyData.GatewayReviewDetails is null)
            {
                application.ApplyData.GatewayReviewDetails = new ApplyGatewayDetails();
            }

            application.ApplyData.GatewayReviewDetails.UkrlpDetails             = request.ApplyGatewayDetails.UkrlpDetails;
            application.ApplyData.GatewayReviewDetails.RoatpRegisterDetails     = request.ApplyGatewayDetails.RoatpRegisterDetails;
            application.ApplyData.GatewayReviewDetails.CompaniesHouseDetails    = request.ApplyGatewayDetails.CompaniesHouseDetails;
            application.ApplyData.GatewayReviewDetails.CharityCommissionDetails = request.ApplyGatewayDetails.CharityCommissionDetails;
            application.ApplyData.GatewayReviewDetails.SourcesCheckedOn         = request.ApplyGatewayDetails.SourcesCheckedOn;

            return await _applyRepository.UpdateGatewayApplyData(request.ApplicationId, application.ApplyData, request.UserId, request.UserName);
        }
    }
}
