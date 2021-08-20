using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.InternalApi.Services;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Services
{
    [TestFixture]
    public class BankHolidayServiceTests
    {
        private Mock<IBankHolidayRepository> _repository;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IBankHolidayRepository>();
            var bankHolidays = new List<BankHoliday>
            {
                new BankHoliday {BankHolidayDate = new DateTime(2021, 8, 30)},
                new BankHoliday {BankHolidayDate = new DateTime(2021, 12, 27)}, 
                new BankHoliday {BankHolidayDate = new DateTime(2021, 12, 28)},
                new BankHoliday {BankHolidayDate = new DateTime(2022, 1, 3)},
                new BankHoliday {BankHolidayDate = new DateTime(2022, 4, 15)},
                new BankHoliday {BankHolidayDate = new DateTime(2022, 4, 18)},
                new BankHoliday {BankHolidayDate = new DateTime(2022, 5, 2)},
                new BankHoliday {BankHolidayDate = new DateTime(2022, 6, 2)},
                new BankHoliday {BankHolidayDate = new DateTime(2022, 6, 3)},
                new BankHoliday {BankHolidayDate = new DateTime(2022, 8, 29)},
                new BankHoliday {BankHolidayDate = new DateTime(2022, 12, 26)},
                new BankHoliday {BankHolidayDate = new DateTime(2022, 12, 27)}
            };
            _repository.Setup(x => x.GetBankHolidays()).ReturnsAsync(bankHolidays);

        }

        [TestCase("2021-03-01", 1, "2021-03-02")] // Monday -> Tuesday
        [TestCase("2021-03-02", 1, "2021-03-03")] // Tuesday -> Wednesday
        [TestCase("2021-03-03", 1, "2021-03-04")] // Wednesday -> Thursday
        [TestCase("2021-03-04", 1, "2021-03-05")] // Thursday -> Friday
        [TestCase("2021-03-05", 1, "2021-03-08")] // Friday -> Monday (Saturday & Sunday are weekend)
        [TestCase("2021-03-06", 1, "2021-03-09")] // Saturday -> Tuesday (It's the weekend so Monday would be the starting working day)
        [TestCase("2021-03-07", 1, "2021-03-09")] // Sunday -> Tuesday (It's the weekend so Monday would be the starting working day)
        public async Task GetWorkingDaysAhead_one_day_in_period_with_no_bank_holidays_returns_expected_Date(DateTime startDate, int daysAhead, DateTime expectedDate)
        {
            var service = new BankHolidayService(_repository.Object);
            var returnedDate = await service.GetWorkingDaysAheadDate(startDate, daysAhead);
            returnedDate.Should().BeSameDateAs(expectedDate);
        }

        [TestCase("2021-12-24", 1, "2021-12-29")] // Friday -> Thursday (2 bank holidays)
        [TestCase("2021-12-25", 1, "2021-12-30")] // Saturday -> Thursday (It's a weekend then a bank holiday so Wednesday would be the starting working day)
        [TestCase("2021-12-26", 1, "2021-12-30")] // Sunday -> Thursday (It's a weekend then a bank holiday so Wednesday would be the starting working day)
        [TestCase("2021-12-27", 1, "2021-12-30")] // Monday -> Thursday (It's a bank holiday so Wednesday would be the starting working day)
        [TestCase("2021-12-28", 1, "2021-12-30")] // Tuesday -> Thursday (It's a bank holiday so Wednesday would be the starting working day)
        public async Task GetWorkingDaysAhead_one_day_in_period_with_bank_holidays_returns_expected_Date(DateTime startDate, int daysAhead, DateTime expectedDate)
        {
            var service = new BankHolidayService(_repository.Object);
            var returnedDate = await service.GetWorkingDaysAheadDate(startDate, daysAhead);
            returnedDate.Should().BeSameDateAs(expectedDate);
        }

        [TestCase("2021-01-01", 10, "2021-01-15")]
        [TestCase("2022-02-01", 10, "2022-02-15")]
        [TestCase("2022-03-01", 15, "2022-03-22")]
        [TestCase("2022-04-01", 20, "2022-05-04")]
        [TestCase("2022-05-01", 30, "2022-06-16")]
        [TestCase("2022-06-01", 10, "2022-06-17")]
        [TestCase("2022-07-01", 10, "2022-07-15")]
        [TestCase("2022-08-01", 10, "2022-08-15")]
        [TestCase("2021-08-19", 1, "2021-08-20")]
        [TestCase("2021-03-01", 1, "2021-03-02")]
        [TestCase("2021-08-16 15:38:30.6366667",10,"2021-08-31")]
        public async Task GetWorkingDaysAhead_returns_expectedDate(DateTime startDate, int daysAhead, DateTime expectedDate)
        {
            var service = new BankHolidayService(_repository.Object);
            var returnedDate = await service.GetWorkingDaysAheadDate(startDate, daysAhead);
            returnedDate.Should().BeSameDateAs(expectedDate);
        }
    }
}
