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
            AddGlobalSupperess("CS1701");
            AddGlobalSupperess("CS1702");
            AddGlobalSupperess("CS1705");
            AddGlobalSupperess("CS162");
            AddGlobalSupperess("CS219");
            AddGlobalSupperess("CS0414");
            AddGlobalSupperess("CS0616");
            AddGlobalSupperess("CS0649");
            AddGlobalSupperess("CS0693");
            AddGlobalSupperess("CS1591");
            AddGlobalSupperess("CS1998");


            // CS0162 - Unreachable code detected.
            // CS0219 - The variable 'V' is assigned but its value is never used.
            // CS0414 - The private field 'F' is assigned but its value is never used.
            // CS0616 - Member is obsolete.
            // CS0649 - Field 'F' is never assigned to, and will always have its default value.
            // CS0693 - Type parameter 'type parameter' has the same name as the type parameter from outer type 'T'
            // CS1591 - Missing XML comment for publicly visible type or member 'Type_or_Member'
            // CS1998 - This async method lacks 'await' operators and will run synchronously


        }
        public NatashaCSharpCompiler()
        {

            AllowUnsafe = true;
            Enum_OutputKind = OutputKind.DynamicallyLinkedLibrary;
            Enum_OptimizationLevel = OptimizationLevel.Release;
            AssemblyOutputKind = AssemblyBuildKind.Stream;
            SuppressDiagnostics = GlobalSuppressDiagnostics;

        }

        public static void AddGlobalSupperess(string errorcode)
        {
            GlobalSuppressDiagnostics[errorcode] = ReportDiagnostic.Suppress;
        }

    }

}
