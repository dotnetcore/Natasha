using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Natasha.CSharp.Extension.Inner
{
    internal static class SyntaxNodeExtension
    {

        //internal static CompilationUnitSyntax GetRoot(string content)
        //{

        //    SyntaxTree tree = CSharpSyntaxTree.ParseText(content, new CSharpParseOptions(LanguageVersion.Latest));
        //    return tree.GetCompilationUnitRoot();

        //}

        private static IEnumerable<T> GetNodes<T>(SyntaxNode node)
        {
            return node.DescendantNodes().OfType<T>();
        }




        /// <summary>
        /// 根据命名空间和类的位置获取类型
        /// </summary>
        /// <param name="namespaceNode">命名空间节点</param>
        /// <param name="index">命名空间里的第index-1个 类</param>
        /// <returns></returns>
        internal static string? GetClassName(this SyntaxNode namespaceNode, int index = 0)
        {

            return GetDataStructString<ClassDeclarationSyntax>(namespaceNode, index);

        }




        /// <summary>
        /// 根据命名空间和结构体的位置获取类型
        /// </summary>
        /// <param name="namespaceNode">命名空间节点</param>
        /// <param name="index">命名空间里的第index-1个 结构体</param>
        /// <returns></returns>
        internal static string? GetStructName(this SyntaxNode namespaceNode, int index = 0)
        {

            return GetDataStructString<StructDeclarationSyntax>(namespaceNode, index);

        }


        /// <summary>
        /// 根据命名空间和记录的位置获取类型
        /// </summary>
        /// <param name="namespaceNode">命名空间节点</param>
        /// <param name="index">命名空间里的第index-1个 Record</param>
        /// <returns></returns>
        internal static string? GetRecordName(this SyntaxNode namespaceNode, int index = 0)
        {

            return GetDataStructString<RecordDeclarationSyntax>(namespaceNode, index);

        }




        /// <summary>
        /// 根据命名空间和接口的位置获取类型
        /// </summary>
        /// <param name="namespaceNode">命名空间节点</param>
        /// <param name="index">命名空间里的第index-1个接口</param>
        /// <returns></returns>
        internal static string GetInterfaceName(this SyntaxNode namespaceNode, int index = 0)
        {

            return GetDataStructString<InterfaceDeclarationSyntax>(namespaceNode, index);

        }




        /// <summary>
        /// 根据命名空间和枚举的位置获取类型
        /// </summary>
        /// <param name="namespaceNode">命名空间节点</param>
        /// <param name="index">命名空间里的第index-1个枚举</param>
        /// <returns></returns>
        internal static string GetEnumName(this SyntaxNode namespaceNode, int index = 0)
        {

            return GetDataStructString<EnumDeclarationSyntax>(namespaceNode, index);

        }





        /// <summary>
        /// 获取命名空间
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="namespaceIndex"></param>
        /// <returns></returns>
        internal static SyntaxNode NamespaceNode(this SyntaxTree tree, int namespaceIndex = 0)
        {
            var root = tree.GetCompilationUnitRoot();
            var namespaceDeclarationSyntaxes = GetNodes<NamespaceDeclarationSyntax>(root);

            if (namespaceDeclarationSyntaxes.Any())
            {
                return namespaceDeclarationSyntaxes.ElementAt(namespaceIndex);
            }
            else
            {
                return root;
            }
        }



        private static string GetDataStructString<T>(SyntaxNode namespaceNode, int index = 0) where T : BaseTypeDeclarationSyntax
        {

            var nodes = GetNodes<T>(namespaceNode);
            var node = nodes.ElementAtOrDefault(index);
            if (node != null)
            {
                return node.Identifier.Text;
            }
            return string.Empty;

        }



        /// <summary>
        /// 根据命名空间和方法的位置获取类型
        /// </summary>
        /// <param name="namespaceNode">命名空间节点</param>
        /// <param name="index">命名空间里的第index-1个方法</param>
        internal static string GetMethodName(this SyntaxNode namespaceNode, int index = 0)
        {

            var nodes = GetNodes<MethodDeclarationSyntax>(namespaceNode);
            var node = nodes.ElementAtOrDefault(index);
            if (node != null)
            {
                return node.Identifier.Text;
            }
            return string.Empty;

        }


        internal static string GetFirstOopName(this SyntaxTree syntaxTree)
        {
            var node = syntaxTree.NamespaceNode();
            var result = node.GetClassName();
            if (string.IsNullOrEmpty(result))
            {
                result = node.GetStructName();
                if (string.IsNullOrEmpty(result))
                {
                    result = node.GetRecordName();
                    if (string.IsNullOrEmpty(result))
                    {
                        result = node.GetInterfaceName();
                        if (string.IsNullOrEmpty(result))
                        {
                            result = node.GetEnumName();
                            if (string.IsNullOrEmpty(result))
                            {
                                result = "Not found oop name!";
                            }
                        }
                    }
                }
            }
            return result!;
        }


    }
}

