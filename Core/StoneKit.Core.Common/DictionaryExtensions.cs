namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static TValue? GetValue<TKey, TValue>(this IDictionary<TKey, TValue> value, TKey key)
        {
            if (value.TryGetValue(key, out TValue? result))
            {
                return result;
            };

            return default;
        }
    }
}
