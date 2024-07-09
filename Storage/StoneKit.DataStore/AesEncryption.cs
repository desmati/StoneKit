namespace StoneKit.DataStore;

using System.IO;
using System.Security.Cryptography;
using System.Text;

internal static class AesEncryption
{
    private static bool enabled = false;
    private static byte[] Key = [];
    private static byte[] IV = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="AesEncryption"/> class.
    /// </summary>
    /// <param name="key">The encryption key. If not provided, a default key will be used.</param>
    public static void Init(string? encryptionKey)
    {
        if (string.IsNullOrEmpty(encryptionKey))
        {
            return;
        }

        enabled = true;

        using var sha256 = SHA256.Create();
        Key = sha256.ComputeHash(Encoding.UTF8.GetBytes(encryptionKey));
        IV = Key[..16];
    }

    /// <summary>
    /// Encrypts the specified byte array using AES encryption and returns the encrypted bytes.
    /// </summary>
    /// <param name="plainBytes">The byte array to encrypt.</param>
    /// <returns>The encrypted text as a byte array.</returns>
    public static byte[] Encrypt(this byte[] plainBytes)
    {
        if (!enabled)
        {
            return plainBytes;
        }

        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        csEncrypt.Write(plainBytes, 0, plainBytes.Length);
        csEncrypt.FlushFinalBlock();

        return msEncrypt.ToArray();
    }

    /// <summary>
    /// Decrypts the specified encrypted byte array using AES decryption and returns the decrypted bytes.
    /// </summary>
    /// <param name="cipherBytes">The encrypted byte array.</param>
    /// <returns>The decrypted text as a byte array.</returns>
    public static byte[] Decrypt(this byte[] cipherBytes)
    {
        if (!enabled)
        {
            return cipherBytes;
        }

        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var msDecrypt = new MemoryStream(cipherBytes);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var msPlain = new MemoryStream();
        csDecrypt.CopyTo(msPlain);

        return msPlain.ToArray();
    }
}
