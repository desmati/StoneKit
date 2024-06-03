using System.Text;

namespace System.Security.Cryptography
{
    public sealed class Hasher : IHasher
    {
        public byte[] ConvertBytesToSha512(byte[] input)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                return sha512.ComputeHash(input);
            }
        }

        public byte[] ConvertBytesToSha256(byte[] input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(input);
            }
        }

        public byte[] ConvertStringToSha512(string input)
        {
            return ConvertBytesToSha512(Encoding.UTF8.GetBytes(input));
        }

        public byte[] ConvertStringToSha256(string input)
        {
            return ConvertBytesToSha256(Encoding.UTF8.GetBytes(input));
        }
    }
}