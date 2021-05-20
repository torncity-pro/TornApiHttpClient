using System;

namespace TornApiHttpClient.Extensions
{
    public static class Util
    {
        public static DateTime TimestampToDateTime(this double timestamp)
        {
            return DateTime.UnixEpoch.AddSeconds(timestamp);
        }

        public static DateTime TimestampToDateTime(this int timestamp)
        {
            return DateTime.UnixEpoch.AddSeconds(timestamp);
        }

        public static double DateTimeToTimestamp(this DateTime datetime)
        {
            return (datetime - DateTime.UnixEpoch).TotalSeconds;
        }
    }
}
