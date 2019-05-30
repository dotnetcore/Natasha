using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;

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
        public static Assembly StreamComplier(string content, string className = null, Action<string> errorAction = null)
        {

            //写入分析树
            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(content);


            //创建语言编译
            CSharpCompilation compilation = CSharpCompilation.Create(
                className,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                syntaxTrees: new[] { tree },
                references: References);


            //锁住内容
            lock (content)
            {
                //编译并生成程序集
                using (MemoryStream stream = new MemoryStream())
                {
                    var fileResult = compilation.Emit(stream);
                    if (fileResult.Success)
                    {
                        var result = Assembly.Load(stream.GetBuffer());
                        return result;

                    }
                    else
                    {
                        //错误处理
                        foreach (var item in fileResult.Diagnostics)
                        {
                            errorAction?.Invoke(item.GetMessage());
                        }
                    }
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
        public static Assembly FileComplier(string content, string className = null, Action<string> errorAction = null)
        {

            //写入分析树
            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(content);


            //类名获取
            if (className == null)
            {
                className = GetClassName(content);
            }


            //生成路径
            string path = $"{LibPath}{className}.dll";
            if (DynamicDlls.ContainsKey(path))
            {
                return DynamicDlls[path];
            }


            //创建语言编译
            CSharpCompilation compilation = CSharpCompilation.Create(
                className,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                syntaxTrees: new[] { tree },
                references: References);


            //锁住内容
            lock (content)
            {
                //再次检查缓存，解决并发问题
                if (DynamicDlls.ContainsKey(path))
                {
                    return DynamicDlls[path];
                }

                //编译到文件
                var fileResult = compilation.Emit(path);
                if (fileResult.Success)
                {

                    References.Add(MetadataReference.CreateFromFile(path));
                    //为了实现动态中的动态，使用文件加载模式常驻内存
                    var result = Assembly.LoadFrom(path);
                    DynamicDlls[path] = result;
                    return result;
                }
                else
                {
                    foreach (var item in fileResult.Diagnostics)
                    {
                        errorAction?.Invoke(item.GetMessage());
                    }
                }
            }
            return null;
        }
    }
}
