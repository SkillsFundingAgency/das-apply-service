using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Data
{
    public class OversightReviewRepository : IOversightReviewRepository
    {
        private readonly IApplyConfig _config;

        public OversightReviewRepository(IConfigurationService configurationService)
        {
            _config = configurationService.GetConfig().Result;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_config.SqlConnectionString);
        }

        public async Task<OversightReview> GetByApplicationId(Guid applicationId)
        {
            using (var connection = GetConnection())
            {
                return await connection.QuerySingleAsync<OversightReview>(
                    "select * from OversightReview where ApplicationId = @applicationId",
                    new
                    {
                        applicationId
                    });
            }
        }

        public async Task Add(OversightReview entity)
        {
            using (var connection = GetConnection())
            {
                await connection.ExecuteAsync(
                    @"INSERT INTO [OversightReview]
                        ([Id],
                        [ApplicationId],
                        [Status],
                        [ApplicationDeterminedDate],
                        [InternalComments],
                        [ExternalComments],
                        [UserId],
                        [UserName],
                        [CreatedOn])
                        VALUES (
                        @Id,
                        @ApplicationId,
                        @Status,
                        @ApplicationDeterminedDate,
                        @InternalComments,
                        @ExternalComments,
                        @UserId,
                        @UserName,
                        @CreatedOn)",
                    entity);
            }
        }
    }
}
