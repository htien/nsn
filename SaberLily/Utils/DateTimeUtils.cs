using System;

namespace SaberLily.Utils
{
    public class DateTimeUtils
    {
        public static DateTime Jan1st1970 
        {
            get { return new DateTime(1970, 1, 1, 0, 0, 0); }
        }

        public static long CurrentTimeMillis
        {
            get { return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds; }
        }

        public static long UnixTicks
        {
            get { return DateTime.UtcNow.Ticks - Jan1st1970.Ticks; }
        }

        public static int UnixTimestamp
        {
            get { return Convert.ToInt32(UnixTicks / 10E+6); }
        }

        public static int ConvertToUnixTimestamp(DateTime date)
        {
            return Convert.ToInt32(Math.Floor((date - Jan1st1970).TotalSeconds));
        }

        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            return Jan1st1970.AddSeconds(timestamp);
        }
    }
}
