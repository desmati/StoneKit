namespace System.Reflection
{
    /// <summary>
    /// Provides utility methods for emitting 'this' type references using IL code generation.
    /// </summary>
    public static class EmitThis
    {
        /// <summary>
        /// Loads the 'this' type as an IEmitterType using IL code generation.
        /// </summary>
        /// <param name="thisType">The Type of the 'this' object to be loaded.</param>
        /// <returns>An IEmitterType representing the loaded 'this' type.</returns>
        public static IEmitterType Load(Type thisType)
        {
            // Delegates to the EmitArgument class to load the 'this' type as an argument
            return EmitArgument.Load(thisType, 0);
        }
    }
}
