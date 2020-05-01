using Dapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
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

        private const string ApplicationSummaryFields = @"ApplicationId,
                            org.Name AS OrganisationName,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.UKPRN') AS Ukprn,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ReferenceNumber') AS ApplicationReferenceNumber,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ProviderRouteName') AS ProviderRoute,
                            JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn') AS SubmittedDate,
                            Assessor1Name,
                            Assessor2Name,
                            Assessor1UserId,
                            Assessor2UserId
            ";

        private const string InProgressApplicationsWhereClause = @"
                            apply.DeletedAt IS NULL AND 
                            (
                                -- Assigned to the current user and in progress
                                (apply.Assessor1ReviewStatus = @inProgressReviewStatus AND apply.Assessor1UserId = @userId) OR (apply.Assessor1ReviewStatus = @inProgressReviewStatus AND apply.Assessor1UserId = @userId)
                                OR
                                -- Assigned to any two other assessors and in progress
                                (apply.Assessor1UserId IS NOT NULL AND apply.Assessor2UserId IS NOT NULL AND (apply.Assessor1ReviewStatus = @inProgressReviewStatus OR Assessor2ReviewStatus = @inProgressReviewStatus))
                            )";

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
                        $@"SELECT 
                            {ApplicationSummaryFields}
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
                await connection.ExecuteAsync(@"UPDATE Apply SET Assessor1UserId = @userId, Assessor1Name = @userName, Assessor1ReviewStatus = @inProgressReviewStatus
                                                Where ApplicationId = @applicationId",
                    new
                    {
                        applicationId,
                        userId,
                        userName,
                        inProgressReviewStatus = AssessorReviewStatus.InProgress
                    });
            }
        }

        public async Task UpdateAssessor2(Guid applicationId, string userId, string userName)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE Apply SET Assessor2UserId = @userId, Assessor2Name = @userName, Assessor2ReviewStatus = @inProgressReviewStatus
                                                Where ApplicationId = @applicationId",
                    new
                    {
                        applicationId,
                        userId,
                        userName,
                        inProgressReviewStatus = AssessorReviewStatus.InProgress
                    });
            }
        }

        public async Task<List<RoatpAssessorApplicationSummary>> GetInProgressAssessorApplications(string userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .QueryAsync<RoatpAssessorApplicationSummary>(
                        $@"SELECT 
                            {ApplicationSummaryFields}
	                        FROM Apply apply
	                        INNER JOIN Organisations org ON org.Id = apply.OrganisationId
	                        WHERE {InProgressApplicationsWhereClause}
                            ORDER BY JSON_VALUE(apply.ApplyData, '$.ApplyDetails.ApplicationSubmittedOn')",
                        new
                        {
                            inProgressReviewStatus = AssessorReviewStatus.InProgress,
                            userId = userId
                        })).ToList();
            }
        }

        public async Task<int> GetInProgressAssessorApplicationsCount(string userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .ExecuteScalarAsync<int>(
                        $@"SELECT COUNT(1)
	                      FROM Apply apply
	                      WHERE {InProgressApplicationsWhereClause}",
                        new
                        {
                            inProgressReviewStatus = AssessorReviewStatus.InProgress,
                            userId = userId
                        }));
            }
        }
    }
}