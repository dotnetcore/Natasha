using System;

namespace Natasha.CSharp
{
    public static class NInstance
    {

        public static Func<T> Creator<T>()
        {

            return NDelegate.UseDomain(DomainManagement.Create(typeof(T).GetDomain()!.Name)).Func<T>($"return new {typeof(T).GetDevelopName()}();");
        
        }




        public static Delegate Creator(Type type)
        {

            return FastMethodOperator
                .UseDomain(DomainManagement.Create(type.GetDomain()!.Name))
                .Body($"return new {type.GetDevelopName()}();")
                .Return(type)
                .Compile();

        }

    }

}