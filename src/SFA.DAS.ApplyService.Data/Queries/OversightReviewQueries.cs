using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types.QueryResults;

namespace SFA.DAS.ApplyService.Data.Queries
{
    public class OversightReviewQueries : IOversightReviewQueries
    {
        private readonly IApplyConfig _config;

        public OversightReviewQueries(IConfigurationService configurationService)
        {
            _config = configurationService.GetConfig().Result;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_config.SqlConnectionString);
        }

        public async Task<PendingOversightReviews> GetPendingOversightReviews()
        {
            using (var connection = GetConnection())
            {
                var reviews = (await connection.QueryAsync<PendingOversightReview>(@"SELECT 
                            apply.ApplicationId AS ApplicationId,
							 org.Name AS OrganisationName,
                            apply.GatewayReviewStatus,
                            apply.FinancialReviewStatus,
                            apply.ModerationStatus AS ModerationReviewStatus,
					        JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            REPLACE(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName'),' provider','') AS ProviderRoute,
							JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS ApplicationSubmittedDate
                              FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId
                         LEFT JOIN OversightReview r ON r.ApplicationId = apply.ApplicationId
	                      WHERE apply.DeletedAt IS NULL
                          and r.Status is null
                          and ((GatewayReviewStatus  in (@gatewayReviewStatusPass)
						  and AssessorReviewStatus in (@assessorReviewStatusApproved,@assessorReviewStatusDeclined)
						  and FinancialReviewStatus in (@financialReviewStatusApproved,@financialReviewStatusDeclined, @financialReviewStatusExempt)) 
                            OR GatewayReviewStatus in (@gatewayReviewStatusFail, @gatewayReviewStatusReject))
                            order by CAST(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS DATE) ASC,  Org.Name ASC", new
                {
                    gatewayReviewStatusPass = GatewayReviewStatus.Pass,
                    gatewayReviewStatusFail = GatewayReviewStatus.Fail,
                    GatewayReviewStatusReject = GatewayReviewStatus.Reject,
                    assessorReviewStatusApproved = AssessorReviewStatus.Approved,
                    assessorReviewStatusDeclined = AssessorReviewStatus.Declined,
                    financialReviewStatusApproved = FinancialReviewStatus.Pass,
                    financialReviewStatusDeclined = FinancialReviewStatus.Fail,
                    financialReviewStatusExempt = FinancialReviewStatus.Exempt
                })).ToList();

                return new PendingOversightReviews
                {
                    Reviews = reviews
                };
            }
        }

        public async Task<CompletedOversightReviews> GetCompletedOversightReviews()
        {
            using (var connection = GetConnection())
            {
                var reviews = (await connection.QueryAsync<CompletedOversightReview>(@"SELECT 
                            apply.Id AS Id,
                            apply.ApplicationId AS ApplicationId,
							 org.Name AS OrganisationName,
					        JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            REPLACE(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName'),' provider','') AS ProviderRoute,
							JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS ApplicationSubmittedDate,
							r.Status as OversightStatus,
                            apply.ApplicationStatus,
							r.ApplicationDeterminedDate
                              FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId
                          INNER JOIN OversightReview r ON r.ApplicationId = apply.ApplicationId
	                      WHERE apply.DeletedAt IS NULL
                          and ((GatewayReviewStatus  in (@gatewayReviewStatusPass)
						  and AssessorReviewStatus in (@assessorReviewStatusApproved,@assessorReviewStatusDeclined)
						  and FinancialReviewStatus in (@financialReviewStatusApproved,@financialReviewStatusDeclined, @financialReviewStatusExempt)) 
                            OR GatewayReviewStatus in (@gatewayReviewStatusFail, @gatewayReviewStatusReject))
                            order by CAST(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS DATE) ASC,  Org.Name ASC", new
                {
                    gatewayReviewStatusPass = GatewayReviewStatus.Pass,
                    gatewayReviewStatusFail = GatewayReviewStatus.Fail,
                    GatewayReviewStatusReject = GatewayReviewStatus.Reject,
                    assessorReviewStatusApproved = AssessorReviewStatus.Approved,
                    assessorReviewStatusDeclined = AssessorReviewStatus.Declined,
                    financialReviewStatusApproved = FinancialReviewStatus.Pass,
                    financialReviewStatusDeclined = FinancialReviewStatus.Fail,
                    financialReviewStatusExempt = FinancialReviewStatus.Exempt
                })).ToList();

                return new CompletedOversightReviews
                {
                    Reviews = reviews
                };
            }
        }


        public async Task<ApplicationOversightDetails> GetOversightDetails(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var applyDataResults = await connection.QueryAsync<ApplicationOversightDetails>(@"SELECT 
                            apply.Id AS Id,
                            apply.ApplicationId AS ApplicationId,
							 org.Name AS OrganisationName,
					        JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            REPLACE(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName'),' provider','') AS ProviderRoute,
							JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS ApplicationSubmittedDate,
                            apply.ApplicationStatus,
							outcome.Status as OversightStatus,
							outcome.ApplicationDeterminedDate,
                            outcome.UserName as OversightUserName,
							apply.AssessorReviewStatus,
							contacts.Email as ApplicationEmailAddress,
							apply.GatewayReviewStatus,
							JSON_VALUE(apply.ApplyData, '$.GatewayReviewDetails.OutcomeDateTime') AS GatewayOutcomeMadeDate,
							apply.GatewayUserName as GatewayOutcomeMadeBy,
							JSON_VALUE(apply.ApplyData, '$.GatewayReviewDetails.Comments') AS GatewayComments,
                            JSON_VALUE(apply.ApplyData, '$.GatewayReviewDetails.ExternalComments') AS GatewayExternalComments,
							apply.FinancialReviewStatus,
							JSON_VALUE(apply.FinancialGrade, '$.SelectedGrade') AS FinancialGradeAwarded,
							JSON_VALUE(apply.FinancialGrade, '$.GradedDateTime') AS FinancialHealthAssessedOn,
							JSON_VALUE(apply.FinancialGrade, '$.GradedBy') AS FinancialHealthAssessedBy,
                            JSON_VALUE(apply.FinancialGrade, '$.Comments') AS FinancialHealthComments,
                            JSON_VALUE(apply.FinancialGrade, '$.ExternalComments') AS FinancialHealthExternalComments,
							apply.ModerationStatus as ModerationReviewStatus,
							JSON_VALUE(apply.ApplyData, '$.ModeratorReviewDetails.OutcomeDateTime') AS ModerationOutcomeMadeOn,
							JSON_VALUE(apply.ApplyData, '$.ModeratorReviewDetails.ModeratorName') AS ModeratedBy,
							JSON_VALUE(apply.ApplyData, '$.ModeratorReviewDetails.ModeratorComments') AS ModerationComments,
                            outcome.[InProgressDate],
                            outcome.[InProgressUserId],
                            outcome.[InProgressUserName],
                            outcome.[InProgressInternalComments],
                            outcome.[InProgressExternalComments],
                            outcome.[GatewayApproved],
                            outcome.[ModerationApproved],
                            outcome.[InternalComments],
                            outcome.[ExternalComments]
                              FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId
                          LEFT JOIN OversightReview outcome ON outcome.ApplicationId = apply.ApplicationId
						  LEFT OUTER JOIN contacts on contacts.ApplyOrganisationId = org.Id
                        WHERE apply.ApplicationId = @applicationId",
                    new { applicationId });

                return applyDataResults.FirstOrDefault();
            }
        }

    }
}
