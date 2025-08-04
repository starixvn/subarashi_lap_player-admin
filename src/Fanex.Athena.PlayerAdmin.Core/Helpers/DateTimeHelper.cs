using System;

namespace Fanex.Athena.PlayerAdmin.Core.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

        private static readonly TimeZoneInfo DatabaseServerTimezoneInfo = TimeZoneInfo.CreateCustomTimeZone(
            "DB Server Time",
            TimeSpan.FromHours(-4),
            "(GMT-04:00) DB Server Time",
            "DB Server Time");

        public static DateTimeOffset DatabaseServerNowOffset =>
            TimeZoneInfo.ConvertTime(UtcNow, DatabaseServerTimezoneInfo);

        public static DateTime DatabaseServerNow => DatabaseServerNowOffset.DateTime;

        public static DateTime DatabaseServerToDay => DatabaseServerNow.Date;

        public static DateTime CreateDateTime(int year, int month, int day, int hour = 0, int minute = 0, int second = 0, TimeSpan? offset = null)
        {
            var actualOffset = offset ?? DatabaseServerNowOffset.Offset;
            return new DateTimeOffset(year, month, day, hour, minute, second, actualOffset).DateTime;
        }
    }
}