using System.Linq.Expressions;
using System.Reflection.Mapping;

namespace StoneKit.TransverseMapper.Config.Bindings
{
    /// <summary>
    /// Represents a configuration for binding between source and target types with specific source and target types.
    /// </summary>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TTarget">The target type.</typeparam>
    internal sealed class BindingConfigOf<TSource, TTarget> : BindingConfig, IBindingConfig<TSource, TTarget>
    {
        /// <summary>
        /// Binds the specified source expression to the corresponding target expression.
        /// </summary>
        /// <param name="source">The source expression.</param>
        /// <param name="target">The target expression.</param>
        public void Bind(Expression<Func<TSource, object>> source, Expression<Func<TTarget, object>> target)
        {
            List<string> sourcePath = GetMemberInfoPath(source);
            List<string> targetPath = GetMemberInfoPath(target);

            if (sourcePath.Count == 1 && targetPath.Count == 1 &&
                string.Equals(sourcePath[0], targetPath[0], StringComparison.Ordinal))
            {
                return;
            }

            BindFields(sourcePath, targetPath);
        }

        // TODO: Uncomment the method below once it is implemented and working
        // /// <summary>
        // /// Binds the specified target expression to the specified value.
        // /// </summary>
        // /// <typeparam name="TField">The type of the target field.</typeparam>
        // /// <param name="target">The target expression.</param>
        // /// <param name="value">The value to bind.</param>
        // public void Bind<TField>(Expression<Func<TTarget, TField>> target, TField value)
        // {
        //     Func<object, object> func = x => value;
        //     BindConverter(GetMemberInfo(target), func);
        // }

        /// <summary>
        /// Binds the specified target expression to the specified target type.
        /// </summary>
        /// <param name="target">The target expression.</param>
        /// <param name="targetType">The target type.</param>
        public void Bind(Expression<Func<TTarget, object>> target, Type targetType)
        {
            string targetName = GetMemberInfo(target);
            BindType(targetName, targetType);
        }

        /// <summary>
        /// Ignores the specified source expression during the binding process.
        /// </summary>
        /// <param name="expression">The source expression to ignore.</param>
        public void Ignore(Expression<Func<TSource, object>> expression)
        {
            string memberName = GetMemberInfo(expression);
            IgnoreSourceField(memberName);
        }

        private static string GetMemberInfo<T, TField>(Expression<Func<T, TField>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member == null)
            {
                var unaryExpression = expression.Body as UnaryExpression;
                if (unaryExpression != null)
                {
                    member = unaryExpression.Operand as MemberExpression;
                }

                if (member == null)
                {
                    throw new ArgumentException("Expression is not a MemberExpression", nameof(expression));
                }
            }
            return member.Member.Name;
        }

        private static List<string> GetMemberInfoPath<T, TField>(Expression<Func<T, TField>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member == null)
            {
                var unaryExpression = expression.Body as UnaryExpression;
                if (unaryExpression != null)
                {
                    member = unaryExpression.Operand as MemberExpression;
                }

                if (member == null)
                {
                    throw new ArgumentException("Expression is not a MemberExpression", nameof(expression));
                }
            }
            var result = new List<string>();
            do
            {
                var resultMember = member.Member;
                result.Add(resultMember.Name);
                member = member.Expression as MemberExpression;
            }
            while (member != null);
            result.Reverse();
            return result;
        }
    }
}
