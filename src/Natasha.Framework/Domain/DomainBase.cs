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

        public static DomainBase DefaultDomain;

        public readonly ConcurrentDictionary<Assembly, PortableExecutableReference> ReferencesFromStream;
        public readonly ConcurrentDictionary<string, PortableExecutableReference> ReferencesFromFile;
#if NETSTANDARD2_0
        public string Name;
#else
        public readonly static Func<AssemblyDependencyResolver, Dictionary<string, string>> GetDictionary;
        static DomainBase()
        {
            //从依赖中拿到所有的路径，该属性未开放
            var methodInfo = typeof(AssemblyDependencyResolver).GetField("_assemblyPaths", BindingFlags.NonPublic | BindingFlags.Instance);
            GetDictionary = item => (Dictionary<string, string>)methodInfo.GetValue(item);
            _shareLibraries = new ConcurrentQueue<string>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in assemblies)
            {

                try
                {
                    _shareLibraries.Enqueue(asm.Location);
                }
                catch (Exception)
                {

                }

            }
        }
#endif


        /// <summary>
        /// 构造函数，需要给当前域传key
        /// </summary>
        /// <param name="key"></param>
        public DomainBase(string key)

#if  !NETSTANDARD2_0
            : base(isCollectible: true, name: key)
#endif

        {
#if NETSTANDARD2_0
            Name = key;
#endif
            //初始化【程序集】-【引用】缓存
            ReferencesFromStream = new ConcurrentDictionary<Assembly, PortableExecutableReference>();
            ReferencesFromFile = new ConcurrentDictionary<string, PortableExecutableReference>();
            if (key == "Default")
            {
                DefaultDomain = this;
                Default.Resolving += Default_Resolving;
#if !NETSTANDARD2_0
                Default.ResolvingUnmanagedDll += Default_ResolvingUnmanagedDll;
#endif
            }
        }

        /// <summary>
        /// 系统域加载事件
        /// </summary>
        /// <param name="arg1">当前域</param>
        /// <param name="arg2">被加载的程序集名</param>
        /// <returns></returns>
        public abstract Assembly Default_Resolving(AssemblyLoadContext arg1, AssemblyName arg2);
        /// <summary>
        /// 系统域非托管程序集加载事件
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        public abstract IntPtr Default_ResolvingUnmanagedDll(Assembly arg1, string arg2);

        /// <summary>
        /// 使用自定义引用
        /// </summary>
        public bool UseCustomReferences;
        public void CustomReferences()
        {
            UseCustomReferences = true;
        }

        private static readonly ConcurrentQueue<string> _shareLibraries;
        public void UseShareLibraries()
        {

            foreach (var asmPath in _shareLibraries)
            {

                AddReferencesFromDllFile(asmPath);

            }

        }

        public abstract HashSet<PortableExecutableReference> GetCompileReferences();

        public abstract Assembly CompileStreamHandler(Stream stream, string AssemblyName);

        public abstract Assembly CompileFileHandler(string dllFile, string pdbFile, string AssemblyName);

        public abstract Assembly LoadPluginFromFile(string path, params string[] excludePaths);

        public abstract Assembly LoadPluginFromStream(string path, params string[] excludePaths);

        public virtual void AddDeps(string path, params string[] excludePaths)
        {

#if !NETSTANDARD2_0
            AddReferencesFromDepsJsonFile(path, excludePaths);
#else
            AddReferencesFromFileStream(path, excludePaths);
#endif

        }

        public abstract void Remove(string path);

        public abstract void Remove(Assembly assembly);

        public int Count
        {
            get { return ReferencesFromStream.Count + ReferencesFromFile.Count; }
        }

        public virtual void Dispose()
        {
#if !NETSTANDARD2_0
            DependencyResolver = null;
#endif
            ReferencesFromStream.Clear();
            ReferencesFromFile.Clear();
        }

        public virtual void AddReferencesFromDllFile(string path, params string[] excludePaths)
        {

            if (excludePaths.Length == 0)
            {
                ReferencesFromFile[Path.GetFileNameWithoutExtension(path)] = MetadataReference.CreateFromFile(path);
            }
            else
            {
                HashSet<string> exclude;
                if (excludePaths == default)
                {
                    exclude = new HashSet<string>();
                }
                else
                {
                    exclude = new HashSet<string>(excludePaths);
                }

                if (!exclude.Contains(path))
                {
                    ReferencesFromFile[Path.GetFileNameWithoutExtension(path)] = MetadataReference.CreateFromFile(path);
                }
            }

        }
        public virtual void AddReferencesFromFileStream(string path, params string[] excludePaths)
        {

            if (excludePaths.Length == 0)
            {
                ReferencesFromFile[Path.GetFileNameWithoutExtension(path)] = MetadataReference.CreateFromStream(new FileStream(path, FileMode.Open, FileAccess.Read));
            }
            else
            {
                HashSet<string> exclude;
                if (excludePaths == default)
                {
                    exclude = new HashSet<string>();
                }
                else
                {
                    exclude = new HashSet<string>(excludePaths);
                }

                if (!exclude.Contains(path))
                {
                    ReferencesFromFile[Path.GetFileNameWithoutExtension(path)] = MetadataReference.CreateFromStream(new FileStream(path, FileMode.Open, FileAccess.Read));
                }
            }

        }
        public virtual void AddReferencesFromAssemblyStream(Assembly assembly, Stream stream)
        {

            ReferencesFromStream[assembly] = MetadataReference.CreateFromStream(stream);

        }
        public virtual void AddReferencesFromFolder(string path, bool subFolder = true, params string[] excludePaths)
        {

            if (excludePaths.Length == 0)
            {
                var dllPath = Path.GetDirectoryName(path);
                string[] dllFiles = Directory.GetFiles(dllPath, "*.dll", subFolder ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                for (int i = 0; i < dllFiles.Length; i++)
                {

                    AddReferencesFromDllFile(dllFiles[i]);
                }
            }
            else
            {

                HashSet<string> exclude;
                if (excludePaths == default)
                {
                    exclude = new HashSet<string>();
                }
                else
                {
                    exclude = new HashSet<string>(excludePaths);
                }


                var dllPath = Path.GetDirectoryName(path);
                string[] dllFiles = Directory.GetFiles(dllPath, "*.dll", subFolder ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                for (int i = 0; i < dllFiles.Length; i++)
                {
                    if (!exclude.Contains(dllFiles[i]))
                    {
                        AddReferencesFromDllFile(dllFiles[i]);
                    }
                }

            }



        }

#if !NETSTANDARD2_0
        public AssemblyDependencyResolver DependencyResolver;
#endif
        public virtual void AddReferencesFromDepsJsonFile(string path, params string[] excludePaths)
        {

            if (excludePaths.Length == 0)
            {

#if !NETSTANDARD2_0

                DependencyResolver = new AssemblyDependencyResolver(path);
                var newMapping = GetDictionary(DependencyResolver);

                foreach (var item in newMapping)
                {

                    AddReferencesFromDllFile(item.Value);

                }

#endif
            }
            else
            {
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

                DependencyResolver = new AssemblyDependencyResolver(path);
                var newMapping = GetDictionary(DependencyResolver);

                foreach (var item in newMapping)
                {
                    if (!exclude.Contains(item.Value))
                    {
                        AddReferencesFromDllFile(item.Value);
                    }
                }

#endif
            }
        }

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