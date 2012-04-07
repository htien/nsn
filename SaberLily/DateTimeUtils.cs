using System;

namespace SaberLily
{
    public static class DateTimeUtils
    {
        public static long UnixTicks
        {
            get { return DateTime.UtcNow.Ticks - DateTime.Parse("1970/01/01 00:00:00").Ticks; }
        }

        public static int UnixTimestamp
        {
            get { return Convert.ToInt32(UnixTicks / 10E+6); }
        }

    }
}
