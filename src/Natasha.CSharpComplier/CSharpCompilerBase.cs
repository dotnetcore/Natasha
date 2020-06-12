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
        public Platform Enum_Platform;
        public OptimizationLevel Enum_OptimizationLevel;
        public ConcurrentDictionary<string, ReportDiagnostic> SuppressDiagnostics;
        public static readonly Action<CSharpCompilationOptions,uint> SetTopLevelBinderFlagDelegate;
        public const uint _allowPrivateFlag = (uint)1 << 22;
        public CSharpCompilation Compilation;


        static CSharpCompilerBase()
        {
            SetTopLevelBinderFlagDelegate = (Action<CSharpCompilationOptions,uint>)Delegate.CreateDelegate(
                typeof(Action<CSharpCompilationOptions,uint>), typeof(CSharpCompilationOptions)
                .GetProperty("TopLevelBinderFlags", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetMethod);
        }
        public CSharpCompilerBase()
        {

            Enum_Platform = Platform.AnyCpu;
            SuppressDiagnostics = new ConcurrentDictionary<string, ReportDiagnostic>();

        }


        public override CSharpCompilation GetCompilation()
        {
            //..ComparisonResult = AssemblyIdentityComparer.ComparisonResult.EquivalentIgnoringVersion;
            //var a = new AssemblyIdentityComparer();
            var compilationOptions = new CSharpCompilationOptions(
                                   concurrentBuild: true,
                                   metadataImportOptions: MetadataImportOptions.All,
                                   outputKind: Enum_OutputKind,
                                   optimizationLevel: Enum_OptimizationLevel,
                                   allowUnsafe: AllowUnsafe,
                                   platform: Enum_Platform,
                                   checkOverflow:false,
                                   assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default,
                                   strongNameProvider: new DesktopStrongNameProvider(),
                                   specificDiagnosticOptions: SuppressDiagnostics);
            SetTopLevelBinderFlagDelegate(compilationOptions, _allowPrivateFlag);
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
