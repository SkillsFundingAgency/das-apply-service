using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public class CriminalComplianceChecksQuestionLookupService : ICriminalComplianceChecksQuestionLookupService
    {
        private List<CriminalComplianceGatewayConfig> _criminalComplianceQuestions;
        private List<CriminalComplianceGatewayOverrideConfig> _soleTraderOverrides;
        private IInternalQnaApiClient _apiClient;

        private const string SoleTraderAnswer = "Sole trader";

        public CriminalComplianceChecksQuestionLookupService(IOptions<List<CriminalComplianceGatewayConfig>> criminalComplianceConfig,
                                                             IOptions<List<CriminalComplianceGatewayOverrideConfig>> soleTraderOverrides, 
                                                             IInternalQnaApiClient qnaApiClient)
        {
            _apiClient = qnaApiClient;

            _criminalComplianceQuestions = criminalComplianceConfig.Value;
            _soleTraderOverrides = soleTraderOverrides.Value;
        }

        public QnaQuestionDetails GetQuestionDetailsForGatewayPageId(Guid applicationId, string gatewayPageId)
        {
            var questionConfig = _criminalComplianceQuestions.FirstOrDefault(q => q.GatewayPageId == gatewayPageId);
            if (questionConfig == null)
            {
                return null;
            }

            var questionDetails = new QnaQuestionDetails
            {
                SectionId = questionConfig.SectionId,
                PageId = questionConfig.QnaPageId,
                QuestionId = questionConfig.QnaQuestionId
            };

            if (questionConfig.SectionId == RoatpWorkflowSectionIds.CriminalComplianceChecks.CheckOnWhosInControl)
            {
                var soleTraderAnswer = _apiClient.GetQuestionTag(applicationId, RoatpWorkflowQuestionTags.SoleTraderOrPartnership).GetAwaiter().GetResult();
                if (soleTraderAnswer == null || soleTraderAnswer != SoleTraderAnswer) 
                {
                    return questionDetails;
                }
                var overrideConfig = _soleTraderOverrides.FirstOrDefault(x => x.GatewayPageId == gatewayPageId);
                if (overrideConfig != null)
                {
                    questionDetails.SectionId = overrideConfig.SectionId;
                    questionDetails.PageId = overrideConfig.QnaPageId;
                    questionDetails.QuestionId = overrideConfig.QnaQuestionId;
                }
            }

            return questionDetails;
        }


    }
}
