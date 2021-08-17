using System;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public interface IBankHolidayService
    {
        DateTime? GetWorkingDaysAheadDate(DateTime? startDate, int numberOfDaysAhead);
    }
}