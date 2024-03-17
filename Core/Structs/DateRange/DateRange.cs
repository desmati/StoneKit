namespace System
{

    /// <summary>
    /// Represents a range of dates, allowing checks for inclusion of a date within the range.
    /// </summary>
    public struct DateRange
    {
        internal const string DateRange_NO_RANGE_PROVIDED = "No range provided";

        /// <summary>
        /// Gets or sets the start date of the range.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the range.
        /// </summary>
        public DateTime? EndDate { get; set; }

        public bool HasValue => StartDate.HasValue || EndDate.HasValue;
        public bool HasRange => StartDate.HasValue && EndDate.HasValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRange"/> class with optional start and end dates.
        /// Throws an exception if end date is earlier than start date (if both are provided).
        /// </summary>
        /// <param name="startDate">Optional start date of the range.</param>
        /// <param name="endDate">Optional end date of the range.</param>
        public DateRange(DateTime? startDate = null, DateTime? endDate = null)
        {
            // Check if the provided end date is earlier than the start date (if both are provided).
            if (endDate.HasValue && startDate.HasValue && endDate < startDate)
            {
                throw new ArgumentException("End date must be greater than or equal to start date.");
            }

            // Set the properties with the provided values (or null if not provided).
            StartDate = startDate;
            EndDate = endDate;
        }

        /// <summary>
        /// Checks if a given date falls within the date range defined by the StartDate and EndDate properties.
        /// </summary>
        /// <param name="date">The date to be checked.</param>
        /// <param name="isInclusive">Indicates whether the check is inclusive (default is true).</param>
        /// <returns>
        /// True if the date is within the range. If isInclusive is true, the date must be greater than or equal to the start date
        /// and less than or equal to the end date to return true. If isInclusive is false, the date must be strictly greater than
        /// the start date and strictly less than the end date to return true.
        /// </returns>
        /// <exception cref="Exception">Thrown when neither start nor end dates are provided.</exception>
        public bool Contains(DateTime date, bool isInclusive = true)
        {

            // If neither start nor end date is provided.
            if (!StartDate.HasValue && !EndDate.HasValue)
            {
                throw new Exception(DateRange_NO_RANGE_PROVIDED);
            }

            // If the start date is provided but the end date is not.
            if (StartDate.HasValue && !EndDate.HasValue)
            {
                // Check if the date is within the range based on the inclusive/exclusive flag.
                if (isInclusive)
                {
                    return date >= StartDate.Value;
                }
                else
                {
                    return date > StartDate.Value;
                }
            }

            // If the end date is provided but the start date is not.
            if (!StartDate.HasValue && EndDate.HasValue)
            {
                // Check if the date is within the range based on the inclusive/exclusive flag.
                if (isInclusive)
                {
                    return date <= EndDate.Value;
                }
                else
                {
                    return date < EndDate.Value;
                }
            }

            // If inclusive check is requested.
            if (isInclusive)
            {
                // Check if the date is greater than or equal to the start date,
                // and less than or equal to the end date.
                return (!StartDate.HasValue || date >= StartDate) && (!EndDate.HasValue || date <= EndDate);
            }
            else // If exclusive check is requested.
            {
                // Check if the date is greater than the start date, and less than the end date.
                return (!StartDate.HasValue || date > StartDate) && (!EndDate.HasValue || date < EndDate);
            }
        }

    }



}
