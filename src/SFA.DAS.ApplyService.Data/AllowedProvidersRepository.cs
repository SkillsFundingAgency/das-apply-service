using Dapper;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Interfaces;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Data
{
    public class AllowedProvidersRepository : IAllowedProvidersRepository
    {
        private readonly IApplyConfig _config;

        public AllowedProvidersRepository(IConfigurationService configurationService)
        {
            _config = configurationService.GetConfig().Result;
        }

        public async Task<bool> IsUkprnOnAllowedProvidersList(int ukprn)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return await connection.QuerySingleAsync<bool>(@"SELECT
                                                                      CASE WHEN EXISTS 
                                                                      (
                                                                            SELECT UKPRN
                                                                            FROM AllowedProviders
                                                                            WHERE UKPRN = @ukprn
                                                                             AND GETUTCDATE() BETWEEN StartDateTime and EndDateTime
                                                                      )
                                                                      THEN 'TRUE'
                                                                      ELSE 'FALSE'
                                                                  END",
                                                                  new { ukprn });
            }
        }
    }
}
