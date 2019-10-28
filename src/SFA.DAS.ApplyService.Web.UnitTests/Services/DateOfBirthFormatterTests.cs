using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApplyService.Web.Services;

namespace SFA.DAS.ApplyService.Web.UnitTests.Services
{
    [TestFixture]
    public class DateOfBirthFormatterTests
    {
        [TestCase("1", "1990", "Jan 1990")]
        [TestCase("2", "1991", "Feb 1991")]
        [TestCase("3", "1992", "Mar 1992")]
        [TestCase("4", "1993", "Apr 1993")]
        [TestCase("5", "1994", "May 1994")]
        [TestCase("6", "1995", "Jun 1995")]
        [TestCase("7", "1996", "Jul 1996")]
        [TestCase("8", "1997", "Aug 1997")]
        [TestCase("9", "1998", "Sep 1998")]
        [TestCase("10", "1999", "Oct 1999")]
        [TestCase("11", "1989", "Nov 1989")]
        [TestCase("12", "1988", "Dec 1988")]
        public void Format_date_of_birth_converts_month_and_year_to_short_date(string month, string year, string expectedValue)
        {
            var formattedDob = DateOfBirthFormatter.FormatDateOfBirth(month, year);

            formattedDob.Should().Be(expectedValue);
        }

        [TestCase("Jan 1990", "1")]
        [TestCase("Feb 1991", "2")]
        [TestCase("Mar 1992", "3")]
        [TestCase("Apr 1993", "4")]
        [TestCase("May 1994", "5")]
        [TestCase("Jun 1995", "6")]
        [TestCase("Jul 1996", "7")]
        [TestCase("Aug 1997", "8")]
        [TestCase("Sep 1998", "9")]
        [TestCase("Oct 1999", "10")]
        [TestCase("Nov 1989", "11")]
        [TestCase("Dec 1988", "12")]
        public void Get_month_number_converts_short_date_to_month_number(string shortDate, string expectedValue)
        {
            var monthNumber = DateOfBirthFormatter.GetMonthNumberFromShortDateOfBirth(shortDate);

            monthNumber.Should().Be(expectedValue);
        }

        [TestCase("Jan 1990", "1990")]       
        public void Get_year_converts_short_date_to_year(string shortDate, string expectedValue)
        {
            var year = DateOfBirthFormatter.GetYearFromShortDateOfBirth(shortDate);

            year.Should().Be(expectedValue);
        }
    }
}
