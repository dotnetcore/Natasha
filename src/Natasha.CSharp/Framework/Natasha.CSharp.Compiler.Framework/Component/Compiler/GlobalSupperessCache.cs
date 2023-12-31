using Microsoft.CodeAnalysis;
using System.Collections.Concurrent;

namespace Natasha.CSharp.Component.Compiler
{
    internal static class GlobalSupperessCache
    {
        internal static readonly ConcurrentDictionary<string, ReportDiagnostic> _globalSuppressDiagnostics;
        static GlobalSupperessCache()
        {
            _globalSuppressDiagnostics = new();
            AddGlobalSupperess("CA1050");
            AddGlobalSupperess("CA1822");
            AddGlobalSupperess("CS1701");
            AddGlobalSupperess("CS1702");
            AddGlobalSupperess("CS1705");
            AddGlobalSupperess("CS2008");
            AddGlobalSupperess("CS162");
            AddGlobalSupperess("CS0219");
            AddGlobalSupperess("CS0414");
            AddGlobalSupperess("CS0616");
            AddGlobalSupperess("CS0649");
            AddGlobalSupperess("CS0693");
            AddGlobalSupperess("CS1591");
            AddGlobalSupperess("CS1998");
            //AddGlobalSupperess("RS1014");
            //AddGlobalSupperess("CA1822");
            //AddGlobalSupperess("CS8604");
            // CS8019
            // CS0162 - Unreachable code detected.
            // CS0219 - The variable 'V' is assigned but its value is never used.
            // CS0414 - The private field 'F' is assigned but its value is never used.
            // CS0616 - Member is obsolete.
            // CS0649 - Field 'F' is never assigned to, and will always have its default value.
            // CS0693 - Type parameter 'type parameter' has the same name as the type parameter from outer type 'T'
            // CS1591 - Missing XML comment for publicly visible type or member 'Type_or_Member'
            // CS1998 - This async method lacks 'await' operators and will run synchronously

        }
        public static void AddGlobalSupperess(string errorcode)
        {
            _globalSuppressDiagnostics[errorcode] = ReportDiagnostic.Suppress;
        }
    }
}
