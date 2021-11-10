using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharp.Builder;
using Natasha.Framework;
using System;
using System.Diagnostics;

namespace Natasha.CSharp
{

    public static class DelegateOperator<T> where T : Delegate
    {

        public static T Delegate(string content,
            AssemblyCSharpBuilder builder, 
            Action<AssemblyCSharpBuilder>? option = default, 
            Func<FakeMethodOperator, FakeMethodOperator>? methodAction = null, 
            Func<OopBuilder,OopBuilder>? oopAction = null, 
            params NamespaceConverter[]? usings)
        {
#if DEBUG
            Stopwatch stopwatch = new();
            stopwatch.Start();
#endif
            var method = typeof(T).GetMethod("Invoke")!;
            var @operator = FakeMethodOperator.UseDomain(builder.Compiler.Domain, option);
            @operator.AssemblyBuilder = builder;
            @operator
                .UseMethod(method)
                .Using(usings)
                .StaticMethodBody(content);
            methodAction?.Invoke(@operator);
            oopAction?.Invoke(@operator.OopHandler);
#if DEBUG
            stopwatch.StopAndShowCategoreInfo("[Operator]", "模板构建耗时", 1);
#endif
            return @operator.Compile<T>();

        }




        public static T AsyncDelegate(string content,
           AssemblyCSharpBuilder builder,
            Action<AssemblyCSharpBuilder>? option = default, 
            Func<FakeMethodOperator, FakeMethodOperator>? methodAction = null,
            Func<OopBuilder, OopBuilder>? oopAction = null,
            params NamespaceConverter[]? usings)
        {

            var method = typeof(T).GetMethod("Invoke")!;
            var @operator = FakeMethodOperator.UseDomain(builder.Compiler.Domain, option);
            @operator.AssemblyBuilder = builder;
            @operator
                .UseMethod(method)
                .Async()
                .Using(usings)
                .StaticMethodBody(content);
            methodAction?.Invoke(@operator);
            oopAction?.Invoke(@operator.OopHandler);
            return @operator.Compile<T>();

        }




        public static T UnsafeDelegate(string content,
            AssemblyCSharpBuilder builder,
            Action<AssemblyCSharpBuilder>? option = default, 
            Func<FakeMethodOperator, FakeMethodOperator>? methodAction = null,
            Func<OopBuilder, OopBuilder>? oopAction = null,
            params NamespaceConverter[]? usings)
        {

            var method = typeof(T).GetMethod("Invoke")!;
            var @operator = FakeMethodOperator.UseDomain(builder.Compiler.Domain, option);
            @operator.AssemblyBuilder = builder;
            @operator
                .UseMethod(method)
                .Unsafe()
                .Using(usings)
                .StaticMethodBody(content);
            methodAction?.Invoke(@operator);
            oopAction?.Invoke(@operator.OopHandler);
            return @operator.Compile<T>();

        }




        public static T UnsafeAsyncDelegate(string content,
           AssemblyCSharpBuilder builder,
            Action<AssemblyCSharpBuilder>? option = default, 
            Func<FakeMethodOperator, FakeMethodOperator>? methodAction = null,
            Func<OopBuilder, OopBuilder>? oopAction = null,
            params NamespaceConverter[]? usings)
        {

            var method = typeof(T).GetMethod("Invoke")!;
            var @operator = FakeMethodOperator.UseDomain(builder.Compiler.Domain, option);
            @operator.AssemblyBuilder = builder;
            @operator
                .UseMethod(method)
                .Unsafe()
                .Async()
                .Using(usings)
                .StaticMethodBody(content);
            methodAction?.Invoke(@operator);
            oopAction?.Invoke(@operator.OopHandler);
            return @operator.Compile<T>();

        }

    }

}
