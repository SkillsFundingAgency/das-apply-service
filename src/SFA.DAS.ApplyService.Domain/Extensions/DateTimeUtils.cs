using System;

namespace SFA.DAS.ApplyService.Domain.Extensions
{
    public static class DateTimeUtils
    {
        public static string ToSfaShortDateString(this DateTime time)
        {
            return time.ToString("d MMMM yyyy");
        }

        public static string ToSfaShortDateString(this DateTime? time)
        {
            return time?.ToString("d MMMM yyyy") ?? string.Empty;
        }
    }
}
