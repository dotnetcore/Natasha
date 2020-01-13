using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
using Natasha.Core;
using Natasha.Core.Complier;
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


        public readonly object ObjLock;
        public readonly string DomainPath;
        public event DomainAssemblyEvent LoadAssemblyEvent;
        public event DomainNativeAssemblyEvent LoadNativeEvent;
        public event DomainAssemblyEvent RemoveAssemblyEvent;
        public event DomainNativeAssemblyEvent RemoveNativeEvent;

        public readonly ConcurrentDictionary<string, Assembly> OutfileMapping;
        public readonly ConcurrentDictionary<Assembly, string> PathMapping;
        public readonly LinkedList<PortableExecutableReference> ReferencesCache;
        public readonly ConcurrentDictionary<Assembly, AssemblyUnitInfo> AssemblyMappings;
        public readonly ConcurrentDictionary<string, PortableExecutableReference> ShortReferenceMappings;


#if NETSTANDARD2_0
        public string Name;
#else
        public readonly static Func<AssemblyDependencyResolver, Dictionary<string, string>> GetDictionary;
        static AssemblyDomain()
        {

            var methodInfo = typeof(AssemblyDependencyResolver).GetField("_assemblyPaths", BindingFlags.NonPublic | BindingFlags.Instance);
            GetDictionary = item => (Dictionary<string, string>)methodInfo.GetValue(item);

        }
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
            _load_resolver = new AssemblyDependencyResolver(AppDomain.CurrentDomain.BaseDirectory);
#else
            Name = key;
#endif
            ObjLock = new object();


            DomainPath = Path.Combine(IComplier.CurrentPath, key);
            OutfileMapping = new ConcurrentDictionary<string, Assembly>();
            PathMapping = new ConcurrentDictionary<Assembly, string>();
            AssemblyMappings = new ConcurrentDictionary<Assembly, AssemblyUnitInfo>();
            ShortReferenceMappings = new ConcurrentDictionary<string, PortableExecutableReference>();

            if (key == "Default")
            {

                var _ref = DependencyContext.Default.CompileLibraries
                           .SelectMany(cl => cl.ResolveReferencePaths())
                           .Select(asm =>
                           {

                               var table = MetadataReference.CreateFromFile(asm);
                               ShortReferenceMappings[Path.GetFileName(asm)] = table;
                               return table;

                           });

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
            DomainManagment.Add(key, this);

        }




        public bool RemoveDll(string path)
        {

            if (path == default)
            {
                throw new NullReferenceException("Path is null! This method can't be passed a null instance.");
            }


            if (OutfileMapping.ContainsKey(path))
            {

                return RemoveAssembly(OutfileMapping[path]);

            }
            return false;

        }




        public bool RemoveType(Type type)
        {


            if (type == default)
            {
                throw new NullReferenceException("Type is null! This method can't be passed a null instance.");
            }

            return RemoveAssembly(type.Assembly);


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

                    if (PathMapping.ContainsKey(assembly))
                    {

                        var path = PathMapping[assembly];
                        while (!ShortReferenceMappings.TryRemove(Path.GetFileName(path), out var _)) { };


                        while (!OutfileMapping.TryRemove(path, out var _)) { };
                        while (!PathMapping.TryRemove(assembly, out var _)) { };
                        RemoveAssemblyEvent?.Invoke(path, assembly);

                    }

                    lock (ReferencesCache)
                    {

                        var info = AssemblyMappings[assembly];
                        if (ReferencesCache.Contains(info.Reference.Value))
                        {

                            ReferencesCache.Remove(info.Reference);

                        }

                    }
                    while (!AssemblyMappings.TryRemove(assembly, out var _)) { };


                    return true;

                }

            }

            return false;

        }




        public void Dispose()
        {


#if !NETSTANDARD2_0
            _load_resolver = null;
#endif
            ShortReferenceMappings.Clear();
            ReferencesCache.Clear();
            OutfileMapping.Clear();
            AssemblyMappings.Clear();
            PathMapping.Clear();

        }


#if !NETSTANDARD2_0
        private AssemblyDependencyResolver _load_resolver;
