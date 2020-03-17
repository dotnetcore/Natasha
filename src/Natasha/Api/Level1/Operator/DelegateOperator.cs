using System;

namespace Natasha.Operator
{
    public static class DelegateOperator<T> where T : Delegate
    {

        public static T Delegate(string content, Action<AssemblyComplier> option, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator.Default(option)
                .UseMethod(method)
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T AsyncDelegate(string content, Action<AssemblyComplier> option, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator.Default(option)
                .UseMethod(method)
                .UseAsync()
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T UnsafeDelegate(string content, Action<AssemblyComplier> option, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator.Default(option)
                .UseMethod(method)
                .UseUnsafe()
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T UnsafeAsyncDelegate(string content, Action<AssemblyComplier> option, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator.Default(option)
                .UseMethod(method)
                .UseUnsafe()
                .UseAsync()
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T Delegate(string content, AssemblyDomain domain = default, Action<AssemblyComplier> option = default, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator.Create(domain, option)
                .UseMethod(method)
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T AsyncDelegate(string content, AssemblyDomain domain = default, Action<AssemblyComplier> option = default, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator.Create(domain, option)
                .UseMethod(method)
                .UseAsync()
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T UnsafeDelegate(string content, AssemblyDomain domain = default, Action<AssemblyComplier> option = default, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator.Create(domain, option)
                .UseMethod(method)
                .UseUnsafe()
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }




        public static T UnsafeAsyncDelegate(string content, AssemblyDomain domain = default, Action<AssemblyComplier> option = default, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator.Create(domain, option)
                .UseMethod(method)
                .UseUnsafe()
                .UseAsync()
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();

        }

    }

}
