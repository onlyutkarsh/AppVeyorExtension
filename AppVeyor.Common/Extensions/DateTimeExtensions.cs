using System;

namespace AppVeyor.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public enum Precision
        {
            Second,
            Minute,
            Hour,
            Day,
            Week,
            Month,
            Year,
            None
        }
        public static DateTime Round(this DateTime date, TimeSpan span)
        {
            long ticks = (date.Ticks + (span.Ticks / 2) + 1) / span.Ticks;
            return new DateTime(ticks * span.Ticks);
        }

        public static DateTime Floor(this DateTime date, TimeSpan span)
        {
            long ticks = (date.Ticks / span.Ticks);
            return new DateTime(ticks * span.Ticks);
        }

        public static DateTime Ceil(this DateTime date, TimeSpan span)
        {
            long ticks = (date.Ticks + span.Ticks - 1) / span.Ticks;
            return new DateTime(ticks * span.Ticks);
        }

        public static string PrettyDate(this DateTime timeSubmitted)
        {
            // accepts standard DateTime: 5/12/2011 2:36:00 PM 
            // returns: "# month(s)/week(s)/day(s)/hour(s)/minute(s)/second(s)) ago"
            string prettyDate;

            TimeSpan diff = DateTime.Now - timeSubmitted;

            if (diff.Seconds <= 0)
            {
                prettyDate = timeSubmitted.ToString("dd, MMM, yyyy");
            }
            else if (diff.Days > 365 )
            {
                var year = Math.Round(diff.Days / 365.0);
                prettyDate = String.Format("{0} year{1}ago", year, (year >= 2 ? "s " : " "));
            }
            else if (diff.Days > 30)
            {
                var month = Math.Round(diff.Days / 30.0);
                prettyDate = String.Format("{0} month{1}ago", month, (month >= 2 ? "s " : " "));
            }
            else if (diff.Days > 7)
            {
                var week = Math.Round(diff.Days / 7.0);
                prettyDate = String.Format("{0} week{1}ago", week, (week >= 2 ? "s " : " "));
            }
            else if (diff.Days >= 1)
            {
                prettyDate = String.Format("{0} day{1}ago", diff.Days, (diff.Days >= 2 ? "s " : " "));
            }
            else if (diff.Hours >= 1)
            {
                prettyDate = String.Format("{0} hour{1}ago", diff.Hours, (diff.Hours >= 2 ? "s " : " "));
            }
            else if (diff.Minutes >= 1)
            {
                prettyDate = String.Format("{0} minute{1}ago", diff.Minutes, (diff.Minutes >= 2 ? "s " : " "));
            }
            else
            {
                prettyDate = String.Format("{0} second{1}ago", diff.Seconds, (diff.Seconds >= 2 ? "s " : " "));
            }
            return prettyDate;
        }

        public static string PrettyDate(this string timeSubmitted)
        {
            DateTime date;
            if (DateTime.TryParse(timeSubmitted, out date))
            {
                return date.PrettyDate();
            }
            return "Invalid Date";
        }

        public static string GetHumanizedRunningFromTime(this string startedTime)
        {
            DateTime startedAt;
            if (DateTime.TryParse(startedTime, out startedAt))
            {
                var timeSpan = DateTime.Now.Subtract(startedAt);
                return string.Format(" - {0} min {1} sec", timeSpan.Minutes, timeSpan.Seconds);

            };
            return string.Empty;

        }
    }
}
