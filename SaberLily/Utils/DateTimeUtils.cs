using System;

namespace SaberLily.Utils
{
    public static class DateTimeUtils
    {
        public static DateTime Jan1st1970 
        {
            get { return DateTime.Parse("1970/01/01 00:00:00"); }
        }

        public static int CurrentTimeMillis
        {
            get { return (int)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds; }
        }

        public static long UnixTicks
        {
            get { return DateTime.UtcNow.Ticks - Jan1st1970.Ticks; }
        }

        public static int UnixTimestamp
        {
            get { return Convert.ToInt32(UnixTicks / 10E+6); }
        }

    }
}
