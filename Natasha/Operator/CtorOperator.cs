using Natasha.Engine.Builder.Reverser;
using System;

namespace Natasha
{
    public class CtorOperator
    {
        public static Func<T> NewDelegate<T>()
        {
            return MethodBuilder.NewMethod
                .Using<T>()
                .Body($@"return new {TypeReverser.Get(typeof(T))}();")
                .Return<T>()
                .Create<Func<T>>();
        }
        public static Func<object> NewDelegate(Type type)
        {
            return MethodBuilder.NewMethod
               .Using(type)
               .Body($@"return new {TypeReverser.Get(type)}();")
               .Return<object>()
               .Create<Func<object>>();
        }
    }
}
