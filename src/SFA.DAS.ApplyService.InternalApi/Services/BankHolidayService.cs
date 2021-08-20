using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public class BankHolidayService : IBankHolidayService
    {
        private readonly IBankHolidayRepository _bankHolidayRepository;

        public BankHolidayService(IBankHolidayRepository bankHolidayRepository)
        {
            _bankHolidayRepository = bankHolidayRepository;
        }

        public async Task<List<DateTime>> GetBankHolidays()
        {
            var bankHolidays = await _bankHolidayRepository.GetBankHolidays();

            return bankHolidays.Select(hol => hol.BankHolidayDate).ToList();
        }

        public async Task<DateTime> GetWorkingDaysAheadDate(DateTime startDate, int numberOfDaysAhead)
        {
            var bankHolidays = await GetBankHolidays() as IReadOnlyCollection<DateTime>;

            var startingDay = startDate.Date;

            // Must adjust to the next starting day if it's not a working day to begin with
            if(IsWeekend(startingDay) || IsBankHoliday(bankHolidays, startingDay))
            {
                startingDay = GetNextWorkingDay(bankHolidays, startingDay);
            }

            var workingDays = new List<DateTime> { startingDay };

            for(int count = 0; count < numberOfDaysAhead; count++)
            {
                var nextWorkingDay = GetNextWorkingDay(bankHolidays, startingDay);
                workingDays.Add(nextWorkingDay);
                startingDay = nextWorkingDay;
            }

            return workingDays.Last();
        }

        private static DateTime GetNextWorkingDay(IReadOnlyCollection<DateTime> bankHolidays, DateTime date)
        {
            do
            {
                date = date.AddDays(1);
            } while (IsWeekend(date) || IsBankHoliday(bankHolidays, date));

            return date;
        }

        private static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday
                || date.DayOfWeek == DayOfWeek.Sunday;
        }

        private static bool IsBankHoliday(IReadOnlyCollection<DateTime> bankHolidays, DateTime date)
        {
            return bankHolidays.Contains(date);
        }
    }
}

