using System.Linq.Expressions;

namespace System.Reflection.Mapping
{
    /// <summary>
    /// Represents the configuration interface for binding between source and target types.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public interface IBindingConfig<TSource, TTarget>
    {
        /// <summary>
        /// Binds the specified source expression to the target expression.
        /// </summary>
        /// <param name="source">The source expression.</param>
        /// <param name="target">The target expression.</param>
        void Bind(Expression<Func<TSource, object>> source, Expression<Func<TTarget, object>> target);

        /// <summary>
        /// Binds the target expression to the specified target type.
        /// </summary>
        /// <param name="target">The target expression.</param>
        /// <param name="targetType">The target type.</param>
        void Bind(Expression<Func<TTarget, object>> target, Type targetType);

        /// <summary>
        /// Ignores the specified source expression during mapping.
        /// </summary>
        /// <param name="expression">The source expression to ignore.</param>
        void Ignore(Expression<Func<TSource, object>> expression);
    }
}
