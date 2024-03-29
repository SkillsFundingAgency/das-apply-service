﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;
using SFA.DAS.ApplyService.Infrastructure.Database;
using OversightReview = SFA.DAS.ApplyService.Domain.QueryResults.OversightReview;

namespace SFA.DAS.ApplyService.Data.Queries
{
    public class OversightReviewQueries : IOversightReviewQueries
    {
        private readonly IDbConnectionHelper _dbConnectionHelper;

        public OversightReviewQueries(IDbConnectionHelper dbConnectionHelper)
        {
            _dbConnectionHelper = dbConnectionHelper;
        }

        public async Task<PendingOversightReviews> GetPendingOversightReviews(string searchTerm, string sortColumn, string sortOrder)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var orderByClause = $"{GetSortColumnForNew(sortColumn)} { GetOrderByDirection(sortOrder)}";

                var reviews = (await connection.QueryAsync<PendingOversightReview>($@"SELECT 
                            apply.ApplicationId AS ApplicationId,
                            apply.ApplicationStatus,
                            org.Name AS OrganisationName,
                            apply.GatewayReviewStatus,
                            fr.Status as FinancialReviewStatus,
                            apply.ModerationStatus AS ModerationReviewStatus,
					        apply.UKPRN,
                            REPLACE(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName'),' provider','') AS ProviderRoute,
							JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS ApplicationSubmittedDate
                              FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId
                         LEFT JOIN OversightReview r ON r.ApplicationId = apply.ApplicationId
                         LEFT OUTER JOIN FinancialReview fr on fr.ApplicationId = apply.ApplicationId
                         LEFT JOIN Appeal Appeal on apply.ApplicationId = Appeal.ApplicationId
	                      WHERE apply.DeletedAt IS NULL
                          AND Appeal.Status IS NULL
                          AND ( @searchString = '%%' OR apply.UKPRN LIKE @searchString OR org.Name LIKE @searchString )
                          and r.Status is null
                          and ((GatewayReviewStatus in (@gatewayReviewStatusPass)
						  and AssessorReviewStatus in (@assessorReviewStatusApproved,@assessorReviewStatusDeclined)
						  and fr.Status in (@financialReviewStatusApproved,@financialReviewStatusDeclined, @financialReviewStatusExempt)) 
                            OR GatewayReviewStatus in (@gatewayReviewStatusFail, @gatewayReviewStatusRejected)
                            OR apply.ApplicationStatus = @applicationStatusRemoved)
                            ORDER BY {orderByClause}, org.Name ASC", new
                {
                    searchString = $"%{searchTerm}%",
                    gatewayReviewStatusPass = GatewayReviewStatus.Pass,
                    gatewayReviewStatusFail = GatewayReviewStatus.Fail,
                    GatewayReviewStatusRejected = GatewayReviewStatus.Rejected,
                    assessorReviewStatusApproved = AssessorReviewStatus.Approved,
                    assessorReviewStatusDeclined = AssessorReviewStatus.Declined,
                    financialReviewStatusApproved = FinancialReviewStatus.Pass,
                    financialReviewStatusDeclined = FinancialReviewStatus.Fail,
                    financialReviewStatusExempt = FinancialReviewStatus.Exempt,
                    applicationStatusRemoved = ApplicationStatus.Removed
                })).ToList();

                return new PendingOversightReviews
                {
                    Reviews = reviews
                };
            }
        }

        public async Task<CompletedOversightReviews> GetCompletedOversightReviews(string searchTerm, string sortColumn, string sortOrder)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var orderByClause = $"{GetSortColumnForNew(sortColumn)} { GetOrderByDirection(sortOrder)}";

                var reviews = (await connection.QueryAsync<CompletedOversightReview>($@"SELECT 
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
                          LEFT JOIN Appeal Appeal on apply.ApplicationId = Appeal.ApplicationId
	                      WHERE apply.DeletedAt IS NULL
                          AND Appeal.Status IS NULL
                          AND ( @searchString = '%%' OR apply.UKPRN LIKE @searchString OR org.Name LIKE @searchString )
                        ORDER BY {orderByClause}, org.Name ASC", new
                {
                    searchString = $"%{searchTerm}%"
                })).ToList();

