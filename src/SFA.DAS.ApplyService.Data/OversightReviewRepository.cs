using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

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
                return await connection.QuerySingleOrDefaultAsync<OversightReview>(
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
                        [GatewayApproved],
                        [ModerationApproved],
                        [Status],
                        [ApplicationDeterminedDate],
                        [InternalComments],
                        [ExternalComments],
                        [UserId],
                        [UserName],
                        [InProgressDate],
                        [InProgressUserId],
                        [InProgressUserName],
                        [InProgressInternalComments],
                        [InProgressExternalComments],
                        [CreatedOn])
                        VALUES (
                        @Id,
                        @ApplicationId,
                        @GatewayApproved,
                        @ModerationApproved,
                        @Status,
                        @ApplicationDeterminedDate,
                        @InternalComments,
                        @ExternalComments,
                        @UserId,
                        @UserName,
                        @InProgressDate,
                        @InProgressUserId,
                        @InProgressUserName,
                        @InProgressInternalComments,
                        @InProgressExternalComments,
                        @CreatedOn)",
                    entity);
            }
        }

        public async Task Update(OversightReview entity)
        {
            using (var connection = GetConnection())
            {
                await connection.ExecuteAsync(
                    @"UPDATE [OversightReview]
                        SET [GatewayApproved] = @GatewayApproved,
                        [ModerationApproved] = @ModerationApproved,
                        [Status] = @Status,
                        [ApplicationDeterminedDate] = @ApplicationDeterminedDate,
                        [InternalComments] = @InternalComments,
                        [ExternalComments] = @ExternalComments,
                        [UserId] =  @UserId,
                        [UserName] =  @UserName,
                        [InProgressDate] = @InProgressDate,
                        [InProgressUserId] = @InProgressUserId,
                        [InProgressUserName] = @InProgressUserName,
                        [InProgressInternalComments] = @InProgressInternalComments,
                        [InProgressExternalComments] = @InProgressExternalComments,
                        [UpdatedOn] = @updatedOn
                        WHERE [Id] = @id",
                    entity);
            }
        }
    }
}
