#if !NETCOREAPP3_0_OR_GREATER
using System;

namespace Natasha.CSharp
{
    public static class NInstance
    {

        public static Func<T> Creator<T>()
        {

            return NDelegate.DefaultDomain().Func<T>($"return new {typeof(T).GetDevelopName()}();");
        
        }




        public static Delegate Creator(Type type)
        {

            return FastMethodOperator
                .DefaultDomain()
                .Body($"return new {type.GetDevelopName()}();")
                .Return(type)
                .Compile();

        }

    }

}
#endif