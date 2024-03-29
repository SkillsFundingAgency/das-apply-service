﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Data.UnitOfWork;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Infrastructure.Database;

namespace SFA.DAS.ApplyService.Data.Repositories.UnitOfWorkRepositories
{
    public class AppealFileRepository : IAppealFileRepository
    {
        private readonly IDbConnectionHelper _dbConnectionHelper;
        private readonly IUnitOfWork _unitOfWork;   

        public AppealFileRepository(IDbConnectionHelper dbConnectionHelper, IUnitOfWork unitOfWork)
        {
            _dbConnectionHelper = dbConnectionHelper;
            _unitOfWork = unitOfWork;
        }

        public void Add(AppealFile entity)
        {
            _unitOfWork.Register(() => PersistAdd(entity));
        }

        public void Update(AppealFile entity)
        {
            _unitOfWork.Register(() => PersistUpdate(entity));
        }

        public void Remove(Guid entityId)
        {
            _unitOfWork.Register(() => PersistRemoval(entityId));
        }

        public async Task<AppealFile> Get(Guid entityId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<AppealFile>(
                    @"SELECT * FROM [AppealFile] WHERE Id = @entityId",
                    new { entityId });
            }
        }

        public async Task<AppealFile> Get(Guid applicationId, string fileName)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<AppealFile>(
                    @"SELECT * FROM [AppealFile] WHERE ApplicationId = @applicationId AND FileName = @fileName",
                    new { applicationId, fileName });
            }
        }

        public async Task<IEnumerable<AppealFile>> GetAllForApplication(Guid applicationId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QueryAsync<AppealFile>(
                    @"SELECT * FROM [AppealFile] WHERE ApplicationId = @applicationId",
                    new { applicationId });
            }
        }

        public async Task PersistAdd(AppealFile entity)
        {
            var transaction = _unitOfWork.GetTransaction();

            await transaction.Connection.ExecuteAsync(
                @"INSERT INTO [AppealFile]
                    ([Id],
                    [ApplicationId],
                    [FileName],
                    [ContentType],
                    [Size],
                    [UserId],
                    [UserName],
                    [CreatedOn])
                    VALUES (
                    @Id,
                    @ApplicationId,
                    @FileName,
                    @ContentType,
                    @Size,
                    @UserId,
                    @UserName,
                    @CreatedOn)",
                entity, transaction);
        }

        public async Task PersistUpdate(AppealFile entity)
        {
            var transaction = _unitOfWork.GetTransaction();

            await transaction.Connection.ExecuteAsync(
                @"UPDATE [AppealFile]
                    SET [FileName] = @FileName,
                    [ContentType] = @ContentType,
                    [Size] = @Size,
                    [UserId] =  @UserId,
                    [UserName] =  @UserName,
                    [CreatedOn] = @CreatedOn
                WHERE [Id] = @id",
                entity, transaction);
        }

        public async Task PersistRemoval(Guid entityId)
        {
            var transaction = _unitOfWork.GetTransaction();

            await transaction.Connection.ExecuteAsync(
                "DELETE FROM [AppealFile] WHERE Id = @entityId",
                    new { entityId }, transaction);
        }
    }
}
