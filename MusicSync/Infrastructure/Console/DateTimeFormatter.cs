using System.Globalization;

namespace MusicSync.Infrastructure.Console;

public sealed class DateTimeFormatter(TimeProvider timeProvider)
{
    public string ToDisplay(DateTimeOffset value)
    {
        return TimeZoneInfo.ConvertTime(value, timeProvider.LocalTimeZone)
            .ToString("g", CultureInfo.CurrentCulture);
    }
}
