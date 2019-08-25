using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.Complier.Model;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;

namespace Natasha.Complier
{
    public abstract partial class IComplier
    {

        /// <summary>
        /// 使用内存流进行脚本编译
        /// </summary>
        /// <param name="sourceContent">脚本内容</param>
        /// <param name="errorAction">发生错误执行委托</param>
        /// <returns></returns>
        public static (Assembly Assembly, ImmutableArray<Diagnostic> Errors, CSharpCompilation Compilation) StreamComplier(ComplierModel model)
        {

            var domain = model.Domain;
            lock (domain)
            {


                //创建语言编译
                CSharpCompilation compilation = CSharpCompilation.Create(
                                  model.AssemblyName,
                                   options: new CSharpCompilationOptions(
                                       outputKind: OutputKind.DynamicallyLinkedLibrary,
                                       optimizationLevel: OptimizationLevel.Release,
                                       allowUnsafe: true),
                                   syntaxTrees: model.Trees ,
                                   references: model.References);


                //编译并生成程序集
                MemoryStream stream = new MemoryStream();
                var fileResult = compilation.Emit(stream);
                if (fileResult.Success)
                {

                    return (domain.Handler(stream), default, compilation);

                }
                else
                {

                    stream.Dispose();
                    return (null, fileResult.Diagnostics, compilation);

                }

            }

        }

    }
}
