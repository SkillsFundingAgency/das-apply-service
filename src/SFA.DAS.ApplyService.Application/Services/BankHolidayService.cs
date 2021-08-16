using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Services
{
    public class BankHolidayService : IBankHolidayService
    {
        public DateTime? GetWorkingDaysAheadDate(DateTime? startDate, int numberOfDaysAhead)
        {

            if (startDate == null)
                return null;

            var actualNumberOfDaysAhead = numberOfDaysAhead;
            var bankHolidays = BankHolidays;

            var startDay = 0;
            while (startDay <= actualNumberOfDaysAhead)
            {
                var currentDay = startDate.Value.AddDays(startDay);

                if (currentDay.DayOfWeek == DayOfWeek.Saturday || currentDay.DayOfWeek == DayOfWeek.Sunday ||
                    bankHolidays.Contains(currentDay))
                {
                    actualNumberOfDaysAhead +=1;
                }

                startDay +=1;
            }

            return startDate.Value.AddDays(actualNumberOfDaysAhead);
        }

        public List<DateTime> BankHolidays {
            get
            {
                var bankHolidays = new List<DateTime>
                {
                    new DateTime(2021, 8, 30), 
                    new DateTime(2021, 12, 27),
                    new DateTime(2021, 12, 28),
                    new DateTime(2022, 1, 3),
                    new DateTime(2022, 4, 15), 
                    new DateTime(2022, 4, 18),  
                    new DateTime(2022, 5, 2), 
                    new DateTime(2022, 6, 2),  
                    new DateTime(2022, 6, 3),  
                    new DateTime(2022, 8, 29),  
                    new DateTime(2022, 12, 26),
                    new DateTime(2022, 12, 27)
                };

                return bankHolidays;
            }
        }
    }
}

