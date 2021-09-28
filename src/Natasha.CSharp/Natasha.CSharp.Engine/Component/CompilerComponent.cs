using Microsoft.CodeAnalysis.CSharp;
using Natasha.Framework;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace Natasha.CSharpEngine
{

    public static class CompilerComponent
    {

        public static Func<CompilerBase<CSharpCompilation, CSharpCompilationOptions>> GetCompiler;
        public static void RegisterDefault<T>() where T : CompilerBase<CSharpCompilation, CSharpCompilationOptions>, new()
        {
#if DEBUG
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            DynamicMethod method = new DynamicMethod("Compilation" + Guid.NewGuid().ToString(), typeof(T), new Type[0]);
            ILGenerator il = method.GetILGenerator();
            ConstructorInfo ctor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);
            GetCompiler = (Func<T>)(method.CreateDelegate(typeof(Func<T>)));

            var compilerHandler = GetCompiler();
            var option = compilerHandler.GetCompilationOptions();
            var compiler = compilerHandler.GetCompilation(option);

#if DEBUG
            stopwatch.StopAndShowCategoreInfo("[Compiler]", "编译器初始化", 1);
#endif
        }

    }
}
