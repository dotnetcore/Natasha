using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Natasha
{

    public class ScriptComplier
    {
        public static string LibPath;
        public static ConcurrentBag<string> DynamicDlls;
        public static ConcurrentBag<PortableExecutableReference> References;
        static ScriptComplier()
        {
            LibPath = AppDomain.CurrentDomain.BaseDirectory + "lib\\";
            var _ref = DependencyContext.Default.CompileLibraries
                                .SelectMany(cl => cl.ResolveReferencePaths())
                                .Select(asm => MetadataReference.CreateFromFile(asm));


            DynamicDlls = new ConcurrentBag<string>();
            References = new ConcurrentBag<PortableExecutableReference>(_ref);

            if (Directory.Exists(LibPath))
            {
                Directory.Delete(LibPath, true);
            }
            Directory.CreateDirectory(LibPath);
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
        /// 通过语法解析获取脚本内容中的第一个类
        /// </summary>
        /// <param name="content">脚本内容</param>
        /// <returns></returns>
        public static string GetClassName(string content)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(content);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            MemberDeclarationSyntax firstMember = root.Members[0];
            var NameSpaceDeclaration = (NamespaceDeclarationSyntax)firstMember;
            var ClassDeclaration = (ClassDeclarationSyntax)NameSpaceDeclaration.Members[0];
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
            var fileResult = compilation.Emit(path);
            
            if (fileResult.Success)
            {

                DynamicDlls.Add(path);
                References.Add(MetadataReference.CreateFromFile(path));

                //为了实现动态中的动态，使用文件加载模式常驻内存
                return Assembly.LoadFrom(path);
            }
            else
            {
                foreach (var item in fileResult.Diagnostics)
                {
                    errorAction?.Invoke(item.GetMessage());
                }
            }

            return null;
            
        }
    }
}
