using System;

namespace SFA.DAS.ApplyService.Web.Services
{
    public static class DateOfBirthFormatter
    {
        private static string[] ShortMonthNames = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public static string FormatDateOfBirth(string month, string year)
        {
            var index = (Convert.ToInt32(month)) - 1;

            if (index < 0 || index > 11)
            {
                return string.Empty;
            }
            return $"{ShortMonthNames[index]} {year}";
        }

        public static string GetMonthNumberFromShortDateOfBirth(string shortDob)
        {
            var endIndex = shortDob.IndexOf(" ");
            if (endIndex < 0)
            {
                return string.Empty;
            }
            var monthSegment = shortDob.Substring(0, endIndex);
            for (var index = 0; index < ShortMonthNames.Length; index++)
            {
                if (ShortMonthNames[index] == monthSegment)
                {
                    return (index + 1).ToString();
                }
            }
            return "1";
        }

        public static string GetYearFromShortDateOfBirth(string shortDob)
        {
            var endIndex = shortDob.IndexOf(" ");
            if (endIndex < 0)
            {
                return string.Empty;
            }
            return shortDob.Substring(endIndex).Trim();
        }
    }
}
