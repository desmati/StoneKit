using System.Text;

namespace System.Security.Cryptography
{
    /// <summary>
    /// Provides functionality to generate a PIN using Google Authenticator.
    /// </summary>
    public class GoogleAuthenticator
    {
        /// <summary>
        /// Generates a PIN using the provided secret key.
        /// </summary>
        /// <param name="secretKey">The secret key used to generate the PIN.</param>
        /// <returns>A PIN generated using the Google Authenticator algorithm.</returns>
        public static string GeneratePIN(string secretKey)
        {
            int digits = 6; // Number of digits in the OTP
            long timeStep = 30L; // Time step in seconds

            // Calculate the current interval number (number of time steps since Unix epoch)
            long iterationNumber = GetCurrentUnixTimestamp() / timeStep;

            // Convert the secret key to bytes
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);

            // Convert the iteration number to a byte array
            byte[] counter = BitConverter.GetBytes(iterationNumber);

            // Ensure the counter is in big-endian format
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(counter);
            }

            // Create the HMACSHA1 object with the key
            using (HMACSHA1 hmac = new HMACSHA1(keyBytes))
            {
                // Compute the HMAC hash
                byte[] hash = hmac.ComputeHash(counter);

                // Get the offset from the last nibble of the hash
                int offset = hash[hash.Length - 1] & 0x0f;

                // Extract a 4-byte (32-bit) segment from the hash starting at the offset
                int binary =
                    ((hash[offset] & 0x7f) << 24) |
                    (hash[offset + 1] << 16) |
                    (hash[offset + 2] << 8) |
                    hash[offset + 3];

                // Compute the OTP value (truncate to the number of digits)
                int otp = binary % (int)Math.Pow(10, digits);

                // Format the OTP as a zero-padded string
                return otp.ToString(new string('0', digits));
            }
        }

        /// <summary>
        /// Gets the current Unix timestamp (seconds since Unix epoch).
        /// </summary>
        /// <returns>The current Unix timestamp.</returns>
        private static long GetCurrentUnixTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}
