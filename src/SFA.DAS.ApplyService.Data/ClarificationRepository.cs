using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;

namespace SFA.DAS.ApplyService.Data
{
    public class ClarificationRepository : IClarificationRepository
    {
        private readonly IApplyConfig _config;

        public ClarificationRepository(IConfigurationService configurationService)
        {
            _config = configurationService.GetConfig().GetAwaiter().GetResult();
        }

        public async Task<ModerationOutcome> GetModerationOutcome(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var moderationOutcomeResults = await connection.QueryAsync<ModerationOutcome>(
                                                                @"SELECT  outcome.[ApplicationId]
			                                                            , outcome.[SequenceNumber]
			                                                            , outcome.[SectionNumber]
			                                                            , outcome.[PageId]
                                                                        , CASE 
                                                                            WHEN outcome.[ModeratorUserId] = apply.[Assessor1UserId] THEN apply.[Assessor1Name]
                                                                            WHEN outcome.[ModeratorUserId] = apply.[Assessor2UserId] THEN apply.[Assessor2Name]
                                                                            ELSE [ModeratorUserId]
                                                                          END AS ModeratorName
			                                                            , outcome.[ModeratorUserId]
			                                                            , outcome.[ModeratorReviewStatus]
			                                                            , outcome.[ModeratorReviewComment]
		                                                            FROM  [dbo].[ModeratorPageReviewOutcome] outcome
                                                                    INNER JOIN [dbo].[Apply] apply ON outcome.[ApplicationId] = apply.ApplicationId
		                                                            WHERE outcome.[ApplicationId] = @applicationId AND
				                                                          outcome.[SequenceNumber] = @sequenceNumber AND
				                                                          outcome.[SectionNumber] = @sectionNumber AND
				                                                          outcome.[PageId] = @pageId",
                    new { applicationId, sequenceNumber, sectionNumber, pageId });

                return moderationOutcomeResults.FirstOrDefault();
            }
        }

        public async Task<List<ClarificationPageReviewOutcome>> GetClarificationPageReviewOutcomesForSection(Guid applicationId, int sequenceNumber, int sectionNumber)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var pageReviewOutcomeResults = await connection.QueryAsync<ClarificationPageReviewOutcome>(
                                                                @"SELECT [ApplicationId]
			                                                            ,[SequenceNumber]
			                                                            ,[SectionNumber]
			                                                            ,[PageId]
			                                                            ,[ModeratorUserId]
			                                                            ,[ModeratorReviewStatus]
			                                                            ,[ModeratorReviewComment]
			                                                            ,[ClarificationUserId] AS UserId
                                                                        ,CASE WHEN ([ClarificationStatus] IS NULL AND [ModeratorReviewStatus] = @passModeratorStatus) THEN @passModeratorStatus
                                                                              ELSE [ClarificationStatus]
                                                                         END AS [Status]
			                                                            ,[ClarificationComment] AS Comment
			                                                            ,[ClarificationResponse]
		                                                            FROM [dbo].[ModeratorPageReviewOutcome]
		                                                            WHERE [ApplicationId] = @applicationId AND
				                                                        [SequenceNumber] = @sequenceNumber AND
				                                                        [SectionNumber] = @sectionNumber",
                    new { applicationId, sequenceNumber, sectionNumber, passModeratorStatus = ModerationStatus.Pass });

                return pageReviewOutcomeResults.ToList();
            }
        }

        public async Task<ClarificationPageReviewOutcome> GetClarificationPageReviewOutcome(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var pageReviewOutcomeResults = await connection.QueryAsync<ClarificationPageReviewOutcome>(
                                                                @"SELECT [ApplicationId]
			                                                            ,[SequenceNumber]
			                                                            ,[SectionNumber]
			                                                            ,[PageId]
			                                                            ,[ModeratorUserId]
			                                                            ,[ModeratorReviewStatus]
			                                                            ,[ModeratorReviewComment]
			                                                            ,[ClarificationUserId] AS UserId
                                                                        ,CASE WHEN ([ClarificationStatus] IS NULL AND [ModeratorReviewStatus] = @passModeratorStatus) THEN @passModeratorStatus
                                                                              ELSE [ClarificationStatus]
                                                                         END AS [Status]
			                                                            ,[ClarificationComment] AS Comment
			                                                            ,[ClarificationResponse]
		                                                            FROM [dbo].[ModeratorPageReviewOutcome]
		                                                            WHERE [ApplicationId] = @applicationId AND
				                                                        [SequenceNumber] = @sequenceNumber AND
				                                                        [SectionNumber] = @sectionNumber AND
				                                                        [PageId] = @pageId",
                    new { applicationId, sequenceNumber, sectionNumber, pageId, passModeratorStatus = ModerationStatus.Pass });

                return pageReviewOutcomeResults.FirstOrDefault();
            }
        }

        public async Task<List<ClarificationPageReviewOutcome>> GetAllClarificationPageReviewOutcomes(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var pageReviewOutcomeResults = await connection.QueryAsync<ClarificationPageReviewOutcome>(
                                                                @"SELECT [ApplicationId]
			                                                            ,[SequenceNumber]
			                                                            ,[SectionNumber]
			                                                            ,[PageId]
			                                                            ,[ModeratorUserId]
			                                                            ,[ModeratorReviewStatus]
			                                                            ,[ModeratorReviewComment]
			                                                            ,[ClarificationUserId] AS UserId
                                                                        ,CASE WHEN ([ClarificationStatus] IS NULL AND [ModeratorReviewStatus] = @passModeratorStatus) THEN @passModeratorStatus
                                                                              ELSE [ClarificationStatus]
                                                                         END AS [Status]
			                                                            ,[ClarificationComment] AS Comment
			                                                            ,[ClarificationResponse] AS ClarificationResponse
		                                                            FROM [dbo].[ModeratorPageReviewOutcome]
		                                                            WHERE [ApplicationId] = @applicationId",
                    new { applicationId, passModeratorStatus = ModerationStatus.Pass });

                return pageReviewOutcomeResults.ToList();
            }
        }

        public async Task SubmitClarificationPageOutcome(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId, string userId, string status, string comment, string clarificationResponse)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(
                    @"UPDATE [ModeratorPageReviewOutcome]
			            SET [ClarificationUserId] = @userId
                            , [ClarificationStatus] = @status
				            , [ClarificationComment] = @comment
                            , [ClarificationResponse] = @clarificationResponse
                            , [ClarificationUpdatedAt] = GETUTCDATE()
				            , [UpdatedAt] = GETUTCDATE()
				            , [UpdatedBy] = @userId
			            WHERE [ApplicationId] = @applicationId AND
					          [SequenceNumber] = @sequenceNumber AND
					          [SectionNumber] = @sectionNumber AND
					          [PageId] = @pageId",
                    new { applicationId, sequenceNumber, sectionNumber, pageId, userId, status, comment, clarificationResponse });

                /*
                // Future Work - Update Moderation Status from 'Clarification Sent' to 'In Clarification'
                await connection.ExecuteAsync(
                    @"UPDATE [Apply]
			            SET ModerationStatus = @clarificationInProgressStatus
                            , UpdatedAt = GETUTCDATE()
				            , UpdatedBy = @userId
			            WHERE ApplicationId = @applicationId AND DeletedAt IS NULL
                              AND ModerationStatus = @clarificationSentStatus",
                    new { applicationId, userId, clarificationInProgressStatus = ModerationStatus.ClarificationInProgress, clarificationSentStatus = ModerationStatus.ClarificationSent });
                */
            }
        }
    }
}
