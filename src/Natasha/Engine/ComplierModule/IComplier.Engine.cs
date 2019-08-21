using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Natasha.Log;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Natasha.Complier
{
    public abstract partial class IComplier
    {

        private readonly static AdhocWorkspace _workSpace;

        static IComplier()
        {

            _workSpace = new AdhocWorkspace();
            _workSpace.AddSolution(SolutionInfo.Create(SolutionId.CreateNewId("formatter"), VersionStamp.Default));

            NSucceed.Enabled = false;
            NWarning.Enabled = false;

        }




        public static (SyntaxTree Tree, string[] TypeNames, string Formatter, IEnumerable<Diagnostic> Errors) GetTreeInfo(string content)
        {

            SyntaxTree tree = CSharpSyntaxTree.ParseText(content, new CSharpParseOptions(LanguageVersion.Latest));
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();


            root = (CompilationUnitSyntax)Formatter.Format(root, _workSpace);
            var errors = root.GetDiagnostics();
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


            return (root.SyntaxTree, result.ToArray(), formatter, errors);

        }



        /// <summary>
        /// 使用内存流进行脚本编译
        /// </summary>
        /// <param name="sourceContent">脚本内容</param>
        /// <param name="errorAction">发生错误执行委托</param>
        /// <returns></returns>
        public static (Assembly Assembly, ImmutableArray<Diagnostic> Errors, CSharpCompilation Compilation) StreamComplier(string name, SyntaxTree tree, AssemblyDomain domain)
        {

            lock (domain)
            {


                if (domain.ClassMapping.ContainsKey(name))
                {

                    return (domain.ClassMapping[name], default, null);

                }


                //创建语言编译
                CSharpCompilation compilation = CSharpCompilation.Create(
                                  name,
                                   options: new CSharpCompilationOptions(
                                       outputKind: OutputKind.DynamicallyLinkedLibrary,
                                       optimizationLevel: OptimizationLevel.Release,
                                       allowUnsafe: true),
                                   syntaxTrees: new[] { tree },
                                   references: domain.References);


                //编译并生成程序集
                using (MemoryStream stream = new MemoryStream())
                {

                    var fileResult = compilation.Emit(stream);
                    if (fileResult.Success)
                    {

                        stream.Position = 0;
                        var result = domain.LoadFromStream(stream);
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
