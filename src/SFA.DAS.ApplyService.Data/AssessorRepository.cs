using Dapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;

namespace SFA.DAS.ApplyService.Data
{
    public class AssessorRepository : IAssessorRepository
    {
        private readonly IApplyConfig _config;
        private readonly ILogger<AssessorRepository> _logger;

        private const string ApplicationSummaryFields = @"ApplicationId,
                            org.Name AS OrganisationName,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName') AS ProviderRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS SubmittedDate,
                            Assessor1Name,
                            Assessor2Name,
                            Assessor1UserId,
                            Assessor2UserId
            ";

        private const string NewApplicationsWhereClause = @"
                            apply.DeletedAt IS NULL AND apply.GatewayReviewStatus = @gatewayReviewStatusApproved
                            -- Not assigned to current user
                            AND ISNULL(apply.Assessor1UserId, '') <> @userId AND ISNULL(apply.Assessor2UserId, '') <> @userId
                            -- Can be assigned to at least one assessor
                            AND (apply.Assessor1UserId IS NULL OR apply.Assessor2UserId IS NULL)
                            ";

        private const string InProgressApplicationsWhereClause = @"
                            apply.DeletedAt IS NULL AND apply.GatewayReviewStatus = @gatewayReviewStatusApproved
                            AND
                            (
                                -- Assigned to the current user and in progress
                                (apply.Assessor1ReviewStatus = @inProgressReviewStatus AND apply.Assessor1UserId = @userId) OR (apply.Assessor2ReviewStatus = @inProgressReviewStatus AND apply.Assessor2UserId = @userId)
                                OR
                                -- Assigned to any two other assessors and in progress
                                (apply.Assessor1UserId IS NOT NULL AND apply.Assessor2UserId IS NOT NULL AND (apply.Assessor1ReviewStatus = @inProgressReviewStatus OR Assessor2ReviewStatus = @inProgressReviewStatus))
                            )";

        public AssessorRepository(IConfigurationService configurationService, ILogger<AssessorRepository> logger)
        {
            _logger = logger;
            _config = configurationService.GetConfig().GetAwaiter().GetResult();
        }

        public async Task<List<AssessorApplicationSummary>> GetNewAssessorApplications(string userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .QueryAsync<AssessorApplicationSummary>(
                        $@"SELECT 
                            {ApplicationSummaryFields}
	                       FROM Apply apply
	                       INNER JOIN Organisations org ON org.Id = apply.OrganisationId
	                       WHERE {NewApplicationsWhereClause}
                           ORDER BY CONVERT(char(10), JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn')) ASC, org.Name ASC",
                        new
                        {
                            gatewayReviewStatusApproved = GatewayReviewStatus.Pass,
                            userId = userId
                        })).ToList();
            }
        }

