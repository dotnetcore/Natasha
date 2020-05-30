using Natasha;
using Natasha.CSharp;
using Natasha.CSharp.Builder;
using System;
using System.Collections.Generic;

namespace Core21
{
    class Program
    {
        static void Main(string[] args)
        {
            DomainManagement.RegisterDefault<AssemblyDomain>();
            //var @operator = FastMethodOperator.DefaultDomain();
            //var actionDelegate = @operator
            //    .Param(typeof(string), "parameter")
            //    .Body("Console.WriteLine(parameter);")
            //    .Compile();

            //actionDelegate.DynamicInvoke("HelloWorld!");
            //var action = (Action<string>)actionDelegate;
            //action("HelloWorld!");
            //actionDelegate.DisposeDomain();

            //起个类
            NClass nClass = NClass.DefaultDomain();
            nClass
                .Namespace("MyNamespace")
                .Public()
                .DefinedName("MyClass")
                .Ctor(ctor=>ctor.Public().Body("MyField=\"Hello\";"))
                .Property(prop => prop
                    .DefinedType(typeof(string))
                    .DefinedName("MyProperty")
                    .Public()
                    .OnlyGetter("return \"World!\";")
                    );


            //添加方法
            MethodBuilder mb = new MethodBuilder();
            mb
                .Public()
                .Override()
                .DefinedName("ToString")
                .Body("return MyField+\" \"+MyProperty;")
                .Return(typeof(string));
            nClass.Method(mb);


            //添加字段
            FieldBuilder fb = nClass.GetFieldBuilder();
            fb.Public()
                .DefinedName("MyField")
                .DefinedType<string>();


            //动态调用动态创建的类
            var action = NDelegate
                .RandomDomain()
                .Action("Console.WriteLine((new MyClass()).ToString());", nClass.GetType());

            action();
            action.DisposeDomain();
            //Console.WriteLine(typeof(List<int>[]).GetRuntimeName());
            //Console.WriteLine(typeof(List<int>[,]).GetRuntimeName());
            //Console.WriteLine(typeof(int[,]).GetRuntimeName());
            //Console.WriteLine(typeof(int[][]).GetRuntimeName());
            //Console.WriteLine(typeof(int[][,,,]).GetRuntimeName());
            Console.ReadKey();
        }
    }
}
