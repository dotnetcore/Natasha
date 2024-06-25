using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha.CSharp.Compiler.Utils
{
    internal class NatashaCoreClrShim
    {

        internal static bool IsRunningOnCoreClr => ALC.Type != null;

        internal static class ALC
        {
            internal static readonly Type? Type = ReflectionUtilities.TryGetType(
               "System.Runtime.Loader.AssemblyLoadContext, System.Runtime.Loader, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
        }

        internal static class ACX
        {
            internal static readonly Type? Type = ReflectionUtilities.TryGetType(
                "System.AppContext, System.AppContext, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

            // only available in netstandard 1.6+
            internal static readonly Func<string, object>? GetData =
                Type!.GetTypeInfo().GetDeclaredMethod("GetData")?.CreateDelegate<Func<string, object>>();
        }
    }
}
