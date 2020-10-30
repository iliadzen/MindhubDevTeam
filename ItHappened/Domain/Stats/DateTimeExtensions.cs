using System;

namespace ItHappened.Domain.Stats
{
    public static class DateTimeExtensions
    {
        public static DateTime GetLast(this DateTime date, DayOfWeek day) =>
            date.AddDays(-(DaysInWeek - (int) day + (int) date.DayOfWeek) % DaysInWeek);

        public static DateTime GetNext(this DateTime date, DayOfWeek day) =>
            date.AddDays(+(DaysInWeek + (int) day - (int) date.DayOfWeek) % DaysInWeek);

        private static readonly int DaysInWeek = Enum.GetNames(typeof(DayOfWeek)).Length;
    }
}