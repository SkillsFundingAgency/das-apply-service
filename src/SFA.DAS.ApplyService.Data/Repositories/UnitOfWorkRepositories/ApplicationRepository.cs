using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Data.UnitOfWork;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Data.Repositories.UnitOfWorkRepositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly IApplyConfig _config;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationRepository(IUnitOfWork unitOfWork, IConfigurationService configurationService)
        {
            _unitOfWork = unitOfWork;
            _config = configurationService.GetConfig().Result;
        }

        public async Task<Apply> GetApplication(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<Apply>(
                    @"SELECT * FROM apply WHERE ApplicationId = @applicationId",
                    new { applicationId });
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
                    FinancialReviewStatus = @FinancialReviewStatus,
                    FinancialGrade = @FinancialGrade,
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
