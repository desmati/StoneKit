namespace System.Text
{
    public static class StringExtensions
    {

        public static string NormalizePathSeparators(this string path)
        {
            return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }

    }
}
