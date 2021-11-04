using System;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Data.DapperTypeHandlers;
using SFA.DAS.ApplyService.Data.UnitOfWork;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Infrastructure.Database;

namespace SFA.DAS.ApplyService.Data.Repositories.UnitOfWorkRepositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly IDbConnectionHelper _dbConnectionHelper;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationRepository(IDbConnectionHelper dbConnectionHelper, IUnitOfWork unitOfWork)
        {
            _dbConnectionHelper = dbConnectionHelper;
            _unitOfWork = unitOfWork;

            SqlMapper.AddTypeHandler(typeof(ApplyData), new ApplyDataHandler());
        }

        public async Task<Apply> GetApplication(Guid applicationId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {

                return await connection.QuerySingleOrDefaultAsync<Apply>(
                    @"SELECT * FROM apply WHERE ApplicationId = @applicationId",
                    new {applicationId});
            }
        }

        public void Update(Apply application)
        {
            _unitOfWork.Register(() => PersistUpdate(application));
        }

        public async Task PersistUpdate(Apply application)
        {
            var transaction = _unitOfWork.GetTransaction();

            await transaction.Connection.ExecuteAsync(
                @"UPDATE [Apply] SET
                    ApplicationStatus = @ApplicationStatus,
                    GatewayReviewStatus = @GatewayReviewStatus,
                    AssessorReviewStatus = @AssessorReviewStatus,
                    Assessor1UserId = @Assessor1UserId,
                    Assessor2UserId = @Assessor2UserId,
                    Assessor1Name = @Assessor1Name,
                    Assessor2Name = @Assessor2Name,
                    Assessor1ReviewStatus = @Assessor1ReviewStatus,
                    Assessor2ReviewStatus = @Assessor2ReviewStatus,
                    ModerationStatus = @ModerationStatus,
                    GatewayUserId = @gatewayUserId,
                    GatewayUserName = @gatewayUserName,
                    UpdatedBy = @updatedBy,
                    UpdatedAt = @updatedAt
                    WHERE [Id] = @id",
                application, transaction);
        }
    }
}
