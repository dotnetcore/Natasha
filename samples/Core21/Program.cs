using Natasha;
using Natasha.CSharp;
using System;
using System.Collections.Generic;

namespace Core21
{
    class Program
    {
        static void Main(string[] args)
        {
            DomainManagement.RegisterDefault<AssemblyDomain>();
            var @operator = FastMethodOperator.DefaultDomain();
            var actionDelegate = @operator
                .Param(typeof(string), "parameter")
                .Body("Console.WriteLine(parameter);")
                .Compile();

            actionDelegate.DynamicInvoke("HelloWorld!");
            var action = (Action<string>)actionDelegate;
            action("HelloWorld!");
            actionDelegate.DisposeDomain();
            //Console.WriteLine(typeof(List<int>[]).GetRuntimeName());
            //Console.WriteLine(typeof(List<int>[,]).GetRuntimeName());
            //Console.WriteLine(typeof(int[,]).GetRuntimeName());
            //Console.WriteLine(typeof(int[][]).GetRuntimeName());
            //Console.WriteLine(typeof(int[][,,,]).GetRuntimeName());
            Console.ReadKey();
        }
    }
}
