using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RefAssembly.Compile.Method.Utils
{
    internal static class MemberHelper
    {
        internal static Type GetType(string memberScript, string? assemblyName = null)
        {
            string code = @$"public class A{{{memberScript}}}";

            AssemblyCSharpBuilder builder =  assemblyName == null?new(): new(assemblyName);
            builder
                .UseRandomLoadContext()
                .UseSmartMode()
                .Add(code);

            return builder
                .GetAssembly()
                .GetTypeFromShortName("A");
        }

        internal static Assembly GetAssembly(string memberScript, string? assemblyName = null)
        {
            string code = @$"public class A{{{memberScript}}}";

            AssemblyCSharpBuilder builder = assemblyName == null ? new() : new(assemblyName);
            builder
                .UseRandomLoadContext()
                .UseSmartMode()
                .Add(code);

            return builder
                .GetAssembly();
        }
    }
}
