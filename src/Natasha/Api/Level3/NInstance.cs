using Natasha;
using System;

namespace Natash
{
    public class NInstance
    {

        public static Func<T> Creator<T>()
        {

            return NDelegate.Use(typeof(T).GetDomain()).Func<T>($"return new {typeof(T).GetDevelopName()}();");
        
        }




        public static Delegate Creator(Type type)
        {

            return FastMethodOperator
                .Use(type.GetDomain())
                .Body($"return new {type.GetDevelopName()}();")
                .Return(type)
                .Compile();

        }

    }

}
