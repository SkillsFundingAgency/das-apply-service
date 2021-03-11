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
    public class AppealRepository : IAppealRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplyConfig _config;

        public AppealRepository(IConfigurationService configurationService, IUnitOfWork unitOfWork)
        {
            _config = configurationService.GetConfig().Result;
            _unitOfWork = unitOfWork;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_config.SqlConnectionString);
        }

        public void Add(Appeal entity)
        {
            _unitOfWork.Register(() => PersistAdd(entity));
        }

        public async Task<Appeal> GetByOversightReviewId(Guid oversightReviewId)
        {
            using (var connection = GetConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Appeal>(
                    @"SELECT * FROM [Appeal] WHERE OversightReviewId = @oversightReviewId",
                    new { oversightReviewId });
            }
        }

        public async Task PersistAdd(Appeal entity)
        {
            var transaction = _unitOfWork.GetTransaction();

            await transaction.Connection.ExecuteAsync(
                @"INSERT INTO [Appeal]
                    ([Id],
                    [OversightReviewId],
                    [Message],
                    [UserId],
                    [UserName],
                    [CreatedOn])
                    VALUES (
                    @Id,
                    @OversightReviewId,
                    @Message,
                    @UserId,
                    @UserName,
                    @CreatedOn)",
                entity, transaction);
        }
    }
}
