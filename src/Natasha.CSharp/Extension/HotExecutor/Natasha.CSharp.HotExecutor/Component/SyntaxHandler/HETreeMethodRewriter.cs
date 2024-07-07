using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace Natasha.CSharp.HotExecutor.Component
{
    internal class HETreeMethodRewriter : CSharpSyntaxRewriter
    {
        private readonly ConcurrentDictionary<string, HashSet<MethodSyntaxPluginBase>> _methodPlugins;
        private readonly ConcurrentDictionary<string, HashSet<TriviaSyntaxPluginBase>> _methodTriviaPlugins;
        public HETreeMethodRewriter()
        {
            _methodPlugins = [];
            _methodTriviaPlugins = [];
        }

        public HETreeMethodRewriter RegistePlugin(MethodSyntaxPluginBase methodSyntaxPlugin)
        {
            if (_methodPlugins.TryGetValue(methodSyntaxPlugin.MethodName, out var methodPlugins))
            {
                methodPlugins.Add(methodSyntaxPlugin);
            }
            else
            {
                _methodPlugins[methodSyntaxPlugin.MethodName] = [methodSyntaxPlugin];
            }
            if (_methodTriviaPlugins.TryGetValue(methodSyntaxPlugin.MethodName, out var triviaPlugins))
            {

                if (methodSyntaxPlugin.TriviaSyntaxPluginBases.Count>0)
                {
                    triviaPlugins.UnionWith(methodSyntaxPlugin.TriviaSyntaxPluginBases);
                }
                
            }
            else
            {
                _methodTriviaPlugins[methodSyntaxPlugin.MethodName] = new(methodSyntaxPlugin.TriviaSyntaxPluginBases);
            
            }
            return this;
        }
        public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax methodNode)
        {
            var name = methodNode.Identifier.Text;
            if (_methodTriviaPlugins.TryGetValue(name,out var triviaPlugins))
            {
                foreach (var plugin in triviaPlugins)
                {
                    plugin.Initialize();
                }
                var methodTriviaRewriter = new HEMethodTriviaRewriter(triviaPlugins);
                methodTriviaRewriter.Visit(methodNode);
            }
            if (_methodPlugins.TryGetValue(name, out var plugins))
            {
                var body = methodNode.Body;
                foreach (var plugin in plugins)
                {
                    var result = plugin.Handle(body);
                    if (result != null)
                    {
                        body = result;
                    }
                }
                if (body != methodNode.Body)
                {
                    var node = methodNode.WithBody(body);
                    Console.WriteLine(node.ToFullString());
                    return methodNode.WithBody(body);
                }
            }
            return base.VisitMethodDeclaration(methodNode);
        }

    }

}
