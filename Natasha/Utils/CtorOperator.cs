using Natasha.Engine.Builder.Reverser;
using System;

namespace Natasha
{
    public class CtorOperator
    {
        public static Func<T> NewDelegate<T>(Type type=null)
        {
            var builder = MethodBuilder.NewMethod;
            if (type==null)
            {
                //直接使用T的类型作为初始化类型
                type = typeof(T);
            }
            else
            {
                //T为object，那么自动加载type的命名空间
                builder.Using(type);
            }
            return builder
                .Using<T>()
                .Body($@"return new {TypeReverser.Get(type)}();")
                .Return<T>()
                .Create<Func<T>>();
        }
        public static Delegate NewDelegate(Type type)
        {
            return MethodBuilder.NewMethod
               .Body($@"return new {TypeReverser.Get(type)}();")
               .Return(type)
               .Create();
        }
    }
}
