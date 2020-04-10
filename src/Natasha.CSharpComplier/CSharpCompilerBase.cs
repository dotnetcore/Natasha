using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.Framework;

namespace Natasha.CSharpComplier
{
    public abstract class CSharpCompilerBase : CompilerBase<CSharpCompilation>
    {

        public CSharpCompilation Compilation;
        public CSharpCompilerBase()
        {
            Enum_OutputKind = OutputKind.DynamicallyLinkedLibrary;
            Enum_OptimizationLevel = OptimizationLevel.Release;
            AllowUnsafe = true;
        }


        public bool AllowUnsafe;
        public OutputKind Enum_OutputKind;
        public OptimizationLevel Enum_OptimizationLevel;


        public override CSharpCompilation GetCompilation()
        {

            return Compilation = CSharpCompilation.Create(
                               AssemblyName,
                               options: new CSharpCompilationOptions(
                                   outputKind: Enum_OutputKind,
                                   optimizationLevel: Enum_OptimizationLevel,
                                   allowUnsafe: AllowUnsafe),
                               syntaxTrees: CompileTrees,
                               references: Domain.GetCompileReferences());

        }
    }
}
