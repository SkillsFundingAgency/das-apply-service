using Dapper;
using SFA.DAS.ApplyService.Data.DapperTypeHandlers;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Types;
using SFA.DAS.ApplyService.Infrastructure.Database;

namespace SFA.DAS.ApplyService.Data
{
    public class ApplyRepository : IApplyRepository
    {
        private readonly IDbConnectionHelper _dbConnectionHelper;

        public ApplyRepository(IDbConnectionHelper dbConnectionHelper)
        {
            _dbConnectionHelper = dbConnectionHelper;

            SqlMapper.AddTypeHandler(typeof(ApplyData), new ApplyDataHandler());
            SqlMapper.AddTypeHandler(typeof(OrganisationDetails), new OrganisationDetailsHandler());
            SqlMapper.AddTypeHandler(typeof(QnAData), new QnADataHandler());
            SqlMapper.AddTypeHandler(typeof(List<FinancialEvidence>), new FinancialEvidencesDataHandler());
        }

        public async Task<Guid> StartApplication(Guid applicationId, ApplyData applyData, Guid organisationId, Guid createdBy)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QuerySingleAsync<Guid>(
                    @"INSERT INTO Apply (ApplicationId, OrganisationId, ApplicationStatus, ApplyData, AssessorReviewStatus, GatewayReviewStatus, CreatedBy, CreatedAt)
                                        OUTPUT INSERTED.[ApplicationId] 
                                        VALUES (@applicationId, @organisationId, @applicationStatus, @applyData, @assessorReviewStatus, @gatewayReviewStatus,@createdBy, GETUTCDATE())",
                    new { applicationId, organisationId, applicationStatus = ApplicationStatus.InProgress, applyData, assessorReviewStatus = AssessorReviewStatus.Draft, gatewayReviewStatus = GatewayReviewStatus.Draft, createdBy });
            }
        }

        public async Task<Apply> GetApplication(Guid applicationId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Apply>(
                    @"SELECT * FROM apply WHERE ApplicationId = @applicationId",
                    new { applicationId });
            }
        }

        public async Task<Apply> GetApplicationByUserId(Guid applicationId, Guid signinId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return (await connection.QuerySingleOrDefaultAsync<Apply>(@"SELECT top 1 a.* FROM Contacts c
                                                    INNER JOIN Apply a ON a.OrganisationId = c.ApplyOrganisationID
                                                    WHERE c.SigninId = @signinId and a.ApplicationId = @ApplicationId  AND a.CreatedBy = c.Id", new { signinId, applicationId }));
            }
        }

        public async Task<List<Apply>> GetUserApplications(Guid signinId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return (await connection.QueryAsync<Apply>(@"SELECT a.* FROM Contacts c
                                                    INNER JOIN Apply a ON a.OrganisationId = c.ApplyOrganisationID
                                                    WHERE c.SigninId = @signinId AND a.CreatedBy = c.Id", new { signinId })).ToList();
            }
        }

        public async Task<List<Apply>> GetOrganisationApplications(Guid signinId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return (await connection.QueryAsync<Apply>(@"SELECT DISTINCT a.* FROM Contacts c
                                                    INNER JOIN Apply a ON a.OrganisationId = c.ApplyOrganisationID
                                                    WHERE c.SigninId = @signinId", new { signinId })).ToList();
            }
        }
        
        public async Task<List<Apply>> GetApplicationsByUkprn(string ukprn)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return (await connection.QueryAsync<Apply>(@"select a.*
                      from dbo.Apply a
                      where UKPRN = @ukprn",
                    new { ukprn })).ToList();
            }
        }

        public async Task<FinancialReviewDetails> GetFinancialReviewDetails(Guid applicationId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<FinancialReviewDetails>(
                    @"SELECT * FROM FinancialReview WHERE ApplicationId = @applicationId",
                    new { applicationId });
            }
        }

        public async Task<List<ClarificationFile>> GetFinancialReviewClarificationFiles(Guid applicationId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return (await connection.QueryAsync<ClarificationFile>(
                    @"SELECT * FROM [FinancialReviewClarificationFile] WHERE ApplicationId = @applicationId",
                    new { applicationId })).ToList();
            }
        }

        public async Task UpdateApplication(Apply application)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                await connection.ExecuteAsync(
                    @"UPDATE [Apply] SET
                        ApplicationStatus = @ApplicationStatus,
                        GatewayReviewStatus = @GatewayReviewStatus,
                        AssessorReviewStatus = @AssessorReviewStatus,
                        Assessor1UserId = @Assessor1UserId,
                        Assessor2UserId = @Assessor2UserId,
                        Assessor1Name = @Assessor1Name,
                        Assessor2Name = @Assessor2Name,
                        Assessor1ReviewStatus = @Assessor1ReviewStatus,
                        Assessor2ReviewStatus = @Assessor2ReviewStatus,
                        ModerationStatus = @ModerationStatus,
                        GatewayUserId = @gatewayUserId,
                        GatewayUserName = @gatewayUserName,
                        UpdatedBy = @updatedBy,
                        UpdatedAt = @updatedAt
                        WHERE [Id] = @id",
                    application);
            }
        }

        public async Task<bool> CanSubmitApplication(Guid applicationId)
        {
            var canSubmit = false;

            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var application = await GetApplication(applicationId);

                var invalidApplicationStatuses = new List<string> { ApplicationStatus.Successful, ApplicationStatus.Rejected, ApplicationStatus.Unsuccessful, ApplicationStatus.Removed, ApplicationStatus.Withdrawn, ApplicationStatus.Cancelled };

                // Application must exist and has not already been Approved, Rejected, Removed, Widthdrawn or Cancelled, in progress appeal or AppealSuccessful
                if (application != null && !invalidApplicationStatuses.Contains(application.ApplicationStatus))
                {
                    var otherAppsInProgress = await connection.QueryAsync<Domain.Entities.Apply>(@"
                                                        SELECT a.*
                                                        FROM Apply a
                                                        INNER JOIN Organisations o ON o.Id = a.OrganisationId
														INNER JOIN Contacts con ON a.OrganisationId = con.ApplyOrganisationID
                                                        WHERE a.OrganisationId = (SELECT OrganisationId FROM Apply WHERE ApplicationId = @applicationId)
														AND a.CreatedBy <> (SELECT CreatedBy FROM Apply WHERE ApplicationId = @applicationId)
                                                        AND a.ApplicationStatus NOT IN (@applicationStatusSuccessful, @applicationStatusUnsuccessful, @applicationStatusRejected, @applicationStatusRemoved, @applicationStatusWithdrawn, @applicationStatusCancelled, @applicationStatusInProgressAppeal, @applicationStatusAppealSuccessful)",
                                                            new
                                                            {
                                                                applicationId,
                                                                applicationStatusSuccessful = ApplicationStatus.Successful,   
                                                                applicationStatusUnsuccessful = ApplicationStatus.Unsuccessful,
                                                                applicationStatusRejected = ApplicationStatus.Rejected,   
                                                                applicationStatusRemoved = ApplicationStatus.Removed,
                                                                applicationStatusWithdrawn = ApplicationStatus.Withdrawn,
                                                                applicationStatusCancelled = ApplicationStatus.Cancelled,
                                                                applicationStatusInProgressAppeal = ApplicationStatus.InProgressAppeal,
                                                                applicationStatusAppealSuccessful = ApplicationStatus.AppealSuccessful
                                                            });

                    canSubmit = !otherAppsInProgress.Any();
                }
            }

            return canSubmit;
        }

        public async Task<bool> SubmitApplication(Guid applicationId, ApplyData applyData, FinancialData financialData, Guid submittedBy)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(@"UPDATE Apply
                                                SET ApplicationStatus = @ApplicationStatus, 
                                                    ApplyData = @applyData, 
                                                    AssessorReviewStatus = @AssessorReviewStatus, 
                                                    GatewayReviewStatus = @GatewayReviewStatus, 
                                                    UpdatedBy = @submittedBy, 
                                                    UpdatedAt = GETUTCDATE() 
                                                WHERE  (Apply.ApplicationId = @applicationId)",
                                                new { applicationId, 
                                                      ApplicationStatus = ApplicationStatus.Submitted, 
                                                      applyData,
                                                      AssessorReviewStatus = AssessorReviewStatus.New, 
                                                      GatewayReviewStatus = GatewayReviewStatus.New,
                                                      submittedBy });

                await connection.ExecuteAsync(@"insert into FinancialData ([ApplicationId]
               ,[TurnOver],[Depreciation],[ProfitLoss],[Dividends],[IntangibleAssets]
               ,[Assets],[Liabilities],[ShareholderFunds],[Borrowings],[AccountingReferenceDate],[AccountingPeriod],[AverageNumberofFTEEmployees])
                values (@ApplicationId, @TurnOver,@Depreciation, @ProfitLoss,@Dividends,@IntangibleAssets
               ,@Assets,@Liabilities,@ShareholderFunds,@Borrowings,@AccountingReferenceDate,@AccountingPeriod,@AverageNumberofFTEEmployees)",
               financialData);

                connection.Close();

                return rowsAffected > 0;
            }
        }

        public async Task<Guid> SnapshotApplication(Guid applicationId, Guid snapshotApplicationId, List<ApplySequence> newSequences)
        {
            var currentApplication = await GetApplication(applicationId);

            if (currentApplication != null)
            {
                var newApplyData = new ApplyData
                {
                    Sequences = newSequences,
                    ApplyDetails = currentApplication.ApplyData?.ApplyDetails
                };

                using (var connection = _dbConnectionHelper.GetDatabaseConnection())
                {
                    return await connection.QuerySingleAsync<Guid>(
                        @"INSERT INTO ApplySnapshots (ApplicationId, SnapshotApplicationId, SnapshotDate, OrganisationId, ApplicationStatus, ApplyData, GatewayReviewStatus, AssessorReviewStatus)
                          OUTPUT INSERTED.[SnapshotApplicationId] 
                          VALUES (@ApplicationId, @snapshotApplicationId, GETUTCDATE(), @OrganisationId, @ApplicationStatus, @newApplyData, @GatewayReviewStatus, @AssessorReviewStatus)",
                        new
                        {
                            currentApplication.ApplicationId,
                            snapshotApplicationId,
                            currentApplication.OrganisationId,
                            currentApplication.ApplicationStatus,
                            newApplyData,
                            currentApplication.GatewayReviewStatus,
                            currentApplication.AssessorReviewStatus
                        });
                }
            }

            return Guid.Empty;
        }      

        public async Task<bool> ChangeProviderRoute(Guid applicationId, int providerRoute, string providerRouteName)
        {
            var application = await GetApplication(applicationId);
            var applyData = application?.ApplyData;

            if (application != null && applyData?.ApplyDetails != null)
            {
                applyData.ApplyDetails.ProviderRoute = providerRoute;
                applyData.ApplyDetails.ProviderRouteName = providerRouteName;

                using (var connection = _dbConnectionHelper.GetDatabaseConnection())
                {
                    await connection.ExecuteAsync(@"UPDATE Apply
                                                    SET  ApplyData = @applyData
                                                    WHERE  ApplicationId = @ApplicationId",
                                                    new { application.ApplicationId, applyData });
                }

                return true;
            }

            return false;
        }
        


        public async Task<List<RoatpFinancialSummaryItem>> GetOpenFinancialApplications(string searchTerm, string sortColumn, string sortOrder)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var orderByClause = $"{GetSortColumnForNew(sortColumn)} { GetOrderByDirectionFinancial(sortOrder)}";

                return (await connection
                    .QueryAsync<RoatpFinancialSummaryItem>(
                        $@"SELECT 
                            apply.Id AS Id,
                            apply.ApplicationId AS ApplicationId,
                            apply.ApplicationStatus AS ApplicationStatus,
                            apply.GatewayReviewStatus AS GatewayReviewStatus,
                            apply.AssessorReviewStatus AS AssessorReviewStatus,
                            IsNull(fr.Status,@financialStatusNew) AS FinancialReviewStatus,
                            org.Name AS OrganisationName,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName') AS ApplicationRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS SubmittedDate,
                            JSON_VALUE(apply.ApplyData, '$.GatewayReviewDetails.OutcomeDateTime') AS GatewayOutcomeDate,
                            CASE seq.NotRequired WHEN 'false' THEN 'Not exempt' ELSE 'Exempt' END AS DeclaredInApplication
	                      FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId	 
                        LEFT OUTER JOIN FinancialReview fr on fr.ApplicationId = apply.ApplicationId
                        CROSS APPLY OPENJSON(apply.ApplyData)
                        WITH (
                            Sequences nvarchar(max) '$.Sequences' AS JSON
                        ) AS i
                        CROSS APPLY (
                            SELECT *
                            FROM OPENJSON(i.Sequences)
                            WITH (
                                [SequenceNo] nvarchar(max) '$.SequenceNo',
                                [NotRequired] nvarchar(max) '$.NotRequired'
                            )
                        ) seq
                        WHERE seq.SequenceNo = @financialHealthSequence
                          AND apply.GatewayReviewStatus IN (@gatewayStatusPass) -- NOTE: If Gateway did not pass then it goes straight to Oversight
                          AND apply.ApplicationStatus = @applicationStatusGatewayAssessed AND apply.DeletedAt IS NULL
                          AND isnull(fr.Status,@financialStatusNew) IN (@financialStatusDraft, @financialStatusNew, @financialStatusInProgress)
                          AND ( @searchString = '%%' OR apply.UKPRN LIKE @searchString OR org.Name LIKE @searchString )
                        ORDER BY {orderByClause}, org.Name ASC",
                        new
                        {
                            financialHealthSequence = 2,
                            gatewayStatusPass = GatewayReviewStatus.Pass,
                            applicationStatusGatewayAssessed = ApplicationStatus.GatewayAssessed,
                            financialStatusDraft = FinancialReviewStatus.Draft,
                            financialStatusNew = FinancialReviewStatus.New,
                            financialStatusInProgress = FinancialReviewStatus.InProgress,
                            searchString = $"%{searchTerm}%"
                        })).ToList();
            }
        }

        public async Task<List<RoatpFinancialSummaryDownloadItem>> GetOpenFinancialApplicationsForDownload()
        {
            var sql = @"SELECT 
                            apply.Id AS Id,
                            apply.ApplicationId AS ApplicationId,
                            apply.ApplicationStatus AS ApplicationStatus,
                            apply.GatewayReviewStatus AS GatewayReviewStatus,
                            apply.AssessorReviewStatus AS AssessorReviewStatus,
                            IsNull(fr.Status,@financialStatusNew) AS FinancialReviewStatus,
                            org.Name AS OrganisationName,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName') AS ApplicationRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS SubmittedDate,
                            JSON_VALUE(apply.ApplyData, '$.GatewayReviewDetails.OutcomeDateTime') AS GatewayOutcomeDate,
                            JSON_VALUE(apply.ApplyData, '$.GatewayReviewDetails.CompaniesHouseDetails.CompanyNumber') AS CompanyNumber,
                            JSON_VALUE(apply.ApplyData, '$.GatewayReviewDetails.CharityCommissionDetails.CharityNumber') AS CharityNumber,
                            CASE seq.NotRequired WHEN 'false' THEN 'Not exempt' ELSE 'Exempt' END AS DeclaredInApplication,
                            fd.ApplicationId,
                            fd.TurnOver,
                            fd.Depreciation,
                            fd.ProfitLoss,
                            fd.Dividends,
                            fd.IntangibleAssets,
                            fd.Assets,
                            fd.Liabilities,
                            fd.ShareholderFunds,
                            fd.Borrowings,
                            fd.AccountingReferenceDate,
                            fd.AccountingPeriod,
                            fd.AverageNumberofFTEEmployees
	                      FROM Apply apply
                          LEFT JOIN FinancialData fd on fd.ApplicationId = apply.ApplicationId
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId	         
                            LEFT OUTER JOIN FinancialReview fr on fr.ApplicationId = apply.ApplicationId
                        CROSS APPLY OPENJSON(apply.ApplyData)
                        WITH (
                            Sequences nvarchar(max) '$.Sequences' AS JSON
                        ) AS i
                        CROSS APPLY (
                            SELECT *
                            FROM OPENJSON(i.Sequences)
                            WITH (
                                [SequenceNo] nvarchar(max) '$.SequenceNo',
                                [NotRequired] nvarchar(max) '$.NotRequired'
                            )
                        ) seq
                        WHERE seq.SequenceNo = @financialHealthSequence
                          AND apply.ApplicationStatus = @applicationStatusGatewayAssessed AND apply.DeletedAt IS NULL
                          AND IsNull(fr.Status,@financialStatusNew) IN ( @financialStatusDraft, @financialStatusNew, @financialStatusInProgress)
                          AND apply.GatewayReviewStatus IN (@gatewayStatusPass)
                        ORDER BY CAST(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS DATE) ASC, org.Name ASC";

            var parameters = new
            {
                financialHealthSequence = 2,
                applicationStatusGatewayAssessed = ApplicationStatus.GatewayAssessed,
                financialStatusDraft = FinancialReviewStatus.Draft,
                financialStatusNew = FinancialReviewStatus.New,
                financialStatusInProgress = FinancialReviewStatus.InProgress,
                gatewayStatusPass = GatewayReviewStatus.Pass
            };

            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var results = await connection
                    .QueryAsync<RoatpFinancialSummaryDownloadItem, FinancialData, RoatpFinancialSummaryDownloadItem>(
                        sql, ((item, data) =>
                            {
                                item.FinancialData = data;
                                return item;
                            }
                        ), parameters, null, true, "ApplicationId");

                return results.ToList();
            }
        }

        public async Task<List<RoatpFinancialSummaryItem>> GetClarificationFinancialApplications(string searchTerm, string sortColumn, string sortOrder)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var orderByClause = $"{GetSortColumnForNew(sortColumn)} { GetOrderByDirectionFinancial(sortOrder)}";

                return (await connection
                    .QueryAsync<RoatpFinancialSummaryItem>(
                        $@"SELECT 
                            apply.Id AS Id,
                            apply.ApplicationId AS ApplicationId,
                            apply.ApplicationStatus AS ApplicationStatus,
                            apply.GatewayReviewStatus AS GatewayReviewStatus,
                            apply.AssessorReviewStatus AS AssessorReviewStatus,
                            fr.Status AS FinancialReviewStatus,
                            org.Name AS OrganisationName,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName') AS ApplicationRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS SubmittedDate,
                            fr.ClarificationRequestedOn AS ClarificationRequestedDate,
                            fr.ClarificationRequestedBy AS OutcomeMadeBy,
                            JSON_VALUE(apply.ApplyData, '$.GatewayReviewDetails.OutcomeDateTime') AS GatewayOutcomeDate,
                            CASE seq.NotRequired WHEN 'false' THEN 'Not exempt' ELSE 'Exempt' END AS DeclaredInApplication
	                      FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId	 
                          LEFT OUTER JOIN FinancialReview fr on fr.ApplicationId = apply.ApplicationId
                        CROSS APPLY OPENJSON(apply.ApplyData)
                        WITH (
                            Sequences nvarchar(max) '$.Sequences' AS JSON
                        ) AS i
                        CROSS APPLY (
                            SELECT *
                            FROM OPENJSON(i.Sequences)
                            WITH (
                                [SequenceNo] nvarchar(max) '$.SequenceNo',
                                [NotRequired] nvarchar(max) '$.NotRequired'
                            )
                        ) seq
                        WHERE seq.SequenceNo = @financialHealthSequence
                          AND apply.GatewayReviewStatus IN (@gatewayStatusPass) -- NOTE: If Gateway did not pass then it goes straight to Oversight
                          AND apply.ApplicationStatus = @applicationStatusGatewayAssessed AND apply.DeletedAt IS NULL
                          AND fr.Status IN (@financialStatusClarificationSent)
                          AND ( @searchString = '%%' OR apply.UKPRN LIKE @searchString OR org.Name LIKE @searchString )
                       ORDER BY {orderByClause}, org.Name ASC",
                        new
                        {
                            financialHealthSequence = 2,
                            gatewayStatusPass = GatewayReviewStatus.Pass,
                            applicationStatusGatewayAssessed = ApplicationStatus.GatewayAssessed,
                            financialStatusClarificationSent = FinancialReviewStatus.ClarificationSent,
                            searchString = $"%{searchTerm}%"
                        })).ToList();
            }
        }

        public async Task<List<RoatpFinancialSummaryItem>> GetClosedFinancialApplications(string searchTerm, string sortColumn, string sortOrder)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var orderByClause = $"{GetSortColumnForNew(sortColumn ?? "SubmittedDate")} { GetOrderByDirectionFinancial(sortOrder)}";

                return (await connection
                   .QueryAsync<RoatpFinancialSummaryItem>(
                       $@"SELECT 
                            apply.Id AS Id,
                            apply.ApplicationId AS ApplicationId,
                            apply.ApplicationStatus  AS ApplicationStatus,
                            apply.ApplicationStatus AS ApplicationStatus,
                            apply.GatewayReviewStatus AS GatewayReviewStatus,
                            apply.AssessorReviewStatus AS AssessorReviewStatus,
                            fr.Status AS FinancialReviewStatus,
                            org.Name AS OrganisationName,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName') AS ApplicationRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS SubmittedDate,
                            CASE 
                                WHEN apply.ApplicationStatus = @applicationStatusWithdrawn THEN JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationWithdrawnOn')
                                WHEN apply.ApplicationStatus = @applicationStatusRemoved THEN JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationRemovedOn')
                                ELSE fr.GradedOn
                            END AS OutcomeMadeDate,
                            CASE 
                                WHEN apply.ApplicationStatus = @applicationStatusWithdrawn THEN JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationWithdrawnBy')
                                WHEN apply.ApplicationStatus = @applicationStatusRemoved THEN JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationRemovedBy')
                                ELSE fr.GradedBy
                            END AS OutcomeMadeBy,
                            fr.SelectedGrade AS SelectedGrade,
                            JSON_VALUE(apply.ApplyData, '$.GatewayReviewDetails.OutcomeDateTime') AS GatewayOutcomeDate,
                            CASE seq.NotRequired WHEN 'false' THEN 'Not exempt' ELSE 'Exempt' END AS DeclaredInApplication
	                      FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId	
                            LEFT OUTER JOIN FinancialReview fr on fr.ApplicationId = apply.ApplicationId
                        CROSS APPLY OPENJSON(apply.ApplyData)
                        WITH (
                            Sequences nvarchar(max) '$.Sequences' AS JSON
                        ) AS i
                        CROSS APPLY (
                            SELECT *
                            FROM OPENJSON(i.Sequences)
                            WITH (
                                [SequenceNo] nvarchar(max) '$.SequenceNo',
                                [NotRequired] nvarchar(max) '$.NotRequired'
                            )
                        ) seq
                        WHERE seq.SequenceNo = @financialHealthSequence
                          AND apply.GatewayReviewStatus IN (@gatewayStatusPass) -- NOTE: If Gateway did not pass then it goes straight to Oversight
                          AND apply.DeletedAt IS NULL
                          AND (
                               apply.ApplicationStatus IN (@applicationStatusWithdrawn, @applicationStatusRemoved)
                               OR fr.Status IN (@financialStatusApproved, @financialStatusDeclined, @financialStatusExempt)
                              )
                          AND ( @searchString = '%%' OR apply.UKPRN LIKE @searchString OR org.Name LIKE @searchString )
                        ORDER BY {orderByClause}, org.Name ASC",
                       new
                       {
                           financialHealthSequence = 2,
                           gatewayStatusPass = GatewayReviewStatus.Pass,
                           applicationStatusWithdrawn = ApplicationStatus.Withdrawn,
                           applicationStatusRemoved = ApplicationStatus.Removed,
                           financialStatusApproved = FinancialReviewStatus.Pass,
                           financialStatusDeclined = FinancialReviewStatus.Fail,
                           financialStatusExempt = FinancialReviewStatus.Exempt,
                           searchString = $"%{searchTerm}%"
                       })).ToList();
            }
        }

        public async Task<RoatpFinancialApplicationsStatusCounts> GetFinancialApplicationsStatusCounts(string searchTerm)
        {
            // Note: For now it is easier to run all three queries. It may make sense to do something similar to that done with EPAO
            var openApplications = await GetOpenFinancialApplications(searchTerm, null, null);
            var clarificationApplications = await GetClarificationFinancialApplications(searchTerm, null, null);
            var closedApplications = await GetClosedFinancialApplications(searchTerm, null, null);

            return new RoatpFinancialApplicationsStatusCounts
            {
                ApplicationsOpen = openApplications.Count,
                ApplicationsWithClarification = clarificationApplications.Count,
                ApplicationsClosed = closedApplications.Count
            };
        }

        public async Task<bool> StartFinancialReview(Guid applicationId, string reviewer)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                await connection.ExecuteAsync(@"if exists(select * from apply where ApplicationId = @applicationId 
                                                        AND apply.ApplicationStatus = @applicationStatusGatewayAssessed AND apply.DeletedAt IS NULL)
                                                        BEGIN
                                                        if not exists (select * from FinancialReview where ApplicationId = @applicationId)
	                                                        BEGIN
	                                                         insert into FinancialReview (applicationId, Status)
		                                                        values (@applicationId, @financialReviewStatusInProgress)

	                                                        update Apply set updatedBy =@updatedBy, updatedAt=@updatedAt where applicationId = @applicationId
	                                                        END
                                                        END",
                        new
                        {
                            applicationId,
                            applicationStatusGatewayAssessed = ApplicationStatus.GatewayAssessed,
                            financialReviewStatusInProgress = FinancialReviewStatus.InProgress,
                            updatedAt = DateTime.UtcNow,
                            updatedBy = reviewer
                        });
            }

            return true;
        }

        public async Task<bool> RecordFinancialGrade(Guid applicationId, FinancialReviewDetails financialReviewDetails, string financialReviewStatus)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                await connection.ExecuteAsync(@"UPDATE FinancialReview 
                                                         SET FinancialDueDate = @financialDueDate,
                                                         SelectedGrade = @selectedGrade,
                                                         GradedBy = @GradedBy,
                                                         GradedOn = @GradedOn,
                                                         ClarificationRequestedOn = @clarificationRequestedOn,
                                                         ClarificationRequestedBy = @clarificationRequestedBy,
                                                         FinancialEvidences= @financialEvidences,
                                                         Comments = @Comments,
                                                         ExternalComments = @ExternalComments,
                                                         Status = @financialReviewStatus,
                                                         ClarificationResponse=@ClarificationResponse
                                                         WHERE ApplicationId = @applicationId",
                                                new
                                                {
                                                    applicationId,
                                                    financialReviewDetails.SelectedGrade,
                                                    financialReviewDetails.FinancialDueDate,
                                                    financialReviewDetails.GradedBy,
                                                    gradedOn = financialReviewDetails.GradedOn,
                                                    clarificationRequestedBy = financialReviewDetails.ClarificationRequestedBy,
                                                    clarificationRequestedOn = financialReviewDetails.ClarificationRequestedOn,
                                                    financialEvidences = financialReviewDetails.FinancialEvidences,
                                                    financialReviewDetails.Comments,
                                                    financialReviewDetails.ExternalComments,
                                                    financialReviewStatus,
                                                    financialReviewDetails.ClarificationResponse
                                                });
            }

            return true;
        }

        public async Task<bool> AddFinancialReviewClarificationFile(Guid applicationId, string filename)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                await connection.ExecuteAsync(@"INSERT INTO [FinancialReviewClarificationFile] (ApplicationId, Filename) Values (@applicationId, @filename)  ",
                    new
                    {
                        applicationId,
                        filename
                    });
            }
            return true;
        }

        public async Task<bool> RemoveFinancialReviewClarificationFile(Guid applicationId, string filename)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                await connection.ExecuteAsync(@"DELETE FROM [FinancialReviewClarificationFile] WHERE ApplicationId=@applicationId and Filename= @filename  ",
                    new
                    {
                        applicationId,
                        filename
                    });
            }
            return true;
        }

        public async Task UpdateApplicationStatus(Guid applicationId, string status, string userId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                await connection.ExecuteAsync(@"UPDATE Apply
                                                SET  ApplicationStatus = @status,
                                                UpdatedBy = @userId,
                                                UpdatedAt = GETUTCDATE()
                                                WHERE ApplicationId = @ApplicationId", new {applicationId, status, userId});
            }
        }

        public async Task<Contact> GetContactForApplication(Guid applicationId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Contact>(@"SELECT con.* FROM Contacts con 
                                                                        INNER JOIN Apply appl ON appl.CreatedBy = con.Id
                                                                        WHERE appl.ApplicationId = @ApplicationId",
                    new { applicationId });
            }
        }

        public async Task<Organisation> GetOrganisationForApplication(Guid applicationId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Organisation>(@"SELECT org.* FROM Organisations org 
                                                                        INNER JOIN Apply appl ON appl.OrganisationId = org.Id
                                                                        WHERE appl.ApplicationId = @ApplicationId",
                    new {applicationId});
            }
        }

        public async Task<List<ApplicationOversightDownloadDetails>> GetOversightsForDownload(DateTime dateFrom, DateTime dateTo)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return (await connection.QueryAsync<ApplicationOversightDownloadDetails>(@"SELECT 
                            apply.Id AS Id,
                            apply.ApplicationId AS ApplicationId,
							 org.Name AS OrganisationName,
					        JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            REPLACE(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName'),' provider','') AS ProviderRoute,
							JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS ApplicationSubmittedDate,
                            REPLACE(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteNameOnRegister'),' provider','') AS ProviderRouteNameOnRegister,
							JSON_VALUE(apply.ApplyData, '$.ApplyDetails.OrganisationType') AS OrganisationType,
                            JSON_VALUE(org.OrganisationDetails, '$.RoatpDetails.StatusId') As OrganisationStatusId,
                            JSON_VALUE(apply.ApplyData, '$.GatewayReviewDetails.CompaniesHouseDetails.CompanyNumber') AS CompanyNumber,
                            CONCAT(orgAddresses.AddressLine1 , ',' , orgAddresses.AddressLine2 , ',' , orgAddresses.AddressLine3 , ',' , orgAddresses.City , ',' ,orgAddresses.Postcode) AS LegalAddress,
                            apply.ApplicationStatus,
							apply.ApplicationDeterminedDate,
                            apply.GatewayReviewStatus as GatewayOutcome,
                            apply.AssessorReviewStatus  as AssessorOutcome,
                            CASE fr.SelectedGrade WHEN @financialGradeInadequate THEN 'Fail' ELSE 'Pass' END as FHCOutcome,
                            CASE WHEN apply.GatewayReviewStatus = @gatewayReviewStatusPass AND apply.AssessorReviewStatus = @assessorReviewStatusApproved AND fr.SelectedGrade <> @financialGradeInadequate THEN 'Pass' ELSE 'Fail' END as OverallOutcome
                            FROM Apply apply
	                        INNER JOIN Organisations org ON org.Id = apply.OrganisationId
                            INNER JOIN OrganisationAddresses orgAddresses ON orgAddresses.OrganisationId = org.Id
                            LEFT JOIN OversightReview r on r.ApplicationId = apply.ApplicationId
                            LEFT OUTER JOIN FinancialReview fr on fr.ApplicationId = apply.ApplicationId
	                      WHERE apply.DeletedAt IS NULL
                          AND JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') BETWEEN @dateFrom AND @dateTo
                          and ((GatewayReviewStatus  in (@gatewayReviewStatusPass)
						  and AssessorReviewStatus in (@assessorReviewStatusApproved,@assessorReviewStatusDeclined)
						  and fr.Status in (@financialReviewStatusApproved,@financialReviewStatusDeclined, @financialReviewStatusExempt))
                            OR GatewayReviewStatus in (@gatewayReviewStatusFail, @gatewayReviewStatusReject))
						  and r.[Status] is null
                             order by cast(apply.ApplicationDeterminedDate as DATE) ASC, CAST(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS DATE) ASC,  Org.Name ASC", new
                {
                    gatewayReviewStatusPass = GatewayReviewStatus.Pass,
                    gatewayReviewStatusFail = GatewayReviewStatus.Fail,
                    GatewayReviewStatusReject = GatewayReviewStatus.Rejected,
                    assessorReviewStatusApproved = AssessorReviewStatus.Approved,
                    assessorReviewStatusDeclined = AssessorReviewStatus.Declined,
                    financialReviewStatusApproved = FinancialReviewStatus.Pass,
                    financialReviewStatusDeclined = FinancialReviewStatus.Fail,
                    financialReviewStatusExempt = FinancialReviewStatus.Exempt,
                    oversightReviewStatusPass = OversightReviewStatus.Successful,
                    oversightReviewStatusFail = OversightReviewStatus.Unsuccessful,
                    financialGradeInadequate = Domain.Roatp.FinancialApplicationSelectedGrade.Inadequate,
                    dateFrom = dateFrom.ToString("yyyy-MM-dd"), dateTo = dateTo.AddDays(1).Date.ToString("yyyy-MM-dd")
                })).ToList();
            }
        }

        public async Task<bool> SubmitReapplicationRequest(Guid applicationId, string userId)
        {
            var applyData = await GetApplyData(applicationId);

            if (applyData?.ApplyDetails == null) return false;
            applyData.ApplyDetails.RequestToReapplyMade = true;
            applyData.ApplyDetails.RequestToReapplyBy = userId;
            applyData.ApplyDetails.RequestToReapplyOn = DateTime.UtcNow;

            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                await connection.ExecuteAsync(@"UPDATE Apply
                                                SET  ApplyData = @applyData
                                                WHERE  ApplicationId = @ApplicationId",
                    new { applicationId, applyData });
            }

            return true;
        }

        public async Task<IEnumerable<RoatpApplicationStatus>> GetExistingApplicationStatusByUkprn(string ukprn)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var applicationStatuses = await connection.QueryAsync<RoatpApplicationStatus>(
                    @"select a.Id AS ApplicationId, a.ApplicationStatus AS Status
                      from dbo.Apply a
                      where UKPRN = @ukprn",
                 new { ukprn });

                return await Task.FromResult(applicationStatuses);
            }
        }

        public async Task<string> GetNextRoatpApplicationReference()
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var nextInSequence = (await connection.QueryAsync<int>(@"SELECT NEXT VALUE FOR RoatpAppReferenceSequence")).FirstOrDefault();

                return $"APR{nextInSequence}";
            }
        }

        public async Task<bool> StartAssessorReview(Guid applicationId, string reviewer)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                await connection.ExecuteAsync(@"UPDATE Apply SET AssessorReviewStatus = @assessorReviewStatusInProgress,
                                                UpdatedAt = @updatedAt, UpdatedBy = @updatedBy
                                                WHERE ApplicationId = @applicationId
                                                AND AssessorReviewStatus IN ( @draftStatus, @newStatus )",
                        new
                        {
                            applicationId,
                            assessorReviewStatusInProgress = AssessorReviewStatus.InProgress,
                            updatedAt = DateTime.UtcNow,
                            updatedBy = reviewer,
                            draftStatus = AssessorReviewStatus.Draft,
                            newStatus = AssessorReviewStatus.New
                        });
            }

            return await Task.FromResult(true);
        }

        public async Task<ApplyData> GetApplyData(Guid applicationId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<ApplyData>(@"SELECT ApplyData FROM Apply WHERE ApplicationId = @applicationId",
                    new { applicationId });
            }
        }

        public async Task<bool> UpdateApplyData(Guid applicationId, ApplyData applyData, string updatedBy)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                await connection.ExecuteAsync(@"UPDATE Apply SET ApplyData = @applyData,
                                                UpdatedAt = @updatedAt, UpdatedBy = @updatedBy
                                                WHERE ApplicationId = @applicationId",
                            new
                            {
                                applicationId,
                                updatedAt = DateTime.UtcNow,
                                updatedBy,
                                applyData
                            });
            }

            return await Task.FromResult(true);
        }


        private static string GetSortColumnForNew(string requestedColumn)
        {
            switch (requestedColumn)
            {
                case "SubmittedDate":
                    return " CAST(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS DATE) ";
                case "OutcomeMadeBy":
                    return " fr.ClarificationRequestedBy ";
                case "OutcomeMadeByClosed":
                    return  $@" CASE 
                                WHEN apply.ApplicationStatus = '{ApplicationStatus.Withdrawn}'THEN JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationWithdrawnBy')
                                WHEN apply.ApplicationStatus = '{ApplicationStatus.Removed}' THEN JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationRemovedBy')
                                ELSE fr.GradedBy
                            END ";
                case "OutcomeMadeDate":
                    return $@" CASE 
                                WHEN apply.ApplicationStatus = '{ApplicationStatus.Withdrawn}' THEN JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationWithdrawnOn')
                                WHEN apply.ApplicationStatus = '{ApplicationStatus.Removed}' THEN JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationRemovedOn')
                                ELSE fr.GradedOn
                            END";
                default:
                    return " CAST(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS DATE) ";
            }
        }

        private static string GetOrderByDirectionFinancial(string sortOrder)
        {
            return "ascending".Equals(sortOrder, StringComparison.InvariantCultureIgnoreCase) ? " ASC " : " DESC ";
        }
    }
}