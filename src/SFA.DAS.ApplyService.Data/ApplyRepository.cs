using Dapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Data.DapperTypeHandlers;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply.Gateway;

namespace SFA.DAS.ApplyService.Data
{
    public class ApplyRepository : IApplyRepository
    {
        private readonly IApplyConfig _config;
        private readonly ILogger<ApplyRepository> _logger;

        public ApplyRepository(IConfigurationService configurationService, ILogger<ApplyRepository> logger)
        {
            _logger = logger;
            _config = configurationService.GetConfig().Result;
            
            SqlMapper.AddTypeHandler(typeof(ApplyData), new ApplyDataHandler());
            SqlMapper.AddTypeHandler(typeof(OrganisationDetails), new OrganisationDetailsHandler());
            SqlMapper.AddTypeHandler(typeof(QnAData), new QnADataHandler());
            SqlMapper.AddTypeHandler(typeof(ApplicationData), new ApplicationDataHandler());
            SqlMapper.AddTypeHandler(typeof(FinancialReviewDetails), new FinancialReviewDetailsDataHandler());
        }

        public async Task<Guid> StartApplication(Guid applicationId, ApplyData applyData, Guid organisationId, Guid createdBy)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return await connection.QuerySingleAsync<Guid>(
                    @"INSERT INTO Apply (ApplicationId, OrganisationId, ApplicationStatus, ApplyData, AssessorReviewStatus, GatewayReviewStatus, FinancialReviewStatus, CreatedBy, CreatedAt)
                                        OUTPUT INSERTED.[ApplicationId] 
                                        VALUES (@applicationId, @organisationId, @applicationStatus, @applyData, @reviewStatus, @gatewayReviewStatus, @financialReviewStatus, @createdBy, GETUTCDATE())",
                    new { applicationId, organisationId, applicationStatus = ApplicationStatus.InProgress, applyData, reviewStatus = ApplicationReviewStatus.Draft, gatewayReviewStatus = GatewayReviewStatus.Draft, financialReviewStatus = FinancialReviewStatus.Draft, createdBy });
            }
        }

        public async Task<Apply> GetApplication(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var application = await connection.QuerySingleOrDefaultAsync<Apply>(@"SELECT * FROM apply WHERE ApplicationId = @applicationId", new { applicationId });

                //if (application != null)
                //{
                //    application.ApplyingOrganisation = await GetOrganisationForApplication(applicationId);
                //    application.ApplyingContact = await GetContactForApplication(applicationId);
                //}

                return application;
            }
        }

        public async Task<Apply> GetApplicationByUserId(Guid applicationId, Guid signinId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QuerySingleOrDefaultAsync<Apply>(@"SELECT top 1 a.* FROM Contacts c
                                                    INNER JOIN Apply a ON a.OrganisationId = c.ApplyOrganisationID
                                                    WHERE c.SigninId = @signinId and a.ApplicationId = @ApplicationId", new { signinId, applicationId }));
            }
        }

        public async Task<List<Apply>> GetUserApplications(Guid signinId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<Apply>(@"SELECT a.* FROM Contacts c
                                                    INNER JOIN Apply a ON a.OrganisationId = c.ApplyOrganisationID
                                                    WHERE c.SigninId = @signinId AND a.CreatedBy = c.Id", new { signinId })).ToList();
            }
        }

        public async Task<List<Apply>> GetOrganisationApplications(Guid signinId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<Apply>(@"SELECT a.* FROM Contacts c
                                                    INNER JOIN Apply a ON a.OrganisationId = c.ApplyOrganisationID
                                                    WHERE c.SigninId = @signinId", new { signinId })).ToList();
            }
        }

        public async Task<List<GatewayPageAnswerSummary>> GetGatewayPageAnswers(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<Domain.Entities.GatewayPageAnswerSummary>(@"SELECT ApplicationId, PageId, Status, Comments FROM GatewayAnswer
                                                    WHERE ApplicationId = @applicationId", new { applicationId })).ToList();
            }
        }

        public async Task<GatewayPageAnswer> GetGatewayPageAnswer(Guid applicationId, string pageId)
        {

            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QuerySingleOrDefaultAsync<GatewayPageAnswer>(@"SELECT * from GatewayAnswer
                                                    WHERE applicationId = @applicationId and pageid = @pageId",
                    new {applicationId, pageId}));
            }
        }

        public async Task<string> GetGatewayPageStatus(Guid applicationId, string pageId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QuerySingleOrDefaultAsync<string>(@"SELECT Status from GatewayAnswer WHERE applicationId = @applicationId and pageid = @pageId",
                    new { applicationId, pageId }));
            }
        }

        public async Task<string> GetGatewayPageComments(Guid applicationId, string pageId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QuerySingleOrDefaultAsync<string>(@"SELECT Comments from GatewayAnswer WHERE applicationId = @applicationId and pageid = @pageId",
                    new { applicationId, pageId }));
            }
        }

        public async Task SubmitGatewayPageAnswer(Guid applicationId, string pageId, string userId, string userName, string status, string comments)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.OpenAsync(default);
                await connection.ExecuteAsync(
                    @"IF NOT EXISTS (select * from GatewayAnswer where applicationId = @applicationId and pageId = @pageId)
	                                                    INSERT INTO GatewayAnswer ([ApplicationId],[PageId],[Status],[comments],[CreatedAt],[CreatedBy])
														     values (@applicationId, @pageId,@status,@comments,GetUTCDATE(),@userName)
                                                    ELSE
                                                     UPDATE GatewayAnswer
                                                                SET  Status = @status, Comments =@comments, UpdatedBy = @userName, UpdatedAt = GETUTCDATE()
                                                                WHERE  ApplicationId = @applicationId and pageId = @pageId",
                    new { applicationId, pageId, status, comments, userName });

                await connection.ExecuteAsync(
                    "update [Apply] set [GatewayUserId]=@userId, [GatewayUserName]=@userName WHERE [ApplicationId] = @applicationId",
                    new { applicationId, userId, userName });

                connection.Close();
            }
        }

        public async Task<bool> UpdateGatewayReviewStatusAndComment(Guid applicationId, ApplyData applyData, string gatewayReviewStatus, string userId, string userName)
        {
            var applicationStatus = ApplicationStatus.GatewayAssessed;
            if(gatewayReviewStatus.Equals(GatewayReviewStatus.ClarificationSent))
                applicationStatus = ApplicationStatus.Submitted;

            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE [Apply]
                                                   SET [ApplicationStatus] = @applicationStatus
                                                      ,[GatewayReviewStatus] = @gatewayReviewStatus
                                                      ,[ApplyData] = @applyData
                                                      ,[UpdatedAt] = GetUTCDATE()
                                                      ,[UpdatedBy] = @userName 
                                                      ,[GatewayUserId] = @userId
                                                      ,[GatewayUsername] = @userName
                                                 WHERE [ApplicationId] = @applicationId",
                    new { applicationId, applicationStatus, gatewayReviewStatus, applyData, userId, userName });
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> CanSubmitApplication(Guid applicationId)
        {
            var canSubmit = false;

            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var application = await GetApplication(applicationId);
                var invalidApplicationStatuses = new List<string> { ApplicationStatus.Approved, ApplicationStatus.Rejected };

                // Application must exist and has not already been Approved or Rejected
                if (application != null && !invalidApplicationStatuses.Contains(application.ApplicationStatus))
                {
                    var otherAppsInProgress = await connection.QueryAsync<Domain.Entities.Apply>(@"
                                                        SELECT a.*
                                                        FROM Apply a
                                                        INNER JOIN Organisations o ON o.Id = a.OrganisationId
														INNER JOIN Contacts con ON a.OrganisationId = con.ApplyOrganisationID
                                                        WHERE a.OrganisationId = (SELECT OrganisationId FROM Apply WHERE ApplicationId = @applicationId)
														AND a.CreatedBy <> (SELECT CreatedBy FROM Apply WHERE ApplicationId = @applicationId)
                                                        AND a.ApplicationStatus NOT IN (@applicationStatusApproved, @applicationStatusApprovedRejected)",
                                                            new
                                                            {
                                                                applicationId,
                                                                applicationStatusApproved = ApplicationStatus.Approved,
                                                                applicationStatusApprovedRejected = ApplicationStatus.Rejected
                                                            });

                    canSubmit = !otherAppsInProgress.Any();
                }
            }

            return canSubmit;
        }

        public async Task SubmitApplication(Guid applicationId, ApplyData applyData, FinancialData financialData, Guid submittedBy)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(@"UPDATE Apply
                                                SET ApplicationStatus = @ApplicationStatus, 
                                                    ApplyData = @applyData, 
                                                    AssessorReviewStatus = @ReviewStatus, 
                                                    GatewayReviewStatus = @GatewayReviewStatus, 
                                                    FinancialReviewStatus = @FinancialReviewStatus,
                                                    UpdatedBy = @submittedBy, 
                                                    UpdatedAt = GETUTCDATE() 
                                                WHERE  (Apply.ApplicationId = @applicationId)",
                                                new { applicationId, 
                                                      ApplicationStatus = ApplicationStatus.Submitted, 
                                                      applyData, 
                                                      ReviewStatus = ApplicationReviewStatus.New, 
                                                      GatewayReviewStatus = GatewayReviewStatus.New, 
                                                      FinancialReviewStatus = FinancialReviewStatus.New,
                                                      submittedBy });

                await connection.ExecuteAsync(@"insert into FinancialData ([ApplicationId]
               ,[TurnOver],[Depreciation],[ProfitLoss],[Dividends],[IntangibleAssets]
               ,[Assets],[Liabilities],[ShareholderFunds],[Borrowings])
                values (@ApplicationId, @TurnOver,@Depreciation, @ProfitLoss,@Dividends,@IntangibleAssets
               ,@Assets,@Liabilities,@ShareholderFunds,@Borrowings)",
               financialData);

                connection.Close();
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

                using (var connection = new SqlConnection(_config.SqlConnectionString))
                {
                    return await connection.QuerySingleAsync<Guid>(
                        @"INSERT INTO ApplySnapshots (ApplicationId, SnapshotApplicationId, SnapshotDate, OrganisationId, ApplicationStatus, ApplyData, GatewayReviewStatus, AssessorReviewStatus, FinancialReviewStatus, FinancialGrade)
                          OUTPUT INSERTED.[SnapshotApplicationId] 
                          VALUES (@ApplicationId, @snapshotApplicationId, GETUTCDATE(), @OrganisationId, @ApplicationStatus, @newApplyData, @GatewayReviewStatus, @AssessorReviewStatus, @FinancialReviewStatus, @FinancialGrade)",
                        new
                        {
                            currentApplication.ApplicationId,
                            snapshotApplicationId,
                            currentApplication.OrganisationId,
                            currentApplication.ApplicationStatus,
                            newApplyData,
                            currentApplication.GatewayReviewStatus,
                            currentApplication.AssessorReviewStatus,
                            currentApplication.FinancialReviewStatus,
                            currentApplication.FinancialGrade
                        });
                }
            }

            return Guid.Empty;
        }

        public async Task<List<RoatpApplicationSummaryItem>> GetNewGatewayApplications()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .QueryAsync<RoatpApplicationSummaryItem>(
                        @"SELECT 
                            apply.Id AS Id,
                            apply.ApplicationId AS ApplicationId,
                            apply.ApplicationStatus AS ApplicationStatus,
                            apply.GatewayReviewStatus AS GatewayReviewStatus,
                            apply.AssessorReviewStatus AS AssessorReviewStatus,
                            apply.FinancialReviewStatus AS FinancialReviewStatus,
                            org.Name AS OrganisationName,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName') AS ApplicationRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS SubmittedDate
	                      FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId
	                      WHERE apply.ApplicationStatus = @applicationStatusSubmitted AND apply.DeletedAt IS NULL
	                        AND apply.GatewayReviewStatus = @gatewayReviewStatusNew",
                        new
                        {
                            applicationStatusSubmitted = ApplicationStatus.Submitted,
                            gatewayReviewStatusNew = GatewayReviewStatus.New
                        })).ToList();
            }
        }

        public async Task<List<RoatpApplicationSummaryItem>> GetInProgressGatewayApplications()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .QueryAsync<RoatpApplicationSummaryItem>(
                        @"SELECT 
                            apply.Id AS Id,
                            apply.ApplicationId AS ApplicationId,
                            apply.ApplicationStatus AS ApplicationStatus,
                            apply.GatewayReviewStatus AS GatewayReviewStatus,
                            apply.AssessorReviewStatus AS AssessorReviewStatus,
                            apply.FinancialReviewStatus AS FinancialReviewStatus,
                            apply.GatewayUserName,
                            org.Name AS OrganisationName,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName') AS ApplicationRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS SubmittedDate
	                      FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId
	                      WHERE apply.ApplicationStatus = @applicationStatusSubmitted AND apply.DeletedAt IS NULL
	                        AND apply.GatewayReviewStatus in (@gatewayReviewStatusInProgress, @gatewayReviewStatusClarificationSent)",
                        new
                        {
                            applicationStatusSubmitted = ApplicationStatus.Submitted,
                            gatewayReviewStatusInProgress = GatewayReviewStatus.InProgress,
                            gatewayReviewStatusClarificationSent = GatewayReviewStatus.ClarificationSent
                        })).ToList();
            }
        }

        public async Task<List<RoatpApplicationSummaryItem>> GetClosedGatewayApplications()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .QueryAsync<RoatpApplicationSummaryItem>(
                        @"SELECT 
                            apply.Id AS Id,
                            apply.ApplicationId AS ApplicationId,
                            apply.ApplicationStatus AS ApplicationStatus,
                            apply.GatewayReviewStatus AS GatewayReviewStatus,
                            apply.AssessorReviewStatus AS AssessorReviewStatus,
                            apply.FinancialReviewStatus AS FinancialReviewStatus,
                            apply.GatewayUserName,                            
                            org.Name AS OrganisationName,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName') AS ApplicationRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS SubmittedDate
	                      FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId
	                      WHERE apply.ApplicationStatus = @applicationStatusGatewayAssessed AND apply.DeletedAt IS NULL
	                        AND apply.GatewayReviewStatus IN (@gatewayReviewStatusApproved, @gatewayReviewStatusFailed, @gatewayReviewStatusRejected)",
                        new
                        {
                            applicationStatusGatewayAssessed = ApplicationStatus.GatewayAssessed,
                            gatewayReviewStatusApproved = GatewayReviewStatus.Pass,
                            gatewayReviewStatusFailed = GatewayReviewStatus.Fail,
                            GatewayReviewStatusRejected = GatewayReviewStatus.Reject
                        })).ToList();
            }
        }

        public async Task StartGatewayReview(Guid applicationId, string reviewer)
        {
            var application = await GetApplication(applicationId);

            if (application != null && application.GatewayReviewStatus == GatewayReviewStatus.New)
            {
                application.GatewayReviewStatus = GatewayReviewStatus.InProgress;
                application.UpdatedBy = reviewer;
                application.UpdatedAt = DateTime.UtcNow;

                using (var connection = new SqlConnection(_config.SqlConnectionString))
                {
                    await connection.ExecuteAsync(@"UPDATE Apply
                                                    SET  GatewayReviewStatus = @GatewayReviewStatus, UpdatedBy = @UpdatedBy, UpdatedAt = GETUTCDATE() 
                                                    WHERE Apply.ApplicationId = @ApplicationId",
                                                    new { application.ApplicationId, application.ApplyData, application.GatewayReviewStatus, application.UpdatedBy });
                }
            }
        }

        public async Task<bool> ChangeProviderRoute(Guid applicationId, int providerRoute, string providerRouteName)
        {
            var application = await GetApplication(applicationId);
            var applyData = application?.ApplyData;

            if (application != null && applyData?.ApplyDetails != null)
            {
                applyData.ApplyDetails.ProviderRoute = providerRoute;
                applyData.ApplyDetails.ProviderRouteName = providerRouteName;

                using (var connection = new SqlConnection(_config.SqlConnectionString))
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
        
        public async Task EvaluateGateway(Guid applicationId, bool isGatewayApproved, string evaluatedBy)
        {
            var application = await GetApplication(applicationId);

            if (application != null && application.GatewayReviewStatus == GatewayReviewStatus.InProgress)
            {
                application.UpdatedBy = evaluatedBy;
                application.UpdatedAt = DateTime.UtcNow;

                if(isGatewayApproved)
                {
                    application.ApplicationStatus = ApplicationStatus.GatewayAssessed;
                    application.GatewayReviewStatus = GatewayReviewStatus.Pass;
                }
                else
                {
                    application.ApplicationStatus = ApplicationStatus.Rejected;
                    application.GatewayReviewStatus = GatewayReviewStatus.Fail;
                }

                using (var connection = new SqlConnection(_config.SqlConnectionString))
                {
                    await connection.ExecuteAsync(@"UPDATE Apply
                                                    SET  ApplicationStatus = @ApplicationStatus, GatewayReviewStatus = @GatewayReviewStatus, UpdatedBy = @UpdatedBy, UpdatedAt = GETUTCDATE() 
                                                    WHERE Apply.ApplicationId = @ApplicationId",
                                                    new { application.ApplicationId, application.ApplyData, application.ApplicationStatus, application.GatewayReviewStatus, application.UpdatedBy });
                }
            }
        }

        public async Task<List<RoatpFinancialSummaryItem>> GetOpenFinancialApplications()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .QueryAsync<RoatpFinancialSummaryItem>(
                        @"SELECT 
                            apply.Id AS Id,
                            apply.ApplicationId AS ApplicationId,
                            apply.ApplicationStatus AS ApplicationStatus,
                            apply.GatewayReviewStatus AS GatewayReviewStatus,
                            apply.AssessorReviewStatus AS AssessorReviewStatus,
                            apply.FinancialReviewStatus AS FinancialReviewStatus,
                            apply.FinancialGrade AS FinancialReviewDetails,
                            org.Name AS OrganisationName,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName') AS ApplicationRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS SubmittedDate,
                            JSON_VALUE(apply.ApplyData, '$.GatewayReviewDetails.OutcomeDateTime') AS GatewayOutcomeDateTime,
                            CASE s.NotRequired WHEN 'false' THEN 'Not exempt' ELSE 'Exempt' END AS DeclaredInApplication
	                      FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId	                      
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
                        ) s
                        WHERE s.SequenceNo = @financialHealthSequence
                        AND apply.ApplicationStatus = @applicationStatusGatewayAssessed AND apply.DeletedAt IS NULL
                        AND apply.FinancialReviewStatus IN ( @financialStatusDraft, @financialStatusNew, @financialStatusInProgress)
                        AND apply.GatewayReviewStatus IN (@gatewayStatusPass)",
                        new
                        {
                            financialHealthSequence = 2,
                            applicationStatusGatewayAssessed = ApplicationStatus.GatewayAssessed,
                            financialStatusDraft = FinancialReviewStatus.Draft,
                            financialStatusNew = FinancialReviewStatus.New,
                            financialStatusInProgress = FinancialReviewStatus.InProgress,
                            gatewayStatusPass = GatewayReviewStatus.Pass
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
                            apply.FinancialReviewStatus AS FinancialReviewStatus,
                            apply.FinancialGrade AS FinancialReviewDetails,
                            org.Name AS OrganisationName,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName') AS ApplicationRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS SubmittedDate,
                            JSON_VALUE(apply.ApplyData, '$.GatewayReviewDetails.OutcomeDateTime') AS GatewayOutcomeDateTime,
                            CASE s.NotRequired WHEN 'false' THEN 'Not exempt' ELSE 'Exempt' END AS DeclaredInApplication,
                            fd.ApplicationId,
                            fd.TurnOver,
                            fd.Depreciation,
                            fd.ProfitLoss,
                            fd.Dividends,
                            fd.IntangibleAssets,
                            fd.Assets,
                            fd.Liabilities,
                            fd.ShareholderFunds,
                            fd.Borrowings
	                      FROM Apply apply
                          LEFT JOIN FinancialData fd on fd.ApplicationId = apply.ApplicationId
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId	                      
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
                        ) s
                        WHERE s.SequenceNo = @financialHealthSequence
                        AND apply.ApplicationStatus = @applicationStatusGatewayAssessed AND apply.DeletedAt IS NULL
                        AND apply.FinancialReviewStatus IN ( @financialStatusDraft, @financialStatusNew, @financialStatusInProgress)
                        AND apply.GatewayReviewStatus IN (@gatewayStatusPass)";


            var parameters = new
            {
                financialHealthSequence = 2,
                applicationStatusGatewayAssessed = ApplicationStatus.GatewayAssessed,
                financialStatusDraft = FinancialReviewStatus.Draft,
                financialStatusNew = FinancialReviewStatus.New,
                financialStatusInProgress = FinancialReviewStatus.InProgress,
                gatewayStatusPass = GatewayReviewStatus.Pass
            };

            try
            {



                using (var connection = new SqlConnection(_config.SqlConnectionString))
                {
                    var results = await connection
                        .QueryAsync<RoatpFinancialSummaryDownloadItem, FinancialData, RoatpFinancialSummaryDownloadItem
                        >(
                            sql, ((item, data) =>
                                {
                                    item.FinancialData = data;
                                    return item;
                                }
                            ), parameters, null, true, "ApplicationId");

                    return results.ToList();
                }
            }
            catch (Exception ex)
            {
                var m = ex.Message;
                throw;
            }
        }

        public async Task<List<RoatpFinancialSummaryItem>> GetClarificationFinancialApplications()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .QueryAsync<RoatpFinancialSummaryItem>(
                        @"SELECT 
                            apply.Id AS Id,
                            apply.ApplicationId AS ApplicationId,
                            apply.ApplicationStatus AS ApplicationStatus,
                            apply.GatewayReviewStatus AS GatewayReviewStatus,
                            apply.AssessorReviewStatus AS AssessorReviewStatus,
                            apply.FinancialReviewStatus AS FinancialReviewStatus,
                            apply.FinancialGrade AS FinancialReviewDetails,
                            org.Name AS OrganisationName,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName') AS ApplicationRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS SubmittedDate,
                            JSON_VALUE(apply.FinancialGrade, '$.GradedDateTime') AS FeedbackAddedDate,
                            JSON_VALUE(apply.ApplyData, '$.GatewayReviewDetails.OutcomeDateTime') AS GatewayOutcomeDateTime,
                            CASE s.NotRequired WHEN 'false' THEN 'Not exempt' ELSE 'Exempt' END AS DeclaredInApplication
	                      FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId	                      
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
                        ) s
                        WHERE s.SequenceNo = @financialHealthSequence AND s.NotRequired = 'false'
                        AND apply.DeletedAt IS NULL
                        AND apply.FinancialReviewStatus IN ( @financialStatusClarificationSent )
                        AND apply.GatewayReviewStatus IN (@gatewayStatusPass)",
                        new
                        {
                            financialHealthSequence = 2,
                            financialStatusClarificationSent = FinancialReviewStatus.ClarificationSent,
                            gatewayStatusPass = GatewayReviewStatus.Pass
                        })).ToList();
            }
        }

        public async Task<List<RoatpFinancialSummaryItem>> GetClosedFinancialApplications()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                   .QueryAsync<RoatpFinancialSummaryItem>(
                       @"SELECT 
                            apply.Id AS Id,
                            apply.ApplicationId AS ApplicationId,
                            apply.ApplicationStatus AS ApplicationStatus,
                            apply.GatewayReviewStatus AS GatewayReviewStatus,
                            apply.AssessorReviewStatus AS AssessorReviewStatus,
                            apply.FinancialReviewStatus AS FinancialReviewStatus,
                            apply.FinancialGrade AS FinancialReviewDetails,
                            org.Name AS OrganisationName,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName') AS ApplicationRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS SubmittedDate,
                            JSON_VALUE(apply.FinancialGrade, '$.GradedDateTime') AS ClosedDate,
                            JSON_VALUE(apply.ApplyData, '$.GatewayReviewDetails.OutcomeDateTime') AS GatewayOutcomeDateTime,
                            CASE s.NotRequired WHEN 'false' THEN 'Not exempt' ELSE 'Exempt' END AS DeclaredInApplication
	                      FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId	
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
                        ) s
                        WHERE s.SequenceNo = @financialHealthSequence
                        AND apply.DeletedAt IS NULL
                        AND apply.FinancialReviewStatus IN ( @financialStatusApproved, @financialStatusDeclined, @financialStatusExempt )
                        AND apply.GatewayReviewStatus IN (@gatewayStatusPass)",
                       new
                       {
                           financialHealthSequence = 2,
                           financialStatusApproved = FinancialReviewStatus.Pass,
                           financialStatusDeclined = FinancialReviewStatus.Fail,
                           financialStatusExempt = FinancialReviewStatus.Exempt,
                           gatewayStatusPass = GatewayReviewStatus.Pass
                       })).ToList();
            }
        }

        public async Task<RoatpFinancialApplicationsStatusCounts> GetFinancialApplicationsStatusCounts()
        {
            // Note: For now it is easier to run all three queries. It may make sense to do something similar to that done with EPAO
            var openApplications = await GetOpenFinancialApplications();
            var clarificationApplications = await GetClarificationFinancialApplications();
            var closedApplications = await GetClosedFinancialApplications();

            return new RoatpFinancialApplicationsStatusCounts
            {
                ApplicationsOpen = openApplications.Count,
                ApplicationsWithClarification = clarificationApplications.Count,
                ApplicationsClosed = closedApplications.Count
            };
        }

        public async Task<bool> StartFinancialReview(Guid applicationId, string reviewer)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE Apply SET FinancialReviewStatus = @financialReviewStatusInProgress,
                                                UpdatedAt = @updatedAt, UpdatedBy = @updatedBy
                                                WHERE ApplicationId = @applicationId
                                                AND apply.ApplicationStatus = @applicationStatusGatewayAssessed AND apply.DeletedAt IS NULL
                                                AND FinancialReviewStatus IN ( @draftStatus, @newStatus )",
                        new
                        {
                            applicationId,
                            applicationStatusGatewayAssessed = ApplicationStatus.GatewayAssessed,
                            financialReviewStatusInProgress = FinancialReviewStatus.InProgress,
                            updatedAt = DateTime.UtcNow,
                            updatedBy = reviewer,
                            draftStatus = FinancialReviewStatus.Draft,
                            newStatus = FinancialReviewStatus.New
                        });
            }

            return true;
        }

        public async Task<bool> RecordFinancialGrade(Guid applicationId, FinancialReviewDetails financialReviewDetails, string financialReviewStatus)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE Apply 
                                                         SET FinancialGrade = @financialReviewDetails,
                                                         FinancialReviewStatus = @financialReviewStatus
                                                         WHERE ApplicationId = @applicationId",
                                                new
                                                {
                                                    applicationId,
                                                    financialReviewDetails,
                                                    financialReviewStatus
                                                });
            }

            return true;
        }

        public async Task<bool> UpdateFinancialReviewDetails(Guid applicationId, FinancialReviewDetails financialReviewDetails)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
                {
                    await connection.ExecuteAsync(@"UPDATE Apply 
                                                         SET FinancialGrade = @financialReviewDetails
                                                         WHERE ApplicationId = @applicationId
                                                            AND apply.DeletedAt IS NULL",
                        new
                        {
                            applicationId,
                            financialReviewDetails
                        });
                }
                return true;
        }

        public async Task<bool> IsUkprnWhitelisted(int ukprn)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return await connection.QuerySingleAsync<bool>(@"SELECT
                                                                      CASE WHEN EXISTS 
                                                                      (
                                                                            SELECT UKPRN FROM dbo.WhitelistedProviders WHERE UKPRN = @ukprn
                                                                      )
                                                                      THEN 'TRUE'
                                                                      ELSE 'FALSE'
                                                                  END",
                                                                  new { ukprn });
            }
        }



        // NOTE: This is old stuff or things which are not migrated over yet
        public async Task UpdateApplicationStatus(Guid applicationId, string status)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE Apply
                                                SET  ApplicationStatus = @status                                                
                                                WHERE ApplicationId = @ApplicationId", new {applicationId, status});
            }
        }

        public async Task<List<RoatpApplicationSummaryItem>> GetOpenApplications()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .QueryAsync<RoatpApplicationSummaryItem>(
                        @"SELECT 
                            apply.Id AS Id,
                            apply.ApplicationId AS ApplicationId,
                            apply.ApplicationStatus AS ApplicationStatus,
                            apply.GatewayReviewStatus AS GatewayReviewStatus,
                            apply.AssessorReviewStatus AS AssessorReviewStatus,
                            apply.FinancialReviewStatus AS FinancialReviewStatus,
                            org.Name AS OrganisationName,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName') AS ApplicationRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS SubmittedDate
	                      FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId
	                      WHERE apply.ApplicationStatus = @applicationStatusGatewayAssessed AND apply.DeletedAt IS NULL
	                        AND apply.GatewayReviewStatus = @gatewayReviewStatusApproved",
                        new
                        {
                            applicationStatusGatewayAssessed = ApplicationStatus.GatewayAssessed,
                            gatewayReviewStatusApproved = GatewayReviewStatus.Pass
                        })).ToList();
            }
        }

        public async Task<List<RoatpApplicationSummaryItem>> GetFeedbackAddedApplications()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return(await connection
                    .QueryAsync<RoatpApplicationSummaryItem>(
                        @"SELECT 
                            apply.Id AS Id,
                            apply.ApplicationId AS ApplicationId,
                            apply.ApplicationStatus AS ApplicationStatus,
                            apply.GatewayReviewStatus AS GatewayReviewStatus,
                            apply.AssessorReviewStatus AS AssessorReviewStatus,
                            apply.FinancialReviewStatus AS FinancialReviewStatus,
                            org.Name AS OrganisationName,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName') AS ApplicationRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS SubmittedDate
	                      FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId
	                      WHERE apply.ApplicationStatus = @applicationStatusFeedbackAdded AND apply.DeletedAt IS NULL",
                        new
                        {
                            applicationStatusFeedbackAdded = ApplicationStatus.FeedbackAdded
                        })).ToList();
            }
        }

        public async Task<List<RoatpApplicationSummaryItem>> GetClosedApplications()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .QueryAsync<RoatpApplicationSummaryItem>(
                        @"SELECT 
                            apply.Id AS Id,
                            apply.ApplicationId AS ApplicationId,
                            apply.ApplicationStatus AS ApplicationStatus,
                            apply.GatewayReviewStatus AS GatewayReviewStatus,
                            apply.AssessorReviewStatus AS AssessorReviewStatus,
                            apply.FinancialReviewStatus AS FinancialReviewStatus,
                            org.Name AS OrganisationName,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName') AS ApplicationRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS SubmittedDate
	                      FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId	
	                        WHERE ApplicationStatus IN ( @applicationStatusApproved, @applicationStatusDeclined ) AND apply.DeletedAt IS NULL",
                        new
                        {
                            applicationStatusApproved = ApplicationStatus.Approved,
                            applicationStatusDeclined = ApplicationStatus.Rejected
                        })).ToList();
            }
        }

        public async Task<Contact> GetContactForApplication(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<Contact>(@"SELECT con.* FROM Contacts con 
                                                                        INNER JOIN Apply appl ON appl.CreatedBy = con.Id
                                                                        WHERE appl.ApplicationId = @ApplicationId",
                    new { applicationId });
            }
        }

        public async Task<Organisation> GetOrganisationForApplication(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<Organisation>(@"SELECT org.* FROM Organisations org 
                                                                        INNER JOIN Apply appl ON appl.OrganisationId = org.Id
                                                                        WHERE appl.ApplicationId = @ApplicationId",
                    new {applicationId});
            }
        }

        public async Task<List<ApplicationOversightDetails>> GetOversightsPending()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<ApplicationOversightDetails>(@"SELECT 
                            apply.Id AS Id,
                            apply.ApplicationId AS ApplicationId,
							 org.Name AS OrganisationName,
					        JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            REPLACE(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName'),' provider','') AS ProviderRoute,
							JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS ApplicationSubmittedDate,
							apply.OversightStatus,
							Apply.ApplicationDeterminedDate
                              FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId
	                      WHERE apply.DeletedAt IS NULL
                          and ((GatewayReviewStatus  in (@gatewayReviewStatusPass)
						  and AssessorReviewStatus in (@assessorReviewStatusApproved,@assessorReviewStatusDeclined)
						  and FinancialReviewStatus in (@financialReviewStatusApproved,@financialReviewStatusDeclined, @financialReviewStatusExempt)) 
                            OR GatewayReviewStatus in (@gatewayReviewStatusFail, @gatewayReviewStatusReject))
                            and apply.OversightStatus NOT IN (@oversightReviewStatusPass,@oversightReviewStatusFail)
                            order by CAST(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS DATE) ASC,  Org.Name ASC", new
                        {
                            gatewayReviewStatusPass = GatewayReviewStatus.Pass,
                            gatewayReviewStatusFail = GatewayReviewStatus.Fail,
                            GatewayReviewStatusReject = GatewayReviewStatus.Reject,
                            assessorReviewStatusApproved = AssessorReviewStatus.Approved,
                            assessorReviewStatusDeclined = AssessorReviewStatus.Declined,
                            financialReviewStatusApproved = FinancialReviewStatus.Pass,
                            financialReviewStatusDeclined = FinancialReviewStatus.Fail,
                            financialReviewStatusExempt = FinancialReviewStatus.Exempt,
                            oversightReviewStatusPass= OversightReviewStatus.Successful,
                            oversightReviewStatusFail = OversightReviewStatus.Unsuccessful

                        })).ToList();
            }
        }


        public async Task<List<ApplicationOversightDetails>> GetOversightsCompleted()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<ApplicationOversightDetails>(@"SELECT 
                            apply.Id AS Id,
                            apply.ApplicationId AS ApplicationId,
							 org.Name AS OrganisationName,
					        JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            REPLACE(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName'),' provider','') AS ProviderRoute,
							JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS ApplicationSubmittedDate,
							apply.OversightStatus,
							Apply.ApplicationDeterminedDate
                              FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId
	                      WHERE apply.DeletedAt IS NULL
                          and ((GatewayReviewStatus  in (@gatewayReviewStatusPass)
						  and AssessorReviewStatus in (@assessorReviewStatusApproved,@assessorReviewStatusDeclined)
						  and FinancialReviewStatus in (@financialReviewStatusApproved,@financialReviewStatusDeclined, @financialReviewStatusExempt))
                            OR GatewayReviewStatus in (@gatewayReviewStatusFail, @gatewayReviewStatusReject))

						  and apply.OversightStatus IN (@oversightReviewStatusPass,@oversightReviewStatusFail) 
                             order by cast(Apply.ApplicationDeterminedDate as DATE) ASC, CAST(JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS DATE) ASC,  Org.Name ASC", new
                    {
                        gatewayReviewStatusPass = GatewayReviewStatus.Pass,
                        gatewayReviewStatusFail = GatewayReviewStatus.Fail,
                        GatewayReviewStatusReject = GatewayReviewStatus.Reject,
                        assessorReviewStatusApproved = AssessorReviewStatus.Approved,
                        assessorReviewStatusDeclined = AssessorReviewStatus.Declined,
                        financialReviewStatusApproved = FinancialReviewStatus.Pass,
                        financialReviewStatusDeclined = FinancialReviewStatus.Fail,
                        financialReviewStatusExempt = FinancialReviewStatus.Exempt,
                        oversightReviewStatusPass = OversightReviewStatus.Successful,
                        oversightReviewStatusFail = OversightReviewStatus.Unsuccessful

                    })).ToList();
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
							apply.OversightStatus,
							Apply.ApplicationDeterminedDate
                              FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId
                        WHERE apply.ApplicationId = @applicationId",
                    new { applicationId });

                return applyDataResults.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<GatewayApplicationStatusCount>> GetGatewayApplicationStatusCounts()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var applicationStatuses = await connection.QueryAsync<GatewayApplicationStatusCount>(
                    @"select
                        a.GatewayReviewStatus as GatewayApplicationStatus,
                        count(1) as 'Count'
                        from Apply a
                        where a.DeletedAt is null
                        group by a.GatewayReviewStatus
                        ");

                return applicationStatuses;
            }
        }

        public async Task<IEnumerable<RoatpApplicationStatus>> GetExistingApplicationStatusByUkprn(string ukprn)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
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
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var nextInSequence = (await connection.QueryAsync<int>(@"SELECT NEXT VALUE FOR RoatpAppReferenceSequence")).FirstOrDefault();

                return $"APR{nextInSequence}";
            }
        }

        public async Task<bool> StartAssessorReview(Guid applicationId, string reviewer)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
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
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var applyDataResults = await connection.QueryAsync<ApplyData>(@"SELECT ApplyData FROM Apply WHERE ApplicationId = @applicationId",
                    new { applicationId });

                return applyDataResults.FirstOrDefault();
            }
        }

        public async Task<bool> UpdateApplyData(Guid applicationId, ApplyData applyData, string updatedBy)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
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

        public async Task<bool> UpdateOversightReviewStatus(Guid applicationId, string oversightStatus, DateTime applicationDeterminedDate, string updatedBy)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE Apply 
                                                SET OversightStatus = @oversightStatus, 
                                                ApplicationDeterminedDate = @applicationDeterminedDate,
                                                UpdatedBy = @updatedBy,
                                                UpdatedAt = @updatedAt
                                                WHERE ApplicationId = @applicationId",
                            new
                            {
                                applicationId,
                                updatedAt = DateTime.UtcNow,
                                updatedBy,
                                oversightStatus,
                                applicationDeterminedDate
                            });
            }

            return await Task.FromResult(true);
        }

    }
}