using System;

namespace Natasha.Operator
{
    public static class DelegateOperator<T> where T : Delegate
    {

        public static T Delegate(string content, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator.Default
                .UseMethod(method)
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T AsyncDelegate(string content, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator.Default
                .UseMethod(method)
                .UseAsync()
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T UnsafeDelegate(string content, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator.Default
                .UseMethod(method)
                .UseUnsafe()
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T UnsafeAsyncDelegate(string content, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator.Default
                .UseMethod(method)
                .UseUnsafe()
                .UseAsync()
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T Delegate(string content, AssemblyDomain domain = default, bool complieInFile = false, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            FakeMethodOperator @operator = FakeMethodOperator.Create(domain, complieInFile);
            return @operator
                .UseMethod(method)
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T AsyncDelegate(string content, AssemblyDomain domain = default, bool complieInFile = false, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            FakeMethodOperator @operator = FakeMethodOperator.Create(domain, complieInFile);
            return @operator
                .UseMethod(method)
                .UseAsync()
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T UnsafeDelegate(string content, AssemblyDomain domain = default, bool complieInFile = false, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            FakeMethodOperator @operator = FakeMethodOperator.Create(domain, complieInFile);
            return @operator
                .UseMethod(method)
                .UseUnsafe()
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T UnsafeAsyncDelegate(string content, AssemblyDomain domain = default, bool complieInFile = false, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            FakeMethodOperator @operator = FakeMethodOperator.Create(domain, complieInFile);
            return @operator
                .UseMethod(method)
                .UseUnsafe()
                .UseAsync()
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }

    }

}
