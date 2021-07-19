using Dapper;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply.AllowedProviders;
using SFA.DAS.ApplyService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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

        public async Task<bool> CanUkprnStartApplication(int ukprn)
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

        public async Task<List<AllowedProvider>> GetAllowedProvidersList(string sortColumn, string sortOrder)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var orderByClause = $"{GetSortColumn(sortColumn)} { GetOrderByDirection(sortOrder)}";

                return (await connection.QueryAsync<AllowedProvider>($@"SELECT * FROM AllowedProviders
                                                              ORDER BY {orderByClause}, UKPRN ASC")).ToList();
            }
        }

        public async Task<bool> AddToAllowedProvidersList(int ukprn, DateTime startDateTime, DateTime endDateTime)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var insertedUkprn = await connection.QuerySingleAsync<int?>(
                    @"INSERT INTO [AllowedProviders] (UKPRN, StartDateTime, EndDateTime, AddedDateTime)
                      OUTPUT INSERTED.[UKPRN]
                      VALUES (@Ukprn, @StartDateTime, @EndDateTime, GETUTCDATE())",
                    new {
                            Ukprn = ukprn,
                            StartDateTime = startDateTime.Date,
                            EndDateTime = endDateTime.Date.Add(new TimeSpan(23, 59, 59))
                        });

                return insertedUkprn.HasValue && insertedUkprn == ukprn;
            }
        }

        private static string GetSortColumn(string requestedColumn)
        {
            switch (requestedColumn)
            {
                case "AddedDate":
                    return "AddedDateTime";

                case "StartDate":
                default:
                    return "StartDateTime";
            }
        }

        private static string GetOrderByDirection(string sortOrder)
        {
            return "ascending".Equals(sortOrder, StringComparison.InvariantCultureIgnoreCase) ? "ASC" : "DESC";
        }
    }
}
