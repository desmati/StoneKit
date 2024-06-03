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

        /// <summary>
        /// Attempts to add all key-value pairs from the specified itemsToAdd dictionary to the specified dictionary.
        /// If the dictionary is null, a new dictionary is created.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to which items should be added. If null, a new dictionary is created.</param>
        /// <param name="itemsToAdd">The dictionary containing items to add. If null, no items are added.</param>
        public static void TryAdd<TKey, TValue>(this Dictionary<TKey, TValue>? dictionary, Dictionary<TKey, TValue>? itemsToAdd) where TKey:notnull
        {
            if(itemsToAdd == null)
            {
                return;
            }

            if(dictionary == null)
            {
                dictionary = new Dictionary<TKey, TValue>();
            }

            foreach (var item in itemsToAdd)
            {
                dictionary.Add(item.Key, item.Value);
            }
        }
    }
}
