using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Data.Repositories
{
    public class BankHolidayRepository: IBankHolidayRepository
    {
        private readonly IApplyConfig _config;

        public BankHolidayRepository(IConfigurationService configurationService)
        {
            _config = configurationService.GetConfig().Result;
        }

        public async Task<List<BankHoliday>> GetBankHolidays()
        {
           
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<BankHoliday>(
                    @"SELECT * FROM BankHoliday")).ToList();
            }
        }
    }
}
