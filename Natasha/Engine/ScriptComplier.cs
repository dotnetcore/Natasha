using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Natasha
{

    public class ScriptComplier
    {
        public static string LibPath;
        public static ConcurrentDictionary<string,Assembly> DynamicDlls;
        public static ConcurrentBag<PortableExecutableReference> References;
        //public static bool IsComplete;

        static ScriptComplier()
        {
            // IsComplete = false;
            //初始化路径
            LibPath = AppDomain.CurrentDomain.BaseDirectory + "lib\\";
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

            //IsComplete = true;
        }


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
        /// <param name="index">命名空间里的第index个类</param>
        /// <param name="namespaceIndex">第x个命名空间</param>
        /// <returns></returns>
        public static string GetClassName(string content,int index=1,int namespaceIndex=1)
        {
            index -= 1;
            namespaceIndex -= 1;
            SyntaxTree tree = CSharpSyntaxTree.ParseText(content);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            MemberDeclarationSyntax firstMember = root.Members[namespaceIndex];
            var NameSpaceDeclaration = (NamespaceDeclarationSyntax)firstMember;
            var ClassDeclaration = (ClassDeclarationSyntax)NameSpaceDeclaration.Members[index];
            var identifier = (IdentifierNameSyntax)NameSpaceDeclaration.Name;
            return $"{identifier.Identifier.Text.Trim()}.{ClassDeclaration.Identifier.Text}";
        }

        /// <summary>
        /// 脚本编译，根据类名生成dll
        /// </summary>
        /// <param name="content">脚本内容</param>
        /// <param name="className">指定类名</param>
        /// <param name="errorAction">发生错误执行委托</param>
        /// <returns></returns>
        public static Assembly Complier(string content, string className = null, Action<string> errorAction = null)
        {

            //写入分析树
            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(content);

            //添加程序及引用


            //创建dll
            if (className == null)
            {
                className = GetClassName(content);
            }


            //创建语言编译
            CSharpCompilation compilation = CSharpCompilation.Create(
                className,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                syntaxTrees: new[] { tree },
                references: References);

            string path = $"{LibPath}{className}.dll";
            lock (content)
            {
                if (DynamicDlls.ContainsKey(path))
                {
                    return DynamicDlls[path];
                }
                var fileResult = compilation.Emit(path);

                if (fileResult.Success)
                {
                   
                    References.Add(MetadataReference.CreateFromFile(path));
                    //为了实现动态中的动态，使用文件加载模式常驻内存
                    var result = Assembly.LoadFrom(path);
                    DynamicDlls[path]= result;
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
