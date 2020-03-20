using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public interface ICriminalComplianceChecksQuestionLookupService
    {
        QnaQuestionDetails GetQuestionDetailsForGatewayPageId(string gatewayPageId);
    }
}
