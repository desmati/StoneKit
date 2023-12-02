using System.Collections;
using System.Reflection;

namespace StoneKit.TransverseMapper.Mappers.Collections
{
    /// <summary>
    /// Base class for mapping collections from one type to another.
    /// </summary>
    /// <typeparam name="TSource">The source collection type.</typeparam>
    /// <typeparam name="TTarget">The target collection type.</typeparam>
    internal abstract class CollectionMapper<TSource, TTarget> : MapperOf<TSource, TTarget>
        where TTarget : class
    {
        /// <summary>
        /// Converts an individual item within the collection.
        /// </summary>
        /// <param name="item">The item to convert.</param>
        /// <returns>The converted item.</returns>
        protected virtual object ConvertItem(object item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts an individual item key within the collection.
        /// </summary>
        /// <param name="item">The item key to convert.</param>
        /// <returns>The converted item key.</returns>
        protected virtual object ConvertItemKey(object item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts a source dictionary to a target dictionary.
        /// </summary>
        /// <param name="source">The source dictionary.</param>
        /// <returns>The converted target dictionary.</returns>
        protected virtual TTarget DictionaryToDictionary(IEnumerable source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts a source dictionary to a target dictionary with template types.
        /// </summary>
        protected Dictionary<TTargetKey, TTargetValue> DictionaryToDictionaryTemplate<TSourceKey, TSourceValue, TTargetKey, TTargetValue>(IEnumerable source)
            where TTargetKey : notnull
        {
            var result = new Dictionary<TTargetKey, TTargetValue>();
            foreach (KeyValuePair<TSourceKey, TSourceValue> item in source)
            {
                var key = (TTargetKey)ConvertItemKey(item.Key!);
                var value = (TTargetValue)ConvertItem(item.Value!);
                result.Add(key, value);
            }
            return result;
        }

        /// <summary>
        /// Converts a source enumerable to a target array.
        /// </summary>
        protected virtual TTarget EnumerableToArray(IEnumerable source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts a source enumerable to a target list.
        /// </summary>
        protected virtual TTarget EnumerableToList(IEnumerable source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts a source enumerable to a target ArrayList.
        /// </summary>
        protected virtual TTarget EnumerableToArrayList(IEnumerable source)
        {
            var result = new ArrayList();

            foreach (var item in source)
            {
                result.Add(ConvertItem(item));
            }

            return (result as TTarget)!;
        }

        /// <summary>
        /// Converts a source enumerable to a target list with template types.
        /// </summary>
        protected List<TTargetItem> EnumerableToListTemplate<TTargetItem>(IEnumerable source)
        {
            var result = new List<TTargetItem>();
            foreach (var item in source)
            {
                result.Add((TTargetItem)ConvertItem(item));
            }
            return result;
        }

        /// <summary>
        /// Converts a source enumerable to a target list of deep cloneable items with template types.
        /// </summary>
        protected List<TTargetItem> EnumerableOfDeepCloneableToListTemplate<TTargetItem>(IEnumerable source)
        {
            var result = new List<TTargetItem>();
            result.AddRange((IEnumerable<TTargetItem>)source);
            return result;
        }

        /// <summary>
        /// Converts a source enumerable to a target enumerable.
        /// </summary>
        protected virtual TTarget EnumerableToEnumerable(IEnumerable source)
        {
            IList result = null!;
            foreach (var item in source)
            {
                if (result == null)
                {
                    result = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(item.GetType()))!;
                }

                result.Add(ConvertItem(item));
            }
            return (result as TTarget)!;
        }

        /// <summary>
        /// Maps the source collection to the target collection.
        /// </summary>
        /// <param name="source">The source collection.</param>
        /// <param name="target">The target collection.</param>
        /// <returns>The mapped target collection.</returns>
        protected override TTarget MapCore(TSource source, TTarget target)
        {
            Type targetType = typeof(TTarget);
            var enumerable = (IEnumerable)source!;

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
