namespace System
{
    /// <summary>
    /// Represents an age in years and days.
    /// </summary>
    public struct Age
    {
        public Age(int years, int days)
        {
            Years = years;
            Days = days;
        }

        /// <summary>
        /// Gets or sets the number of years.
        /// </summary>
        public int Years { get; }

        /// <summary>
        /// Gets or sets the number of days.
        /// </summary>
        public int Days { get; }

        /// <summary>
        /// Gets the minimum age for an adult.
        /// </summary>
        public static Age MinAgeAdult => new Age(12, 0);

        /// <summary>
        /// Gets the maximum age for a child.
        /// </summary>
        public static Age MaxAgeChild => new Age(11, 364);

        /// <summary>
        /// Gets the maximum age for an infant.
        /// </summary>
        public static Age MaxAgeInfant => new Age(1, 365);

        public static bool operator ==(Age a, Age b) => a.Years == b.Years && a.Days == b.Days;
        public static bool operator !=(Age a, Age b) => !(a == b);
        public static bool operator >=(Age a, Age b) => a.Years > b.Years || (a.Years == b.Years && a.Days >= b.Days);
        public static bool operator <=(Age a, Age b) => a.Years < b.Years || (a.Years == b.Years && a.Days <= b.Days);
        public static bool operator >(Age a, Age b) => a.Years > b.Years || (a.Years == b.Years && a.Days > b.Days);
        public static bool operator <(Age a, Age b) => a.Years < b.Years || (a.Years == b.Years && a.Days < b.Days);

        public override bool Equals(object? obj) => obj is Age age && this == age;

        public override int GetHashCode() => HashCode.Combine(Years, Days);
    }
}
