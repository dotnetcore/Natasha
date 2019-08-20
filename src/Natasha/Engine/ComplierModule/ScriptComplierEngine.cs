using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Natasha.Log;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Natasha.Complier
{

    /// <summary>
    /// 核心编译引擎
    /// </summary>
    public class ScriptComplierEngine
    {

        private readonly static AdhocWorkspace _workSpace;

        static ScriptComplierEngine()
        {

            _workSpace = new AdhocWorkspace();
            _workSpace.AddSolution(SolutionInfo.Create(SolutionId.CreateNewId("formatter"), VersionStamp.Default));

            NSucceed.Enabled = false;
            NWarning.Enabled = false;

        }




        public static (SyntaxTree Tree, string[] ClassNames, string formatter, IEnumerable<Diagnostic> errors) GetTreeInfo(string content)
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
        public static Assembly StreamComplier(string sourceContent, AssemblyDomain domain, out string formatContent, ref List<Diagnostic> diagnostics)
        {

            lock (domain)
            {

                var (tree, typeNames, formatter, errors) = GetTreeInfo(sourceContent.Trim());
                formatContent = formatter;
                diagnostics.AddRange(errors);
                if (diagnostics.Count() != 0)
                {

                    return null;

                }


                if (domain.ClassMapping.ContainsKey(typeNames[0]))
                {

                    return domain.ClassMapping[typeNames[0]];

                }


                //创建语言编译
                CSharpCompilation compilation = CSharpCompilation.Create(
                                   typeNames[0],
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


                        if (NSucceed.Enabled)
                        {

                            NSucceed logSucceed = new NSucceed();
                            logSucceed.WrapperCode(formatter);
                            logSucceed.Handler(compilation, result);

                        }
                        return result;

                    }
                    else
                    {

                        diagnostics.AddRange(fileResult.Diagnostics);
                        if (NError.Enabled)
                        {

                            NError logError = new NError();
                            logError.WrapperCode(formatter);
                            logError.Handler(compilation, fileResult.Diagnostics);
                            logError.Write();
                        }

                    }

                }

            }
            return null;

        }

    }

}
