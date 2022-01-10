using System;

namespace Natasha.CSharp
{
    public static class DelegateExtension
    {

        public static Delegate Create(this Type instance, string content, params NamespaceConverter[] usings)
        {
            var method = instance.GetMethod("Invoke")!;
            return FakeMethodOperator
                .DefaultDomain()
                .UseMethod(method)
                .Using(usings)
                .StaticMethodBody(content)
                .Compile();
        }




        public static NatashaReferenceDomain GetDomain(this Delegate @delegate)
        {

            return @delegate.Method.Module.Assembly.GetDomain();

        }



        public static void DisposeDomain(this Delegate @delegate)
        {

            @delegate.Method.Module.Assembly.DisposeDomain();

        }
    }
}