#endif

        protected override Assembly Load(AssemblyName assemblyName)
        {

#if !NETSTANDARD2_0
            string assemblyPath = _load_resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {

                var assembly = Handler(new FileStream(assemblyPath, FileMode.Open));
                LoadAssemblyEvent?.Invoke(assemblyPath, assembly);
                return assembly;

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
            string libraryPath = _load_resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {

                var ptr = LoadUnmanagedDllFromPath(libraryPath);
                LoadNativeEvent.Invoke(libraryPath, ptr);
                return ptr;

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
        internal Assembly Handler(Stream stream = null)
        {

            if (stream != null)
            {

                return Handler(new AssemblyUnitInfo(this, stream));

            }
            return default;

        }

        internal Assembly Handler(string path)
        {

            if (path != default)
            {

                return Handler(new AssemblyUnitInfo(this, path));

            }
            return default;

        }

        internal Assembly Handler(AssemblyUnitInfo info)
        {

            lock (ObjLock)
            {

                Assembly result = info.Assembly;
                if (!AssemblyMappings.ContainsKey(result))
                {

                    if (result != default)
                    {
                        AssemblyMappings[result] = info;
                        LoadAssemblyEvent?.Invoke(result.CodeBase, result);
                    }

                    lock (ReferencesCache)
                    {
                        ReferencesCache.AddLast(info.Reference);
                    }


                }

                return result;
            }

        }




        /// <summary>
        /// 使用外部文件加载程序集
        /// </summary>
        /// <param name="path">dll文件路径</param>
        /// <param name="isCover">是否覆盖原有的同路径的dll</param>
        /// <returns></returns>
        public Assembly LoadFile(string path, bool isCover = false, params string[] excludePaths)
        {

            if (isCover) { RemoveDll(path); }
            HashSet<string> exclude;
            if (excludePaths == default)
            {
                exclude = new HashSet<string>();
            }
            else
            {
                exclude = new HashSet<string>(excludePaths);
            }

#if !NETSTANDARD2_0

            _load_resolver = new AssemblyDependencyResolver(path);
            var newMapping = GetDictionary(_load_resolver);

            foreach (var item in newMapping)
            {

                if (!exclude.Contains(item.Value))
                {
                    var info = new AssemblyUnitInfo(this, item.Value);
                    Assembly assembly = Handler(info);
                    OutfileMapping[item.Value] = assembly;
                    PathMapping[assembly] = item.Value;
                    ShortReferenceMappings[Path.GetFileName(item.Value)] = info.Reference.Value;
                }

            }

#else

            var dllPath = Path.GetDirectoryName(path);
            string[] dllFiles = Directory.GetFiles(dllPath, "*.dll", SearchOption.AllDirectories);
            for (int i = 0; i < dllFiles.Length; i++)
            {

                if (!exclude.Contains(dllFiles[i]))
                {
                    var info = new AssemblyUnitInfo(this, dllFiles[i]);
                    Assembly assembly = Handler(info);
                    OutfileMapping[dllFiles[i]] = assembly;
                    PathMapping[assembly] = dllFiles[i];
                    ShortReferenceMappings[Path.GetFileName(dllFiles[i])] = info.Reference.Value;
                }
                
            }

#endif

            return OutfileMapping[path];

        }




        public Assembly LoadStream(string path, bool isCover = false, params string[] excludePaths)
        {

            if (isCover) { RemoveDll(path); }
            HashSet<string> exclude;
            if (excludePaths == default)
            {
                exclude = new HashSet<string>();
            }
            else
            {
                exclude = new HashSet<string>(excludePaths);
            }

#if !NETSTANDARD2_0

            _load_resolver = new AssemblyDependencyResolver(path);
            var newMapping = GetDictionary(_load_resolver);


            foreach (var item in newMapping)
            {

                if (!exclude.Contains(item.Value))
                {
                    var info = new AssemblyUnitInfo(this, new FileStream(item.Value, FileMode.Open));
                    Assembly assembly = Handler(info);
                    OutfileMapping[item.Value] = assembly;
                    PathMapping[assembly] = item.Value;
                    ShortReferenceMappings[Path.GetFileName(item.Value)] = info.Reference.Value;
                }

            }


#else

            var dllPath = Path.GetDirectoryName(path);
            string[] dllFiles = Directory.GetFiles(dllPath, "*.dll", SearchOption.AllDirectories);
            for (int i = 0; i < dllFiles.Length; i++)
            {
                if (!exclude.Contains(dllFiles[i]))
                {
                    var info = new AssemblyUnitInfo(this, new FileStream(dllFiles[i], FileMode.Open));
                    Assembly assembly = Handler(info);
                    OutfileMapping[dllFiles[i]] = assembly;
                    PathMapping[assembly] = dllFiles[i];
                    ShortReferenceMappings[Path.GetFileName(dllFiles[i])] = info.Reference.Value;
                }
            }

#endif

            return OutfileMapping[path];

        }
    }

}
