using Natasha.Caller.Wrapper;
using System;
namespace Natasha
{
    public class EntityOperator
    {
        public static DynamicBase Create(Type type)
        {
           return DynamicCallerBuilder.Ctor(type);
        }
    }
    public class EntityOperator<T>
    {
        public static DynamicBase Create()
        {
            return DynamicCallerBuilder<T>.Ctor();
        }
    }
}
