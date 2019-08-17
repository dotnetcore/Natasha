using Natasha.Complier;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Natasha
{

#if NETCOREAPP3_0
    public class AssemblyDomain : AssemblyLoadContext, IDisposable
    {

        public readonly ConcurrentBag<Assembly> Assemblys;
        public readonly ConcurrentBag<string> Paths;


        private readonly AssemblyDependencyResolver _resolver;
        public AssemblyDomain() : this(ScriptComplierEngine.LibPath)
        {
        }




        public AssemblyDomain(string pluginPath) : base(isCollectible:true)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
            Assemblys = new ConcurrentBag<Assembly>();
            Paths = new ConcurrentBag<string>();
        }




        public void Dispose()
        {
            this.Clear();
            this.Unload();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }




        private void Clear()
        {
            Assemblys.Clear();
            foreach (var item in Paths)
            {
                Directory.Delete(item);
            }
        }




        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                Assembly assembly = LoadFromAssemblyPath(assemblyPath);
                Assemblys.Add(assembly);
                Paths.Add(assembly.Location);
                return assembly;
            }

            return null;
        }




        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }

            return IntPtr.Zero;
        }
    }
#endif

}
