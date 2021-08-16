using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IBankHolidayRepository
    {
        Task<List<BankHoliday>> GetBankHolidays();
    }
}
