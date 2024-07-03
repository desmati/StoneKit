namespace System.Text.Encodings;

/// <summary>
/// Provides functionality for Base32 encoding and decoding.
/// </summary>
public static class Base32Encoding
{
    // Characters used in Base32 encoding
    private static readonly char[] Digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();

    // Map for decoding Base32 characters
    private static readonly int[] Map = new int[128];

    static Base32Encoding()
    {
        // Initialize the map with default values
        for (int i = 0; i < Map.Length; i++)
            Map[i] = -1;

        // Populate the map with indices of the characters in the Digits array
        for (int i = 0; i < Digits.Length; i++)
            Map[Digits[i]] = i;
    }

    /// <summary>
    /// Converts a Base32 encoded string to a byte array.
    /// </summary>
    /// <param name="base32">The Base32 encoded string.</param>
    /// <returns>A byte array.</returns>
    public static byte[] ToBytes(string base32)
    {
        // Remove any padding characters
        base32 = base32.TrimEnd('=');

        // Calculate the number of bytes
        int byteCount = base32.Length * 5 / 8;
        byte[] bytes = new byte[byteCount];

        byte curByte = 0, bitsRemaining = 8;
        int mask = 0, arrayIndex = 0;

        foreach (char c in base32)
        {
            // Get the value of the current character
            int cValue = Map[c];

            if (bitsRemaining > 5)
            {
                // If there are more than 5 bits remaining, shift the value and add it to the current byte
                mask = cValue << (bitsRemaining - 5);
                curByte = (byte)(curByte | mask);
                bitsRemaining -= 5;
            }
            else
            {
                // If there are 5 or fewer bits remaining, add part of the value to the current byte and start a new byte
                mask = cValue >> (5 - bitsRemaining);
                curByte = (byte)(curByte | mask);
                bytes[arrayIndex++] = curByte;
                curByte = (byte)(cValue << (3 + bitsRemaining));
                bitsRemaining += 3;
            }
        }

        // Add the last byte if necessary
        if (arrayIndex != byteCount)
        {
            bytes[arrayIndex] = curByte;
        }

        return bytes;
    }

    /// <summary>
    /// Converts a byte array to a Base32 encoded string.
    /// </summary>
    /// <param name="data">The byte array.</param>
    /// <returns>A Base32 encoded string.</returns>
    public static string ToString(byte[] data)
    {
        // Return an empty string if the input is null or empty
        if (data == null || data.Length == 0)
            return string.Empty;

        // Calculate the number of characters in the output string
        int charCount = (int)Math.Ceiling(data.Length / 5d) * 8;
        char[] result = new char[charCount];

        byte nextChar = 0, bitsRemaining = 5;
        int arrayIndex = 0;

        foreach (byte b in data)
        {
            // Add the most significant bits of the current byte to the next character
            nextChar = (byte)(nextChar | (b >> (8 - bitsRemaining)));
            result[arrayIndex++] = Digits[nextChar];

            if (bitsRemaining < 4)
            {
                // If there are fewer than 4 bits remaining, add the rest of the current byte and start a new character
                nextChar = (byte)((b >> (3 - bitsRemaining)) & 31);
                result[arrayIndex++] = Digits[nextChar];
                bitsRemaining += 5;
            }

            bitsRemaining -= 3;
            nextChar = (byte)((b << bitsRemaining) & 31);
        }

        // Add the last character if necessary
        if (arrayIndex != charCount)
        {
            result[arrayIndex++] = Digits[nextChar];
            while (arrayIndex != charCount)
                result[arrayIndex++] = '=';
        }

        return new string(result);
    }
}
