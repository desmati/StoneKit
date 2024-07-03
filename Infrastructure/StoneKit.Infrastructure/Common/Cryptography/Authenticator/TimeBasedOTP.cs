namespace System.Security.Cryptography;

public static class TimeBasedOneTimePassword
{
    /// <summary>
    /// Computes a TOTP (Time-based One-Time Password) based on the provided key, counter, and number of digits.
    /// </summary>
    /// <param name="keyBytes">The secret key in byte array format.</param>
    /// <param name="counter">The counter value, typically derived from the current time.</param>
    /// <param name="digits">The number of digits for the TOTP.</param>
    /// <returns>A TOTP as a string with the specified number of digits.</returns>
    public static string ComputeTimeBasedOTP(this byte[] keyBytes, long counter, int digits)
    {
        // Convert the counter to a byte array in big-endian order
        byte[] counterBytes = BitConverter.GetBytes(counter);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(counterBytes); // Ensure big-endian order

        // Initialize HMAC-SHA-1 with the secret key
        using (HMACSHA1 hmac = new HMACSHA1(keyBytes))
        {
            // Compute the HMAC hash of the counter
            byte[] hash = hmac.ComputeHash(counterBytes);

            // Get the offset value from the last nibble of the hash
            int offset = hash[hash.Length - 1] & 0xf;

            // Calculate the binary code (truncate the hash)
            int binaryCode =
                ((hash[offset] & 0x7f) << 24) | // Take the first byte and mask the most significant bit
                ((hash[offset + 1] & 0xff) << 16) | // Take the second byte
                ((hash[offset + 2] & 0xff) << 8) | // Take the third byte
                (hash[offset + 3] & 0xff); // Take the fourth byte

            // Compute the TOTP value by taking binaryCode modulo 10^digits
            int totp = binaryCode % (int)Math.Pow(10, digits);

            // Return the TOTP as a zero-padded string with the specified number of digits
            return totp.ToString(new string('0', digits));
        }
    }

}