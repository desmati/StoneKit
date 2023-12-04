namespace StoneKit.TransverseMapper.Mappers.Types.Convertible
{
    /// <summary>
    /// Mapper for converting types using a provided converter function.
    /// </summary>
    internal sealed class ConvertibleTypeMapper : Mapper
    {
        private readonly Func<object, object> _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertibleTypeMapper"/> class.
        /// </summary>
        /// <param name="converter">The converter function.</param>
        public ConvertibleTypeMapper(Func<object, object> converter)
        {
            _converter = converter;
        }

        /// <summary>
        /// Maps the source object to the target object using the provided converter.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        /// <returns>The result of the conversion.</returns>
        protected override object MapCore(object source, object target)
        {
            if (_converter == null)
            {
                return source;
            }
            if (source == null)
            {
                return target;
            }
            return _converter(source);
        }
    }
}
