using Microsoft.CodeAnalysis;
using Natasha.Framework;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Natasha.CSharpEngine
{

    public static class CompilerManagement
    {
        public static Func<CompilerBase<Compilation, CompilationOptions>> GetCompiler;
        public static void RegisterDefault<T>() where T : CompilerBase<Compilation, CompilationOptions>, new()
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
