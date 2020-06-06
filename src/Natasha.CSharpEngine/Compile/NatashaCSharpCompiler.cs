using Microsoft.CodeAnalysis;
using Natasha.CSharpCompiler;
using Natasha.Error.Model;
using Natasha.Framework;
using System.Collections.Concurrent;

namespace Natasha.CSharpEngine.Compile
{
    public class NatashaCSharpCompiler : CSharpCompilerBase
    {

        public readonly static ConcurrentDictionary<string, ReportDiagnostic> GlobalSuppressDiagnostics;
        static NatashaCSharpCompiler()
        {
            GlobalSuppressDiagnostics = new ConcurrentDictionary<string, ReportDiagnostic>();
            GlobalSuppressDiagnostics["CS1701"] = ReportDiagnostic.Suppress;
            GlobalSuppressDiagnostics["CS1702"] = ReportDiagnostic.Suppress;
            GlobalSuppressDiagnostics["CS1705"] = ReportDiagnostic.Suppress;
        }
        public NatashaCSharpCompiler()
        {

            AllowUnsafe = true;
            Enum_OutputKind = OutputKind.DynamicallyLinkedLibrary;
            Enum_OptimizationLevel = OptimizationLevel.Release;
            AssemblyOutputKind = AssemblyBuildKind.Stream;
            SuppressDiagnostics = GlobalSuppressDiagnostics;

        }

    }

}
