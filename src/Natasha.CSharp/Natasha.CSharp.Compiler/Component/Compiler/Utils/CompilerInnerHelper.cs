using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Reflection;

namespace Natasha.CSharp.Compiler.Component
{
    internal static class CompilerInnerHelper
    {
        internal static readonly Action<CSharpCompilationOptions, uint> SetTopLevelBinderFlagDelegate;
        internal static readonly Action<CSharpCompilationOptions, bool> SetReferencesSupersedeLowerVersionsDelegate;
        internal static readonly Action<CSharpCompilationOptions, bool> SetDebugPlusModeDelegate;

        static CompilerInnerHelper()
        {
            SetTopLevelBinderFlagDelegate = (Action<CSharpCompilationOptions, uint>)Delegate.CreateDelegate(
               typeof(Action<CSharpCompilationOptions, uint>), typeof(CSharpCompilationOptions)
               .GetProperty("TopLevelBinderFlags", BindingFlags.Instance | BindingFlags.NonPublic)!
               .SetMethod!);

            SetReferencesSupersedeLowerVersionsDelegate = (Action<CSharpCompilationOptions, bool>)Delegate.CreateDelegate(
                typeof(Action<CSharpCompilationOptions, bool>), typeof(CSharpCompilationOptions)
                .GetProperty("ReferencesSupersedeLowerVersions", BindingFlags.Instance | BindingFlags.NonPublic)!
                .SetMethod!);

            SetDebugPlusModeDelegate = (Action<CSharpCompilationOptions, bool>)Delegate.CreateDelegate(
                typeof(Action<CSharpCompilationOptions, bool>), typeof(CSharpCompilationOptions)
                .GetProperty("DebugPlusMode", BindingFlags.Instance | BindingFlags.NonPublic)!
                .SetMethod!);

            //var list = typeof(CSharpCompilationOptions).GetProperties(BindingFlags.Instance | BindingFlags.NonPublic);

            //SetEnableEditAndContinueDelegate = (Action<CompilationOptions, bool>)Delegate.CreateDelegate(
            //    typeof(Action<CompilationOptions, bool>), typeof(CSharpCompilationOptions)
            //    .GetProperty("EnableEditAndContinue", BindingFlags.Instance | BindingFlags.NonPublic)!
            //    .SetMethod!);
        }
    }
}
