using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Data
{
    public class ClarificationRepository : IClarificationRepository
    {
        private readonly IApplyConfig _config;

        public ClarificationRepository(IConfigurationService configurationService)
        {
            _config = configurationService.GetConfig().GetAwaiter().GetResult();
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
			                                                            ,[ModeratorUserName]
			                                                            ,[ModeratorReviewStatus]
			                                                            ,[ModeratorReviewComment]
			                                                            ,[ClarificationUserId] AS UserId
			                                                            ,[ClarificationUserName] AS UserName
                                                                        ,CASE WHEN ([ClarificationStatus] IS NULL AND [ModeratorReviewStatus] = @passModeratorStatus) THEN @passModeratorStatus
                                                                              ELSE [ClarificationStatus]
                                                                         END AS [Status]
			                                                            ,[ClarificationComment] AS Comment
			                                                            ,[ClarificationResponse]
                                                                        ,[ClarificationFile]
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
			                                                            ,[ModeratorUserName]
			                                                            ,[ModeratorReviewStatus]
			                                                            ,[ModeratorReviewComment]
			                                                            ,[ClarificationUserId] AS UserId
			                                                            ,[ClarificationUserName] AS UserName
                                                                        ,CASE WHEN ([ClarificationStatus] IS NULL AND [ModeratorReviewStatus] = @passModeratorStatus) THEN @passModeratorStatus
                                                                              ELSE [ClarificationStatus]
                                                                         END AS [Status]
			                                                            ,[ClarificationComment] AS Comment
			                                                            ,[ClarificationResponse]
                                                                        ,[ClarificationFile]
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
			                                                            ,[ModeratorUserName]
			                                                            ,[ModeratorReviewStatus]
			                                                            ,[ModeratorReviewComment]
			                                                            ,[ClarificationUserId] AS UserId
			                                                            ,[ClarificationUserName] AS UserName
                                                                        ,CASE WHEN ([ClarificationStatus] IS NULL AND [ModeratorReviewStatus] = @passModeratorStatus) THEN @passModeratorStatus
                                                                              ELSE [ClarificationStatus]
                                                                         END AS [Status]
			                                                            ,[ClarificationComment] AS Comment
			                                                            ,[ClarificationResponse]
                                                                        ,[ClarificationFile]
		                                                            FROM [dbo].[ModeratorPageReviewOutcome]
		                                                            WHERE [ApplicationId] = @applicationId",
                    new { applicationId, passModeratorStatus = ModerationStatus.Pass });

                return pageReviewOutcomeResults.ToList();
            }
        }

        public async Task SubmitClarificationPageOutcome(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId, string userId, string userName, string status, string comment, string clarificationResponse, string clarificationFile)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(
                    @"UPDATE [ModeratorPageReviewOutcome]
			            SET [ClarificationUserId] = @userId
                            , [ClarificationUserName] = @userName
                            , [ClarificationStatus] = @status
				            , [ClarificationComment] = @comment
                            , [ClarificationResponse] = @clarificationResponse
                            , [ClarificationFile] = ISNULL(@clarificationFile, [ClarificationFile])
                            , [ClarificationUpdatedAt] = GETUTCDATE()
				            , [UpdatedAt] = GETUTCDATE()
				            , [UpdatedBy] = @userId
			            WHERE [ApplicationId] = @applicationId AND
					          [SequenceNumber] = @sequenceNumber AND
					          [SectionNumber] = @sectionNumber AND
					          [PageId] = @pageId",
                    new { applicationId, sequenceNumber, sectionNumber, pageId, userId, userName, status, comment, clarificationResponse, clarificationFile });

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

        public async Task DeleteClarificationFile(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId, string clarificationFile)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(
                    @"UPDATE [ModeratorPageReviewOutcome]
			            SET [ClarificationFile] = NULL
                            , [ClarificationUpdatedAt] = GETUTCDATE()
			            WHERE [ApplicationId] = @applicationId AND
					          [SequenceNumber] = @sequenceNumber AND
					          [SectionNumber] = @sectionNumber AND
					          [PageId] = @pageId AND
                              [ClarificationFile] = @clarificationFile",
                    new { applicationId, sequenceNumber, sectionNumber, pageId, clarificationFile });
            }
        }
    }
}
