using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Data.UnitOfWork;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Infrastructure.Database;

namespace SFA.DAS.ApplyService.Data.Repositories.UnitOfWorkRepositories
{
    public class AppealUploadRepository : IAppealUploadRepository
    {
        private readonly IDbConnectionHelper _dbConnectionHelper;
        private readonly IUnitOfWork _unitOfWork;   

        public AppealUploadRepository(IDbConnectionHelper dbConnectionHelper, IUnitOfWork unitOfWork)
        {
            _dbConnectionHelper = dbConnectionHelper;
            _unitOfWork = unitOfWork;
        }

        public void Add(AppealUpload entity)
        {
            _unitOfWork.Register(() => PersistAdd(entity));
        }

        public void Remove(Guid entityId)
        {
            _unitOfWork.Register(() => PersistRemoval(entityId));
        }

        public void Update(AppealUpload entity)
        {
            _unitOfWork.Register(() => PersistUpdate(entity));
        }

        public async Task<AppealUpload> GetById(Guid entityId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QuerySingleAsync<AppealUpload>(
                    @"SELECT * FROM [AppealUpload] WHERE Id = @entityId",
                    new { entityId });
            }
        }

        public async Task<IEnumerable<AppealUpload>> GetByApplicationId(Guid applicationId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QueryAsync<AppealUpload>(
                    @"SELECT * FROM [AppealUpload] WHERE ApplicationId = @applicationId",
                    new { applicationId });
            }
        }

        public async Task PersistAdd(AppealUpload entity)
        {
            var transaction = _unitOfWork.GetTransaction();

            await transaction.Connection.ExecuteAsync(
                @"INSERT INTO [AppealUpload]
                    ([Id],
                    [ApplicationId],
                    [AppealId],
                    [FileStorageReference],
                    [Filename],
                    [ContentType],
                    [Size],
                    [UserId],
                    [UserName],
                    [CreatedOn])
                    VALUES (
                    @Id,
                    @ApplicationId,
                    @AppealId,
                    @FileStorageReference,
                    @Filename,
                    @ContentType,
                    @Size,
                    @UserId,
                    @UserName,
                    @CreatedOn)",
                entity, transaction);
        }

        public async Task PersistUpdate(AppealUpload entity)
        {
            var transaction = _unitOfWork.GetTransaction();

            await transaction.Connection.ExecuteAsync(
                @"UPDATE [AppealUpload] SET
                    [ApplicationId] = @ApplicationId,
                    [AppealId] = @AppealId,
                    [FileStorageReference] = @FileStorageReference,
                    [Filename] = @Filename,
                    [ContentType] = @ContentType,
                    [Size] = @Size,
                    [UserId] = @UserId,
                    [UserName] = @UserName,
                    [CreatedOn] = @CreatedOn
                    WHERE [Id] = @Id",
                entity, transaction);
        }

        public async Task PersistRemoval(Guid entityId)
        {
            var transaction = _unitOfWork.GetTransaction();

            await transaction.Connection.ExecuteAsync(
                "DELETE FROM [AppealUpload] WHERE Id = @entityId",
                    new { entityId }, transaction);
        }
    }
}
