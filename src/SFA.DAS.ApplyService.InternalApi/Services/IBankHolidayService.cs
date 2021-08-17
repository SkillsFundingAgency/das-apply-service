using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public interface IBankHolidayService
    {
        DateTime? GetWorkingDaysAheadDate(DateTime? startDate, int numberOfDaysAhead);
        List<DateTime> GetBankHolidays();
    }
}