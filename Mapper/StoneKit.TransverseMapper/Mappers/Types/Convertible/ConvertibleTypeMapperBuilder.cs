using StoneKit.TransverseMapper.Mappers.Builder;
using StoneKit.TransverseMapper.Mappers.Classes.Members;

using System.ComponentModel;
using System.Reflection;

namespace StoneKit.TransverseMapper.Mappers.Types.Convertible
{
    /// <summary>
    /// Builder for creating <see cref="ConvertibleTypeMapper"/> instances.
    /// </summary>
    internal sealed class ConvertibleTypeMapperBuilder : MapperBuilder
    {
        private static readonly Func<object, object> _nothingConverter = x => x;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertibleTypeMapperBuilder"/> class.
        /// </summary>
        /// <param name="config">The mapper builder configuration.</param>
        public ConvertibleTypeMapperBuilder(IMapperBuilderConfig config) : base(config)
        {
        }

        /// <summary>
        /// Gets the scope name for the mapper builder.
        /// </summary>
        protected override string ScopeName => "ConvertibleTypeMappers";

        /// <summary>
        /// Builds a <see cref="ConvertibleTypeMapper"/> for the specified type pair.
        /// </summary>
        /// <param name="typePair">The type pair.</param>
        /// <returns>The created mapper.</returns>
        protected override Mapper BuildCore(TypePair typePair)
        {
            Func<object, object> converter = GetConverter(typePair);
            return new ConvertibleTypeMapper(converter);
        }

        /// <summary>
        /// Builds a <see cref="ConvertibleTypeMapper"/> for the specified mapping member.
        /// </summary>
        /// <param name="parentTypePair">The parent type pair.</param>
        /// <param name="mappingMember">The mapping member.</param>
        /// <returns>The created mapper.</returns>
        protected override Mapper BuildCore(TypePair parentTypePair, MappingMember mappingMember)
        {
            return BuildCore(mappingMember.TypePair);
        }

        /// <summary>
        /// Checks if the specified type pair is supported by this mapper builder.
        /// </summary>
        /// <param name="typePair">The type pair.</param>
        /// <returns><c>true</c> if the type pair is supported; otherwise, <c>false</c>.</returns>
        protected override bool IsSupportedCore(TypePair typePair)
        {
            return IsSupportedType(typePair.Source) || typePair.HasTypeConverter();
        }

        private static Maybe<Func<object, object>> ConvertEnum(TypePair pair)
        {
            Func<object, object> result;
            if (pair.IsEnumTypes)
            {
                result = x => Convert.ChangeType(x, pair.Source);
                return result.ToMaybe();
            }

            if (pair.Target.IsEnum)
            {
                if (!pair.Source.IsEnum)
                {
                    if (pair.Source == typeof(string))
                    {
                        result = x => Enum.Parse(pair.Target, x.ToString());
                        return result.ToMaybe();
                    }
                }
                result = x => Enum.ToObject(pair.Target, Convert.ChangeType(x, Enum.GetUnderlyingType(pair.Target)));
                return result.ToMaybe();
            }

            if (pair.Source.IsEnum)
            {
                result = x => Convert.ChangeType(x, pair.Target);
                return result.ToMaybe();
            }
            return Maybe<Func<object, object>>.Empty;
        }

        private static Func<object, object> GetConverter(TypePair pair)
        {
            if (pair.IsDeepCloneable)
            {
                return _nothingConverter;
            }

            TypeConverter fromConverter = TypeDescriptor.GetConverter(pair.Source);
            if (fromConverter.CanConvertTo(pair.Target))
            {
                return x => fromConverter.ConvertTo(x, pair.Target);
            }

            TypeConverter toConverter = TypeDescriptor.GetConverter(pair.Target);
            if (toConverter.CanConvertFrom(pair.Source))
            {
                return x => toConverter.ConvertFrom(x);
            }

            Maybe<Func<object, object>> enumConverter = ConvertEnum(pair);
            if (enumConverter.HasValue)
            {
                return enumConverter.Value;
            }

            if (pair.IsNullableToNotNullable)
            {
                return GetConverter(new TypePair(Nullable.GetUnderlyingType(pair.Source), pair.Target));
            }

            if (pair.Target.IsNullable())
            {
                return GetConverter(new TypePair(pair.Source, Nullable.GetUnderlyingType(pair.Target)));
            }

            return null;
        }

        private bool IsSupportedType(Type value)
        {
            return value.IsPrimitive
                   || value == typeof(string)
                   || value == typeof(Guid)
                   || value.IsEnum
                   || value == typeof(decimal)
                   || value.IsNullable() && IsSupportedType(Nullable.GetUnderlyingType(value));
        }
    }
}
