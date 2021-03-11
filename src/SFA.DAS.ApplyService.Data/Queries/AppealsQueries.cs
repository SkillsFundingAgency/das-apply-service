using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Data.Queries
{
    public class AppealsQueries : IAppealsQueries
    {
        private readonly IApplyConfig _config;

        public AppealsQueries(IConfigurationService configurationService)
        {
            _config = configurationService.GetConfig().Result;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_config.SqlConnectionString);
        }

        public async Task<AppealFiles> GetStagedAppealFiles(Guid applicationId)
        {
            using (var connection = GetConnection())
            {
                var files = (await connection.QueryAsync<AppealFile>(
                    @"SELECT Id, Filename FROM [AppealUpload] where ApplicationId = @applicationId and AppealId IS NULL ORDER BY CreatedOn ASC",
                    new
                    {
                        applicationId
                    })).ToList();

                return new AppealFiles
                {
                    Files = files
                };
            }
        }

        public async Task<Appeal> GetAppeal(Guid applicationId, Guid oversightReviewId)
        {
            using (var connection = GetConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Appeal>(
                    @"SELECT a.Message, a.UserId, a.UserName, a.CreatedOn
                        FROM [Appeal] a
                        JOIN [OversightReview] r ON r.Id = a.OversightReviewId
                        where a.OversightReviewId = @oversightReviewId
                        AND r.ApplicationId = @applicationId",
                    new
                    {
                        applicationId,
                        oversightReviewId
                    });
            }
        }
    }
}
