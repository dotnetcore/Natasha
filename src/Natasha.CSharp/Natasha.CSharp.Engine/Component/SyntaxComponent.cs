using Microsoft.CodeAnalysis;
using Natasha.Framework;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace Natasha.CSharpEngine
{

    public static class SyntaxComponent
    {

        public static Func<SyntaxBase> GetSyntax;
        public static void RegisterDefault<T>() where T : SyntaxBase, new()
        {
#if DEBUG
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            DynamicMethod method = new DynamicMethod("Syntax" + Guid.NewGuid().ToString(), typeof(T), new Type[0]);
            ILGenerator il = method.GetILGenerator();
            ConstructorInfo ctor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);
            GetSyntax = (Func<T>)(method.CreateDelegate(typeof(Func<T>)));

            //var syntaxBase = GetSyntax();
            //syntaxBase.AddTreeToCache("public class NatashaInitializerTest{}");


#if DEBUG
            stopwatch.StopAndShowCategoreInfo("[Syntax]", "语法树初始化", 1);
#endif
        }

    }
}
