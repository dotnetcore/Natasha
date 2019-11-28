using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
using Natasha.AssemblyModule.Model;
using Natasha.Complier;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Natasha
{

    public class AssemblyDomain : AssemblyLoadContext, IDisposable
    {

        public readonly ConcurrentDictionary<Assembly, AssemblyUnitInfo> AssemblyMappings;
        public readonly ConcurrentDictionary<string, Assembly> OutfileMapping;
        public readonly LinkedList<PortableExecutableReference> ReferencesCache;
        public readonly HashSet<Type> TypeCache;
        public readonly object ObjLock;
        public readonly string DomainPath;
        public int GCCount;
#if NETSTANDARD2_0
        public string Name;
#endif



        public int Count
        {
            get { return ReferencesCache.Count; }
        }




        public AssemblyDomain(string key)

#if  !NETSTANDARD2_0
            : base(isCollectible: true, name: key)
#endif

        {
#if !NETSTANDARD2_0
            _resolver = new AssemblyDependencyResolver(AppDomain.CurrentDomain.BaseDirectory);
#else
            Name = key;
#endif
            ObjLock = new object();


            DomainPath = Path.Combine(IComplier.CurrentPath, key);
            if (!Directory.Exists(DomainPath))
            {

                Directory.CreateDirectory(DomainPath);

            }

            DomainManagment.Add(key, this);
            TypeCache = new HashSet<Type>();
            OutfileMapping = new ConcurrentDictionary<string, Assembly>();
            AssemblyMappings = new ConcurrentDictionary<Assembly, AssemblyUnitInfo>();


            if (key == "Default")
            {

                var _ref = DependencyContext.Default.CompileLibraries
                                .SelectMany(cl => cl.ResolveReferencePaths())
                                .Select(asm => MetadataReference.CreateFromFile(asm));

                ReferencesCache = new LinkedList<PortableExecutableReference>(_ref);
                Default.Resolving += Default_Resolving;
#if !NETSTANDARD2_0
                Default.ResolvingUnmanagedDll += Default_ResolvingUnmanagedDll;
#endif

            }
            else
            {

                ReferencesCache = new LinkedList<PortableExecutableReference>();

            }

        }




        /// <summary>
        /// 创建一个程序集编译类
        /// </summary>
        /// <param name="name">程序集名字</param>
        /// <returns></returns>
        public NAssembly CreateAssembly(string name = default)
        {
            NAssembly result = new NAssembly(name);
            result.Options.Domain = this;
            return result;
        }




        public bool RemoveDll(string path)
        {
            if (path == default)
            {
                throw new NullReferenceException("Path is null! This method can't be passed a null instance.");
            }


            if (OutfileMapping.ContainsKey(path))
            {

                bool result = RemoveAssembly(OutfileMapping[path]);
                return result;

            }
            return false;

        }




        public bool RemoveType(Type type)
        {

            if (type == default)
            {
                throw new NullReferenceException("Type is null! This method can't be passed a null instance.");
            }

            lock (ObjLock)
            {

                if (TypeCache.Contains(type))
                {
                    return RemoveAssembly(type.Assembly);
                }

            }
            return false;

        }




        public bool RemoveAssembly(Assembly assembly)
        {

            if (assembly == default)
            {
                throw new NullReferenceException("Assembly is null!  This method can't be passed a null instance.");
            }

            lock (ObjLock)
            {

                if (AssemblyMappings.ContainsKey(assembly))
                {

                    if (OutfileMapping.ContainsKey(assembly.Location))
                    {
                        while (!OutfileMapping.TryRemove(assembly.Location, out var _)) { };
                    }


                    var info = AssemblyMappings[assembly];
                    ReferencesCache.Remove(info.Reference);
                    foreach (var item in assembly.GetTypes())
                    {
                        TypeCache.Remove(item);
                    }
                    return true;

                }

            }

            return false;

        }




        public void Dispose()
        {

#if  !NETSTANDARD2_0
            _resolver = null;
#endif
            ReferencesCache.Clear();
            TypeCache.Clear();
            OutfileMapping.Clear();
            AssemblyMappings.Clear();

        }


#if  !NETSTANDARD2_0
        private AssemblyDependencyResolver _resolver;
#endif

        protected override Assembly Load(AssemblyName assemblyName)
        {

#if  !NETSTANDARD2_0
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {

                return Handler(new FileStream(assemblyPath, FileMode.Open));

            }
#endif
            return null;

        }

        private Assembly Default_Resolving(AssemblyLoadContext arg1, AssemblyName arg2)
        {

            return Load(arg2);

        }



        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {

#if !NETSTANDARD2_0
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }
#endif
            return IntPtr.Zero;

        }

        private IntPtr Default_ResolvingUnmanagedDll(Assembly arg1, string arg2)
        {

            return LoadUnmanagedDll(arg2);

        }




        /// <summary>
        /// 缓存表的原子操作，缓存程序集，并写入引用表
        /// </summary>
        /// <param name="stream">程序集流</param>
        /// <returns></returns>
        public Assembly Handler(Stream stream = null)
        {

            if (stream != null)
            {

                return Handler(new AssemblyUnitInfo(this, stream));

            }
            return default;

        }

        public Assembly Handler(string path)
        {

            if (path != default)
            {

                return Handler(new AssemblyUnitInfo(this, path));

            }
            return default;

        }

        public Assembly Handler(AssemblyUnitInfo info)
        {

            lock (ObjLock)
            {
                Assembly result = info.Assembly;
                if (result != default)
                {
                    AssemblyMappings[result] = info;
                    TypeCache.UnionWith(result.GetTypes());
                }
                ReferencesCache.AddLast(info.Reference);
                return result;
            }

        }




        /// <summary>
        /// 使用外部文件加载程序集
        /// </summary>
        /// <param name="path">dll文件路径</param>
        /// <param name="isCover">是否覆盖原有的同路径的dll</param>
        /// <returns></returns>
        public Assembly LoadFile(string path, bool isCover = false)
        {

#if !NETSTANDARD2_0
            _resolver = new AssemblyDependencyResolver(path);
#endif
            if (isCover) { RemoveDll(path); }


            var result = Handler(new AssemblyUnitInfo(this, path));
            OutfileMapping[path] = result;
            return result;

        }
        public Assembly LoadStream(string path, bool isCover = false)
        {

#if !NETSTANDARD2_0
            _resolver = new AssemblyDependencyResolver(path);
#endif
            if (isCover) { RemoveDll(path); }


            var result = Handler(new AssemblyUnitInfo(this, new FileStream(path, FileMode.Open)));
            OutfileMapping[path] = result;
            return result;

        }
    }

}
