using System;

namespace SFA.DAS.ApplyService.Application.Services
{
    public static class DateOfBirthFormatter
    {
        private static string[] ShortMonthNames = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public static string FormatDateOfBirth(string month, string year)
        {
            if (String.IsNullOrWhiteSpace(month) || String.IsNullOrWhiteSpace(year))
            {
                return string.Empty;
            }

            int monthValue = 0;
            int yearValue = 0;
            int.TryParse(month, out monthValue);
            int.TryParse(year, out yearValue);

            if (monthValue == 0 || yearValue == 0)
            {
                return string.Empty;
            }

            var index = monthValue - 1;

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


        public static string GetMonthYearDescription(string shortDob)
        {
            var separator =',';
            if (string.IsNullOrEmpty(shortDob))
            {
                return string.Empty;
            }

            var details = shortDob.Split(separator);
            if (details.Length != 2)
                return string.Empty;

            var year = details[1];
            var month = details[0];


            if (!int.TryParse(month, out var monthNumber))
                return string.Empty;

            if (monthNumber < 1 || monthNumber > 12)
                return string.Empty;

            var monthDescription = ShortMonthNames[monthNumber - 1];

            return $"{monthDescription} {year}";
        }
    }
}
