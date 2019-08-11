using System;

namespace Natasha.Operator
{
    public static class DelegateOperator<T> where T : Delegate
    {

        public static T CreateUsingStrings(string content, params string[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator
                .New
                .UseMethod(method)
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T Delegate(string content, params Type[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator
                .New
                .UseMethod(method)
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T AsyncDelegate(string content, params Type[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator
                .New
                .UseMethod(method)
                .UseAsync()
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T UnsafeDelegate(string content, params Type[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator
                .New
                .UseMethod(method)
                .UseUnsafe()
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T UnsafeAsyncDelegate(string content, params Type[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator
                .New
                .UseMethod(method)
                .UseUnsafe()
                .UseAsync()
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }

    }

}
