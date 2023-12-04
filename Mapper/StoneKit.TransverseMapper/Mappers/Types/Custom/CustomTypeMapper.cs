namespace StoneKit.TransverseMapper.Mappers.Types.Custom
{
    /// <summary>
    /// Mapper for custom type conversion using a provided converter function.
    /// </summary>
    internal sealed class CustomTypeMapper : Mapper
    {
        private readonly Func<object, object> _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTypeMapper"/> class.
        /// </summary>
        /// <param name="converter">The custom converter function.</param>
        public CustomTypeMapper(Func<object, object> converter)
        {
            _converter = converter;
        }

        /// <summary>
        /// Performs the core mapping logic for the custom type mapper.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        /// <returns>The result of the mapping.</returns>
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
