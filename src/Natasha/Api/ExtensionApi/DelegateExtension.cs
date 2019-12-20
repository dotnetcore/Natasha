using Natasha.Core;
using Natasha.Operator;
using System;

namespace Natasha
{
    public static class DelegateExtension
    {

        public static Delegate Create(this Type instance, string content, params NamespaceConverter[] usings)
        {
            var method = instance.GetMethod("Invoke");
            return FakeMethodOperator
                .Create()
                .UseMethod(method)
                .Using(usings)
                .StaticMethodContent(content)
                .Complie();
        }




        public static AssemblyDomain GetDomain(this Delegate @delegate)
        {

            return DomainCache.GetDomain(@delegate);

        }




        public static void RemoveReferences(this Delegate @delegate)
        {

            DomainCache.RemoveReferences(@delegate);

        }





        public static void DisposeDomain(this Delegate @delegate)
        {

            DomainCache.DisposeDomain(@delegate);

        }
    }
}
