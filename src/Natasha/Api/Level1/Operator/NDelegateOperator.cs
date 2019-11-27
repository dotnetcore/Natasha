using System;

namespace Natasha.Operator
{
    public static class NDelegateOperator<T> where T : Delegate
    {

        public static T Delegate(string content, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator
                .MainDomain
                .UseMethod(method)
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T AsyncDelegate(string content, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator
                .MainDomain
                .UseMethod(method)
                .UseAsync()
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T UnsafeDelegate(string content, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator
                .MainDomain
                .UseMethod(method)
                .UseUnsafe()
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T UnsafeAsyncDelegate(string content, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator
                .MainDomain
                .UseMethod(method)
                .UseUnsafe()
                .UseAsync()
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }

    }

}
