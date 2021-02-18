using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Data.UnitOfWork;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Data
{
    public class OversightReviewRepository : IOversightReviewRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplyConfig _config;

        public OversightReviewRepository(IUnitOfWork unitOfWork, IConfigurationService configurationService)
        {
            _unitOfWork = unitOfWork;
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

        public void Add(OversightReview entity)
        {
            _unitOfWork.Register(() => PersistAdd(entity));
        }

        public void Update(OversightReview entity)
        {
            _unitOfWork.Register(() => PersistUpdate(entity));
        }

        public async Task PersistAdd(OversightReview entity)
        {
            var transaction = _unitOfWork.GetTransaction();

            await transaction.Connection.ExecuteAsync(
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
                entity, transaction);
            
        }

        public async Task PersistUpdate(OversightReview entity)
        {
            var transaction = _unitOfWork.GetTransaction();

            await transaction.Connection.ExecuteAsync(
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
                entity, transaction);
            
        }
    }
}
