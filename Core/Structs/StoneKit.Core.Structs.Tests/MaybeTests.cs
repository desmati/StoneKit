namespace StoneKit.Core.Structs.Tests
{
    public class MaybeTests
    {
        [Fact]
        public void Test1()
        {
            var maybeWithValue = new Maybe<int>(42);
            var maybeEmpty = Maybe<int>.Empty;
            var toMaybe = 10.ToMaybe();

            var maybe = Maybe<int>.Empty
                .Or(10)
                .Where(x => x > 5)
                .Perform(x=> Console.WriteLine($"{x} is greater than 5."))
                .Match(
                    value => value < 50,
                    value => Console.WriteLine($"{value} is smaller than 50.")
                )
                .Where(x=> x >100)
                .PerformOnEmpty(() => Console.WriteLine("Performing action on empty Maybe"))
                .Or(100)
                .Finally(x=> Console.WriteLine("I'm done!"));

            Assert.Equal(100, maybe.Value);
            
        }
    }
}