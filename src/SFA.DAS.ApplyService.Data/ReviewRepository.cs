using Dapper;
using SFA.DAS.ApplyService.Application.Review;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Review.Gateway;
using System.Collections.Generic;
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
    }
}