                return new CompletedOversightReviews
                {
                    Reviews = reviews
                };
            }
        }


        public async Task<PendingAppealOutcomes> GetPendingAppealOutcomes(string searchTerm, string sortColumn, string sortOrder)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var orderByClause = $"{GetSortColumnForAppeal(sortColumn)} { GetOrderByDirection(sortOrder)}";

                var reviews = (await connection.QueryAsync<PendingAppealOutcome>($@"SELECT 
                            apply.ApplicationId AS ApplicationId,
                            org.Name AS OrganisationName,
                            apply.UKPRN,
                            REPLACE(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName'),' provider','') AS ProviderRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,                          
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS ApplicationSubmittedDate,
                            oversight.ApplicationDeterminedDate AS ApplicationDeterminedDate,
                            appeal.AppealSubmittedDate AS AppealSubmittedDate,
                            appeal.Status as AppealStatus
                                FROM Apply apply
                            INNER JOIN Organisations org ON org.Id = apply.OrganisationId
                            INNER JOIN OversightReview oversight ON oversight.ApplicationId = apply.ApplicationId
                            INNER JOIN Appeal appeal ON appeal.ApplicationId = apply.ApplicationId
                            WHERE apply.DeletedAt IS NULL
                            AND ( @searchString = '%%' OR apply.UKPRN LIKE @searchString OR org.Name LIKE @searchString )
                            AND appeal.Status IN (@appealStatusSubmitted)
                            ORDER BY {orderByClause}, org.Name ASC", new
                {
                    searchString = $"%{searchTerm}%",
                    appealStatusSubmitted = Types.AppealStatus.Submitted
                })).ToList();

                return new PendingAppealOutcomes
                {
                    Reviews = reviews
                };
            }
        }

        public async Task<CompletedAppealOutcomes> GetCompletedAppealOutcomes(string searchTerm, string sortColumn, string sortOrder)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var orderByClause = $"{GetSortColumnForAppealOutcome(sortColumn)} { GetOrderByDirection(sortOrder)}";

                var reviews = (await connection.QueryAsync<CompletedAppealOutcome>($@"SELECT 
                            apply.ApplicationId AS ApplicationId,
                            org.Name AS OrganisationName,
                            apply.UKPRN,
                            REPLACE(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName'),' provider','') AS ProviderRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,                          
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS ApplicationSubmittedDate,
                            appeal.AppealSubmittedDate AS AppealSubmittedDate,                            
                            ISNULL(appeal.AppealDeterminedDate, appeal.InProgressDate) AS AppealDeterminedDate,
                            appeal.Status AS AppealStatus,
                            oversight.Status as OversightStatus
                                FROM Apply apply
                            INNER JOIN Organisations org ON org.Id = apply.OrganisationId
                            INNER JOIN OversightReview oversight ON oversight.ApplicationId = apply.ApplicationId
                            INNER JOIN Appeal appeal ON appeal.ApplicationId = apply.ApplicationId
                            WHERE apply.DeletedAt IS NULL
                            AND ( @searchString = '%%' OR apply.UKPRN LIKE @searchString OR org.Name LIKE @searchString )
                            AND appeal.Status NOT IN (@appealStatusSubmitted)
                            ORDER BY {orderByClause}, org.Name ASC", new
                {
                    searchString = $"%{searchTerm}%",
                    appealStatusSubmitted = Types.AppealStatus.Submitted                    
                })).ToList();

                return new CompletedAppealOutcomes
                {
                    Reviews = reviews
                };
            }
        }


        public async Task<ApplicationOversightDetails> GetOversightApplicationDetails(Guid applicationId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
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
							fr.Status as FinancialReviewStatus,
							fr.SelectedGrade AS FinancialGradeAwarded,
							fr.GradedOn AS FinancialHealthAssessedOn,
							fr.GradedBy AS FinancialHealthAssessedBy,
                            fr.Comments AS FinancialHealthComments,
                            fr.ExternalComments AS FinancialHealthExternalComments,
							apply.ModerationStatus as ModerationReviewStatus,
							JSON_VALUE(apply.ApplyData, '$.ModeratorReviewDetails.OutcomeDateTime') AS ModerationOutcomeMadeOn,
							JSON_VALUE(apply.ApplyData, '$.ModeratorReviewDetails.ModeratorName') AS ModeratedBy,
							JSON_VALUE(apply.ApplyData, '$.ModeratorReviewDetails.ModeratorComments') AS ModerationComments,
                            apply.Comments 'ApplyInternalComments',
                            apply.ExternalComments 'ApplyExternalComments',
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationRemovedOn') AS ApplicationRemovedOn,                            
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationRemovedBy') AS ApplicationRemovedBy,                            
                            outcome.[Id] as OversightReviewId,        
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
                          LEFT OUTER JOIN FinancialReview fr on fr.ApplicationId = apply.ApplicationId
                        WHERE apply.ApplicationId = @applicationId",
                    new { applicationId });

                return applyDataResults.FirstOrDefault();
            }
        }

        public async Task<OversightReview> GetOversightReview(Guid applicationId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var results = await connection.QueryAsync<OversightReview>(@"SELECT 
                        r.[Id],        
                        r.Status,
						r.ApplicationDeterminedDate,
                        r.[InProgressDate],
                        r.[InProgressUserId],
                        r.[InProgressUserName],
                        r.[InProgressInternalComments],
                        r.[InProgressExternalComments],
                        r.[GatewayApproved],
                        r.[ModerationApproved],
                        r.[InternalComments],
                        r.[ExternalComments],
                        r.[UserId],    
                        r.UserName
                        FROM [OversightReview] r 
                        WHERE r.ApplicationId = @applicationId",
                    new { applicationId });

                return results.FirstOrDefault();
            }
        }

        private static string GetSortColumnForNew(string requestedColumn)
        {
            switch (requestedColumn)
            {
                default:
                    return " CAST(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS DATE) ";
            }
        }

        private static string GetSortColumnForAppeal(string requestedColumn)
        {
            switch (requestedColumn)
            {
                case "ApplicationSubmittedDate":
                    return " CAST(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS DATE) ";
                case "ApplicationDeterminedDate":
                    return $" oversight.ApplicationDeterminedDate ";
                case "AppealSubmittedDate":
                default:
                    return $" appeal.AppealSubmittedDate ";
            }
        }
        private static string GetSortColumnForAppealOutcome(string requestedColumn)
        {
            switch (requestedColumn)
            {
                case "AppealSubmittedDate":
                    return $" appeal.AppealSubmittedDate ";
                case "AppealDeterminedDate":
                default:
                    return $" appeal.AppealDeterminedDate ";
            }
        }

        private static string GetOrderByDirection(string sortOrder)
        {
            return "ascending".Equals(sortOrder, StringComparison.InvariantCultureIgnoreCase) ? " ASC " : " DESC ";
        }
    }
}
