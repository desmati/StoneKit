#define VS2015

using System.Reflection.Mapping;

using Xunit;

namespace UnitTests.Mappings.Attributes
{
    public sealed class MappingWithGenericTypeTests
    {
        interface IEntity<T>
        {
            T Key { get; set; }
        }


        public abstract class Entity<T> : IEntity<T>
        {
            public T Key { get; set; }
        }


        public class SourceDto
        {
            [Bind(nameof(Target.Key), typeof(Target))]
            public long Id { get; set; }
        }


        public class Target : Entity<long>
        {
        }


        private static SourceDto CreateSource()
        {
            return new SourceDto
            {
                Id = 23,
            };
        }

        private static Target CreateTarget()
        {
            return new Target
            {
                Key = 23,
            };
        }

        [Fact]
        public void Map_WithType_Success()
        {
            Transverse.Bind<SourceDto, Target>();

            SourceDto source = CreateSource();
            var target = Transverse.Map<Target>(source);

            Assert.Equal(target.Key, source.Id);
        }

        [Fact]
        public void Map_WithType_Back_Success()
        {
            Transverse.Bind<Target, SourceDto>();
            Target target = CreateTarget();
            var source = Transverse.Map<SourceDto>(target);

            Assert.Equal(source.Id, target.Key);
        }
    }
}