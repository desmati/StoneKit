using System;
using System.Reflection.Mapping;
using Xunit;

namespace StoneKit.TransverseMapper.Tests.Mappings.Classes
{
    public sealed class ClassHierarchyMappingTests
    {
        [Fact]
        public void Map_Hierarchy_Success()
        {
            Transverse.Bind<Source, Target>();

            var source = new Source
            {
                Id = 1,
                String = "tiny"
            };

            var actual = Transverse.Map<Target>(source);

            Assert.Equal(source.Id, actual.Id);
            Assert.Equal(source.String, actual.String);
        }


        public class Source : SourceBase
        {
            public string String { get; set; }
        }


        public abstract class SourceBase
        {
            public int Id { get; set; }
        }


        public abstract class TargetBase
        {
            public int Id { get; set; }
        }


        public sealed class Target : TargetBase
        {
            public string String { get; set; }
        }
    }
}
