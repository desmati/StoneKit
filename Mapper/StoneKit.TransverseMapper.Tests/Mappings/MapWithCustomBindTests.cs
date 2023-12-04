using System;
using Xunit;

namespace UnitTests.Mappings
{
    public sealed class MapWithCustomBindTests
    {
        [Fact]
        public void Map_CustomBind_Success()
        {
            //            Transverse.Bind<SourceCustom, TargetCustom>(config => config.Bind(target => target.Name, "John"));
            //
            //            var source = new SourceCustom();
            //            var result = Transverse.Map<TargetCustom>(source);
            //
            //            Assert.Equal("John", result.Name);
            //            Assert.True(string.IsNullOrWhiteSpace(source.Name));
        }


        public class SourceCustom
        {
            public string Name { get; set; }
        }


        public class TargetCustom
        {
            public string Name { get; set; }
        }
    }
}
