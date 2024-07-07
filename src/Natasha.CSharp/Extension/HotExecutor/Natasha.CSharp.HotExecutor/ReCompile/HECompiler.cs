using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Natasha.CSharp.Compiler;
using Natasha.CSharp.Compiler.Component;
using System.Reflection;

namespace Natasha.CSharp.Extension.HotExecutor
{
    internal static class HECompiler
    {
        private static readonly AssemblyCSharpBuilder _builderCache;
        private static readonly HashSet<MetadataReference> _references;
        static HECompiler()
        {
            _references =[..NatashaLoadContext.DefaultContext.ReferenceRecorder.GetReferences()];
            var delReference = NatashaLoadContext.DefaultContext.ReferenceRecorder.GetSingleReference(Assembly.GetEntryAssembly().GetName());
            if (delReference != null)
            {
                _references.Remove(delReference);
            }
            _builderCache = new();
            _builderCache.WithSpecifiedReferences(_references);
            _builderCache.ConfigCompilerOption(opt => opt
                .WithLowerVersionsAssembly()
                .AppendCompilerFlag(
                    CompilerBinderFlags.IgnoreAccessibility  |
                    CompilerBinderFlags.IgnoreCorLibraryDuplicatedTypes | 
                    CompilerBinderFlags.GenericConstraintsClause | 
                    CompilerBinderFlags.SuppressObsoleteChecks));
            _builderCache
                .UseSmartMode()
                .WithFileOutput()
                .WithoutSemanticCheck()
                .WithPreCompilationOptions()
                .WithoutPreCompilationReferences()
                .WithoutCombineUsingCode();
        }

        public static Task<Assembly> ReCompile(IEnumerable<SyntaxTree> trees,bool isRelease)
        {
            _builderCache.WithRandomAssenblyName();
            _builderCache.UseRandomLoadContext();
            _builderCache.SyntaxTrees.Clear();
            _builderCache.SyntaxTrees.AddRange(trees);

            if (isRelease)
            {
                _builderCache.WithReleasePlusCompile();
            }
            else
            {
                _builderCache.WithDebugPlusCompile(debugger=>debugger.ForAssembly());
            }
            return Task.FromResult(_builderCache.GetAssembly());
        }
    }
}
