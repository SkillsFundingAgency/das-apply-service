using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Services;

namespace SFA.DAS.ApplyService.Application.UnitTests.Services
{
    [TestFixture]
    public class BankHolidayServiceTests
    {
        [TestCase("2020-01-01", 10, "2020-01-15")]
        [TestCase("2021-02-01", 10, "2021-02-15")]
        [TestCase("2021-03-01", 15, "2021-03-22")]
        [TestCase("2021-04-01", 20, "2021-05-04")]
        [TestCase("2021-05-01", 30, "2021-06-15")]
        [TestCase("2021-06-01", 10, "2021-06-15")]
        [TestCase("2021-07-01", 10, "2021-07-15")]
        [TestCase("2021-08-01", 10, "2021-08-13")]
        [TestCase("2021-08-13", 10, "2021-08-27")]
        [TestCase("2021-08-14", 10, "2021-08-27")]
        [TestCase("2021-08-15", 10, "2021-08-27")]
        [TestCase("2021-08-16", 10, "2021-08-31")]
        [TestCase("2021-08-16 14:00:00", 10, "2021-08-31")]
        public void GetWorkingDaysAhead_returns_expectedDate(DateTime startDate,int daysAhead,DateTime? expectedDate)
        {
            var service = new BankHolidayService();
            var returnedDate = service.GetWorkingDaysAheadDate(startDate, daysAhead);
            expectedDate.Should().Be(returnedDate);
        }
    }
}
