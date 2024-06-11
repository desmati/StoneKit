using System.Reflection.Mapping;

namespace UnitTests.Snippets
{
    public sealed class ObjectMapperSnippet : IObjectMapper
    {
        public void Bind<TSource, TTarget>()
        {
            Transverse.Bind<TSource, TTarget>();
        }

        public void Bind<TSource, TTarget>(Action<IBindingConfig<TSource, TTarget>> config)
        {
            Transverse.Bind(config);
        }

        public TTarget Map<TSource, TTarget>(TSource source, TTarget target = default(TTarget))
        {
            return Transverse.Map(source, target);
        }

        public TTarget Map<TTarget>(object source)
        {
            return Transverse.Map<TTarget>(source);
        }
    }

    public interface IObjectMapper
    {
        void Bind<TSource, TTarget>();
        void Bind<TSource, TTarget>(Action<IBindingConfig<TSource, TTarget>> config);
        TTarget Map<TSource, TTarget>(TSource source, TTarget target = default);
        TTarget Map<TTarget>(object source);
    }
}
