namespace StoneKit.DataStore.Tests
{
    internal class SampleData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SampleSubData SubData { get; set; }
    }

    internal class SampleSubData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
