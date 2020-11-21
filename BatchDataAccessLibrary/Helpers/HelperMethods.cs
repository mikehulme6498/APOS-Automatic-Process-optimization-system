using System;
using System.Globalization;

namespace BatchDataAccessLibrary.Helpers
{
    public static class HelperMethods
    {
        public static int GetWeekNumber(DateTime time)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }
        static public DateTime AdjustDateIfTimePassesIntoNextDay(string dateToSort, string timeToSort, DateTime OriginalStartTime)
        {
            // Adds one day to date if time goes past 00:00

            if (timeToSort.EndsWith("."))
            {
                timeToSort = timeToSort.Substring(0, timeToSort.Length - 1);
            }
            if (timeToSort.Length == 5)
            {
                timeToSort += ":59";
            }
            DateTime newTimes = DateTime.ParseExact(dateToSort + " " + timeToSort, "dd/MM/yyyy HH:mm:ss", null);
            double differenceInMinutes = newTimes.Subtract(OriginalStartTime).TotalMinutes;

            if (differenceInMinutes < 0)
            {
                newTimes = newTimes.AddDays(1);
            }

            return newTimes;
        }
    }
}
