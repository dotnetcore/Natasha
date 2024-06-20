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
        private static readonly NatashaUsingCache? _usingCache;
        private static readonly HashSet<MetadataReference> _references;
        static HECompiler()
        {
            ProjectDynamicProxy.CompileInitAction();
            if (VSCSharpProjectInfomation.EnableImplicitUsings)
	        {
                _usingCache = new();
                _usingCache.Using(NatashaLoadContext.DefaultContext.UsingRecorder._usings);
                
            }
            _references =[..NatashaLoadContext.DefaultContext.ReferenceRecorder.GetReferences()];
            var delReference = NatashaLoadContext.DefaultContext.ReferenceRecorder.GetSingleReference(Assembly.GetEntryAssembly().GetName());
            if (delReference != null)
            {
               // _references.Remove(delReference);
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
                .UseRandomLoadContext()
                .UseSmartMode()
                .WithoutSemanticCheck()
                .WithPreCompilationOptions()
                .WithoutPreCompilationReferences()
                .WithoutCombineUsingCode();
        }
        public static void RemoveUsings(IEnumerable<string> usings)
        {
            _usingCache!.Remove(usings);
        }
        public static UsingDirectiveSyntax[] GetDefaultUsingNodes()
        {
            return _usingCache!.GetUsingNodes().ToArray();
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
