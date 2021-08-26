using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Data.UnitOfWork;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Data.Repositories.UnitOfWorkRepositories
{
    public class AppealFileRepository : IAppealFileRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplyConfig _config;

        public AppealFileRepository(IConfigurationService configurationService, IUnitOfWork unitOfWork)
        {
            _config = configurationService.GetConfig().Result;
            _unitOfWork = unitOfWork;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_config.SqlConnectionString);
        }

        public void Add(AppealFile entity)
        {
            _unitOfWork.Register(() => PersistAdd(entity));
        }

        public void Remove(Guid entityId)
        {
            _unitOfWork.Register(() => PersistRemoval(entityId));
        }

        public async Task<AppealFile> Get(Guid entityId)
        {
            using (var connection = GetConnection())
            {
                return await connection.QuerySingleAsync<AppealFile>(
                    @"SELECT * FROM [AppealFile] WHERE Id = @entityId",
                    new { entityId });
            }
        }

        public async Task<AppealFile> Get(Guid applicationId, string fileName)
        {
            using (var connection = GetConnection())
            {
                return await connection.QuerySingleAsync<AppealFile>(
                    @"SELECT * FROM [AppealFile] WHERE ApplicationId = @applicationId AND FileName = @fileName",
                    new { applicationId, fileName });
            }
        }

        public async Task<IEnumerable<AppealFile>> GetAllForApplication(Guid applicationId)
        {
            using (var connection = GetConnection())
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

        public async Task PersistRemoval(Guid entityId)
        {
            var transaction = _unitOfWork.GetTransaction();

            await transaction.Connection.ExecuteAsync(
                "DELETE FROM [AppealFile] WHERE Id = @entityId",
                    new { entityId }, transaction);
        }
    }
}
