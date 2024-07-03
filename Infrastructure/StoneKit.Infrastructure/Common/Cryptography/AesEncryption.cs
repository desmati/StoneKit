namespace System.Security.Cryptography;

using System;
using System.IO;
using System.Text;

/// <summary>
/// Provides methods for AES encryption and decryption.
/// Usage: 
/// const string key = "your-encryption-key";
/// var aesEncryption = new AesEncryption(key);
/// var decryptedText = aesEncryption.Decrypt(aesEncryption.Encrypt(originalText));
/// </summary>
public class AesEncryption
{
    private const string DEFAULT_KEY = "CABD2A4989CC439CB24BE23F6920623F689CC439CB24BE559920623F6CABD2A4989CC439CB24BE55992062CABD2A4989CC439CB24BE559920623F689CC439CB24BE559920623F6CABD2A4989CC439CB24BE559920623F689CC439CB24BE559920623F6920623F689CC439CB24BE559920623F6CABD2A4989CC439CB24BE55992062559920623F689CC439CB24BE559920623F6CABD2A4989CC439CB24BE559920623F689CC439CB24BE5599206AAC8bQZaT5NwkqCCu8s1EIjsjJkdK3XWaKcm1TGkOYCI8ANOFRtONtfbRDeFv8uZntXiWBEFnpfsKgaoupCyBzebqzdpBkU6G6Ouqu3rjfELqShdvrX1EcHOSOBlNZJu4tz2HhDs03VWTrqzTp5EeNtX2f83Rtn5022VlXW8oQiUaU8O0UX8dcfSTo44CZ2Ua2G8MV6i89nxAIC4Q8oTt0SOTW0CRCyOWknEJhe9pnVwSUZc44FYAEIMswCbEM4ykyqhwiJ9C02gu9TiPNzpRwOXfBWubOPZh0IDTxJVkbZtzjSmMhh56zbfKmjY1rJpWo7El6nqtq7niJDkqlKWpZHLitE9fnMfsJPfsvlkRc8joPSjTLV44Aap8rMmZcE1yR7gzb0Wv0bmhibS2YUsCsSbBnm70aKdfL9U8OkAhDnGFYK3BNWHTNLgfP0vkPxhEen397PQE9kr1oSKKa3e86OHvUzhWEDvmfZHJuENR4lUHCZdAokjteHKt1w29FwytdBPLEPforeB3O32A8GEVdcaT3AV0Q2kaPLfrk0YGaXCGhNJ4nSY4rG3Ug1WUGqRLQ8KX6aXbLp3qUnISYK5OfNWf699DULFKoKZQ1YTwIfjsNCkW1drrM8P4D5GMoiy8VgaY21gbLvRSrrZgCT3DLzmMMmEEo4EnncHCZRaNwYmo3whIMaxqdqSOJy5yeg2k2QCvE5Eqrdd2iZpRC43BOMNnvlvRhzY04XAhPHoc6Nehzv5nRO4gQNv5b8CAQtKGyZBB1aSu1xJNBccznqdDd4FBTvEe94p6x5IhwUusUqdkqBT4bOgO0v3Zk0S3T0RXnSdraao9CSqwiABJyTGZp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";

    private readonly byte[] Key;
    private readonly byte[] IV;

    /// <summary>
    /// Initializes a new instance of the <see cref="AesEncryption"/> class.
    /// </summary>
    /// <param name="key">The encryption key. If not provided, a default key will be used.</param>
    public AesEncryption(string key = DEFAULT_KEY)
    {
        using var sha256 = SHA256.Create();
        Key = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
        IV = Key[..16];
    }

    /// <summary>
    /// Encrypts the specified plain text using AES encryption and returns a Base64 string.
    /// </summary>
    /// <param name="plainText">The plain text to encrypt.</param>
    /// <returns>The encrypted text, encoded as a Base64 string.</returns>
    public string EncryptToString(string plainText)
    {
        var encryptedBytes = EncryptToBytes(plainText);
        return Convert.ToBase64String(encryptedBytes);
    }

    /// <summary>
    /// Encrypts the specified plain text using AES encryption and returns the encrypted bytes.
    /// </summary>
    /// <param name="plainText">The plain text to encrypt.</param>
    /// <returns>The encrypted text as a byte array.</returns>
    public byte[] EncryptToBytes(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        using (var swEncrypt = new StreamWriter(csEncrypt))
        {
            swEncrypt.Write(plainText);
        }

        return msEncrypt.ToArray();
    }

    /// <summary>
    /// Encrypts the specified byte array using AES encryption and returns a Base64 string.
    /// </summary>
    /// <param name="plainBytes">The byte array to encrypt.</param>
    /// <returns>The encrypted text, encoded as a Base64 string.</returns>
    public string EncryptToString(byte[] plainBytes)
    {
        var encryptedBytes = EncryptToBytes(plainBytes);
        return Convert.ToBase64String(encryptedBytes);
    }

    /// <summary>
    /// Encrypts the specified byte array using AES encryption and returns the encrypted bytes.
    /// </summary>
    /// <param name="plainBytes">The byte array to encrypt.</param>
    /// <returns>The encrypted text as a byte array.</returns>
    public byte[] EncryptToBytes(byte[] plainBytes)
    {
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
    /// Decrypts the specified encrypted Base64 string using AES decryption and returns the plain text.
    /// </summary>
    /// <param name="cipherText">The encrypted text, encoded as a Base64 string.</param>
    /// <returns>The decrypted plain text.</returns>
    public string DecryptToString(string cipherText)
    {
        var decryptedBytes = DecryptToBytes(cipherText);
        return Encoding.UTF8.GetString(decryptedBytes);
    }

    /// <summary>
    /// Decrypts the specified encrypted Base64 string using AES decryption and returns the decrypted bytes.
    /// </summary>
    /// <param name="cipherText">The encrypted text, encoded as a Base64 string.</param>
    /// <returns>The decrypted text as a byte array.</returns>
    public byte[] DecryptToBytes(string cipherText)
    {
        var cipherBytes = Convert.FromBase64String(cipherText);
        return DecryptToBytes(cipherBytes);
    }

    /// <summary>
    /// Decrypts the specified encrypted byte array using AES decryption and returns the plain text.
    /// </summary>
    /// <param name="cipherBytes">The encrypted byte array.</param>
    /// <returns>The decrypted plain text.</returns>
    public string DecryptToString(byte[] cipherBytes)
    {
        var decryptedBytes = DecryptToBytes(cipherBytes);
        return Encoding.UTF8.GetString(decryptedBytes);
    }

    /// <summary>
    /// Decrypts the specified encrypted byte array using AES decryption and returns the decrypted bytes.
    /// </summary>
    /// <param name="cipherBytes">The encrypted byte array.</param>
    /// <returns>The decrypted text as a byte array.</returns>
    public byte[] DecryptToBytes(byte[] cipherBytes)
    {
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
