using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Natasha.Complier
{
    /// <summary>
    /// 核心编译引擎
    /// </summary>
    public class ScriptComplier
    {
        public readonly static string LibPath;
        public readonly static ConcurrentDictionary<string, Assembly> DynamicDlls;
        public readonly static ConcurrentBag<PortableExecutableReference> References;

        static ScriptComplier()
        {
            //初始化路径
            LibPath = AppDomain.CurrentDomain.BaseDirectory + "lib\\";


            //处理目录
            if (Directory.Exists(LibPath))
            {
                Directory.Delete(LibPath, true);
            }
            Directory.CreateDirectory(LibPath);


            //程序集缓存
            var _ref = DependencyContext.Default.CompileLibraries
                                .SelectMany(cl => cl.ResolveReferencePaths())
                                .Select(asm => MetadataReference.CreateFromFile(asm));
            DynamicDlls = new ConcurrentDictionary<string, Assembly>();
            References = new ConcurrentBag<PortableExecutableReference>(_ref);
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




        /// <summary>
        /// 根据命名空间和类的位置获取类型
        /// </summary>
        /// <param name="content">脚本内容</param>
        /// <param name="classIndex">命名空间里的第index个类</param>
        /// <param name="namespaceIndex">第namespaceIndex个命名空间</param>
        /// <returns></returns>
        public static string GetClassName(string content, int classIndex = 1, int namespaceIndex = 1)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(content);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            MemberDeclarationSyntax firstMember = root.Members[namespaceIndex];
            var NameSpaceDeclaration = (NamespaceDeclarationSyntax)firstMember;
            var ClassDeclaration = (ClassDeclarationSyntax)NameSpaceDeclaration.Members[classIndex];
            var identifier = (IdentifierNameSyntax)NameSpaceDeclaration.Name;
            return $"{identifier.Identifier.Text.Trim()}.{ClassDeclaration.Identifier.Text}";
        }




        /// <summary>
        /// 使用内存流进行脚本编译
        /// </summary>
        /// <param name="content">脚本内容</param>
        /// <param name="className">指定类名</param>
        /// <param name="errorAction">发生错误执行委托</param>
        /// <returns></returns>
        public static Assembly StreamComplier(string content, string className = null, Action<Diagnostic> errorAction = null)
        {
#if DEBUG
            StringBuilder recoder = new StringBuilder(LineFormat(ref content));
#endif


            //写入分析树
            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(content);


            //创建语言编译
            CSharpCompilation compilation = CSharpCompilation.Create(
                className,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                syntaxTrees: new[] { tree },
                references: References);


            //编译并生成程序集
            using (MemoryStream stream = new MemoryStream())
            {
                var fileResult = compilation.Emit(stream);
                if (fileResult.Success)
                {
                    var result = Assembly.Load(stream.GetBuffer());
#if DEBUG
                    recoder.AppendLine("\r\n\r\n------------------------------------------succeed-------------------------------------------");
                    recoder.AppendLine($"\r\n    Lauguage :\t{compilation.Language}___{compilation.LanguageVersion}");
                    recoder.AppendLine($"\r\n    Target :\t\t{className}");
                    recoder.AppendLine($"\r\n    Size :\t\t{stream.Length}");
                    recoder.AppendLine($"\r\n    Assembly : \t{result.FullName}");
                    recoder.AppendLine("\r\n----------------------------------------------------------------------------------------------");
                    NDebug.Succeed("Succeed : " + className, recoder.ToString());
#endif

                    return result;

                }
                else
                {
#if DEBUG
                    recoder.AppendLine("\r\n\r\n------------------------------------------error----------------------------------------------");
                    recoder.AppendLine($"\r\n    Lauguage :\t{compilation.Language}___{compilation.LanguageVersion}");
                    recoder.AppendLine($"\r\n    Target:\t\t{className}");
                    recoder.AppendLine($"\r\n    Error:\t\t共{fileResult.Diagnostics.Length}处错误！");
#endif

                    //错误处理
                    foreach (var item in fileResult.Diagnostics)
                    {
#if DEBUG
                        var temp = item.Location.GetLineSpan().StartLinePosition;
                        var result = GetErrorString(content, item.Location.GetLineSpan());
                        recoder.AppendLine($"\t\t第{temp.Line}行，第{temp.Character}个字符：       内容【{result}】  {item.GetMessage()}");
#endif
                        errorAction?.Invoke(item);
                    }
#if DEBUG
                    recoder.AppendLine("\r\n---------------------------------------------------------------------------------------------");
                    NDebug.Error("Error : " + className, recoder.ToString());
#endif

                }
            }


            return null;
        }




        /// <summary>
        /// 使用文件流进行脚本编译，根据类名生成dll
        /// </summary>
        /// <param name="content">脚本内容</param>
        /// <param name="className">指定类名</param>
        /// <param name="errorAction">发生错误执行委托</param>
        /// <returns></returns>
        public static Assembly FileComplier(string content, string className = null, Action<Diagnostic> errorAction = null)
        {
#if DEBUG
            StringBuilder recoder = new StringBuilder(LineFormat(ref content));
#endif
            //类名获取
            if (className == null)
            {
                className = GetClassName(content);
            }

            string path = $"{LibPath}{className}.dll";

            //生成路径



            if (DynamicDlls.ContainsKey(path))
            {
                return DynamicDlls[path];
            }


            //写入分析树
            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(content);

            //创建语言编译
            CSharpCompilation compilation = CSharpCompilation.Create(
                className,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                syntaxTrees: new[] { tree },
                references: References);


            //编译到文件
            EmitResult fileResult = null;
            try
            {
                fileResult = compilation.Emit(path);
            }
            catch (Exception ex)
            {
                if (ex is IOException)
                {
                    int loop = 0;
                    while (!DynamicDlls.ContainsKey(path))
                    {
                        Thread.Sleep(200);
                        loop += 1;
                    }

                    NDebug.Warning(className, $"    I/O Delay :\t检测到争用，延迟{loop*200}ms调用;\r\n");

                    return DynamicDlls[path];
                }
                return null;
            }

            if (fileResult.Success)
            {
                References.Add(MetadataReference.CreateFromFile(path));
                //为了实现动态中的动态，使用文件加载模式常驻内存
                var result = Assembly.LoadFrom(path);


#if DEBUG
                recoder.AppendLine("\r\n\r\n------------------------------------------succeed-------------------------------------------");
                recoder.AppendLine($"\r\n    Lauguage :\t{compilation.Language}___{compilation.LanguageVersion}");
                recoder.AppendLine($"\r\n    Target :\t\t{className}");
                recoder.AppendLine($"\r\n    Path :\t\t{path}");
                recoder.AppendLine($"\r\n    Assembly : \t{result.FullName}");
                recoder.AppendLine("\r\n----------------------------------------------------------------------------------------------");
                NDebug.Succeed("Succeed : " + className, recoder.ToString());
#endif


                DynamicDlls[path] = result;
                return result;
            }
            else
            {

#if DEBUG
                recoder.AppendLine("\r\n\r\n------------------------------------------error----------------------------------------------");
                recoder.AppendLine($"\r\n    Lauguage :\t{compilation.Language}___{compilation.LanguageVersion}");
                recoder.AppendLine($"\r\n    Target:\t\t{className}");
                recoder.AppendLine($"\r\n    Error:\t\t共{fileResult.Diagnostics.Length}处错误！");
#endif

                foreach (var item in fileResult.Diagnostics)
                {
#if DEBUG
                    var temp = item.Location.GetLineSpan().StartLinePosition;
                    var result = GetErrorString(content, item.Location.GetLineSpan());
                    recoder.AppendLine($"\t\t第{temp.Line}行，第{temp.Character}个字符：       内容【{result}】  {item.GetMessage()}");
#endif
                    errorAction?.Invoke(item);
                }


#if DEBUG
                recoder.AppendLine("\r\n---------------------------------------------------------------------------------------------");
                NDebug.Error("Error : " + className, recoder.ToString());
#endif

            }
            return null;
        }

#if DEBUG

        public static void AddTab(ref string content, string value)
        {
            content = content.Replace(value, value + "\r\n");
        }
        public static string LineFormat(ref string content)
        {
            content = content.Replace("\r\n", "");
            AddTab(ref content, "{");
            AddTab(ref content, "}");
            AddTab(ref content, ";");
            StringBuilder builder = new StringBuilder();
            var arrayLines = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < arrayLines.Length; i++)
            {
                builder.AppendLine($"{i}:\t{arrayLines[i]}");
            }
            return builder.ToString();
        }
        public static string GetErrorString(string content, FileLinePositionSpan linePositionSpan)
        {
            var start = linePositionSpan.StartLinePosition;
            var end = linePositionSpan.EndLinePosition;
            if (start.Line == end.Line
                && start.Character == end.Character)
            {
                var arrayLines = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                var currentErrorLine = arrayLines[start.Line];
                return currentErrorLine.Substring(0, start.Character - 1).Trim();
            }
            else
            {
                if (start.Line == end.Line)
                {
                    var arrayLines = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    var currentErrorLine = arrayLines[start.Line];
                    var endPosition = currentErrorLine.IndexOf(';', start.Character);
                    if (endPosition == -1)
                    {
                        return currentErrorLine.Substring(start.Character).Replace("\r\n", "").Trim();
                    }
                    return currentErrorLine.Substring(start.Character, endPosition - start.Character + 1).Replace("\r\n", "").Trim();
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    var arrayLines = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    var currentErrorLine = arrayLines[start.Line];
                    currentErrorLine.Substring(start.Character).Trim();
                    builder.AppendLine(currentErrorLine);
                    for (int i = start.Line + 1; i < end.Line; i += 1)
                    {
                        builder.AppendLine("\t\t\t" + arrayLines[i].Trim());
                    }
                    currentErrorLine = arrayLines[end.Line];
                    currentErrorLine = currentErrorLine.Substring(0, end.Character).Trim();
                    builder.AppendLine(currentErrorLine);
                    return builder.ToString();
                }
            }

        }
#endif

    }
}
