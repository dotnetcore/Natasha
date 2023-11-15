#if NETCOREAPP3_0_OR_GREATER
using System;

namespace Natasha.CSharp
{
    public static class NInstance
    {

        public static Func<T> Creator<T>()
        {

            return NDelegate.UseDomain(typeof(T).GetDomain()).Func<T>($"return new {typeof(T).GetDevelopName()}();");
        
        }




        public static Delegate Creator(Type type)
        {

            return FastMethodOperator
                .UseDomain(type.GetDomain())
                .Body($"return new {type.GetDevelopName()}();")
                .Return(type)
                .Compile();

        }

    }

}
#endif