using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha.CSharp.Component.Compiler.Utils
{
    internal static class CompilerHelper
    {
        public static readonly Action<CSharpCompilationOptions, uint> SetTopLevelBinderFlagDelegate;
        public static readonly Action<CSharpCompilationOptions, bool> SetReferencesSupersedeLowerVersionsDelegate;

        static CompilerHelper()
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
