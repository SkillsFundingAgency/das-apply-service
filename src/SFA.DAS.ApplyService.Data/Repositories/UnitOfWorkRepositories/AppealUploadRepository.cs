using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Data.UnitOfWork;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Data.Repositories.UnitOfWorkRepositories
{
    public class AppealUploadRepository : IAppealUploadRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplyConfig _config;

        public AppealUploadRepository(IConfigurationService configurationService, IUnitOfWork unitOfWork)
        {
            _config = configurationService.GetConfig().Result;
            _unitOfWork = unitOfWork;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_config.SqlConnectionString);
        }

        public void Add(AppealUpload entity)
        {
            _unitOfWork.Register(() => PersistAdd(entity));
        }

        public void Remove(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public Task<AppealUpload> GetById(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public async Task PersistAdd(AppealUpload entity)
        {
            var transaction = _unitOfWork.GetTransaction();

            await transaction.Connection.ExecuteAsync(
                @"INSERT INTO [AppealUpload]
                    ([Id],
                    [ApplicationId],
                    [AppealId],
                    [FileId],
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
                    @FileId,
                    @Filename,
                    @ContentType,
                    @Size,
                    @UserId,
                    @UserName,
                    @CreatedOn)",
                entity, transaction);
        }
    }
}
