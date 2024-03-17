
namespace StoneKit.Core.Structs.Tests
{
    public class DateRangeTests
    {
        [Fact]
        public void Constructor_WithValidDates_CreatesDateRange()
        {
            // Arrange
            var startDate = new DateTime(2022, 1, 1);
            var endDate = new DateTime(2022, 1, 31);

            // Act
            var range = new DateRange(startDate, endDate);

            // Assert
            Assert.Equal(startDate, range.StartDate);
            Assert.Equal(endDate, range.EndDate);
        }

        [Fact]
        public void Constructor_WithEndDateEarlierThanStartDate_ThrowsArgumentException()
        {
            // Arrange
            var startDate = new DateTime(2022, 1, 31);
            var endDate = new DateTime(2022, 1, 1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new DateRange(startDate, endDate));
        }

        [Fact]
        public void Constructor_NoStartAndEndDate_ThrowsException()
        {
            // Arrange & Act & Assert
            Assert.Throws<Exception>(() =>
            {
                var range = new DateRange();

                range.Contains(new DateTime(2022, 1, 15));
            });
        }
        
        [Fact]
        public void HasValue_ReturnFalse()
        {
            var range = new DateRange(new DateTime(2022, 1, 1));
            Assert.True(range.HasStartDate());
            Assert.False(range.HasEndDate());

            DateRange? range2 = new DateRange(new DateTime(2022, 1, 1));
            Assert.True(range2.HasStartDate());
            Assert.False(range2.HasEndDate());
        }

        [Fact]
        public void Contains_DateWithinRangeWithoutEndDate_ReturnsTrue()
        {
            // Arrange
            var range = new DateRange(new DateTime(2022, 1, 1));

            // Act
            bool result1 = range.Contains(new DateTime(2022, 1, 15));
            bool result2 = range.Contains(new DateTime(2022, 1, 1), true);

            // Assert
            Assert.True(result1);
            Assert.True(result2);
        }

        [Fact]
        public void Contains_DateWithinRangeWithoutStartDate_ReturnsTrue()
        {
            // Arrange
            var range = new DateRange(null, new DateTime(2022, 1, 10));

            // Act
            bool result1_False = range.Contains(new DateTime(2022, 1, 15));
            bool result2_False = range.Contains(new DateTime(2022, 1, 15), true);

            bool result1_True = range.Contains(new DateTime(2022, 1, 5));
            bool result2_True = range.Contains(new DateTime(2022, 1, 10), true);

            // Assert
            Assert.False(result1_False);
            Assert.False(result2_False);
            Assert.True(result1_True);
            Assert.True(result2_True);
        }

        [Fact]
        public void Contains_DateWithinRangeInclusive_ReturnsTrue()
        {
            // Arrange
            var range = new DateRange(new DateTime(2022, 1, 1), new DateTime(2022, 1, 31));

            // Act
            bool result = range.Contains(new DateTime(2022, 1, 15), true);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Contains_DateWithinRangeExclusive_ReturnsFalse()
        {
            // Arrange
            var range = new DateRange(new DateTime(2022, 1, 1), new DateTime(2022, 1, 31));

            // Act
            bool result = range.Contains(new DateTime(2022, 1, 31), false);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void OverlapsWith_OverlappingRanges_ReturnsTrue()
        {
            // Arrange
            var range1 = new DateRange(new DateTime(2022, 1, 1), new DateTime(2022, 1, 15));
            var range2 = new DateRange(new DateTime(2022, 1, 10), new DateTime(2022, 1, 20));

            // Act
            bool result = range1.OverlapsWith(range2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void OverlapsWith_NonOverlappingRanges_ReturnsFalse()
        {
            // Arrange
            var range1 = new DateRange(new DateTime(2022, 1, 1), new DateTime(2022, 1, 15));
            var range2 = new DateRange(new DateTime(2022, 1, 20), new DateTime(2022, 1, 25));

            // Act
            bool result = range1.OverlapsWith(range2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Intersection_RangesOverlap_ReturnsIntersectionRange()
        {
            // Arrange
            var range1 = new DateRange(new DateTime(2022, 1, 1), new DateTime(2022, 1, 15));
            var range2 = new DateRange(new DateTime(2022, 1, 10), new DateTime(2022, 1, 20));

            // Act
            var intersection = range1.Intersection(range2);

            // Assert
            Assert.Equal(new DateTime(2022, 1, 10), intersection.StartDate);
            Assert.Equal(new DateTime(2022, 1, 15), intersection.EndDate);
        }

        [Fact]
        public void Union_RangesDoNotOverlap_ReturnsCombinedRange()
        {
            // Arrange
            var range1 = new DateRange(new DateTime(2022, 1, 1), new DateTime(2022, 1, 10));
            var range2 = new DateRange(new DateTime(2022, 1, 15), new DateTime(2022, 1, 25));

            // Act
            var union = range1.Union(range2);

            // Assert
            Assert.Equal(new DateTime(2022, 1, 1), union.StartDate);
            Assert.Equal(new DateTime(2022, 1, 25), union.EndDate);
        }

        [Fact]
        public void Duration_ValidRange_ReturnsCorrectTimeSpan()
        {
            // Arrange
            var range = new DateRange(new DateTime(2022, 1, 1), new DateTime(2022, 1, 31));

            // Act
            var duration = range.Duration();

            // Assert
            Assert.Equal(TimeSpan.FromDays(30), duration);
        }

        [Fact]
        public void Shift_RangeShiftedByPositiveDuration_ReturnsShiftedRange()
        {
            // Arrange
            var range = new DateRange(new DateTime(2022, 1, 1), new DateTime(2022, 1, 15));
            var shiftDuration = TimeSpan.FromDays(5);

            // Act
            var shiftedRange = range.Shift(shiftDuration);

            // Assert
            Assert.Equal(new DateTime(2022, 1, 6), shiftedRange.StartDate);
            Assert.Equal(new DateTime(2022, 1, 20), shiftedRange.EndDate);
        }

        [Fact]
        public void IsEmpty_EmptyRange_ReturnsTrue()
        {
            // Arrange
            var emptyRange = new DateRange(new DateTime(2022, 1, 1), new DateTime(2022, 1, 1));

            // Act
            var result = emptyRange.IsEmpty();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsEmpty_NonEmptyRange_ReturnsFalse()
        {
            // Arrange
            var nonEmptyRange = new DateRange(new DateTime(2022, 1, 1), new DateTime(2022, 1, 15));

            // Act
            var result = nonEmptyRange.IsEmpty();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsInfinite_RangeWithEndDate_ReturnsFalse()
        {
            // Arrange
            var finiteRange = new DateRange(new DateTime(2022, 1, 1), new DateTime(2022, 1, 15));

            // Act
            var result = finiteRange.IsInfinite();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsInfinite_RangeWithoutEndDate_ReturnsTrue()
        {
            // Arrange
            var infiniteRange = new DateRange(new DateTime(2022, 1, 1), null);

            // Act
            var result = infiniteRange.IsInfinite();

            // Assert
            Assert.True(result);
        }

    }
}
