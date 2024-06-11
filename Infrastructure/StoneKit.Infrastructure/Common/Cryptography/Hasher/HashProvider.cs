using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Provides methods for converting and hashing data using a specified algorithm.
/// </summary>
public class HashProvider : IHashProvider
{
    private readonly Encoding _encoding;
    private readonly HashAlgorithm _hashAlgorithm;
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="HashProvider"/> class with the specified algorithm and encoding.
    /// </summary>
    /// <param name="algorithm">The hashing algorithm to be used. Defaults to SHA512.</param>
    /// <param name="encoding">The text encoding to be used. Defaults to UTF-8.</param>
    public HashProvider(HashAlgorithmTypes? algorithm = null, Encoding? encoding = null)
    {
        _encoding = encoding ?? Encoding.UTF8;
        _hashAlgorithm = CreateHasher(algorithm ?? HashAlgorithmTypes.SHA512);
    }

    /// <summary>
    /// Converts a string to a byte array using the specified encoding.
    /// </summary>
    /// <param name="input">The input string to be converted.</param>
    /// <returns>A byte array representing the input string.</returns>
    public byte[] ConvertToBytes(string input) => _encoding.GetBytes(input);

    /// <summary>
    /// Converts a byte array to a string using the specified encoding.
    /// </summary>
    /// <param name="input">The input byte array to be converted.</param>
    /// <returns>A string representing the input byte array.</returns>
    public string ConvertToString(byte[] input) => _encoding.GetString(input);

    /// <summary>
    /// Computes the hash of the specified byte array.
    /// </summary>
    /// <param name="input">The input byte array to be hashed.</param>
    /// <returns>A byte array representing the hashed value.</returns>
    public byte[] ComputeHash(byte[] input) => _hashAlgorithm.ComputeHash(input);

    /// <summary>
    /// Computes the hash of the specified string.
    /// </summary>
    /// <param name="input">The input string to be hashed.</param>
    /// <returns>A byte array representing the hashed value.</returns>
    public byte[] ComputeHash(string input) => ComputeHash(ConvertToBytes(input));

    /// <summary>
    /// Computes the hash of the specified string and returns the result as a string.
    /// </summary>
    /// <param name="input">The input string to be hashed.</param>
    /// <returns>A string representing the hashed value.</returns>
    public string ComputeHashString(string input) => ConvertToString(ComputeHash(input));

    /// <summary>
    /// Computes the hash of the specified byte array and returns the result as a string.
    /// </summary>
    /// <param name="input">The input byte array to be hashed.</param>
    /// <returns>A string representing the hashed value.</returns>
    public string ComputeHashString(byte[] input) => ConvertToString(ComputeHash(input));

    /// <summary>
    /// Creates a hasher instance based on the specified hashing algorithm.
    /// </summary>
    /// <param name="algorithm">The hashing algorithm to be used.</param>
    /// <returns>An instance of a hash algorithm.</returns>
    private HashAlgorithm CreateHasher(HashAlgorithmTypes algorithm) =>
        algorithm switch
        {
            HashAlgorithmTypes.MD5 => MD5.Create(),
            HashAlgorithmTypes.SHA1 => SHA1.Create(),
            HashAlgorithmTypes.SHA256 => SHA256.Create(),
            HashAlgorithmTypes.SHA384 => SHA384.Create(),
            HashAlgorithmTypes.SHA512 => SHA512.Create(),
            _ => throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null)
        };

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _hashAlgorithm?.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
