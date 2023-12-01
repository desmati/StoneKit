namespace System
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Checks if the object is not null.
        /// </summary>
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        /// <summary>
        /// Checks if the object is null.
        /// </summary>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }
    }
}