namespace System.Reflection.Mapping
{
    /// <summary>
    /// Represents the configuration interface for the Transverse mapper.
    /// </summary>
    public interface ITransverseConfig
    {
        /// <summary>
        /// Sets a custom name matching function used for auto bindings.
        /// </summary>
        /// <param name="nameMatching">The function to match names.</param>
        void NameMatching(Func<string, string, bool> nameMatching);

        /// <summary>
        /// Resets settings to default.
        /// </summary>
        void Reset();
    }
}
