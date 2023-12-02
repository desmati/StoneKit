namespace StoneKit.TransverseMapper.Mappers.Types.Custom
{
    /// <summary>
    /// Mapper for custom type conversions using a provided converter function.
    /// </summary>
    internal sealed class CustomTypeMapper : MapperBase
    {
        private readonly Func<object, object> _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTypeMapper"/> class.
        /// </summary>
        /// <param name="converter">The converter function to be used for type conversion.</param>
        public CustomTypeMapper(Func<object, object> converter)
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
            return _converter(source);
        }
    }
}
