using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public interface IBankHolidayService
    {
        Task<DateTime> GetWorkingDaysAheadDate(DateTime startDate, int numberOfDaysAhead);
        Task<List<DateTime>> GetBankHolidays();
    }
}