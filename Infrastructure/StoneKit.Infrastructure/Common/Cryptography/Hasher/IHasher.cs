namespace System.Security.Cryptography
{
    public interface IHasher
    {
        byte[] ConvertBytesToSha512(byte[] input);
        byte[] ConvertBytesToSha256(byte[] input);
        
        byte[] ConvertStringToSha512(string input);
        byte[] ConvertStringToSha256(string input);
    }
}