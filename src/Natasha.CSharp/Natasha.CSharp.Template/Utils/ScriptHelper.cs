using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;


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
    public static string GetClassName(string content, int namespaceIndex = 1, int classIndex = 1)
    {

        return GetDataStructString<ClassDeclarationSyntax>(content, namespaceIndex, classIndex);

    }




    /// <summary>
    /// 根据命名空间和结构体的位置获取类型
    /// </summary>
    /// <param name="content">脚本内容</param>
    /// <param name="structIndex">命名空间里的第index个结构体</param>
    /// <param name="namespaceIndex">第namespaceIndex个命名空间</param>
    /// <returns></returns>
    public static string GetStructName(string content, int namespaceIndex = 1, int structIndex = 1)
    {

        return GetDataStructString<StructDeclarationSyntax>(content, namespaceIndex, structIndex);

    }




    /// <summary>
    /// 根据命名空间和接口的位置获取类型
    /// </summary>
    /// <param name="content">脚本内容</param>
    /// <param name="interfaceIndex">命名空间里的第index个接口</param>
    /// <param name="namespaceIndex">第namespaceIndex个命名空间</param>
    /// <returns></returns>
    public static string GetInterfaceName(string content, int namespaceIndex = 1, int interfaceIndex = 1)
    {

        return GetDataStructString<InterfaceDeclarationSyntax>(content, namespaceIndex, interfaceIndex);

    }




    /// <summary>
    /// 根据命名空间和枚举的位置获取类型
    /// </summary>
    /// <param name="content">脚本内容</param>
    /// <param name="enumIndex">命名空间里的第index个枚举</param>
    /// <param name="namespaceIndex">第namespaceIndex个命名空间</param>
    /// <returns></returns>
    public static string GetEnumName(string content, int namespaceIndex = 1, int enumIndex = 1)
    {

        return GetDataStructString<EnumDeclarationSyntax>(content, namespaceIndex, enumIndex);

    }




    /// <summary>
    /// 根据命名空间和枚举的位置获取类型
    /// </summary>
    /// <param name="content">脚本内容</param>
    /// <param name="enumIndex">命名空间里的第index个枚举</param>
    /// <param name="namespaceIndex">第namespaceIndex个命名空间</param>
    /// <returns></returns>
    public static string GetMethodName(string content, int enumIndex = 1, int namespaceIndex = 1)
    {

        return GetDataMethodString(content, enumIndex, namespaceIndex);

    }




    public static SyntaxNode GetSpecialNode(string content, int namespaceIndex = 1)
    {

        namespaceIndex -= 1;


        var root = GetRoot(content);
        var result = GetNodes<NamespaceDeclarationSyntax>(root);


        SyntaxNode node;
        if (result.Count() != 0)
        {

            node = result.ToArray()[namespaceIndex];

        }
        else
        {

            node = root;

        }
        return node;
    }




    public static string GetDataStructString<T>(string content, int namespaceIndex = 1, int index = 1) where T : BaseTypeDeclarationSyntax
    {

        index -= 1;
        var nodes = GetNodes<T>(GetSpecialNode(content, namespaceIndex));
        if (nodes.Count() == 0)
        {
            return default;
        }
        return nodes.ToArray()[index].Identifier.Text;

    }




    public static string GetDataMethodString(string content, int index = 1, int namespaceIndex = 1)
    {

        index -= 1;
        var nodes = GetNodes<MethodDeclarationSyntax>(GetSpecialNode(content, namespaceIndex));
        if (nodes.Count() == 0)
        {
            return default;
        }
        return nodes.ToArray()[index].Identifier.Text;

    }

}

