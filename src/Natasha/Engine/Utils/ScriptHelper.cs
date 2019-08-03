using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Natasha
{
    public static class ScriptHelper
    {

        public static CompilationUnitSyntax GetRoot(string content)
        {

            SyntaxTree tree = CSharpSyntaxTree.ParseText(content, new CSharpParseOptions(LanguageVersion.Latest));
            return tree.GetCompilationUnitRoot();

        }


        public static IEnumerable<T> GetNodes<T>(SyntaxNode node)
        {

            return from namespaceNodes 
                   in node.DescendantNodes().OfType<T>()
                   select namespaceNodes;

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
            classIndex -= 1;
            namespaceIndex -= 1;


            var root = GetRoot(content);
            IEnumerable<SyntaxNode> result = GetNodes<NamespaceDeclarationSyntax>(root);


            SyntaxNode node = null;
            if (result.Count() != 0)
            {

                node = result.ToArray()[namespaceIndex];

            }
            else
            {

                node = root;

            }


            var classNodes = GetNodes<ClassDeclarationSyntax>(node);
            return classNodes.ToArray()[classIndex].Identifier.Text;

        }



        /// <summary>
        /// 根据命名空间和结构体的位置获取类型
        /// </summary>
        /// <param name="content">脚本内容</param>
        /// <param name="structIndex">命名空间里的第index个类</param>
        /// <param name="namespaceIndex">第namespaceIndex个命名空间</param>
        /// <returns></returns>
        public static string GetStructName(string content, int structIndex = 1, int namespaceIndex = 1)
        {
            structIndex -= 1;
            namespaceIndex -= 1;


            var root = GetRoot(content);
            IEnumerable<SyntaxNode> result = GetNodes<NamespaceDeclarationSyntax>(root);



            SyntaxNode node = null;
            if (result.Count() != 0)
            {

                node = result.ToArray()[namespaceIndex];

            }
            else
            {

                node = root;

            }


            var structNodes = GetNodes<StructDeclarationSyntax>(node);
            return structNodes.ToArray()[structIndex].Identifier.Text;

        }

    }
}
