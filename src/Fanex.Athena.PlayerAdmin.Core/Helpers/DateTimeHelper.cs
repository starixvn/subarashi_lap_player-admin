using System;

namespace Fanex.Athena.PlayerAdmin.Core.Helpers
{
    public static class DateTimeHelper
    {
        private static readonly TimeZoneInfo DatabaseServerTimezoneInfo = TimeZoneInfo.CreateCustomTimeZone(
            "DB Server Time",
            TimeSpan.FromHours(-4),
            "(GMT-04:00) DB Server Time",
            "DB Server Time");

        public static DateTime DatabaseServerNow =>
            TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                DatabaseServerTimezoneInfo);

        public static DateTime DatabaseServerToDay => DatabaseServerNow.Date;
    }
}