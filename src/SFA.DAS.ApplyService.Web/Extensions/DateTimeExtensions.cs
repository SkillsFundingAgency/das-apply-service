using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApplyService.Web.Extensions;

[ExcludeFromCodeCoverage]
public static class DateTimeExtensions
{
    public static string ToSfaShortDateString(this DateTime time)
    {
        return time.ToString("dd MMMM yyyy");
    }

    public static string ToSfaShortDateString(this DateTime? time)
    {
        return time?.ToString("dd MMMM yyyy");
    }
}
