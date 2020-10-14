using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Data.DapperTypeHandlers;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Data
{
    public class ModeratorRepository : IModeratorRepository
    {
        private readonly IApplyConfig _config;
        private readonly ILogger<ModeratorRepository> _logger;

        public ModeratorRepository(IConfigurationService configurationService, ILogger<ModeratorRepository> logger)
        {
            _logger = logger;
            _config = configurationService.GetConfig().GetAwaiter().GetResult();

            SqlMapper.AddTypeHandler(typeof(ApplyData), new ApplyDataHandler());
        }

        public async Task<BlindAssessmentOutcome> GetBlindAssessmentOutcome(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var blindAssessmentOutcomeResults = await connection.QueryAsync<BlindAssessmentOutcome>(
                                                                @"SELECT outcome.[ApplicationId]
			                                                            ,outcome.[SequenceNumber]
			                                                            ,outcome.[SectionNumber]
			                                                            ,outcome.[PageId]
                                                                        ,apply.[Assessor1Name]
			                                                            ,outcome.[Assessor1UserId]
			                                                            ,outcome.[Assessor1ReviewStatus]
			                                                            ,outcome.[Assessor1ReviewComment]
                                                                        ,apply.[Assessor2Name]
                                                                        ,outcome.[Assessor2UserId]
			                                                            ,outcome.[Assessor2ReviewStatus]
			                                                            ,outcome.[Assessor2ReviewComment]
		                                                            FROM [dbo].[AssessorPageReviewOutcome] outcome
                                                                    INNER JOIN [dbo].[Apply] apply ON outcome.ApplicationId = apply.ApplicationId
		                                                            WHERE outcome.[ApplicationId] = @applicationId AND
				                                                          outcome.[SequenceNumber] = @sequenceNumber AND
				                                                          outcome.[SectionNumber] = @sectionNumber AND
				                                                          outcome.[PageId] = @pageId AND
                                                                          outcome.[Assessor1UserId] = apply.[Assessor1UserId] AND
                                                                          outcome.[Assessor2UserId] = apply.[Assessor2UserId]",
                    new { applicationId, sequenceNumber, sectionNumber, pageId });

                return blindAssessmentOutcomeResults.FirstOrDefault();
            }
        }

        public async Task<ModeratorPageReviewOutcome> GetModeratorPageReviewOutcome(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var pageReviewOutcomeResults = await connection.QueryAsync<ModeratorPageReviewOutcome>(
                                                                @"SELECT [ApplicationId]
			                                                            ,[SequenceNumber]
			                                                            ,[SectionNumber]
			                                                            ,[PageId]
			                                                            ,[ModeratorUserId] AS UserId
			                                                            ,[ModeratorReviewStatus] AS [Status]
			                                                            ,[ModeratorReviewComment] AS Comment
		                                                            FROM [dbo].[ModeratorPageReviewOutcome]
		                                                            WHERE [ApplicationId] = @applicationId AND
				                                                        [SequenceNumber] = @sequenceNumber AND
				                                                        [SectionNumber] = @sectionNumber AND
				                                                        [PageId] = @pageId",
                    new { applicationId, sequenceNumber, sectionNumber, pageId });

                return pageReviewOutcomeResults.FirstOrDefault();
            }
        }

        public async Task<List<ModeratorPageReviewOutcome>> GetModeratorPageReviewOutcomesForSection(Guid applicationId, int sequenceNumber, int sectionNumber)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var pageReviewOutcomeResults = await connection.QueryAsync<ModeratorPageReviewOutcome>(
                                                                @"SELECT [ApplicationId]
			                                                            ,[SequenceNumber]
			                                                            ,[SectionNumber]
			                                                            ,[PageId]
			                                                            ,[ModeratorUserId] AS UserId
			                                                            ,[ModeratorReviewStatus] AS [Status]
			                                                            ,[ModeratorReviewComment] AS Comment
		                                                            FROM [dbo].[ModeratorPageReviewOutcome]
		                                                            WHERE [ApplicationId] = @applicationId AND
				                                                        [SequenceNumber] = @sequenceNumber AND
				                                                        [SectionNumber] = @sectionNumber",
                    new { applicationId, sequenceNumber, sectionNumber });

                return pageReviewOutcomeResults.ToList();
            }
        }

        public async Task<List<ModeratorPageReviewOutcome>> GetAllModeratorPageReviewOutcomes(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var pageReviewOutcomeResults = await connection.QueryAsync<ModeratorPageReviewOutcome>(
                                                                @"SELECT [ApplicationId]
			                                                            ,[SequenceNumber]
			                                                            ,[SectionNumber]
			                                                            ,[PageId]
			                                                            ,[ModeratorUserId] AS UserId
			                                                            ,[ModeratorReviewStatus] AS [Status]
			                                                            ,[ModeratorReviewComment] AS Comment
		                                                            FROM [dbo].[ModeratorPageReviewOutcome]
		                                                            WHERE [ApplicationId] = @applicationId",
                    new { applicationId });

                return pageReviewOutcomeResults.ToList();
            }
        }

        public async Task SubmitModeratorPageOutcome(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId, string userId, string status, string comment)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                // NOTE: CreateEmptyModeratorReview should have been called before getting to this point.
                // This is so that all PageReviewOutcomes are initialized for the Moderator
                await connection.ExecuteAsync(
                    @"UPDATE [ModeratorPageReviewOutcome]
			            SET [ModeratorUserId] = @userId
                            , [ModeratorReviewStatus] = @status
				            , [ModeratorReviewComment] = @comment
				            , [UpdatedAt] = GETUTCDATE()
				            , [UpdatedBy] = @userId
			            WHERE [ApplicationId] = @applicationId AND
					          [SequenceNumber] = @sequenceNumber AND
					          [SectionNumber] = @sectionNumber AND
					          [PageId] = @pageId",
                    new { applicationId, sequenceNumber, sectionNumber, pageId, userId, status, comment });

                // APR-1633 - Update Moderation Status from 'New' to 'In Moderation'
                await connection.ExecuteAsync(
                    @"UPDATE [Apply]
			            SET ModerationStatus = @inModerationStatus
                            , UpdatedAt = GETUTCDATE()
				            , UpdatedBy = @userId
			            WHERE ApplicationId = @applicationId AND DeletedAt IS NULL
                              AND ModerationStatus = @newStatus",
                    new { applicationId, userId, inModerationStatus = ModerationStatus.InProgress, newStatus = ModerationStatus.New });
            }
        }

        public async Task CreateEmptyModeratorReview(Guid applicationId, string userId, List<ModeratorPageReviewOutcome> pageReviewOutcomes)
        {
            var createdAtDateTime = DateTime.UtcNow;

            var dataTable = new DataTable();
            dataTable.Columns.Add("ApplicationId", typeof(Guid));
            dataTable.Columns.Add("SequenceNumber", typeof(int));
            dataTable.Columns.Add("SectionNumber", typeof(int));
            dataTable.Columns.Add("PageId", typeof(string));
            dataTable.Columns.Add("ModeratorUserId", typeof(string));
            dataTable.Columns.Add("CreatedAt", typeof(DateTime));
            dataTable.Columns.Add("CreatedBy", typeof(string));

            foreach (var outcome in pageReviewOutcomes)
            {
                dataTable.Rows.Add(
                    applicationId,
                    outcome.SequenceNumber,
                    outcome.SectionNumber,
                    outcome.PageId,
                    userId,
                    createdAtDateTime,
                    userId
                );
            }

            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.OpenAsync();
                using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, null))
                {
                    bulkCopy.DestinationTableName = "ModeratorPageReviewOutcome";
                    foreach (DataColumn col in dataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }

                    await bulkCopy.WriteToServerAsync(dataTable);
                }
                connection.Close();
            }
        }

        public async Task<bool> SubmitModeratorOutcome(Guid applicationId, ApplyData applyData, string userId,string status)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var rowsAffected = await connection.ExecuteAsync(@"UPDATE Apply
                                                SET ModerationStatus = @status, 
                                                    ApplyData = @applyData,
                                                    UpdatedBy = @userId, 
                                                    UpdatedAt = GETUTCDATE() 
                                                WHERE  (Apply.ApplicationId = @applicationId)",
                    new
                    {
                        applicationId,
                        status,
                        applyData,
                        userId
                    });

                return rowsAffected > 0;
            }
        }
    }
}
