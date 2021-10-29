using System;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Data.UnitOfWork;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Infrastructure.Database;

namespace SFA.DAS.ApplyService.Data.Repositories.UnitOfWorkRepositories
{
    public class AppealRepository : IAppealRepository
    {
        private readonly IDbConnectionHelper _dbConnectionHelper;
        private readonly IUnitOfWork _unitOfWork;

        public AppealRepository(IDbConnectionHelper dbConnectionHelper, IUnitOfWork unitOfWork)
        {
            _dbConnectionHelper = dbConnectionHelper;
            _unitOfWork = unitOfWork;
        }

        public void Add(Appeal entity)
        {
            _unitOfWork.Register(() => PersistAdd(entity));
        }

        public void Update(Appeal entity)
        {
            _unitOfWork.Register(() => PersistUpdate(entity));
        }

        public void Remove(Guid entityId)
        {
            _unitOfWork.Register(() => PersistRemoval(entityId));
        }

        public async Task<Appeal> GetByApplicationId(Guid applicationId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Appeal>(
                    @"SELECT * FROM [Appeal] WHERE ApplicationId = @applicationId",
                    new { applicationId });
            }
        }

        public async Task PersistAdd(Appeal entity)
        {
            var transaction = _unitOfWork.GetTransaction();

            await transaction.Connection.ExecuteAsync(
                @"INSERT INTO [Appeal]
                    ([Id],
                    [ApplicationId],
                    [Status],
                    [HowFailedOnPolicyOrProcesses],
                    [HowFailedOnEvidenceSubmitted],
                    [AppealSubmittedDate],
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
                    @Status,
                    @HowFailedOnPolicyOrProcesses,
                    @HowFailedOnEvidenceSubmitted,
                    @AppealSubmittedDate,
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

        public async Task PersistUpdate(Appeal entity)
        {
            var transaction = _unitOfWork.GetTransaction();

            await transaction.Connection.ExecuteAsync(
                @"UPDATE [Appeal]
                    SET [Status] = @Status,
                    [HowFailedOnPolicyOrProcesses] = @HowFailedOnPolicyOrProcesses,
                    [HowFailedOnEvidenceSubmitted] = @HowFailedOnEvidenceSubmitted,
                    [AppealDeterminedDate] = @AppealDeterminedDate,
                    [InternalComments] = @InternalComments,
                    [ExternalComments] = @ExternalComments,
                    [UserId] =  @UserId,
                    [UserName] =  @UserName,
                    [InProgressDate] = @InProgressDate,
                    [InProgressUserId] = @InProgressUserId,
                    [InProgressUserName] = @InProgressUserName,
                    [InProgressInternalComments] = @InProgressInternalComments,
                    [InProgressExternalComments] = @InProgressExternalComments,
                    [UpdatedOn] = @UpdatedOn
                    WHERE [Id] = @Id",
                entity, transaction);

        }

        public async Task PersistRemoval(Guid entityId)
        {
            var transaction = _unitOfWork.GetTransaction();

            await transaction.Connection.ExecuteAsync(
                "DELETE FROM [Appeal] WHERE Id = @entityId",
                    new { entityId }, transaction);
        }
    }
}
