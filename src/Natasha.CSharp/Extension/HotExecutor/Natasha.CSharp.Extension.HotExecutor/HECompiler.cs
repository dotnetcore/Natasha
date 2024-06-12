using Microsoft.CodeAnalysis;
using Natasha.CSharp.Compiler;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha.CSharp.Extension.HotExecutor
{
    internal static class HECompiler
    {
        private static readonly AssemblyCSharpBuilder _builderCache;
        static HECompiler()
        {
            _builderCache = new();
            _builderCache.ConfigCompilerOption(opt => opt
            .AppendCompilerFlag(
            CompilerBinderFlags.IgnoreAccessibility | CompilerBinderFlags.IgnoreCorLibraryDuplicatedTypes | CompilerBinderFlags.GenericConstraintsClause | CompilerBinderFlags.SuppressObsoleteChecks));
            _builderCache
                .UseRandomLoadContext()
                .UseSmartMode()
                .WithoutSemanticCheck()
                .WithPreCompilationOptions()
                .WithoutPreCompilationReferences()
                .WithoutCombineUsingCode();
        }

        public static Assembly ReCompile(IEnumerable<SyntaxTree> trees,bool isRelease)
        {
            _builderCache.WithRandomAssenblyName();
            _builderCache.SyntaxTrees.Clear();
            _builderCache.SyntaxTrees.AddRange(trees);
            if (isRelease)
            {
                _builderCache.WithReleaseCompile();
            }
            else
            {
                _builderCache.WithDebugCompile();
            }
            return _builderCache.GetAssembly();
        }
    }
}
