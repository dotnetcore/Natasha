using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Natasha.Complier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Natasha
{
    public static class RuntimeComplier
    {


        /// <summary>
        /// 编译脚本，生成委托
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="delegateType">委托类型</param>
        /// <returns></returns>
        public static Delegate GetDelegate(string className, string content, string methodName, Type delegateType)
        {

            //获取程序集
            Assembly assembly = ScriptComplieEngine.StreamComplier(content);


            //判空
            if (assembly == null)
            {

                return null;

            }


            //获取方法委托
            return AssemblyOperator
                .Loader(assembly)[className]
                .GetMethod(methodName)
                .CreateDelegate(delegateType);

        }




        /// <summary>
        /// 编译脚本，生成委托
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="delegateType">委托类型</param>
        /// <returns></returns>
        public static Delegate GetDelegate<T>(string className, string content, string methodName)
        {

            //获取程序集
            Assembly assembly = ScriptComplieEngine.StreamComplier(content);


            //判空
            if (assembly == null)
            {

                return null;

            }


            //获取方法委托
            return AssemblyOperator
                .Loader(assembly)[className]
                .GetMethod(methodName)
                .CreateDelegate(typeof(T));

        }




        /// <summary>
        /// 根据命名空间和类的位置获取类型
        /// </summary>
        /// <param name="content">脚本内容</param>
        /// <param name="classIndex">命名空间里的第index个类</param>
        /// <param name="namespaceIndex">第namespaceIndex个命名空间</param>
        /// <returns></returns>
        public static Type GetClassType(string content, int classIndex = 1, int namespaceIndex = 1)
        {

            //根据索引获取类名
            string className = GetClassName(content, classIndex, namespaceIndex);


            //获取程序集
            Assembly assembly = ScriptComplieEngine.FileComplier(content);


            //获取类型
            return AssemblyOperator.Loader(assembly)[className];

        }
        public static Type GetStructType(string content, int classIndex = 1, int namespaceIndex = 1)
        {

            //根据索引获取类名
            string structName = GetStructName(content, classIndex, namespaceIndex);
 

            //获取程序集
            Assembly assembly = ScriptComplieEngine.FileComplier(content);


            //获取类型
            return AssemblyOperator.Loader(assembly)[structName];

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


            SyntaxTree tree = CSharpSyntaxTree.ParseText(content);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();


            IEnumerable<SyntaxNode> result = from namespaceNodes
                         in root.DescendantNodes().OfType<NamespaceDeclarationSyntax>()
                                             select namespaceNodes;


            SyntaxNode node = null;
            if (result.Count() != 0)
            {

                node = result.ToArray()[namespaceIndex];

            }
            else
            {

                node = root;

            }


            var classResult = new List<string>(from classNodes
                               in node.DescendantNodes().OfType<ClassDeclarationSyntax>()
                                               select classNodes.Identifier.Text);


            classResult.AddRange(from classNodes
                 in node.DescendantNodes().OfType<StructDeclarationSyntax>()
                                 select classNodes.Identifier.Text);
            return classResult[classIndex];

        }



        /// <summary>
        /// 根据命名空间和结构体的位置获取类型
        /// </summary>
        /// <param name="content">脚本内容</param>
        /// <param name="classIndex">命名空间里的第index个类</param>
        /// <param name="namespaceIndex">第namespaceIndex个命名空间</param>
        /// <returns></returns>
        public static string GetStructName(string content, int classIndex = 1, int namespaceIndex = 1)
        {
            classIndex -= 1;
            namespaceIndex -= 1;


            SyntaxTree tree = CSharpSyntaxTree.ParseText(content);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();


            IEnumerable<SyntaxNode> result = from namespaceNodes
                         in root.DescendantNodes().OfType<NamespaceDeclarationSyntax>()
                                             select namespaceNodes;


            SyntaxNode node = null;
            if (result.Count() != 0)
            {

                node = result.ToArray()[namespaceIndex];

            }
            else
            {

                node = root;

            }


            var classResult = new List<string>(from classNodes
                               in node.DescendantNodes().OfType<StructDeclarationSyntax>()
                                               select classNodes.Identifier.Text);


            return classResult[classIndex];

        }

    }

}
