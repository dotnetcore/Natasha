using Natasha.Complier;
using System;

namespace Natasha.Builder
{
    public class OnceMethodBuilder<TBuilder> : OnceMethodTemplate<TBuilder>
    {
        public DefaultComplier ComplierInstance;

        public OnceMethodBuilder()
        {
            ComplierInstance = new DefaultComplier();
        }

        public Delegate Complie()
        {
            return ComplierInstance.Complie(Package(), NameScript);
        }

        public T Complie<T>() where T:Delegate
        {
            return (T)Complie();
        }
    }
}
