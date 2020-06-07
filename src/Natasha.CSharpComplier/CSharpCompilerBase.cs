using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Natasha.Framework;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;

namespace Natasha.CSharpCompiler
{
    public abstract class CSharpCompilerBase : CompilerBase<CSharpCompilation>
    {

        public bool AllowUnsafe;
        public OutputKind Enum_OutputKind;
        public OptimizationLevel Enum_OptimizationLevel;
        public ConcurrentDictionary<string, ReportDiagnostic> SuppressDiagnostics;
        public static readonly PropertyInfo SetTopLevelBinderFlagDelegate;
        public const uint _allowPrivateFlag = (uint)1 << 22;
        public CSharpCompilation Compilation;


        static CSharpCompilerBase()
        {
            SetTopLevelBinderFlagDelegate = typeof(CSharpCompilationOptions).GetProperty("TopLevelBinderFlags", BindingFlags.Instance | BindingFlags.NonPublic);
        }
        public CSharpCompilerBase()
        {

            SuppressDiagnostics = new ConcurrentDictionary<string, ReportDiagnostic>();

        }


        public override CSharpCompilation GetCompilation()
        {

            var compilationOptions = new CSharpCompilationOptions(
                                   metadataImportOptions: MetadataImportOptions.All,
                                   outputKind: Enum_OutputKind,
                                   optimizationLevel: Enum_OptimizationLevel,
                                   allowUnsafe: AllowUnsafe,
                                   specificDiagnosticOptions: SuppressDiagnostics);
            SetTopLevelBinderFlagDelegate.SetValue(compilationOptions, _allowPrivateFlag);
            Compilation = CSharpCompilation.Create(AssemblyName, CompileTrees, Domain.GetCompileReferences(), compilationOptions);
            return Compilation;

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
