using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Natasha
{

    public class AssemblyDomain : AssemblyLoadContext, IDisposable
    {

        public readonly ConcurrentDictionary<string, Assembly> ClassMapping;
        public readonly ConcurrentDictionary<string, Assembly> DynamicDlls;
        public readonly ConcurrentQueue<PortableExecutableReference> References;
        public readonly string LibPath;
        public readonly string KeyName;


#if NETCOREAPP3_0
        private readonly AssemblyDependencyResolver _resolver;
#endif




        public AssemblyDomain(string name)
#if NETCOREAPP3_0
            : base(isCollectible: true)
#endif

        {

            KeyName = name;
            LibPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NatashaLib", name);
            if (Directory.Exists(LibPath))
            {

                Directory.Delete(LibPath, true);

            }
            Directory.CreateDirectory(LibPath);


#if NETCOREAPP3_0
            _resolver = new AssemblyDependencyResolver(LibPath);
#endif


            ClassMapping = new ConcurrentDictionary<string, Assembly>();
            DynamicDlls = new ConcurrentDictionary<string, Assembly>();


            var _ref = DependencyContext.Default.CompileLibraries
                                .SelectMany(cl => cl.ResolveReferencePaths())
                                .Select(asm => MetadataReference.CreateFromFile(asm));
            References = new ConcurrentQueue<PortableExecutableReference>(_ref);
            this.Unloading += AssemblyDomain_Unloading;

        }




        private void AssemblyDomain_Unloading(AssemblyLoadContext obj)
        {
            //throw new NotImplementedException();
        }




        public void Dispose()
        {
            this.Clear();
#if NETCOREAPP3_0
            //this.Unload();
#endif
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //Directory.Delete(LibPath, true);
            
        }




        private void Clear()
        {

#if NETCOREAPP3_0
            References.Clear();
#endif
            ClassMapping.Clear();
            DynamicDlls.Clear();

        }




        protected override Assembly Load(AssemblyName assemblyName)
        {

#if NETCOREAPP3_0
            //string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);

            //if (assemblyPath != null)
            //{

            //    Assembly assembly = LoadFromAssemblyPath(assemblyPath);
            //    CacheAssembly(assembly);
            //    return assembly;

            //}
#endif
            return null;

        }





        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {

#if NETCOREAPP3_0
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }
#endif

            return IntPtr.Zero;

        }





        public void CacheAssembly(Assembly assembly)
        {

            var types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {

                ClassMapping[types[i].Name] = assembly;

            }


            if (!DynamicDlls.ContainsKey(assembly.Location))
            {

                DynamicDlls[assembly.Location] = assembly;
                References.Enqueue(MetadataReference.CreateFromFile(assembly.Location));

            }

        }




        public void LoadFile(string path)
        {

            if (!DynamicDlls.ContainsKey(path))
            {

                var result = LoadFromAssemblyPath(path);
                CacheAssembly(result);

            }

        }




        public Assembly GetDynamicAssembly(string className)
        {

            if (ClassMapping.ContainsKey(className))
            {

                return ClassMapping[className];

            }
            return null;

        }




        /// <summary>
        /// 根据类名获取类，前提类必须是成功编译过的
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Type GetType(string name)
        {

            return ClassMapping[name].GetTypes().First(item => item.Name == name);

        }

    }

}
