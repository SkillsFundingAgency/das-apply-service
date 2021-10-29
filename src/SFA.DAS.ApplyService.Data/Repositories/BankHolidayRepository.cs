using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Infrastructure.Database;

namespace SFA.DAS.ApplyService.Data.Repositories
{
    public class BankHolidayRepository: IBankHolidayRepository
    {
        private readonly IDbConnectionHelper _dbConnectionHelper;

        public BankHolidayRepository(IDbConnectionHelper dbConnectionHelper)
        {
            _dbConnectionHelper = dbConnectionHelper;
        }

        public async Task<List<BankHoliday>> GetBankHolidays()
        {
           
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return (await connection.QueryAsync<BankHoliday>(
                    @"SELECT * FROM BankHoliday where Active=1")).ToList();
            }
        }
    }
}
