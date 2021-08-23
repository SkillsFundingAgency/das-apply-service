using System;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface IBankHolidayService
    {
        DateTime? GetWorkingDaysAheadDate(DateTime? startDate, int numberOfDaysAhead);
    }
}