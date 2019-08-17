using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading;

namespace Natasha.Complier
{

    /// <summary>
    /// 核心编译引擎
    /// </summary>
    public class ScriptComplierEngine
    {

        public readonly static string LibPath;
        public readonly static ConcurrentDictionary<string, Assembly> ClassMapping;
        public readonly static ConcurrentDictionary<string, Assembly> DynamicDlls;
        public readonly static ConcurrentBag<PortableExecutableReference> References;
        private readonly static AdhocWorkspace _workSpace;

        static ScriptComplierEngine()
        {

           _workSpace = new AdhocWorkspace();
            _workSpace.AddSolution(SolutionInfo.Create(SolutionId.CreateNewId("formatter"), VersionStamp.Default));


            //初始化路径
            LibPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib");


            //处理目录
            if (Directory.Exists(LibPath))
            {

                Directory.Delete(LibPath, true);

            }
            Directory.CreateDirectory(LibPath);


            //程序集缓存
            //AppDomain.CurrentDomain.GetAssemblies().Select(item => DependencyContext.Load(item));
            var _ref = DependencyContext.Default.CompileLibraries
                                .SelectMany(cl => cl.ResolveReferencePaths())
                                .Select(asm => MetadataReference.CreateFromFile(asm));


            DynamicDlls = new ConcurrentDictionary<string, Assembly>();
            References = new ConcurrentBag<PortableExecutableReference>(_ref);
            ClassMapping = new ConcurrentDictionary<string, Assembly>();

        }




        public static void LoadFile(string path)
        {

            if (!DynamicDlls.ContainsKey(path))
            {

                AssemblyLoadContext context = AssemblyLoadContext.Default;
                var result = context.LoadFromAssemblyPath(path);
                References.Add(MetadataReference.CreateFromFile(path));
                DynamicDlls[path] = result;

            }
            
        }




        /// <summary>
        /// 初始化目录
        /// </summary>
        public static void Init()
        {

            if (Directory.Exists(LibPath))
            {

                Directory.Delete(LibPath, true);

            }
            Directory.CreateDirectory(LibPath);

        }




        public static (SyntaxTree Tree, string[] ClassNames, string formatter) GetTreeAndClassNames(string content)
        {

            SyntaxTree tree = CSharpSyntaxTree.ParseText(content,new CSharpParseOptions(LanguageVersion.Latest));
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();


            root = (CompilationUnitSyntax)Formatter.Format(root, _workSpace);
            var formatter = root.ToString();


            var result = new List<string>(from classNodes
                         in root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                                          select classNodes.Identifier.Text);


            result.AddRange(from classNodes
                    in root.DescendantNodes().OfType<StructDeclarationSyntax>()
                            select classNodes.Identifier.Text);


            return (root.SyntaxTree, result.ToArray(), formatter);

        }




