using Natasha.Engine.Builder.Reverser;
using System;

namespace Natasha
{
    public class CtorOperator
    {
        public static Func<T> NewDelegate<T>(Type type=null)
        {
            var builder = FastMethod.New;
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
                .MethodBody($@"return new {NameReverser.GetName(type)}();")
                .Return<T>()
                .Complie<Func<T>>();
        }
        public static Delegate NewDelegate(Type type)
        {
            return FastMethod.New
                .MethodBody($@"return new {NameReverser.GetName(type)}();")
                .Return(type)
                .Complie();
        }
    }
}
