using Dapper;
using SFA.DAS.ApplyService.Domain.Apply.AllowedProviders;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Data
{
    public class AllowedProvidersRepository : IAllowedProvidersRepository
    {
        private readonly IDbConnectionHelper _dbConnectionHelper;

        public AllowedProvidersRepository(IDbConnectionHelper dbConnectionHelper)
        {
            _dbConnectionHelper = dbConnectionHelper;
        }

        public async Task<bool> CanUkprnStartApplication(int ukprn)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
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
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var orderByClause = $"{GetSortColumn(sortColumn)} { GetOrderByDirection(sortOrder)}";

                return (await connection.QueryAsync<AllowedProvider>($@"SELECT * FROM AllowedProviders
                                                                        ORDER BY {orderByClause}, UKPRN ASC")).ToList();
            }
        }

        public async Task<AllowedProvider> GetAllowedProviderDetails(int ukprn)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<AllowedProvider>($@"SELECT * FROM AllowedProviders
                                                                                      WHERE UKPRN = @ukprn",
                                                                                    new { ukprn });
            }
        }

        public async Task<bool> AddToAllowedProvidersList(int ukprn, DateTime startDateTime, DateTime endDateTime)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
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

        public async Task<bool> RemoveFromAllowedProvidersList(int ukprn)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var rowsAffected = await connection.ExecuteAsync($@"DELETE FROM AllowedProviders
                                                                    WHERE UKPRN = @ukprn",
                                                                  new { ukprn });

                return rowsAffected > 0;
            }
        }

        private static string GetSortColumn(string requestedColumn)
        {
            switch (requestedColumn)
            {
                case "AddedDate":
                    return "AddedDateTime";

                case "StartDate":
                    return "StartDateTime";

                default:
                    return "AddedDateTime";
            }
        }

        private static string GetOrderByDirection(string sortOrder)
        {
            return "ascending".Equals(sortOrder, StringComparison.InvariantCultureIgnoreCase) ? "ASC" : "DESC";
        }
    }
}
