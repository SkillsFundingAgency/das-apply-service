using Dapper;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Review;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Data.DapperTypeHandlers;
using SFA.DAS.ApplyService.Domain.Entities.Review;
using SFA.DAS.ApplyService.Domain.Review;
using SFA.DAS.ApplyService.Domain.Review.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Data
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly IApplyConfig _config;

        public ReviewRepository(IConfigurationService configurationService)
        {
            _config = configurationService.GetConfig().Result;

            SqlMapper.AddTypeHandler(typeof(List<Outcome>), new GenericTypeHandler<List<Outcome>>());
        }

        public async Task<List<Domain.Entities.Application>> GetSubmittedApplicationsAsync()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var result = (await connection
                    .QueryAsync<Domain.Entities.Application>(
                        @"SELECT * FROM Applications WHERE ApplicationStatus = 'Submitted'"))
                        .ToList() ;

                return result;
            }
        }

        public async Task<GatewayCounts> GetGatewayCountsAsync()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var result = (await connection
                    .QueryAsync<GatewayCounts>(
                        @"SELECT 
	(SELECT Count(*) FROM Applications WHERE ApplicationStatus = 'Submitted') AS NewApplications,
	(SELECT Count(*) FROM review.Gateway WHERE Status = 'InProgress') AS InProgress"))
                        .Single();

                return result;
            }
        }

        public async Task<Gateway> GetGatewayReviewAsync(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var result = (await connection.QueryAsync<Gateway>("SELECT * FROM review.Gateway WHERE ApplicationId = @applicationId",
                    new { applicationId }))
                    .SingleOrDefault();

                return result;
            }
        }

        public async Task UpdateGatewayOutcomesAsync(Guid applicationId, string userId, DateTime changedAt, List<Outcome> outcomesDelta)
        {
            var gatewayReview = await GetGatewayReviewAsync(applicationId);

            var outcomes = GetUpdatedOutcomes(gatewayReview.Outcomes, outcomesDelta);

            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                connection.Open();

                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync("UPDATE review.Gateway SET Outcomes = @outcomes WHERE ApplicationId = @applicationId", new { outcomes, applicationId }, transaction);
                    await InsertAuditAsync(transaction, nameof(UpdateGatewayOutcomesAsync), applicationId, userId, changedAt, outcomesDelta);

                    transaction.Commit();
                }
            }
        }

        private List<Outcome> GetUpdatedOutcomes(List<Outcome> currentOutcomes, List<Outcome> outcomesDelta)
        {
            var updatedOutcomes = currentOutcomes ?? new List<Outcome>();

            foreach (var changedOutcome in outcomesDelta)
            {
                updatedOutcomes.RemoveAll(o =>
                    o.SectionId == changedOutcome.SectionId &&
                    o.PageId == changedOutcome.PageId &&
                    o.QuestionId == changedOutcome.QuestionId);

                updatedOutcomes.Add(changedOutcome);
            }

            return updatedOutcomes;
        }

        private Task InsertAuditAsync(IDbTransaction transaction, string action, Guid applicationId, string userId, DateTime changedAt, object delta)
        {
            var connection = transaction.Connection;

            var changeDelta = JsonConvert.SerializeObject(delta);

            return connection.ExecuteAsync("INSERT INTO review.Audit (Action, ApplicationId, UserId, ChangedAt, ChangeDelta) VALUES (@action, @applicationId, @userId, @changedAt, @changeDelta)",
                new {action, applicationId, userId, changedAt, changeDelta }, transaction);
        }
    }
}
