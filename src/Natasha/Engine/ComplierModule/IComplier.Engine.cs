using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Natasha.Complier
{
    public abstract partial class IComplier
    {

        private readonly static AdhocWorkspace _workSpace;
        private readonly static AssemblyDomain _default;
        static IComplier()
        {

            _workSpace = new AdhocWorkspace();
            _workSpace.AddSolution(SolutionInfo.Create(SolutionId.CreateNewId("formatter"), VersionStamp.Default));
            _default = new AssemblyDomain("Default");

        }




        public static (SyntaxTree Tree, string[] TypeNames, string Formatter, IEnumerable<Diagnostic> Errors) GetTreeInfo(string content)
        {

            SyntaxTree tree = CSharpSyntaxTree.ParseText(content, new CSharpParseOptions(LanguageVersion.Latest));
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();


            root = (CompilationUnitSyntax)Formatter.Format(root, _workSpace);
            var formatter = root.ToString();


            var result = new List<string>(from typeNodes
                         in root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                                          select typeNodes.Identifier.Text);

            result.AddRange(from typeNodes
                    in root.DescendantNodes().OfType<StructDeclarationSyntax>()
                            select typeNodes.Identifier.Text);

            result.AddRange(from typeNodes
                    in root.DescendantNodes().OfType<InterfaceDeclarationSyntax>()
                            select typeNodes.Identifier.Text);

            result.AddRange(from typeNodes
                    in root.DescendantNodes().OfType<EnumDeclarationSyntax>()
                            select typeNodes.Identifier.Text);


            return (root.SyntaxTree, result.ToArray(), formatter, root.GetDiagnostics());

        }



        /// <summary>
        /// 使用内存流进行脚本编译
        /// </summary>
        /// <param name="sourceContent">脚本内容</param>
        /// <param name="errorAction">发生错误执行委托</param>
        /// <returns></returns>
        public static (Assembly Assembly, ImmutableArray<Diagnostic> Errors, CSharpCompilation Compilation) StreamComplier(string name, SyntaxTree tree, AssemblyDomain domain)
        {

            HashSet<PortableExecutableReference> references;
            bool isDefaultDomain = domain == default;
            domain = isDefaultDomain ? _default : domain;


            lock (domain)
            {

                //首先检测缓存是否已经有此类型
                if (domain.TypeMapping.ContainsKey(name))
                {

                    if (domain.CanCover)
                    {

                        domain.RemoveReference(name);
                        _default.RemoveReference(name);

                    }
                    else
                    {
                        return (domain.TypeMapping[name], default, null);
                    }

                }

                references = new HashSet<PortableExecutableReference>(_default.NewReferences);
                //检测自定义域是否有新引用，有则加上
                if (!isDefaultDomain && domain.NewReferences.Count > 0)
                {
                    references.UnionWith(domain.NewReferences);
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
