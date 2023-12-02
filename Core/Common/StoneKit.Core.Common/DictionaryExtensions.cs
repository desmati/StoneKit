using System.Collections.Concurrent;

namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dic, TKey key, TValue defaultValue)
            where TKey : notnull
        {
            if (dic.TryGetValue(key, out TValue? result))
            {
                return result;
            }

            dic.TryAdd(key, defaultValue);

            return defaultValue;
        }
    }
}
