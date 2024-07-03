using System.Text.Encodings;

namespace System.Security.Cryptography;

/// <summary>
/// Provides functionality for OTP-based authentication.
/// </summary>
public static class OtpAuthenticator
{
    /// <summary>
    /// Generates a TOTP PIN using the provided secret key.
    /// </summary>
    /// <param name="secretKey">The secret key used to generate the PIN.</param>
    /// <param name="remainingSeconds">The remaining seconds before the current TOTP expires.</param>
    /// <returns>A TOTP PIN generated using the Google Authenticator algorithm.</returns>
    public static string GeneratePIN(string secretKey, out int remainingSeconds)
    {
        const int timeStep = 30;
        const int totpSize = 6;

        // Convert the secret key to bytes
        byte[] keyBytes = Base32Encoding.ToBytes(secretKey.Replace(" ", ""));

        // Get the current Unix time and calculate the time counter
        long unixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long counter = unixTime / timeStep;

        // Calculate remaining seconds before the current TOTP expires
        remainingSeconds = (int)(timeStep - (unixTime % timeStep));

        // Compute the TOTP code
        string totp = keyBytes.ComputeTimeBasedOTP(counter, totpSize);

        return totp;
    }

    /// <summary>
    /// Generates a new random secret key.
    /// </summary>
    /// <returns>A new secret key in Base32 format.</returns>
    public static string GenerateKey()
    {
        byte[] data = new byte[16]; // 128-bit key
        using (var cryptoServiceProvider = new RNGCryptoServiceProvider())
        {
            cryptoServiceProvider.GetBytes(data); // Generate random bytes
        }
        return Base32Encoding.ToString(data); // Convert to Base32 and return
    }

    /// <summary>
    /// Generates a provisioning URI for use with Google Authenticator.
    /// </summary>
    /// <param name="username">The username for the account.</param>
    /// <param name="secretKey">The secret key for the account.</param>
    /// <param name="keyIssuer">The issuer of the key.</param>
    /// <returns>A provisioning URI.</returns>
    public static string GenerateProvisioningUri(string username, string secretKey, string keyIssuer)
    {
        return $"otpauth://totp/{username}?secret={secretKey}&issuer={keyIssuer}";
    }

    /// <summary>
    /// Verifies a TOTP code using the provided secret key.
    /// </summary>
    /// <param name="secretKey">The secret key used to generate the TOTP.</param>
    /// <param name="code">The TOTP code to verify.</param>
    /// <returns>True if the code is valid, otherwise false.</returns>
    public static bool VerifyCode(string secretKey, string code)
    {
        const int timeStep = 30;
        const int totpSize = 6;

        // Convert the secret key to bytes
        byte[] keyBytes = Base32Encoding.ToBytes(secretKey);

        // Get the current Unix time and calculate the time counter
        long unixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long counter = unixTime / timeStep;

        // Check the current counter and the counter +/- one time step
        for (long i = -1; i <= 1; i++)
        {
            string totp = keyBytes.ComputeTimeBasedOTP(counter + i, totpSize);
            if (totp == code)
            {
                return true;
            }
        }

        return false;
    }



}
