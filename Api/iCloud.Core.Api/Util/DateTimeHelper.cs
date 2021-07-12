using System;

namespace iCloud.Apis.Util
{
    public static class DateTimeHelper
    {
        public static string ToStartTime(this DateTime? date)
        {
            return date.ToStartOfDate()?.ToUniversalTime().ToString("s").Replace("-", "").Replace(":", "") + "Z" ?? null;
        }

        public static string ToEndTime(this DateTime? date)
        {
            return date.ToEndOfDate()?.ToUniversalTime().ToString("s").Replace("-", "").Replace(":", "") + "Z" ?? null;
        }

        public static DateTime? ToStartOfDate(this DateTime? Date)
        {
            if (Date != null) return Date.Value.Date;
            return null;
        }

        public static DateTime? ToEndOfDate(this DateTime? Date)
        {
            if (Date != null) return Date.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            return null;
        }
    }
}
