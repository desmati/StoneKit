using System.Linq.Expressions;

namespace System.Reflection.Mapping
{
    /// <summary>
    /// Represents the configuration interface for object binding between source and target types.
    /// </summary>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TTarget">The target type.</typeparam>
    public interface IBindingConfig<TSource, TTarget>
    {
        /// <summary>
        /// Binds the specified source expression to the corresponding target expression.
        /// </summary>
        /// <param name="source">The source expression.</param>
        /// <param name="target">The target expression.</param>
        void Bind(Expression<Func<TSource, object>> source, Expression<Func<TTarget, object>> target);

        /// <summary>
        /// Binds the specified target expression to the specified target type.
        /// </summary>
        /// <param name="target">The target expression.</param>
        /// <param name="targetType">The target type.</param>
        void Bind(Expression<Func<TTarget, object>> target, Type targetType);

        /// <summary>
        /// Ignores the specified source expression during the binding process.
        /// </summary>
        /// <param name="expression">The source expression to ignore.</param>
        void Ignore(Expression<Func<TSource, object>> expression);

        // TODO: Uncomment the method below once it is implemented and working
        // /// <summary>
        // /// Binds the specified target expression to the specified value.
        // /// </summary>
        // /// <typeparam name="TField">The type of the target field.</typeparam>
        // /// <param name="target">The target expression.</param>
        // /// <param name="value">The value to bind.</param>
        // void Bind<TField>(Expression<Func<TTarget, TField>> target, TField value);
    }
}
