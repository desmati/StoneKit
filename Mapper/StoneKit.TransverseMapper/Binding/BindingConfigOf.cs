using System.Linq.Expressions;
using System.Reflection.Mapping;

namespace StoneKit.TransverseMapper.Binding
{
    /// <summary>
    /// Configuration class for specifying custom bindings between source and target types.
    /// </summary>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TTarget">The target type.</typeparam>
    internal sealed class BindingConfigOf<TSource, TTarget> : BindingConfig, IBindingConfig<TSource, TTarget>
    {
        /// <summary>
        /// Binds source and target expressions based on their member paths.
        /// </summary>
        /// <param name="source">The expression representing the source member.</param>
        /// <param name="target">The expression representing the target member.</param>
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

        /// <summary>
        /// Binds a target expression to a specific target type.
        /// </summary>
        /// <param name="target">The expression representing the target member.</param>
        /// <param name="targetType">The target type to bind to.</param>
        public void Bind(Expression<Func<TTarget, object>> target, Type targetType)
        {
            string targetName = GetMemberInfo(target);
            BindType(targetName, targetType);
        }

        /// <summary>
        /// Ignores a source member specified by the expression during mapping.
        /// </summary>
        /// <param name="expression">The expression representing the source member to ignore.</param>
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
                    throw new ArgumentException("Expression is not a MemberExpression", "expression");
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
