using System.Reflection.Mapping;

using Xunit;

namespace StoneKit.TransverseMapper.Tests.Mappings
{
    public class MappingWithComplexConfigTests
    {
        [Fact]
        public void Map_ComplexBind_Success()
        {
            Transverse.Bind<PersonDto, Person>(
                config =>
                {
                    config.Bind(source => source.Address.Street, target => target.Street);
                    config.Bind(source => source.Address.Phone, target => target.Phone);
                }
            );

            var dto = new PersonDto
            {
                Address = new AddressDto
                {
                    Street = "Street",
                    Phone = "123123"
                },
                Code = "Code",
                Identity = 1,
                Name = "Alex"
            };

            var person = Transverse.Map<Person>(dto);

            Assert.Equal(dto.Identity, person.Identity);
            Assert.Equal(dto.Code, person.Code);
            Assert.Equal(dto.Name, person.Name);
            Assert.Equal(dto.Address.Street, person.Street);
            Assert.Equal(dto.Address.Phone, person.Phone);
        }

        [Fact]
        public void Map_ComplexBind_Success1()
        {
            Transverse.Bind<PersonDto, Person>(
                config =>
                {
                    config.Bind(source => source.Address.Street, target => target.Street);
                    config.Bind(source => source.Address.Phone, target => target.Phone);
                }
            );

            var dto = new PersonDto
            {
                Address = new AddressDto
                {
                    Street = "Street",
                    Phone = "123123"
                },
                Code = "Code",
                Identity = 1,
                Name = "Alex"
            };

            var person = (Person)Transverse.Map(typeof(PersonDto), typeof(Person), dto);

            Assert.Equal(dto.Identity, person.Identity);
            Assert.Equal(dto.Code, person.Code);
            Assert.Equal(dto.Name, person.Name);
            Assert.Equal(dto.Address.Street, person.Street);
            Assert.Equal(dto.Address.Phone, person.Phone);
        }


        public class Person
        {
            public int Identity { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Street { get; set; }
            public string Phone { get; set; }
        }

        public class PersonDto
        {
            public int Identity { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public AddressDto Address { get; set; }
        }

        public class AddressDto
        {
            public string Street { get; set; }
            public string Phone { get; set; }
        }
    }
}
