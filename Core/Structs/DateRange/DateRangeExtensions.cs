using System.Text;

namespace System
{
    /// <summary>
    /// Provides extension methods for the DateRange struct.
    /// </summary>
    public static class DateRangeExtensions
    {
        /// <summary>
        /// Calculates the duration between two dates.
        /// </summary>
        /// <param name="range">The date range to calculate the duration for.</param>
        /// <returns>The duration between the start and end dates.</returns>
        public static TimeSpan Duration(this DateRange range)
        {
            if (!range.HasRange)
            {
                throw new InvalidOperationException("Date range is not defined.");
            }

            return range.EndDate!.Value - range.StartDate!.Value;

        }

        /// <summary>
        /// Checks if two date ranges overlap.
        /// </summary>
        /// <param name="range1">The first date range.</param>
        /// <param name="range2">The second date range.</param>
        /// <returns>True if the date ranges overlap; otherwise, false.</returns>
        public static bool OverlapsWith(this DateRange range1, DateRange range2)
        {
            if (!range1.HasRange || !range2.HasRange)
            {
                throw new InvalidOperationException("Date range is not defined.");
            }

            return range1.StartDate < range2.EndDate && range1.EndDate > range2.StartDate;
        }

        /// <summary>
        /// Calculates the intersection of two date ranges.
        /// </summary>
        /// <param name="range1">The first date range.</param>
        /// <param name="range2">The second date range.</param>
        /// <returns>The intersection of the two date ranges.</returns>
        public static DateRange Intersection(this DateRange range1, DateRange range2)
        {
            if (!range1.HasRange || !range2.HasRange)
            {
                throw new InvalidOperationException("Date range is not defined.");
            }

            var start = range1.StartDate > range2.StartDate ? range1.StartDate : range2.StartDate;
            var end = range1.EndDate < range2.EndDate ? range1.EndDate : range2.EndDate;

            return new DateRange(start, end);
        }

        /// <summary>
        /// Calculates the union of two date ranges.
        /// </summary>
        /// <param name="range1">The first date range.</param>
        /// <param name="range2">The second date range.</param>
        /// <returns>The union of the two date ranges.</returns>
        public static DateRange Union(this DateRange range1, DateRange range2)
        {
            if (!range1.HasRange || !range2.HasRange)
            {
                throw new InvalidOperationException("Date range is not defined.");
            }

            var start = range1.StartDate < range2.StartDate ? range1.StartDate : range2.StartDate;
            var end = range1.EndDate > range2.EndDate ? range1.EndDate : range2.EndDate;

            return new DateRange(start, end);
        }

        /// <summary>
        /// Shifts the date range by a specified duration.
        /// </summary>
        /// <param name="range">The date range to shift.</param>
        /// <param name="duration">The duration to shift the date range by.</param>
        /// <returns>The shifted date range.</returns>
        public static DateRange Shift(this DateRange range, TimeSpan duration)
        {
            var newStartDate = range.StartDate .HasValue ? range.StartDate + duration : null;
            var newEndDate = range.EndDate.HasValue ? range.EndDate + duration : null;

            return new DateRange(newStartDate, newEndDate);
        }

        /// <summary>
        /// Checks if the date range is empty (has no duration).
        /// </summary>
        /// <param name="range">The date range to check.</param>
        /// <returns>True if the date range is empty; otherwise, false.</returns>
        public static bool IsEmpty(this DateRange range)
        {
            return range.StartDate == range.EndDate;
        }

        /// <summary>
        /// Checks if the date range is infinite (has no end date or start date).
        /// </summary>
        /// <param name="range">The date range to check.</param>
        /// <returns>True if the date range is infinite; otherwise, false.</returns>
        public static bool IsInfinite(this DateRange range)
        {
            return !range.EndDate.HasValue || !range.StartDate.HasValue;
        }

        /// <summary>
        /// Gets the duration of the date range in days.
        /// </summary>
        /// <param name="range">The date range.</param>
        /// <returns>The duration of the date range in days.</returns>
        public static double TotalDays(this DateRange range)
        {
            return range.Duration().TotalDays;
        }

        /// <summary>
        /// Gets the duration of the date range in hours.
        /// </summary>
        /// <param name="range">The date range.</param>
        /// <returns>The duration of the date range in hours.</returns>
        public static double Hours(this DateRange range)
        {
            return range.Duration().Hours;
        }

        /// <summary>
        /// Gets the duration of the date range in minutes.
        /// </summary>
        /// <param name="range">The date range.</param>
        /// <returns>The duration of the date range in minutes.</returns>
        public static double Minutes(this DateRange range)
        {
            return range.Duration().Minutes;
        }

        /// <summary>
        /// Gets the duration of the date range in seconds.
        /// </summary>
        /// <param name="range">The date range.</param>
        /// <returns>The duration of the date range in seconds.</returns>
        public static double Seconds(this DateRange range)
        {
            return range.Duration().Seconds;
        }

        /// <summary>
        /// Gets the duration of the date range in seconds.
        /// </summary>
        /// <param name="range">The date range.</param>
        /// <returns>The duration of the date range in seconds.</returns>
        public static double TotalMilliseconds(this DateRange range)
        {
            return range.Duration().TotalMilliseconds;
        }

        public static bool HasStartDate(this DateRange? dateRange)
        {
            return dateRange?.StartDate.HasValue == true;
        }

        public static bool HasEndDate(this DateRange? dateRange)
        {
            return dateRange?.EndDate.HasValue == true;
        }

        public static bool HasStartDate(this DateRange dateRange)
        {
            return dateRange.StartDate.HasValue;
        }

        public static bool HasEndDate(this DateRange dateRange)
        {
            return dateRange.EndDate.HasValue;
        }
    }
}
