using Microsoft.Extensions.DependencyModel;
using Natasha.Framework;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;


public class DomainComponent
{

    public static void RegisterDefault<TDomain>(bool initializeReference = true) where TDomain : DomainBase
    {
#if DEBUG
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
#endif
        DynamicMethod method = new DynamicMethod("Domain" + Guid.NewGuid().ToString(), typeof(DomainBase), new Type[] { typeof(string) });
        ILGenerator il = method.GetILGenerator();
        ConstructorInfo ctor = typeof(TDomain).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(string) }, null);
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Newobj, ctor);
        il.Emit(OpCodes.Ret);
        DomainManagement.CreateDomain = (Func<string, DomainBase>)(method.CreateDelegate(typeof(Func<string, DomainBase>)));
        DomainManagement.Default = DomainManagement.Create("Default");

#if DEBUG
        stopwatch.StopAndShowCategoreInfo("[Domain]", "域初始化", 1);
        stopwatch.Restart();
#endif
        //Mark : 21M Memory
        if (initializeReference)
        {
#if DEBUG
            stopwatch.Restart();
#endif
            //Mark : 89ms
            foreach (var asm in DependencyContext
            .Default
            .CompileLibraries
            .SelectMany(cl => cl.ResolveReferencePaths()))
            {
                DomainManagement.Default.AddReferencesFromDllFile(asm);
            }
#if DEBUG
            stopwatch.StopAndShowCategoreInfo("[Reference]", "引用扫描", 1);
            stopwatch.Restart();
#endif
        }

    }

}
