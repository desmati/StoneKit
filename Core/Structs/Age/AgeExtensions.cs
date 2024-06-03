namespace System
{
    /// <summary>
    /// Utilities for calculating and determining age according to given date.
    /// </summary>
    public static class AgeExtensions
    {
        #region Age Utilities

        /// <summary>
        /// Calculates the age based on the birthday.
        /// </summary>
        /// <param name="birthday">The birth date.</param>
        /// <returns>The calculated age.</returns>
        public static Age GetAge(this DateTime birthday) => GetAge(birthday, DateTime.Now);

        /// <summary>
        /// Calculates the age based on the birthday and a specific date.
        /// </summary>
        /// <param name="birthday">The birth date.</param>
        /// <param name="referenceDate">The specific date to calculate the age at.</param>
        /// <returns>The calculated age.</returns>
        public static Age GetAge(this DateTime birthday, DateTime referenceDate)
        {
            var diff = new DateTime(referenceDate.Subtract(birthday).Ticks);
            return new Age(diff.Year - 1, diff.DayOfYear);
        }

        /// <summary>
        /// Determines if a person is an infant based on their birth date.
        /// An infant is defined as being from 2 weeks to 1 year and 364 days old.
        /// </summary>
        /// <param name="birthday">The birth date.</param>
        /// <returns>True if the person is an infant, otherwise false.</returns>
        public static bool IsInfant(this DateTime birthday) => IsInfant(birthday, DateTime.Now);

        /// <summary>
        /// Determines if a person is an infant based on their birth date and a specific date.
        /// </summary>
        /// <param name="birthday">The birth date.</param>
        /// <param name="referenceDate">The specific date to check against.</param>
        /// <returns>True if the person is an infant, otherwise false.</returns>
        public static bool IsInfant(this DateTime birthday, DateTime referenceDate)
        {
            return birthday.GetAge(referenceDate) <= Age.MaxAgeInfant;
        }

        /// <summary>
        /// Determines if a person is a child based on their birth date.
        /// A child is defined as being up to 11 years and 364 days old.
        /// </summary>
        /// <param name="birthday">The birth date.</param>
        /// <returns>True if the person is a child, otherwise false.</returns>
        public static bool IsChild(this DateTime birthday) => IsChild(birthday, DateTime.Now);

        /// <summary>
        /// Determines if a person is a child based on their birth date and a specific date.
        /// </summary>
        /// <param name="birthday">The birth date.</param>
        /// <param name="referenceDate">The specific date to check against.</param>
        /// <returns>True if the person is a child, otherwise false.</returns>
        public static bool IsChild(this DateTime birthday, DateTime referenceDate)
        {
            return !IsInfant(birthday, referenceDate) && birthday.GetAge(referenceDate) <= Age.MaxAgeChild;
        }

        /// <summary>
        /// Determines if a person is an adult based on their birth date.
        /// </summary>
        /// <param name="birthday">The birth date.</param>
        /// <returns>True if the person is an adult, otherwise false.</returns>
        public static bool IsAdult(this DateTime birthday) => IsAdult(birthday, DateTime.Now);

        /// <summary>
        /// Determines if a person is an adult based on their birth date and a specific date.
        /// </summary>
        /// <param name="birthday">The birth date.</param>
        /// <param name="referenceDate">The specific date to check against.</param>
        /// <returns>True if the person is an adult, otherwise false.</returns>
        public static bool IsAdult(this DateTime birthday, DateTime referenceDate)
        {
            return !IsInfant(birthday, referenceDate) && !IsChild(birthday, referenceDate);
        }

        /// <summary>
        /// Converts the age to an integer representing the number of years.
        /// </summary>
        /// <param name="age">The age to convert.</param>
        /// <returns>The number of years.</returns>
        public static int ToInt32(this Age age) => age.Years;

        #endregion
    }
}
