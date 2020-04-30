using Dapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Data.DapperTypeHandlers;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Data
{
    public class AssessorRepository : IAssessorRepository
    {
        private readonly IApplyConfig _config;
        private readonly ILogger<AssessorRepository> _logger;

        public AssessorRepository(IConfigurationService configurationService, ILogger<AssessorRepository> logger)
        {
            _logger = logger;
            _config = configurationService.GetConfig().Result;
        }

        public async Task<List<RoatpAssessorApplicationSummary>> GetNewAssessorApplications(string userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .QueryAsync<RoatpAssessorApplicationSummary>(
                        @"SELECT 
                            ApplicationId,
                            org.Name AS OrganisationName,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName') AS ProviderRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS SubmittedDate,
                            Assessor1Name,
                            Assessor2Name
	                      FROM Apply apply
	                      INNER JOIN Organisations org ON org.Id = apply.OrganisationId
	                      WHERE apply.DeletedAt IS NULL AND apply.GatewayReviewStatus = 'Approved' AND ISNULL(Assessor1UserId, '') <> @userId AND ISNULL(Assessor2UserId, '') <> @userId AND (Assessor1UserId IS NULL OR Assessor2UserId IS NULL)
                          ORDER BY JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn')",
                        new
                        {
                            gatewayReviewStatusApproved = GatewayReviewStatus.Approved,
                            userId = userId
                        })).ToList();
            }
        }

        public async Task<int> GetNewAssessorApplicationsCount(string userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .ExecuteScalarAsync<int>(
                        @"SELECT COUNT(1)
	                      FROM Apply apply
	                      WHERE apply.DeletedAt IS NULL AND apply.GatewayReviewStatus = @gatewayReviewStatusApproved AND (Assessor1UserId IS NULL OR Assessor1UserId <> @userId AND Assessor2UserId IS NULL)",
                        new
                        {
                            gatewayReviewStatusApproved = GatewayReviewStatus.Approved,
                            userId = userId
                        }));
            }
        }

        public async Task UpdateAssessor1(Guid applicationId, string userId, string userName)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE Apply SET Assessor1UserId = @userId, Assessor1Name = @userName
                                                Where ApplicationId = @applicationId",
                    new
                    {
                        applicationId,
                        userId,
                        userName
                    });
            }
        }

        public async Task UpdateAssessor2(Guid applicationId, string userId, string userName)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE Apply SET Assessor2UserId = @userId, Assessor2Name = @userName
                                                Where ApplicationId = @applicationId",
                    new
                    {
                        applicationId,
                        userId,
                        userName
                    });
            }
        }
    }
}