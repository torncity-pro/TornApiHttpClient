using System;

namespace TornApiHttpClient.Extensions
{
    public static class Util
    {
        public static readonly DateTime UTCDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static DateTime TimestampToDateTime(this double timestamp)
        {
            return UTCDate.AddSeconds(timestamp);
        }

        public static DateTime TimestampToDateTime(this int timestamp)
        {
            return UTCDate.AddSeconds(timestamp);
        }

        public static double DateTimeToTimestamp(this DateTime datetime)
        {
            return (datetime - UTCDate).TotalSeconds;
        }
    }
}
