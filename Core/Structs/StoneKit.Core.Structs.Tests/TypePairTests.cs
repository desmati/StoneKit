namespace StoneKit.Core.Structs.Tests
{
    public class TypePairTests
    {
        [Fact]
        public void TestTypePair()
        {
            var pairCollection = new TypePair(typeof(List<string>), typeof(HashSet<string>));
            Assert.True(pairCollection.IsEnumerableTypes);
            Assert.False(pairCollection.IsEnumTypes);
            
            var pairNumber = new TypePair(typeof(int?), typeof(long));
            Assert.True(pairNumber.IsNullableToNotNullable);
            Assert.False(pairNumber.IsDeepCloneable);
        }
    }
}