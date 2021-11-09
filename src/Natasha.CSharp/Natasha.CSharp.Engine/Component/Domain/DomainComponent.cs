using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
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

    public static void RegisterDefault<TDomain>(bool initializeReference = true) where TDomain : DomainBase
    {
        //Mark : 21M Memory
        if (initializeReference)
        {
#if DEBUG
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            //Mark1 : 89ms
            //Mark2 : 14ms
            Task.Run(() =>
            {
                ConcurrentDictionary<string, PortableExecutableReference> tempCache = new ConcurrentDictionary<string, PortableExecutableReference>();
                IEnumerable<string> paths = DependencyContext
                    .Default
                    .CompileLibraries.SelectMany(cl => cl.ResolveReferencePaths());
                Parallel.ForEach(paths,
                    asm => tempCache[Path.GetFileNameWithoutExtension(asm)] = MetadataReference.CreateFromFile(asm));
                while (DomainBase.DefaultDomain.Name == "init_fake_domain")
                {
                    Task.Delay(200);
                }
                Unsafe.AsRef(DomainBase.DefaultDomain.OtherReferencesFromFile) = tempCache;
            });
#if DEBUG
            stopwatch.StopAndShowCategoreInfo("[Reference]", "引用扫描", 1);
            stopwatch.Restart();
#endif
        }
#if DEBUG
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
#endif
            DynamicMethod method = new DynamicMethod("Domain" + Guid.NewGuid().ToString(), typeof(DomainBase), new Type[] { typeof(string) });
            ILGenerator il = method.GetILGenerator();
            ConstructorInfo ctor = typeof(TDomain).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(string) }, null)!;
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);
            DomainManagement.CreateDomain = (Func<string, DomainBase>)(method.CreateDelegate(typeof(Func<string, DomainBase>)));
            DomainBase.DefaultDomain = DomainManagement.Create("Default");

#if DEBUG
            stopwatch2.StopAndShowCategoreInfo("[Domain]", "域初始化", 1);
#endif

    }

}
