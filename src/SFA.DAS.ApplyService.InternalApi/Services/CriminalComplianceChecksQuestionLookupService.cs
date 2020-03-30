using Microsoft.Extensions.Options;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public class CriminalComplianceChecksQuestionLookupService : ICriminalComplianceChecksQuestionLookupService
    {
        private List<CriminalComplianceGatewayConfig> _criminalComplianceQuestions;

        public CriminalComplianceChecksQuestionLookupService(IOptions<List<CriminalComplianceGatewayConfig>> Configuration)
        {
            _criminalComplianceQuestions = Configuration.Value;            
        }

        public QnaQuestionDetails GetQuestionDetailsForGatewayPageId(string gatewayPageId)
        {
            var questionConfig = _criminalComplianceQuestions.FirstOrDefault(q => q.GatewayPageId == gatewayPageId);
            if (questionConfig == null)
            {
                return null;
            }

            return new QnaQuestionDetails { PageId = questionConfig.QnaPageId, QuestionId = questionConfig.QnaQuestionId };
        }
    }
}
