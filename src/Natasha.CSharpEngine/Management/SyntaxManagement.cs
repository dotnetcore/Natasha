using Natasha.Framework;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Natasha.CSharpEngine
{

    public static class SyntaxManagement
    {

        public static Func<SyntaxBase> GetSyntax;
        public static void RegisterDefault<T>() where T : SyntaxBase, new()
        {

            DynamicMethod method = new DynamicMethod("Syntax" + Guid.NewGuid().ToString(), typeof(T), new Type[0]);
            ILGenerator il = method.GetILGenerator();
            ConstructorInfo ctor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);
            GetSyntax = (Func<T>)(method.CreateDelegate(typeof(Func<T>)));

        }

    }
}
