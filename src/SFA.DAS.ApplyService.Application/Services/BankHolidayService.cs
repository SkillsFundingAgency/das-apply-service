using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Services
{
    public class BankHolidayService : IBankHolidayService
    {
        private readonly IBankHolidayRepository _bankHolidayRepository;

        public BankHolidayService(IBankHolidayRepository bankHolidayRepository)
        {
            _bankHolidayRepository = bankHolidayRepository;
        }

        public DateTime? GetWorkingDaysAheadDate(DateTime? startDate, int numberOfDaysAhead)
        {

            if (startDate == null)
                return null;

            var actualNumberOfDaysAhead = numberOfDaysAhead;
            var bankHolidays =  BankHolidays;

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

        public  List<DateTime> BankHolidays {
            get
            {
                var holidays =  _bankHolidayRepository.GetBankHolidays().Result;

                return holidays.Select(hol => hol.BankHolidayDate).ToList();
            }
        }
    }
}

