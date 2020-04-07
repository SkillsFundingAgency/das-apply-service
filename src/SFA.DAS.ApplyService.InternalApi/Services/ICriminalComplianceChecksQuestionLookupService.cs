using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
using System;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public interface ICriminalComplianceChecksQuestionLookupService
    {
        QnaQuestionDetails GetQuestionDetailsForGatewayPageId(Guid applicationId, string gatewayPageId);
    }
}
