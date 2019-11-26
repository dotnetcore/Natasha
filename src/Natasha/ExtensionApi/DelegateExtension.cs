using Natasha.Operator;
using System;

namespace Natasha.DelegateExtension
{
    public static class DelegateExtension
    {
        public static T Create<T>(this T instance,string content,params NamespaceConverter[] usings) where T: Delegate
        {
            return instance = NDelegateOperator<T>.Delegate(content, usings);
        }

        public static Delegate Create(this Type instance, string content, params NamespaceConverter[] usings)
        {
            var method = instance.GetMethod("Invoke");
            return FakeMethodOperator
                .MainDomain
                .UseMethod(method)
                .Using(usings)
                .StaticMethodContent(content)
                .Complie();
        }
    }
}
