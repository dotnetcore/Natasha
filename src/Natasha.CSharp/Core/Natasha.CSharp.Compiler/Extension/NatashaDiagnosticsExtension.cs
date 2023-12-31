using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Natasha.CSharp
{
    public static class NatashaDiagnosticsExtension
    {
        public static SyntaxNode GetSyntaxNode(this Diagnostic diagnostic, CompilationUnitSyntax root)
        {
            return root.FindNode(diagnostic.Location.SourceSpan);
        }
        public static T? GetTypeSyntaxNode<T>(this Diagnostic diagnostic, CompilationUnitSyntax root) where T : class
        {
            var node = GetSyntaxNode(diagnostic, root);
            while (node is not T && node.Parent != null)
            {
                node = node!.Parent;
            }
            return node as T;
        }
        public static void RemoveDefaultUsingAndUsingNode(this Diagnostic diagnostic, CompilationUnitSyntax root, HashSet<SyntaxNode> removeCollection)
        {
            var usingNode = GetTypeSyntaxNode<UsingDirectiveSyntax>(diagnostic, root);
            if (usingNode != null)
            {
                RemoveUsingAndNode(usingNode, removeCollection);
            }
        }

        public static void RemoveUsingAndNode(this UsingDirectiveSyntax usingDirectiveSyntax, HashSet<SyntaxNode> removeCollection)
        {
            removeCollection.Add(usingDirectiveSyntax);
            var name = usingDirectiveSyntax.Name;
            if (name!=null)
            {
                NatashaReferenceDomain.DefaultDomain.UsingRecorder.Remove(name.ToString());
            }

        }

        public static void RemoveUsingAndNodesFromStartName(this Diagnostic diagnostic, CompilationUnitSyntax root, HashSet<SyntaxNode> removeCollection)
        {
            var usingNode = GetTypeSyntaxNode<UsingDirectiveSyntax>(diagnostic, root);
            if (usingNode!=null)
            {
                var usingNodes = (from usingDeclaration in root.Usings
                                  where usingDeclaration.Name != null && usingDeclaration.Name.ToString().StartsWith(usingNode.Name!.ToString())
                                  select usingDeclaration).ToList();

                removeCollection.UnionWith(usingNodes);
                NatashaReferenceDomain.DefaultDomain.UsingRecorder.Remove(usingNodes.Select(item => item.Name!.ToString()));
                
            }
        }

        public static NatashaReferenceDomain GetDomain(this Delegate @delegate)
        {

            return @delegate.Method.Module.Assembly.GetDomain();

        }



        public static void DisposeDomain(this Delegate @delegate)
        {

            @delegate.Method.Module.Assembly.DisposeDomain();

        }
    }
}


