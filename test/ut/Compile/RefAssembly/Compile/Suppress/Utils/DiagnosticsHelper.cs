using Microsoft.CodeAnalysis;
using Natasha.CSharp.Compiler.Component;
using System;
using System.Collections.Generic;

namespace RefAssembly.Compile.Suppress.Utils
{
    internal static class DiagnosticsHelper
    {
        internal static List<Diagnostic> GetDiagnostics(string script, params string[] diagnosCode)
        {
            AssemblyCSharpBuilder builder = new();
            builder
                .UseRandomLoadContext()
                .UseSmartMode()
                .ConfigCompilerOption(opt => opt.ClearSupperess());

            for (int i = 0; i < diagnosCode.Length; i++)
            {
                builder.ConfigCompilerOption(opt => opt.AddSupperess(diagnosCode[i]));
            }
            builder.Add(script);
            try
            {
                var assembly = builder.GetAssembly();
            }
            catch (Exception ex)
            {

            }
            return WrapperDiagnosties(builder);
        }

        internal static List<Diagnostic> GetDiagnostics(string script, string diagnostiCode, ReportDiagnostic reportDiagnostic)
        {
            AssemblyCSharpBuilder builder = new();
            builder
                .UseRandomLoadContext()
                .UseSmartMode()
                .ConfigCompilerOption(opt => opt.ClearSupperess());

            builder.ConfigCompilerOption(opt => opt.SetCodeReportLevel(diagnostiCode, reportDiagnostic));
            builder.Add(script);
            try
            {
                var assembly = builder.GetAssembly();
            }
            catch (Exception ex)
            {

            }
            return WrapperDiagnosties(builder);
        }

        internal static List<Diagnostic> GetDiagnostics(string script, Action<NatashaCSharpCompilerOptions>? action = null)
        {
            AssemblyCSharpBuilder builder = new();
            builder
                .UseRandomLoadContext()
                .UseSmartMode()
                .ConfigCompilerOption(opt => opt.ClearSupperess());
            if (action!=null)
            {
                builder.ConfigCompilerOption(action);
            }
            builder.FastAddScriptWithoutCheck(script);
            try
            {
                var assembly = builder.GetAssembly();
            }
            catch (Exception ex)
            {

            }
            return WrapperDiagnosties(builder);
        }
        internal static List<Diagnostic> GetDiagnostics(string script,out bool succeed, Action<NatashaCSharpCompilerOptions>? action = null)
        {
            AssemblyCSharpBuilder builder = new();
            builder
                .UseRandomLoadContext()
                .UseSmartMode()
                .ConfigCompilerOption(opt => opt.ClearSupperess());
            if (action != null)
            {
                builder.ConfigCompilerOption(action);
            }
            builder.Add(script);
            try
            {
                var assembly = builder.GetAssembly();
                succeed = true;
            }
            catch (Exception ex)
            {
                succeed = false;
            }
            return WrapperDiagnosties(builder);
        }

        public static List<Diagnostic> WrapperDiagnosties(AssemblyCSharpBuilder builder)
        {
            List<Diagnostic> result = [];
            var diagnostics = builder.GetDiagnostics();
            if (diagnostics.HasValue && diagnostics.Value.Length != 0)
            {
                result.AddRange(diagnostics);
            }
            return result;
        }
    }
}
