using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.Extensions.DependencyModel;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Natasha
{

    public class ScriptComplier
    {
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
        public static Assembly Complier(string content,string className = null,Action<string> errorAction=null)
        {
            //写入分析树
            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(content);

            //添加程序及引用
            var _ref = DependencyContext.Default.CompileLibraries
                                    .SelectMany(cl => cl.ResolveReferencePaths())
                                    .Select(asm => MetadataReference.CreateFromFile(asm));


            //创建dll
            if (className==null)
            {
                className = GetClassName(content);
            }


            //创建语言编译
            CSharpCompilation compilation = CSharpCompilation.Create(
                className,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                syntaxTrees: new[] { tree },
                references: _ref);


            Assembly compiledAssembly=null;
            using (MemoryStream stream = new MemoryStream())
            {
                EmitResult compileResult = compilation.Emit(stream);

                if (compileResult.Success)
                {
                    //从内存中加载程序集
                    compiledAssembly = Assembly.Load(stream.GetBuffer());

                }
                else
                {
                    foreach (var item in compileResult.Diagnostics)
                    {
                        errorAction?.Invoke(item.GetMessage());
                    }
                    //throw new Exception("你的.csproj文件里，需要有：<PreserveCompilationContext>true</PreserveCompilationContext>")
                }
            }
            return compiledAssembly;
        }
    }
}