        public async Task<int> GetNewAssessorApplicationsCount(string userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .ExecuteScalarAsync<int>(
                        $@"SELECT COUNT(1)
	                      FROM Apply apply
	                      WHERE {NewApplicationsWhereClause}",
                        new
                        {
                            gatewayReviewStatusApproved = GatewayReviewStatus.Pass,
                            userId = userId
                        }));
            }
        }

        public async Task UpdateAssessor1(Guid applicationId, string userId, string userName)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE Apply SET Assessor1UserId = @userId, Assessor1Name = @userName, Assessor1ReviewStatus = @inProgressReviewStatus
                                                Where ApplicationId = @applicationId",
                    new
                    {
                        applicationId,
                        userId,
                        userName,
                        inProgressReviewStatus = AssessorReviewStatus.InProgress
                    });
            }
        }

        public async Task UpdateAssessor2(Guid applicationId, string userId, string userName)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE Apply SET Assessor2UserId = @userId, Assessor2Name = @userName, Assessor2ReviewStatus = @inProgressReviewStatus
                                                Where ApplicationId = @applicationId",
                    new
                    {
                        applicationId,
                        userId,
                        userName,
                        inProgressReviewStatus = AssessorReviewStatus.InProgress
                    });
            }
        }

        public async Task<List<AssessorApplicationSummary>> GetInProgressAssessorApplications(string userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .QueryAsync<AssessorApplicationSummary>(
                        $@"SELECT 
                            {ApplicationSummaryFields}
	                        FROM Apply apply
	                        INNER JOIN Organisations org ON org.Id = apply.OrganisationId
	                        WHERE {InProgressApplicationsWhereClause}
                            ORDER BY CONVERT(char(10), JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn')) ASC, org.Name ASC",
                        new
                        {
                            gatewayReviewStatusApproved = GatewayReviewStatus.Pass,
                            inProgressReviewStatus = AssessorReviewStatus.InProgress,
                            userId = userId
                        })).ToList();
            }
        }

        public async Task<int> GetInProgressAssessorApplicationsCount(string userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .ExecuteScalarAsync<int>(
                        $@"SELECT COUNT(1)
	                      FROM Apply apply
	                      WHERE {InProgressApplicationsWhereClause}",
                        new
                        {
                            gatewayReviewStatusApproved = GatewayReviewStatus.Pass,
                            inProgressReviewStatus = AssessorReviewStatus.InProgress,
                            userId = userId
                        }));
            }
        }

        public async Task<List<ModerationApplicationSummary>> GetApplicationsInModeration()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .QueryAsync<ModerationApplicationSummary>(
                        $@"SELECT 
                            {ApplicationSummaryFields}
                            , ModerationStatus
	                        FROM Apply apply
	                        INNER JOIN Organisations org ON org.Id = apply.OrganisationId
	                        WHERE apply.DeletedAt IS NULL AND (Assessor1ReviewStatus = @approvedReviewStatus AND Assessor2ReviewStatus = @approvedReviewStatus) AND ISNULL(ModerationStatus, 'New')  <> @completedModerationStatus
                            ORDER BY CONVERT(char(10), JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn')) ASC, org.Name ASC",
                        new
                        {
                            approvedReviewStatus = AssessorReviewStatus.Approved,
                            completedModerationStatus = ModerationStatus.Complete
                        })).ToList();
            }
        }

        public async Task<int> GetApplicationsInModerationCount()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .ExecuteScalarAsync<int>(
                        $@"SELECT COUNT(1)
	                      FROM Apply apply
	                      WHERE apply.DeletedAt IS NULL AND (Assessor1ReviewStatus = @approvedReviewStatus AND Assessor2ReviewStatus = @approvedReviewStatus) AND ISNULL(ModerationStatus, 'New') <> @completedModerationStatus",
                        new
                        {
                            approvedReviewStatus = AssessorReviewStatus.Approved,
                            completedModerationStatus = ModerationStatus.Complete
                        }));
            }
        }

        private async Task<int> GetAssessorNumber(Guid applicationId, string userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return await connection.ExecuteScalarAsync<int>(
                        $@"SELECT
                                CASE WHEN (Assessor1UserId = @userId) THEN 1
                                     WHEN (Assessor2UserId = @userId) THEN 2
                                     ELSE 0
                                END
                           FROM [Apply]
                           WHERE [ApplicationId] = @applicationId",
                        new
                        {
                            userId = userId,
                            applicationId = applicationId
                        });
            }
        }

        public async Task SubmitAssessorPageOutcome(Guid applicationId,
                                                    int sequenceNumber,
                                                    int sectionNumber,
                                                    string pageId,
                                                    string userId,
                                                    string status,
                                                    string comment)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var assessorNumber = await GetAssessorNumber(applicationId, userId);

                await connection.ExecuteAsync(
                    @"IF (@assessorNumber = 1)
                        BEGIN
                            IF NOT EXISTS (SELECT * FROM [AssessorPageReviewOutcome]
							                        WHERE [ApplicationId] = @applicationId AND
									                        [SequenceNumber] = @sequenceNumber AND
									                        [SectionNumber] = @sectionNumber AND
									                        [PageId] = @pageId)
		                        BEGIN
			                        INSERT INTO [dbo].[AssessorPageReviewOutcome]
					                           ([ApplicationId]
					                           ,[SequenceNumber]
					                           ,[SectionNumber]
					                           ,[PageId]
					                           ,[Assessor1UserId]
					                           ,[Assessor1ReviewStatus]
					                           ,[Assessor1ReviewComment]
					                           ,[CreatedBy])
				                         VALUES
					                           (@applicationId
					                           ,@sequenceNumber
					                           ,@sectionNumber
					                           ,@pageId
					                           ,@userId
					                           ,@status
					                           ,@comment
					                           ,@userId)                     
		                        END
                             ELSE
		                        BEGIN
			                        UPDATE [AssessorPageReviewOutcome]
			                           SET [Assessor1UserId] = @userId
                                          ,[Assessor1ReviewStatus] = @status
				                          ,[Assessor1ReviewComment] = @comment
				                          ,[UpdatedAt] = GETUTCDATE()
				                          ,[UpdatedBy] = @userId
			                        WHERE [ApplicationId] = @applicationId AND
					                        [SequenceNumber] = @sequenceNumber AND
					                        [SectionNumber] = @sectionNumber AND
					                        [PageId] = @pageId
		                        END                                                         
                        END
                      IF (@assessorNumber = 2)
                        BEGIN
                             IF NOT EXISTS (SELECT * FROM [AssessorPageReviewOutcome]
							                        WHERE [ApplicationId] = @applicationId AND
									                        [SequenceNumber] = @sequenceNumber AND
									                        [SectionNumber] = @sectionNumber AND
									                        [PageId] = @pageId)
		                        BEGIN
			                        INSERT INTO [dbo].[AssessorPageReviewOutcome]
					                           ([ApplicationId]
					                           ,[SequenceNumber]
					                           ,[SectionNumber]
					                           ,[PageId]
					                           ,[Assessor2UserId]
					                           ,[Assessor2ReviewStatus]
					                           ,[Assessor2ReviewComment]
					                           ,[CreatedBy])
				                         VALUES
					                           (@applicationId
					                           ,@sequenceNumber
					                           ,@sectionNumber
					                           ,@pageId
					                           ,@userId
					                           ,@status
					                           ,@comment
					                           ,@userId)                     
		                        END
                             ELSE
		                        BEGIN
			                        UPDATE [AssessorPageReviewOutcome]
			                           SET [Assessor2UserId] = @userId
                                          ,[Assessor2ReviewStatus] = @status
				                          ,[Assessor2ReviewComment] = @comment
				                          ,[UpdatedAt] = GETUTCDATE()
				                          ,[UpdatedBy] = @userId
			                        WHERE [ApplicationId] = @applicationId AND
					                        [SequenceNumber] = @sequenceNumber AND
					                        [SectionNumber] = @sectionNumber AND
					                        [PageId] = @pageId
		                        END                   
                        END",
                    new { applicationId, sequenceNumber, sectionNumber, pageId, assessorNumber, userId, status, comment });
            }
        }

        public async Task<AssessorPageReviewOutcome> GetAssessorPageReviewOutcome(Guid applicationId,
                                                                    int sequenceNumber,
                                                                    int sectionNumber,
                                                                    string pageId,
                                                                    string userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var assessorNumber = await GetAssessorNumber(applicationId, userId);

                var pageReviewOutcomeResults = await connection.QueryAsync<AssessorPageReviewOutcome>(
                                                                @"IF (@assessorNumber = 1)
	                                                                BEGIN
		                                                                SELECT [ApplicationId]
			                                                                  ,[SequenceNumber]
			                                                                  ,[SectionNumber]
			                                                                  ,[PageId]
			                                                                  ,@assessorNumber AS AssessorNumber
			                                                                  ,[Assessor1UserId] AS UserId
			                                                                  ,[Assessor1ReviewStatus] AS [Status]
			                                                                  ,[Assessor1ReviewComment] AS Comment
		                                                                  FROM [dbo].[AssessorPageReviewOutcome]
		                                                                  WHERE [ApplicationId] = @applicationId AND
				                                                                [SequenceNumber] = @sequenceNumber AND
				                                                                [SectionNumber] = @sectionNumber AND
				                                                                [PageId] = @pageId AND
				                                                                [Assessor1UserId] = @userId                                                        
	                                                                END
                                                                IF (@assessorNumber = 2)
	                                                                BEGIN
		                                                                SELECT [ApplicationId]
			                                                                  ,[SequenceNumber]
			                                                                  ,[SectionNumber]
			                                                                  ,[PageId]
			                                                                  ,@assessorNumber AS AssessorNumber
			                                                                  ,[Assessor2UserId] AS UserId
			                                                                  ,[Assessor2ReviewStatus] AS [Status]
			                                                                  ,[Assessor2ReviewComment] AS Comment
		                                                                  FROM [dbo].[AssessorPageReviewOutcome]
		                                                                  WHERE [ApplicationId] = @applicationId AND
				                                                                [SequenceNumber] = @sequenceNumber AND
				                                                                [SectionNumber] = @sectionNumber AND
				                                                                [PageId] = @pageId AND
				                                                                [Assessor2UserId] = @userId                      
	                                                                END",
                    new { applicationId, sequenceNumber, sectionNumber, pageId, assessorNumber, userId });

                return pageReviewOutcomeResults.FirstOrDefault();
            }
        }

        public async Task<List<AssessorPageReviewOutcome>> GetAssessorPageReviewOutcomesForSection(Guid applicationId,
                                                            int sequenceNumber,
                                                            int sectionNumber,
                                                            string userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var assessorNumber = await GetAssessorNumber(applicationId, userId);

                var pageReviewOutcomeResults = await connection.QueryAsync<AssessorPageReviewOutcome>(
                                                                @"IF (@assessorNumber = 1)
	                                                                BEGIN
		                                                                SELECT [ApplicationId]
			                                                                  ,[SequenceNumber]
			                                                                  ,[SectionNumber]
			                                                                  ,[PageId]
			                                                                  ,@assessorNumber AS AssessorNumber
			                                                                  ,[Assessor1UserId] AS UserId
			                                                                  ,[Assessor1ReviewStatus] AS [Status]
			                                                                  ,[Assessor1ReviewComment] AS Comment
		                                                                  FROM [dbo].[AssessorPageReviewOutcome]
		                                                                  WHERE [ApplicationId] = @applicationId AND
				                                                                [SequenceNumber] = @sequenceNumber AND
				                                                                [SectionNumber] = @sectionNumber AND
				                                                                [Assessor1UserId] = @userId                                                        
	                                                                END
                                                                IF (@assessorNumber = 2)
	                                                                BEGIN
		                                                                SELECT [ApplicationId]
			                                                                  ,[SequenceNumber]
			                                                                  ,[SectionNumber]
			                                                                  ,[PageId]
			                                                                  ,@assessorNumber AS AssessorNumber
			                                                                  ,[Assessor2UserId] AS UserId
			                                                                  ,[Assessor2ReviewStatus] AS [Status]
			                                                                  ,[Assessor2ReviewComment] AS Comment
		                                                                  FROM [dbo].[AssessorPageReviewOutcome]
		                                                                  WHERE [ApplicationId] = @applicationId AND
				                                                                [SequenceNumber] = @sequenceNumber AND
				                                                                [SectionNumber] = @sectionNumber AND
				                                                                [Assessor2UserId] = @userId                      
	                                                                END",
                    new { applicationId, sequenceNumber, sectionNumber, assessorNumber, userId });

                return pageReviewOutcomeResults.ToList();
            }
        }

        public async Task<List<AssessorPageReviewOutcome>> GetAllAssessorPageReviewOutcomes(Guid applicationId, string userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var assessorNumber = await GetAssessorNumber(applicationId, userId);

                var pageReviewOutcomeResults = await connection.QueryAsync<AssessorPageReviewOutcome>(
                                                                @"IF (@assessorNumber = 1)
	                                                                BEGIN
		                                                                SELECT [ApplicationId]
			                                                                  ,[SequenceNumber]
			                                                                  ,[SectionNumber]
			                                                                  ,[PageId]
			                                                                  ,@assessorNumber AS AssessorNumber
			                                                                  ,[Assessor1UserId] AS UserId
			                                                                  ,[Assessor1ReviewStatus] AS [Status]
			                                                                  ,[Assessor1ReviewComment] AS Comment
		                                                                  FROM [dbo].[AssessorPageReviewOutcome]
		                                                                  WHERE [ApplicationId] = @applicationId AND
				                                                                [Assessor1UserId] = @userId                                                        
	                                                                END
                                                                IF (@assessorNumber = 2)
	                                                                BEGIN
		                                                                SELECT [ApplicationId]
			                                                                  ,[SequenceNumber]
			                                                                  ,[SectionNumber]
			                                                                  ,[PageId]
			                                                                  ,@assessorNumber AS AssessorNumber
			                                                                  ,[Assessor2UserId] AS UserId
			                                                                  ,[Assessor2ReviewStatus] AS [Status]
			                                                                  ,[Assessor2ReviewComment] AS Comment
		                                                                  FROM [dbo].[AssessorPageReviewOutcome]
		                                                                  WHERE [ApplicationId] = @applicationId AND
				                                                                [Assessor2UserId] = @userId                      
	                                                                END",
                    new { applicationId, assessorNumber, userId });

                return pageReviewOutcomeResults.ToList();
            }
        }

		public async Task UpdateAssessorReviewStatus(Guid applicationId, string userId, string status)
		{
			using (var connection = new SqlConnection(_config.SqlConnectionString))
			{
                var assessorNumber = await GetAssessorNumber(applicationId, userId);

                await connection.ExecuteAsync(
                    @"IF (@assessorNumber = 1)
                        BEGIN
		                    UPDATE [Apply]
			                        SET Assessor1ReviewStatus = @status
                                        , UpdatedAt = GETUTCDATE()
				                        , UpdatedBy = @userId
			                        WHERE ApplicationId = @applicationId AND DeletedAt IS NULL AND Assessor1UserId = @userId
                        END
                      IF (@assessorNumber = 2)
                        BEGIN
		                    UPDATE [Apply]
			                        SET Assessor2ReviewStatus = @status
                                        , UpdatedAt = GETUTCDATE()
				                        , UpdatedBy = @userId
			                        WHERE ApplicationId = @applicationId AND DeletedAt IS NULL AND Assessor2UserId = @userId                
                        END",
					new { applicationId, assessorNumber, userId, status });
            }
		}

        public async Task CreateEmptyAssessorReview(Guid applicationId, string userId, List<AssessorPageReviewOutcome> pageReviewOutcomes)
        {
            var assessorNumber = await GetAssessorNumber(applicationId, userId);

            var dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(Guid));
            dataTable.Columns.Add("ApplicationId", typeof(Guid));
            dataTable.Columns.Add("SequenceNumber", typeof(int));
            dataTable.Columns.Add("SectionNumber", typeof(int));
            dataTable.Columns.Add("PageId", typeof(string));
            dataTable.Columns.Add("Assessor1UserId", typeof(string));
            dataTable.Columns.Add("Assessor1ReviewStatus", typeof(string));
            dataTable.Columns.Add("Assessor1ReviewComment", typeof(string));
            dataTable.Columns.Add("Assessor2UserId", typeof(string));
            dataTable.Columns.Add("Assessor2ReviewStatus", typeof(string));
            dataTable.Columns.Add("Assessor2ReviewComment", typeof(string));
            dataTable.Columns.Add("CreatedAt", typeof(DateTime));
            dataTable.Columns.Add("CreatedBy", typeof(string));

            foreach (var outcome in pageReviewOutcomes)
            {
                dataTable.Rows.Add(Guid.NewGuid(),
                    applicationId,
                    outcome.SequenceNumber,
                    outcome.SectionNumber,
                    outcome.PageId,
                    assessorNumber == 1 ? userId : null,
                    null,
                    null,
                    assessorNumber == 2 ? userId : null,
                    null,
                    null,
                    DateTime.UtcNow,
                    userId
                );
            }

            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.OpenAsync();
                using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, null))
                {
                    bulkCopy.DestinationTableName = "AssessorPageReviewOutcome";
                    await bulkCopy.WriteToServerAsync(dataTable);
                }
                connection.Close();
            }
        }
    }
}