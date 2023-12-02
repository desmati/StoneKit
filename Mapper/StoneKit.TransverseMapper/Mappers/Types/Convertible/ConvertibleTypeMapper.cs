namespace StoneKit.TransverseMapper.Mappers.Types.Convertible
{
    /// <summary>
    /// Mapper for converting types using a provided converter function.
    /// </summary>
    internal sealed class ConvertibleTypeMapper : MapperBase
    {
        private readonly Func<object, object> _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertibleTypeMapper"/> class.
        /// </summary>
        /// <param name="converter">The converter function to be used for type conversion.</param>
        public ConvertibleTypeMapper(Func<object, object> converter)
        {
            _converter = converter;
        }

        /// <inheritdoc/>
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
