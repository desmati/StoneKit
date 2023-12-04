using System.Collections;
using System;
using System.Reflection;

namespace StoneKit.TransverseMapper.Mappers.Collections
{
    /// <summary>
    /// Abstract base class for mapping collections from source to target.
    /// </summary>
    /// <typeparam name="TSource">The type of the source collection.</typeparam>
    /// <typeparam name="TTarget">The type of the target collection.</typeparam>
    internal abstract class CollectionMapper<TSource, TTarget> : MapperOf<TSource, TTarget>
        where TTarget : class
    {
        /// <summary>
        /// Converts an individual item during mapping.
        /// </summary>
        /// <param name="item">The item to be converted.</param>
        /// <returns>The converted item.</returns>
        protected virtual object ConvertItem(object item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts an individual item key during mapping.
        /// </summary>
        /// <param name="item">The item key to be converted.</param>
        /// <returns>The converted item key.</returns>
        protected virtual object ConvertItemKey(object item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts a source dictionary to a target dictionary during mapping.
        /// </summary>
        /// <param name="source">The source dictionary.</param>
        /// <returns>The target dictionary.</returns>
        protected virtual TTarget DictionaryToDictionary(IEnumerable source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts a source dictionary to a target dictionary using templates for key and value types during mapping.
        /// </summary>
        protected Dictionary<TTargetKey, TTargetValue> DictionaryToDictionaryTemplate<TSourceKey, TSourceValue, TTargetKey, TTargetValue>(IEnumerable source)
        {
            var result = new Dictionary<TTargetKey, TTargetValue>();
            foreach (KeyValuePair<TSourceKey, TSourceValue> item in source)
            {
                var key = (TTargetKey)ConvertItemKey(item.Key);
                var value = (TTargetValue)ConvertItem(item.Value);
                result.Add(key, value);
            }
            return result;
        }

        protected virtual TTarget EnumerableToArray(IEnumerable source)
        {
            throw new NotImplementedException();
        }

        private int Count(IEnumerable source)
        {
            var collection = source as ICollection;
            if (collection != null)
            {
                return collection.Count;
            }

            var count = 0;
            foreach (object item in source)
            {
                count++;
            }
            return count;
        }

        protected Array EnumerableToArrayTemplate<TTargetItem>(IEnumerable source)
        {
            var result = new TTargetItem[Count(source)];

            int index = 0;
            foreach (var item in source)
            {
                result[index++] = (TTargetItem)ConvertItem(item);
            }

            return result;
        }

        protected virtual TTarget EnumerableToList(IEnumerable source)
        {
            throw new NotImplementedException();
        }

        protected virtual TTarget EnumerableToArrayList(IEnumerable source)
        {
            var result = new ArrayList();

            foreach (var item in source)
            {
                result.Add(ConvertItem(item));
            }

            return result as TTarget;
        }

        protected List<TTargetItem> EnumerableToListTemplate<TTargetItem>(IEnumerable source)
        {
            var result = new List<TTargetItem>();
            foreach (var item in source)
            {
                result.Add((TTargetItem)ConvertItem(item));
            }
            return result;
        }

        protected List<TTargetItem> EnumerableOfDeepCloneableToListTemplate<TTargetItem>(IEnumerable source)
        {
            var result = new List<TTargetItem>();
            result.AddRange((IEnumerable<TTargetItem>)source);
            return result;
        }

        protected virtual TTarget EnumerableToEnumerable(IEnumerable source)
        {
            IList result = null;
            foreach (var item in source)
            {
                if (result == null)
                {
                    result = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(item.GetType()));
                }

                result.Add(ConvertItem(item));
            }
            return result as TTarget;
        }

        /// <summary>
        /// Maps the source collection to the target collection.
        /// </summary>
        /// <param name="source">The source collection to be mapped.</param>
        /// <param name="target">The target collection to be populated.</param>
        /// <returns>The mapped target collection.</returns>
        protected override TTarget MapCore(TSource source, TTarget target)
        {
            Type targetType = typeof(TTarget);
            var enumerable = (IEnumerable)source;

            if (targetType.IsListOf())
            {
                return EnumerableToList(enumerable);
            }
            if (targetType.IsArray)
            {
                return EnumerableToArray(enumerable);
            }
            if (typeof(TSource).IsDictionaryOf() && targetType.IsDictionaryOf())
            {
                return DictionaryToDictionary(enumerable);
            }
            if (targetType == typeof(ArrayList))
            {
                return EnumerableToArrayList(enumerable);
            }
            if (targetType.IsIEnumerable())
            {
                // Default Case
                return EnumerableToEnumerable(enumerable);
            }

            throw new NotSupportedException($"Not supported From {typeof(TSource).Name} To {targetType.Name}");
        }
    }
}
