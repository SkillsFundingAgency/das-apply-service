using System;

namespace SFA.DAS.ApplyService.Application.Services
{
    public interface IBankHolidayService
    {
        DateTime GetWorkingDaysAheadDate(DateTime startDate, int numberOfDaysAhead);
    }
}