        /// <summary>
        /// 使用内存流进行脚本编译
        /// </summary>
        /// <param name="sourceContent">脚本内容</param>
        /// <param name="errorAction">发生错误执行委托</param>
        /// <returns></returns>
        public static Assembly StreamComplier(string sourceContent,out string formatContent, AssemblyLoadContext context, Action<Diagnostic> errorAction = null)
        {

            var (Tree, ClassName, formatter) = GetTreeAndClassNames(sourceContent.Trim());
            formatContent = formatter;
            StringBuilder recoder = new StringBuilder(FormatLineCode(formatter));


            //创建语言编译
            CSharpCompilation compilation = CSharpCompilation.Create(
                ClassName[0],
                options: new CSharpCompilationOptions(
                    outputKind: OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release,
                    allowUnsafe: true),
                syntaxTrees: new[] { Tree },
                references: References);
            

            //编译并生成程序集
            using (MemoryStream stream = new MemoryStream())
            {

                var fileResult = compilation.Emit(stream);
                if (fileResult.Success)
                {

                    stream.Position = 0;
#if NETCOREAPP3_0
                    context ??= AssemblyLoadContext.Default;
#else
                    if (context==null)
                    {
                        context = AssemblyLoadContext.Default;
                    }
#endif

                    var result = context.LoadFromStream(stream);
                    context = null;

                    if (NScriptLog.UseLog)
                    {

                        recoder.AppendLine("\r\n\r\n------------------------------------------succeed-------------------------------------------");
                        recoder.AppendLine($"\r\n    Time :\t\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        recoder.AppendLine($"\r\n    Lauguage :\t{compilation.Language} & {compilation.LanguageVersion}");
                        recoder.AppendLine($"\r\n    Target :\t\t{ClassName}");
                        recoder.AppendLine($"\r\n    Size :\t\t{stream.Length}");
                        recoder.AppendLine($"\r\n    Assembly : \t{result.FullName}");
                        recoder.AppendLine("\r\n----------------------------------------------------------------------------------------------");
                        NScriptLog.Succeed("Succeed : " + ClassName, recoder.ToString());

                    }


                    return result;

                }
                else
                {

                    if (NScriptLog.UseLog)
                    {

                        recoder.AppendLine("\r\n\r\n------------------------------------------error----------------------------------------------");
                        recoder.AppendLine($"\r\n    Time :\t\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        recoder.AppendLine($"\r\n    Lauguage :\t{compilation.Language} & {compilation.LanguageVersion}");
                        recoder.AppendLine($"\r\n    Target:\t\t{ClassName}");
                        recoder.AppendLine($"\r\n    Error:\t\t共{fileResult.Diagnostics.Length}处错误！");

                    }

                    //错误处理
                    foreach (var item in fileResult.Diagnostics)
                    {

                        if (NScriptLog.UseLog)
                        {

                            var temp = item.Location.GetLineSpan().StartLinePosition;
                            var result = GetErrorString(formatter, item.Location.GetLineSpan());
                            recoder.AppendLine($"\t\t第{temp.Line + 1}行，第{temp.Character}个字符：       内容【{result}】  {item.GetMessage()}");
                        }

                        errorAction?.Invoke(item);
                    }


                    if (NScriptLog.UseLog)
                    {

                        recoder.AppendLine("\r\n---------------------------------------------------------------------------------------------");
                        NScriptLog.Error("Error : " + ClassName, recoder.ToString());

                    }

                }

            }

            return null;

        }




        public static Assembly GetDynamicAssembly(string className)
        {

            if (ClassMapping.ContainsKey(className))
            {

                return ClassMapping[className];

            }


            return null;

        }




        /// <summary>
        /// 使用文件流进行脚本编译，根据类名生成dll
        /// </summary>
        /// <param name="sourceContent">脚本内容</param>
        /// <param name="errorAction">发生错误执行委托</param>
        /// <returns></returns>
        public static Assembly FileComplier(string sourceContent, out string formatContent, AssemblyLoadContext context, Action<Diagnostic> errorAction = null)
        {

#if NETCOREAPP3_0
            context ??= AssemblyLoadContext.Default;
#else
                    if (context == null)
                    {
                        context = AssemblyLoadContext.Default;
                    }
#endif
            bool isDefaultContext = context == AssemblyLoadContext.Default;


            //类名获取
            var (Tree, ClassNames, formatter) = GetTreeAndClassNames(sourceContent.Trim());
            formatContent = formatter;


            if (ClassMapping.ContainsKey(ClassNames[0]))
            {
                return ClassMapping[ClassNames[0]];
            }


            //生成路径
            string path = Path.Combine(LibPath, $"{ClassNames[0]}.dll");
            if (isDefaultContext)
            {
                if (DynamicDlls.ContainsKey(path))
                {

                    return DynamicDlls[path];

                }
            }
            


            //创建语言编译
            CSharpCompilation compilation = CSharpCompilation.Create(
                ClassNames[0],
                options: new CSharpCompilationOptions(
                    outputKind: OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release,
                     allowUnsafe: true),
                syntaxTrees: new[] { Tree },
                references: References);


            EmitResult fileResult;
            //编译到文件
            try
            {

                StringBuilder recoder = new StringBuilder(FormatLineCode(formatter));
                fileResult = compilation.Emit(path);
                if (fileResult.Success)
                {

                    //为了实现动态中的动态，使用文件加载模式常驻内存
                    var result = context.LoadFromAssemblyPath(path);
                    if (isDefaultContext)
                    {
                        References.Add(MetadataReference.CreateFromFile(path));
                        for (int i = 0; i < ClassNames.Length; i += 1)
                        {

                            ClassMapping[ClassNames[i]] = result;

                        }
                        DynamicDlls[path] = result;
                    }
                    

                    if (NScriptLog.UseLog)
                    {

                        recoder.AppendLine("\r\n\r\n------------------------------------------succeed-------------------------------------------");
                        recoder.AppendLine($"\r\n    Time :\t\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        recoder.AppendLine($"\r\n    Lauguage :\t{compilation.Language} & {compilation.LanguageVersion}");
                        recoder.AppendLine($"\r\n    Target :\t\t{ClassNames[0]}");
                        recoder.AppendLine($"\r\n    Path :\t\t{path}");
                        recoder.AppendLine($"\r\n    Assembly : \t{result.FullName}");
                        recoder.AppendLine("\r\n----------------------------------------------------------------------------------------------");
                        NScriptLog.Succeed("Succeed : " + ClassNames[0], recoder.ToString());

                    }

                    
                    return result;

                }
                else
                {

                    if (NScriptLog.UseLog)
                    {

                        recoder.AppendLine("\r\n\r\n------------------------------------------error----------------------------------------------");
                        recoder.AppendLine($"\r\n    Time :\t\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        recoder.AppendLine($"\r\n    Lauguage :\t{compilation.Language} & {compilation.LanguageVersion}");
                        recoder.AppendLine($"\r\n    Target:\t\t{ClassNames[0]}");
                        recoder.AppendLine($"\r\n    Error:\t\t共{fileResult.Diagnostics.Length}处错误！");

                    }

                    foreach (var item in fileResult.Diagnostics)
                    {

                        if (NScriptLog.UseLog)
                        {

                            var temp = item.Location.GetLineSpan().StartLinePosition;
                            var result = GetErrorString(formatter, item.Location.GetLineSpan());
                            recoder.AppendLine($"\t\t第{temp.Line + 1}行，第{temp.Character}个字符：       内容【{result}】  {item.GetMessage()}");

                        }
                        errorAction?.Invoke(item);

                    }


                    recoder.AppendLine("\r\n---------------------------------------------------------------------------------------------");
                    NScriptLog.Error("Error : " + ClassNames[0], recoder.ToString());

                }

                formatContent = formatter;
                return null;
            }
            catch (Exception ex)
            {

                if (ex is IOException)
                {
                    if (!Directory.Exists(LibPath))
                    {
                        throw new Exception($"{LibPath}路径不存在，请重新运行程序！");
                    }

                    if (isDefaultContext)
                    {
                        int loop = 0;
                        while (!DynamicDlls.ContainsKey(path))
                        {

                            Thread.Sleep(200);
                            loop += 1;

                        }
                        NScriptLog.Warning(ClassNames[0], $"    I/O Delay :\t检测到争用，延迟{loop * 200}ms调用;\r\n");
                        return DynamicDlls[path];
                    }
                    
                }


                return null;

            }

        }


        public static string FormatLineCode(string content)
        {

            StringBuilder sb = new StringBuilder();
            var arrayLines = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < arrayLines.Length; i+=1)
            {

                sb.AppendLine($"{i+1}\t{arrayLines[i]}");

            }
            return sb.ToString();

        }




        public static string GetErrorString(string content, FileLinePositionSpan linePositionSpan)
        {

            var start = linePositionSpan.StartLinePosition;
            var end = linePositionSpan.EndLinePosition;


            var arrayLines = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            var currentErrorLine = arrayLines[start.Line];


            if (start.Line == end.Line)
            {

                if (start.Character == end.Character)
                {

                    return currentErrorLine.Substring(0, currentErrorLine.Length).Trim();

                }
                else
                {

                    return currentErrorLine.Substring(start.Character, end.Character - start.Character).Trim();

                }


            }
            else
            {

                StringBuilder builder = new StringBuilder();
                currentErrorLine.Substring(start.Character);
                builder.AppendLine(currentErrorLine);
                for (int i = start.Line ; i < end.Line - 1; i += 1)
                {

                    builder.AppendLine("\t\t\t" + arrayLines[i]);

                }


                currentErrorLine = arrayLines[end.Line];
                currentErrorLine = currentErrorLine.Substring(0, end.Character);
                builder.AppendLine(currentErrorLine);


                return builder.ToString();

            }

        }

    }
}
