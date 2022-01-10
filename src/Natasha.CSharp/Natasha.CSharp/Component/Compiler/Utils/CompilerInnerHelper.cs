using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Reflection;

namespace Natasha.CSharp.Component.Compiler.Utils
{
    internal static class CompilerInnerHelper
    {
        internal static readonly Action<CSharpCompilationOptions, uint> SetTopLevelBinderFlagDelegate;
        internal static readonly Action<CSharpCompilationOptions, bool> SetReferencesSupersedeLowerVersionsDelegate;

        static CompilerInnerHelper()
        {
            SetTopLevelBinderFlagDelegate = (Action<CSharpCompilationOptions, uint>)Delegate.CreateDelegate(
               typeof(Action<CSharpCompilationOptions, uint>), typeof(CSharpCompilationOptions)
               .GetProperty("TopLevelBinderFlags", BindingFlags.Instance | BindingFlags.NonPublic)!
               .SetMethod!);

            SetReferencesSupersedeLowerVersionsDelegate = (Action<CompilationOptions, bool>)Delegate.CreateDelegate(
                typeof(Action<CompilationOptions, bool>), typeof(CompilationOptions)
                .GetProperty("ReferencesSupersedeLowerVersions", BindingFlags.Instance | BindingFlags.NonPublic)!
                .SetMethod!);
        }
    }
}
