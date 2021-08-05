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

        public async Task<Appeal> GetByOversightReviewId(Guid oversightReviewId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
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
