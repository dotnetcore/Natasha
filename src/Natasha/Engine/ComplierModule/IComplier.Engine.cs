using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Natasha.Complier
{
    public abstract partial class IComplier
    {

        internal readonly static AdhocWorkspace _workSpace;
        private readonly static AssemblyDomain _default;
        static IComplier()
        {

            _workSpace = new AdhocWorkspace();
            _workSpace.AddSolution(SolutionInfo.Create(SolutionId.CreateNewId("formatter"), VersionStamp.Default));
            _default = new AssemblyDomain("Default");

        }



        /// <summary>
        /// 使用内存流进行脚本编译
        /// </summary>
        /// <param name="sourceContent">脚本内容</param>
        /// <param name="errorAction">发生错误执行委托</param>
        /// <returns></returns>
        public static (Assembly Assembly, ImmutableArray<Diagnostic> Errors, CSharpCompilation Compilation) StreamComplier(string name, SyntaxTree tree, AssemblyDomain domain)
        {


#if NETCOREAPP3_0
            bool isDefaultDomain = domain == default && AssemblyLoadContext.CurrentContextualReflectionContext == default;
            if (isDefaultDomain)
            {

                domain = _default;

            }
            else if (domain == default && AssemblyLoadContext.CurrentContextualReflectionContext != null)
            {

                domain = (AssemblyDomain)(AssemblyLoadContext.CurrentContextualReflectionContext);

            }

#else
            bool isDefaultDomain = domain == default;
            domain = isDefaultDomain ? _default : domain;
#endif



            lock (domain)
            {

                //首先检测缓存是否已经有此类型
                if (domain.TypeMapping.ContainsKey(name))
                {

                    if (domain.CanCover)
                    {

                        domain.RemoveReferenceByClassName(name);

                    }
                    else
                    {
                        return (domain.TypeMapping[name], default, null);
                    }

                }


                //检测自定义域是否有新引用，有则加上
                var references = new HashSet<PortableExecutableReference>(_default.References);
                if (!isDefaultDomain && domain.References.Count > 0)
                {
                    references.UnionWith(domain.References);
                }


                //创建语言编译
                CSharpCompilation compilation = CSharpCompilation.Create(
                                  name + Guid.NewGuid().ToString("N"),
                                   options: new CSharpCompilationOptions(
                                       outputKind: OutputKind.DynamicallyLinkedLibrary,
                                       optimizationLevel: OptimizationLevel.Release,
                                       allowUnsafe: true),
                                   syntaxTrees: new[] { tree },
                                   references: references);


                //编译并生成程序集
                using (MemoryStream stream = new MemoryStream())
                {

                    var fileResult = compilation.Emit(stream);
                    if (fileResult.Success)
                    {

                        //重置流位置
                        stream.Position = 0;


                        //获取程序集
                        Assembly result = isDefaultDomain ?
                            AssemblyLoadContext.Default.LoadFromStream(stream) :
                            domain.LoadFromStream(stream);



                        domain.CacheAssembly(result, stream);
                        return (result, default, compilation);

                    }
                    else
                    {

                        return (null, fileResult.Diagnostics, compilation);

                    }

                }

            }

        }

    }
}
