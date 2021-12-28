using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
using Natasha;
using Natasha.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public class DomainComponent
{

    public static void RegisterDefault<TDomain>(Func<string, bool> excludeReferencesFunc) where TDomain : DomainBase
    {
        //Mark : 21M Memory

#if DEBUG
        Stopwatch stopwatch = new();
        stopwatch.Start();
#endif
        //Mark1 : 89ms
        //Mark2 : 14ms
//        var task = Task.Run(() =>
//        {

           

//        });


//#if DEBUG
//        stopwatch.StopAndShowCategoreInfo("[Reference]", "引用扫描", 1);
//        stopwatch.Restart();
//#endif
        DynamicMethod method = new DynamicMethod("Domain" + Guid.NewGuid().ToString(), typeof(DomainBase), new Type[] { typeof(string) });
        ILGenerator il = method.GetILGenerator();
        ConstructorInfo ctor = typeof(TDomain).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(string) }, null)!;
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Newobj, ctor);
        il.Emit(OpCodes.Ret);
        DomainManagement.CreateDomain = (Func<string, DomainBase>)(method.CreateDelegate(typeof(Func<string, DomainBase>)));
        DomainManagement.Create("Default");

        //ConcurrentDictionary<string, PortableExecutableReference> tempCache = new ConcurrentDictionary<string, PortableExecutableReference>();
        IEnumerable<string> paths = DependencyContext
            .Default
            .CompileLibraries.SelectMany(cl => cl.ResolveReferencePaths());

        var result = Parallel.ForEach(paths,
                            asm =>
                            {
                                var key = Path.GetFileNameWithoutExtension(asm);
                                if (!excludeReferencesFunc!(key))
                                {
                                    DefaultUsing.AddReference(asm);
                                    DomainBase.DefaultDomain.OtherReferencesFromFile[key] = MetadataReference.CreateFromFile(asm);
                                }

                            });
        //while (DomainBase.DefaultDomain.Name == "init_fake_domain")
        //{
        //    Task.Delay(200);
        //}
        while (!result.IsCompleted)
        {
            Task.Delay(100);
        }
        DefaultUsing.AddCompleted();
        //Unsafe.AsRef(DomainBase.DefaultDomain.OtherReferencesFromFile) = tempCache;



       // task.Wait();
#if DEBUG
        stopwatch.StopAndShowCategoreInfo("[Domain]", "域初始化", 1);
#endif

    }

}
