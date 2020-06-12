using Microsoft.CodeAnalysis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Natasha.Framework
{

    public abstract class DomainBase : AssemblyLoadContext, IDisposable
    {
        public DomainBase()
        {
        }

        public readonly ConcurrentDictionary<Assembly, PortableExecutableReference> AssemblyReferences;
#if NETSTANDARD2_0
        public string Name;
#else
        public readonly static Func<AssemblyDependencyResolver, Dictionary<string, string>> GetDictionary;
        static DomainBase()
        {
            var methodInfo = typeof(AssemblyDependencyResolver).GetField("_assemblyPaths", BindingFlags.NonPublic | BindingFlags.Instance);
            GetDictionary = item => (Dictionary<string, string>)methodInfo.GetValue(item);
        }
#endif

        public DomainBase(string key)

#if  !NETSTANDARD2_0
            : base(isCollectible: true, name: key)
#endif

        {
#if NETSTANDARD2_0
            Name = key;
#endif

            AssemblyReferences = new ConcurrentDictionary<Assembly, PortableExecutableReference>();
            if (key == "Default")
            {
                Default.Resolving += Default_Resolving;
#if !NETSTANDARD2_0
                Default.ResolvingUnmanagedDll += Default_ResolvingUnmanagedDll;
#endif
            }
        }

        public virtual HashSet<PortableExecutableReference> GetDefaultReferences()
        {
            return new HashSet<PortableExecutableReference>(DomainManagement.Default.AssemblyReferences.Values);
        }

        public abstract Assembly Default_Resolving(AssemblyLoadContext arg1, AssemblyName arg2);

        public abstract IntPtr Default_ResolvingUnmanagedDll(Assembly arg1, string arg2);

        public virtual HashSet<PortableExecutableReference> GetCompileReferences()
        {
            var sets = GetDefaultReferences();
            if (Name != "default")
            {
                sets.UnionWith(AssemblyReferences.Values);
            }
            return sets;
        }

        public abstract Assembly CompileStreamHandler(Stream stream, string AssemblyName);

        public abstract Assembly CompileFileHandler(string dllFile, string pdbFile, string AssemblyName);

        public abstract Assembly LoadPluginFromFile(string path, params string[] excludePaths);

        public abstract Assembly LoadPluginFromStream(string path, params string[] excludePaths);

        public virtual void AddDeps(string path, params string[] excludePaths)
        {

#if !NETSTANDARD2_0
            AddReferencesFromDepsJsonFile(path,excludePaths);
#else
            AddReferencesFromDllFile(path, excludePaths);
#endif

        }

        public abstract void Remove(string path);

        public abstract void Remove(Assembly assembly);

        public int Count
        {
            get { return AssemblyReferences.Count; }
        }

        public virtual void Dispose()
        {
            AssemblyReferences.Clear();
        }

        public abstract DomainBase GetInstance(string paramaeters);

        public virtual void AddReferencesFromDllFile(string path, params string[] excludePaths)
        {


        }

        public virtual void AddReferencesFromFolder(string path, bool subFolder = true, params string[] excludePaths) { }

        public virtual void AddReferencesFromDepsJsonFile(string path, params string[] excludePaths) { }

        /// <summary>
        /// 将文件转换为程序集，并加载到域
        /// </summary>
        /// <param name="path">外部文件</param>
        /// <returns></returns>
        public virtual Assembly LoadAssemblyFromFile(string path)
        {
            if (Name == "Default")
            {
                return Default.LoadFromAssemblyPath(path);
            }
            else
            {
                return LoadFromAssemblyPath(path);
            }
        }

        /// <summary>
        /// 将流转换为程序集，并加载到域
        /// [手动释放]
        /// </summary>
        /// <param name="stream">外部流</param>
        /// <returns></returns>
        public virtual Assembly LoadAssemblyFromStream(Stream stream)
        {
            if (Name == "Default")
            {
                return Default.LoadFromStream(stream);
            }
            else
            {
                return LoadFromStream(stream);
            }
        }

        /// <summary>
        /// 将文件流转换为程序集，并加载到域
        /// [自动释放]
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual Assembly LoadAssemblyFromStream(string path)
        {
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return LoadAssemblyFromStream(stream);
            }
        }


        public abstract IEnumerable<Assembly> GetPluginAssemblies();

    }
}