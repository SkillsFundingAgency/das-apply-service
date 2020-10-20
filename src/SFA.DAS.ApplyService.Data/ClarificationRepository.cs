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
    }
}
