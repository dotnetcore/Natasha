using Microsoft.CodeAnalysis.CSharp;
using Natasha.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Natasha.CSharpEngine
{

    public static class CompilerManagement
    {
        public static Func<CompilerBase<CSharpCompilation, CSharpCompilationOptions>> GetCompiler;
        public static void RegisterDefault<T>() where T : CompilerBase<CSharpCompilation, CSharpCompilationOptions>, new()
        {

            DynamicMethod method = new DynamicMethod("Compilation" + Guid.NewGuid().ToString(), typeof(T), new Type[0]);
            ILGenerator il = method.GetILGenerator();
            ConstructorInfo ctor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);
            GetCompiler = (Func<T>)(method.CreateDelegate(typeof(Func<T>)));

        }

    }
}
