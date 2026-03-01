namespace TtrxToHtml.Console.Helpers
{
    public static class TimeSpanExtensions
    {
        public static long ToMilliseconds(this TimeSpan timeSpan)
        {
            return Convert.ToInt64(Math.Round(timeSpan.TotalMilliseconds));
        }

        public static string ToMillisecondsString(this TimeSpan timeSpan)
        {
            return timeSpan.ToMilliseconds().ToString();
        }

        public static long TimeOfDay(this DateTime dateTime)
        {
            return dateTime.TimeOfDay.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static long ToMilliseconds(this DateTime dateTime)
        {
            return Convert.ToInt64(Math.Round(dateTime.TimeOfDay.TotalMilliseconds));
        }
    }
}