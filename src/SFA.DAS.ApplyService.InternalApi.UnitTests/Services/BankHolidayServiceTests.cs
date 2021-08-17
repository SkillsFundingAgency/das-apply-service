using System;
using System.Collections.Generic;
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
                new BankHoliday {BankHolidayDate =        new DateTime(2022, 4, 15)},
                new BankHoliday {BankHolidayDate =      new DateTime(2022, 4, 18)},
                new BankHoliday {BankHolidayDate =       new DateTime(2022, 5, 2)},
                new BankHoliday {BankHolidayDate =      new DateTime(2022, 6, 2)},
                new BankHoliday {BankHolidayDate =     new DateTime(2022, 6, 3)},
                new BankHoliday {BankHolidayDate =       new DateTime(2022, 8, 29)},
                new BankHoliday {BankHolidayDate =      new DateTime(2022, 12, 26)},
                new BankHoliday {BankHolidayDate =    new DateTime(2022, 12, 27)}
            };
            _repository.Setup(x => x.GetBankHolidays()).ReturnsAsync(bankHolidays);

        }

        [TestCase("2021-01-01",10,"2021-01-15")]
        [TestCase("2022-02-01", 10, "2022-02-15")]
        [TestCase("2022-03-01", 15, "2022-03-22")]
        [TestCase("2022-04-01", 20, "2022-05-04")]
        [TestCase("2022-05-01", 30, "2022-06-16")]
        [TestCase("2022-06-01", 10, "2022-06-17")]
        [TestCase("2022-07-01", 10, "2022-07-15")]
        [TestCase("2022-08-01", 10, "2022-08-15")]
        public void GetWorkingDaysAhead_returns_expectedDate(DateTime startDate,int daysAhead,DateTime? expectedDate)
        {
            var service = new BankHolidayService(_repository.Object);
            var returnedDate = service.GetWorkingDaysAheadDate(startDate, daysAhead);
            expectedDate.Should().Be(returnedDate);
        }
    }
}
