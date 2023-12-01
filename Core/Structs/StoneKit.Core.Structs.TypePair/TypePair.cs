using System.ComponentModel;
using System.Reflection;

namespace System
{
    /// <summary>
    /// Represents a pair of types. For example, used for object mapping and type conversion.
    /// </summary>
    public struct TypePair : IEquatable<TypePair>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypePair"/> struct.
        /// </summary>
        /// <param name="source">The source type.</param>
        /// <param name="target">The target type.</param>
        public TypePair(Type source, Type target) : this()
        {
            Target = target;
            Source = source;
        }

        /// <summary>
        /// Gets the source type.
        /// </summary>
        public Type Source { get; }

        /// <summary>
        /// Gets the target type.
        /// </summary>
        public Type Target { get; }

        /// <summary>
        /// Gets a value indicating whether the types are equal.
        /// </summary>
        private bool IsEqualTypes => Source == Target;

        /// <summary>
        /// Gets a value indicating whether the types are both enums.
        /// </summary>
        public bool IsEnumTypes => Source.IsEnum && Target.IsEnum;

        /// <summary>
        /// Gets a value indicating whether the types are both enumerable.
        /// </summary>
        public bool IsEnumerableTypes => Source.IsIEnumerable() && Target.IsIEnumerable();

        /// <summary>
        /// Gets a value indicating whether the source type is nullable while the target type is not.
        /// </summary>
        public bool IsNullableToNotNullable => Source.IsNullable() && Target.IsNullable() == false;

        /// <summary>
        /// Gets a value indicating whether the types are both value types.
        /// </summary>
        private bool IsValueTypes => Source.IsValueType && Target.IsValueType;

        /// <summary>
        /// Gets a value indicating whether the types are both primitive types.
        /// </summary>
        private bool IsPrimitiveTypes => Source.IsPrimitive && Target.IsPrimitive;

        private bool IsNullableTypes => Source.IsNullable() && Target.IsNullable();

        /// <summary>
        /// Gets a value indicating whether the types are deep cloneable.
        /// </summary>
        public bool IsDeepCloneable
        {
            get
            {
                if (IsEqualTypes == false)
                {
                    return false;
                }
                else if (IsValueTypes && IsPrimitiveTypes)
                {
                    return true;
                }
                else if (Source == typeof(string) || Source == typeof(decimal) ||
                         Source == typeof(DateTime) || Source == typeof(DateTimeOffset) ||
                         Source == typeof(TimeSpan) || Source == typeof(Guid))
                {
                    return true;
                }
                else if (IsNullableTypes)
                {
                    var nullablePair = new TypePair(Nullable.GetUnderlyingType(Source)!, Nullable.GetUnderlyingType(Target)!);
                    return nullablePair.IsDeepCloneable;
                }
                return false;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="TypePair"/> struct.
        /// </summary>
        /// <param name="source">The source type.</param>
        /// <param name="target">The target type.</param>
        /// <returns>A new instance of the <see cref="TypePair"/> struct.</returns>
        public static TypePair Create(Type source, Type target)
        {
            return new TypePair(source, target);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="TypePair"/> struct using generic types.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <returns>A new instance of the <see cref="TypePair"/> struct.</returns>
        public static TypePair Create<TSource, TTarget>()
        {
            return new TypePair(typeof(TSource), typeof(TTarget));
        }

        /// <summary>
        /// Checks whether the <see cref="TypePair"/> has a <see cref="TypeConverter"/> for type conversion.
        /// </summary>
        /// <returns>True if a <see cref="TypeConverter"/> is available, otherwise false.</returns>
        public bool HasTypeConverter()
        {
            TypeConverter fromConverter = TypeDescriptor.GetConverter(Source);
            if (fromConverter.CanConvertTo(Target))
                return true;

            TypeConverter toConverter = TypeDescriptor.GetConverter(Target);
            return toConverter.CanConvertFrom(Source);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is TypePair && Equals((TypePair)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Source != null ? Source.GetHashCode() : 0) * 397) ^ (Target != null ? Target.GetHashCode() : 0);
            }
        }

        /// <inheritdoc />
        public bool Equals(TypePair other)
        {
            return Source == other.Source && Target == other.Target;
        }
    }
}
