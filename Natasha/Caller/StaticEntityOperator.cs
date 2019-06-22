using Natasha.Caller.Wrapper;
using System;
namespace Natasha
{
    public class StaticEntityOperator
    {
        public static DynamicBase Create(Type type)
        {
           return DynamicStaticCallerBuilder.Ctor(type);
        }
    }
}
