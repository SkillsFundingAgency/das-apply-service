using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
using System;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public class CriminalComplianceChecksQuestionLookupService : ICriminalComplianceChecksQuestionLookupService
    {
        public QnaQuestionDetails GetQuestionDetailsForGatewayPageId(string gatewayPageId)
        {
            switch (gatewayPageId)
            {
                case GatewayPageIds.CCOrganisationCompositionCreditors:
                    {
                        return new QnaQuestionDetails
                        {
                            PageId = RoatpWorkflowPageIds.CriminalComplianceChecks.CompositionCreditors,
                            QuestionId = RoatpCriminalComplianceChecksQuestionIdConstants.CompositionCreditors
                        };
                    }
                case GatewayPageIds.CCOrganisationFailedToRepayFunds:
                    {
                        return new QnaQuestionDetails
                        {
                            PageId = RoatpWorkflowPageIds.CriminalComplianceChecks.OrganisationFailedToRepayFunds,
                            QuestionId = RoatpCriminalComplianceChecksQuestionIdConstants.OrganisationFailedToRepayFunds
                        };
                    }
                default:
                    return null;
            }
        }
    }
}
