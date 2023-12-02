using StoneKit.TransverseMapper.MapperBuilder;
using StoneKit.TransverseMapper.Mappers.Members;

using System.ComponentModel;
using System.Reflection;

namespace StoneKit.TransverseMapper.Mappers.Types.Convertible
{
    /// <summary>
    /// Builder for creating <see cref="ConvertibleTypeMapper"/> instances.
    /// </summary>
    internal sealed class ConvertibleTypeMapperBuilder : MapperBuilderBase
    {
        private static readonly Func<object, object> _nothingConverter = x => x;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertibleTypeMapperBuilder"/> class.
        /// </summary>
        /// <param name="config">The mapper builder configuration.</param>
        public ConvertibleTypeMapperBuilder(IMapperBuilderConfig config) : base(config)
        {
        }

        /// <inheritdoc/>
        protected override string ScopeName => "ConvertibleTypeMappers";

        /// <inheritdoc/>
        protected override MapperBase BuildCore(TypePair typePair)
        {
            Func<object, object> converter = GetConverter(typePair);
            return new ConvertibleTypeMapper(converter);
        }

        /// <inheritdoc/>
        protected override MapperBase BuildCore(TypePair parentTypePair, MappingMember mappingMember)
        {
            return BuildCore(mappingMember.TypePair);
        }

        /// <inheritdoc/>
        protected override bool IsSupportedCore(TypePair typePair)
        {
            return IsSupportedType(typePair.Source) || typePair.HasTypeConverter();
        }

        /// <summary>
        /// Converts the source enum to the target enum or vice versa.
        /// </summary>
        /// <param name="pair">The type pair.</param>
        /// <returns>The converter function if conversion is possible; otherwise, an empty maybe.</returns>
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
                if (pair.Source.IsEnum == false)
                {
                    if (pair.Source == typeof(string))
                    {
                        result = x => Enum.Parse(pair.Target, x.ToString()!);
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

        /// <summary>
        /// Gets the appropriate converter function for the given type pair.
        /// </summary>
        /// <param name="pair">The type pair.</param>
        /// <returns>The converter function or null if no converter is found.</returns>
        private static Func<object, object> GetConverter(TypePair pair)
        {
            if (pair.IsDeepCloneable)
            {
                return _nothingConverter;
            }

            TypeConverter fromConverter = TypeDescriptor.GetConverter(pair.Source);
            if (fromConverter.CanConvertTo(pair.Target))
            {
                return x => fromConverter.ConvertTo(x, pair.Target)!;
            }

            TypeConverter toConverter = TypeDescriptor.GetConverter(pair.Target);
            if (toConverter.CanConvertFrom(pair.Source))
            {
                return x => toConverter.ConvertFrom(x)!;
            }

            Maybe<Func<object, object>> enumConverter = ConvertEnum(pair);
            if (enumConverter.HasValue)
            {
                return enumConverter.Value;
            }

            if (pair.IsNullableToNotNullable)
            {
                return GetConverter(new TypePair(Nullable.GetUnderlyingType(pair.Source)!, pair.Target));
            }

            if (pair.Target.IsNullable())
            {
                return GetConverter(new TypePair(pair.Source, Nullable.GetUnderlyingType(pair.Target)!));
            }

            return null!;
        }

        /// <summary>
        /// Checks if the specified type is supported for conversion.
        /// </summary>
        /// <param name="value">The type to check.</param>
        /// <returns>True if the type is supported; otherwise, false.</returns>
        private bool IsSupportedType(Type value)
        {
            return value.IsPrimitive
                   || value == typeof(string)
                   || value == typeof(Guid)
                   || value.IsEnum
                   || value == typeof(decimal)
                   || value.IsNullable() && IsSupportedType(Nullable.GetUnderlyingType(value)!);
        }
    }
}
