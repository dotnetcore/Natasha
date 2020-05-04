using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Natasha.Framework;
using System;
using System.IO;

namespace Natasha.CSharpCompiler
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



        public override EmitResult EmitToFile(CSharpCompilation compilation)
        {

            EmitResult CompileResult;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                CompileResult = compilation.Emit(DllPath, PdbPath);
            }
            else
            {
                CompileResult = compilation.UnixEmit(DllPath, PdbPath);
            }
            return CompileResult;

        }
        public override EmitResult EmitToStream(CSharpCompilation compilation, MemoryStream stream)
        {
            EmitResult CompileResult;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                CompileResult = compilation.Emit(stream);
            }
            else
            {
                CompileResult = compilation.Emit(stream, options: new EmitOptions(false, DebugInformationFormat.PortablePdb));
            }
            return CompileResult;
        }
    }
}
