namespace System.Security.Cryptography
{
    /// <summary>
    /// Provides methods for converting and hashing data.
    /// </summary>
    public interface IHashProvider : IDisposable
    {
        /// <summary>
        /// Converts a string to a byte array.
        /// </summary>
        /// <param name="input">The input string to be converted.</param>
        /// <returns>A byte array representing the input string.</returns>
        byte[] ConvertToBytes(string input);

        /// <summary>
        /// Converts a byte array to a string.
        /// </summary>
        /// <param name="input">The input byte array to be converted.</param>
        /// <returns>A string representing the input byte array.</returns>
        string ConvertToString(byte[] input);

        /// <summary>
        /// Computes the hash of the specified byte array.
        /// </summary>
        /// <param name="input">The input byte array to be hashed.</param>
        /// <returns>A byte array representing the hashed value.</returns>
        byte[] ComputeHash(byte[] input);

        /// <summary>
        /// Computes the hash of the specified string.
        /// </summary>
        /// <param name="input">The input string to be hashed.</param>
        /// <returns>A byte array representing the hashed value.</returns>
        byte[] ComputeHash(string input);

        /// <summary>
        /// Computes the hash of the specified string and returns the result as a string.
        /// </summary>
        /// <param name="input">The input string to be hashed.</param>
        /// <returns>A string representing the hashed value.</returns>
        string ComputeHashString(string input);

        /// <summary>
        /// Computes the hash of the specified byte array and returns the result as a string.
        /// </summary>
        /// <param name="input">The input byte array to be hashed.</param>
        /// <returns>A string representing the hashed value.</returns>
        string ComputeHashString(byte[] input);
    }
}
