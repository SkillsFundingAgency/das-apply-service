using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public class CriminalComplianceChecksQuestionLookupService : ICriminalComplianceChecksQuestionLookupService
    {
        private Dictionary<string, QnaQuestionDetails> _criminalComplianceChecksQuestions;

        public CriminalComplianceChecksQuestionLookupService()
        {
            // TODO: move this into config
            _criminalComplianceChecksQuestions = new Dictionary<string, QnaQuestionDetails>
            {
                {
                    GatewayPageIds.CCOrganisationCompositionCreditors,
                    new QnaQuestionDetails
                    {
                        PageId = RoatpWorkflowPageIds.CriminalComplianceChecks.CompositionCreditors,
                        QuestionId = RoatpCriminalComplianceChecksQuestionIdConstants.CompositionCreditors
                    }
                },
                {
                    GatewayPageIds.CCOrganisationFailedToRepayFunds,
                    new QnaQuestionDetails
                    {
                        PageId = RoatpWorkflowPageIds.CriminalComplianceChecks.OrganisationFailedToRepayFunds,
                        QuestionId = RoatpCriminalComplianceChecksQuestionIdConstants.OrganisationFailedToRepayFunds
                    }
                },
                {
                    GatewayPageIds.CCOrganisationContractTermination,
                    new QnaQuestionDetails
                    {
                        PageId = RoatpWorkflowPageIds.CriminalComplianceChecks.OrganisationContractTermination,
                        QuestionId = RoatpCriminalComplianceChecksQuestionIdConstants.OrganisationContractTermination
                    }
                }
            };
        }

        public QnaQuestionDetails GetQuestionDetailsForGatewayPageId(string gatewayPageId)
        {
            try
            {
                return _criminalComplianceChecksQuestions[gatewayPageId];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
    }
}
