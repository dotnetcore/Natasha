using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Formatting;
using Natasha.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

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


            var result = new List<string>(from classNodes
                         in root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                                          select classNodes.Identifier.Text);

            result.AddRange(from classNodes
                    in root.DescendantNodes().OfType<StructDeclarationSyntax>()
                            select classNodes.Identifier.Text);

            result.AddRange(from classNodes
                    in root.DescendantNodes().OfType<InterfaceDeclarationSyntax>()
                            select classNodes.Identifier.Text);

            result.AddRange(from classNodes
                    in root.DescendantNodes().OfType<EnumDeclarationSyntax>()
                            select classNodes.Identifier.Text);


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

            var (Tree, ClassNames, formatter, errors) = GetTreeInfo(sourceContent.Trim());
            formatContent = formatter;
            diagnostics.AddRange(errors);
            if (diagnostics.Count() != 0)
            {

                return null;

            }

            //创建语言编译
            CSharpCompilation compilation = CSharpCompilation.Create(
                ClassNames[0],
                options: new CSharpCompilationOptions(
                    outputKind: OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release,
                    allowUnsafe: true),
                syntaxTrees: new[] { Tree },
                references: domain.References);


            //编译并生成程序集
            using (MemoryStream stream = new MemoryStream())
            {

                var fileResult = compilation.Emit(stream);
                if (fileResult.Success)
                {

                    stream.Position = 0;
                    var result = domain.LoadFromStream(stream);


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
                    //错误处理
                    if (NError.Enabled)
                    {

                        NError logError = new NError();
                        logError.WrapperCode(formatter);
                        logError.Handler(compilation, fileResult.Diagnostics);
                        logError.Write();
                    }


                }

            }

            return null;

        }









        /// <summary>
        /// 使用文件流进行脚本编译，根据类名生成dll
        /// </summary>
        /// <param name="sourceContent">脚本内容</param>
        /// <param name="errorAction">发生错误执行委托</param>
        /// <returns></returns>
        public static Assembly FileComplier(string sourceContent, AssemblyDomain domain, out string formatContent, ref List<Diagnostic> diagnostics)
        {


            //类名获取
            var (Tree, ClassNames, formatter, errors) = GetTreeInfo(sourceContent.Trim());
            formatContent = formatter;
            diagnostics.AddRange(errors);
            if (diagnostics.Count() != 0)
            {

                return null;

            }


            if (domain.ClassMapping.ContainsKey(ClassNames[0]))
            {

                return domain.ClassMapping[ClassNames[0]];

            }


            //生成路径
            string path = Path.Combine(domain.LibPath, $"{ClassNames[0]}.dll");
            if (domain.DynamicDlls.ContainsKey(path))
            {

                return domain.DynamicDlls[path];

            }




            //创建语言编译
            CSharpCompilation compilation = CSharpCompilation.Create(
                ClassNames[0],
                options: new CSharpCompilationOptions(
                    outputKind: OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release,
                     allowUnsafe: true),
                syntaxTrees: new[] { Tree },
                references: domain.References);


            EmitResult fileResult;
            //编译到文件
            try
            {

                fileResult = compilation.Emit(path);
                if (fileResult.Success)
                {

                    //为了实现动态中的动态，使用文件加载模式常驻内存
                    var result = domain.LoadFromAssemblyPath(path);
                    domain.CacheAssembly(result);


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

                return null;

            }
            catch (Exception ex)
            {

                if (ex is IOException)
                {

                    if (!Directory.Exists(domain.LibPath))
                    {

                        if (NWarning.Enabled)
                        {
                            NWarning warning = new NWarning();
                            warning.WrapperTitle(ClassNames[0]);
                            warning.Handler($"{domain.LibPath}路径不存在，请重新运行程序！");
                            warning.Write();
                        }
                        throw new Exception($"{domain.LibPath}路径不存在，请重新运行程序！");

                    }

                    int loop = 0;
                    while (!domain.DynamicDlls.ContainsKey(path))
                    {

                        Thread.Sleep(200);
                        loop += 1;

                    }


                    if (NWarning.Enabled)
                    {
                        NWarning warning = new NWarning();
                        warning.WrapperTitle(ClassNames[0]);
                        warning.Handler($"    I/O Delay :\t检测到争用，延迟{loop * 200}ms调用;\r\n");
                        warning.Write();
                    }


                    return domain.DynamicDlls[path];


                }


                return null;

            }

        }

    }

}
