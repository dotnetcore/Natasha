using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Natasha.CSharp.Extension.Inner;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;

namespace Natasha.CSharp.Compiler.Utils
{
    public static class NatashaPrivateAssemblySyntaxHelper
    {
        public static CompilationUnitSyntax? Handle(CompilationUnitSyntax rootSyntax, params object[] objects)
        {
            if (objects == null || objects.Length == 0)
            {
                return null;
            }
            HashSet<Type> types = new(objects.Select(obj =>
            {
                if (obj is Type type)
                {
                    return type;
                }
                else
                {
                    return obj.GetType();
                }
            })!);
            return InternalHandle(rootSyntax, types);
        }

        internal static CompilationUnitSyntax InternalHandle(CompilationUnitSyntax rootSyntax, HashSet<Type> types)
        {
            HashSet<Assembly> assemblies = [];
            foreach (var type in types)
            {
                var assembly = type.Assembly;
                if (!assemblies.Contains(assembly))
                {
                    assemblies.Add(assembly);
                }
            }

            // 创建 assembly 级别的属性列表
            var assemblyAttributeList = assemblies.Select(asm =>
              {
                  var attrSyntax = SyntaxFactory.AttributeList(
                          SyntaxFactory.SingletonSeparatedList(
                              SyntaxFactory.Attribute(
                                  SyntaxFactory
                                    .IdentifierName("assembly:System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute"))
                                    .AddArgumentListArguments(
                                        SyntaxFactory.AttributeArgument(
                                            SyntaxFactory.ParseExpression($"\"{asm.GetName().Name!}\"")))));
                  return attrSyntax
                  .ReplaceToken(
                      attrSyntax.CloseBracketToken,
                      attrSyntax
                            .CloseBracketToken
                            .WithTrailingTrivia(SyntaxFactory.EndOfLine(Environment.NewLine)));
              }).ToArray();


            // 将属性列表添加到 assembly 节点
            rootSyntax = rootSyntax.AddAttributeLists(assemblyAttributeList);
            return rootSyntax;
            //  BaseTypeDeclarationSyntax declaraNode = root
            //      .DescendantNodes()
            //      .OfType<ClassDeclarationSyntax>()
            //      .FirstOrDefault();

            //  declaraNode ??= root
            //          .DescendantNodes()
            //          .OfType<BaseTypeDeclarationSyntax>()
            //          .FirstOrDefault(item =>
            //          item.IsKind(SyntaxKind.ClassDeclaration) ||
            //          item.IsKind(SyntaxKind.StructDeclaration) ||
            //          item.IsKind(SyntaxKind.InterfaceDeclaration) ||
            //          item.IsKind(SyntaxKind.RecordDeclaration) ||
            //          item.IsKind(SyntaxKind.EnumDeclaration) ||
            //          item.IsKind(SyntaxKind.RecordStructDeclaration));

            //  var customAttribute = SyntaxFactory.Attribute(SyntaxFactory.IdentifierName(nameof(IgnoresAccessChecksToAttribute)))
            //.AddArgumentListArguments(
            //      SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("SomeValue"))) // 设置属性值
            //  );
            //  AttributeArgumentListSyntax newAttributeList = SyntaxFactory.att(attrScriptBuilder.ToString(), 0, tree.Options);
            //  declaraNode.AddAttributeLists(newAttributeList);


        }
    }
}
