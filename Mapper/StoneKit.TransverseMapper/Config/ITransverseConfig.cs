namespace System.Reflection.Mapping
{
    /// <summary>
    ///     Configuration for Transverse
    /// </summary>
    public interface ITransverseConfig
    {
        /// <summary>
        ///     Custom name matching function used for auto bindings
        /// </summary>
        /// <param name="nameMatching">Function to match names</param>
        void NameMatching(Func<string, string, bool> nameMatching);

        /// <summary>
        ///     Reset settings to default
        /// </summary>
        void Reset();
    }
}
