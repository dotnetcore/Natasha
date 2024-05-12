using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharp.Compiler.Component;

namespace ExtensionSample
{
    internal class Program
    {

        static void Main(string[] args)
        {
            NatashaManagement.RegistDomainCreator<NatashaDomainCreator>();
            string script = @"
using System;
                [System.Diagnostics.CodeAnalysis.SuppressMessage(""Microsoft.Usage"", ""CA2202:Do not dispose objects multiple times"")]
        public class MyClass
        {
            public void MyMethod()
            {
                // 可能会引发被抑制警告的代码
            }
        }

            ";
            var compilationOptions = new CSharpCompilationOptions( OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Debug, reportSuppressedDiagnostics:true);
            var result = MetadataHelper.GetMetadataAndNamespaceFromMemory(typeof(object).Assembly);
            var compilation = CSharpCompilation.Create("MyAssembly", [CSharpSyntaxTree.ParseText(script)], references: [result.Value.metadata],options: compilationOptions);
            MemoryStream memory = new MemoryStream();
            // 编译代码并获取编译结果
            var compilationResult = compilation.Emit(memory);

            // 检查编译结果是否有错误或警告
            if (compilationResult.Success)
            {
                Console.WriteLine("编译成功，没有错误或警告。");
            }
            else
            {
                var errors = compilationResult.Diagnostics;
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
                Console.WriteLine("编译失败，出现错误或警告。");
            }






            NatashaManagement.RegistDomainCreator<NatashaDomainCreator>();







            var func = "return arg1+arg2;"
                .WithAssemblyBuilder(opt=>opt.AddReferenceAndUsingCode<object>())
                .ToFunc<int, int, int>()!;

            Console.WriteLine(func(1,2));

            AssemblyCSharpBuilder assemblyCSharp = new AssemblyCSharpBuilder();
            assemblyCSharp.ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode<object>());
            assemblyCSharp.Add("public class A{\r\npublic void Show(){\r\n}\r\n}");
            assemblyCSharp.LogCompilationEvent += (log) => { Console.WriteLine(log.ToString()); };
            assemblyCSharp.GetAssembly();
            Console.ReadKey();
        }
    }
}
