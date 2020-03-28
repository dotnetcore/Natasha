using System;

namespace Natasha.Operator
{

    public static class DelegateOperator<T> where T : Delegate
    {

        
        public static T Delegate(string content, AssemblyDomain domain = default, Action<AssemblyCompiler> option = default, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator.Use(domain, option)
                .UseMethod(method)
                .Using(usings)
                .StaticMethodBody(content)
                .Compile<T>();

        }




        public static T AsyncDelegate(string content, AssemblyDomain domain = default, Action<AssemblyCompiler> option = default, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator.Use(domain, option)
                .UseMethod(method)
                .Async()
                .Using(usings)
                .StaticMethodBody(content)
                .Compile<T>();

        }




        public static T UnsafeDelegate(string content, AssemblyDomain domain = default, Action<AssemblyCompiler> option = default, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator.Use(domain, option)
                .UseMethod(method)
                .Unsafe()
                .Using(usings)
                .StaticMethodBody(content)
                .Compile<T>();

        }




        public static T UnsafeAsyncDelegate(string content, AssemblyDomain domain = default, Action<AssemblyCompiler> option = default, params NamespaceConverter[] usings)
        {

            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator.Use(domain, option)
                .UseMethod(method)
                .Unsafe()
                .Async()
                .Using(usings)
                .StaticMethodBody(content)
                .Compile<T>();

        }

    }

}
