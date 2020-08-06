using Microsoft.CodeAnalysis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;


namespace Natasha.Framework
{

    //#if NET472 || NET461 || NET462
    //    public abstract class AssemblyLoadContext : Assembly
    //    {
    //        public readonly string Name;
    //        public AssemblyLoadContext(string name,bool )
    //    }
    //#endif

    /// <summary>
    /// 程序域的基础类，需要继承实现
    /// </summary>
    public abstract class DomainBase : AssemblyLoadContext, IDisposable
    {

        public static DomainBase DefaultDomain;
        public event Action<Assembly> AddAssemblyEvent;
        public event Action<Assembly> RemoveAssemblyEvent;
        /// <summary>
        /// 是否使用最新程序集
        /// </summary>
        public bool UseNewVersionAssmebly;

        /// <summary>
        /// 返回当前域的引用元素 C# 里如 using [xxx];
        /// </summary>
        /// <returns></returns>
        public abstract HashSet<string> GetReferenceElements();

        /// <summary>
        /// 存放内存流编译过来的程序集与引用
        /// </summary>
        public readonly ConcurrentDictionary<Assembly, PortableExecutableReference> ReferencesFromStream;

        /// <summary>
        /// 从插件加载来的程序集
        /// </summary>
        public readonly ConcurrentDictionary<string, Assembly> DllAssemblies;

        /// <summary>
        /// 存放文件过来的程序集与引用
        /// </summary>
        public readonly ConcurrentDictionary<string, PortableExecutableReference> ReferencesFromFile;
#if NETSTANDARD2_0
        public string Name;
#else
        public readonly static Func<AssemblyDependencyResolver, Dictionary<string, string>> GetDictionary;
#endif
        static DomainBase()
        {
#if !NETSTANDARD2_0
            //从依赖中拿到所有的路径，该属性未开放
            var methodInfo = typeof(AssemblyDependencyResolver).GetField("_assemblyPaths", BindingFlags.NonPublic | BindingFlags.Instance);
            GetDictionary = item => (Dictionary<string, string>)methodInfo.GetValue(item);
#endif
            _shareLibraries = new ConcurrentQueue<string>();
            var assemblies = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(asm => !asm.IsDynamic && !string.IsNullOrWhiteSpace(asm.Location))
                .Distinct();

            foreach (var asm in assemblies)
            {

                _shareLibraries.Enqueue(asm.Location);

            }

        }



        /// <summary>
        /// 构造函数，需要给当前域传key，兼容standard2.0
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
            DllAssemblies = new ConcurrentDictionary<string, Assembly>();
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
        /// 共享库引用缓存
        /// </summary>
        private static readonly ConcurrentQueue<string> _shareLibraries;


        /// <summary>
        /// 使用共享库覆盖现有引用缓存
        /// </summary>
        public void UseShareLibraries()
        {

            foreach (var asmPath in _shareLibraries)
            {

                AddReferencesFromDllFile(asmPath);

            }

        }


        /// <summary>
        /// 获取系统域+当前域的所有引用
        /// </summary>
        /// <returns></returns>
        public abstract HashSet<PortableExecutableReference> GetCompileReferences();


        /// <summary>
        /// 内存编译流过来之后需要如何处理
        /// </summary>
        /// <param name="stream">编译后的内存流</param>
        /// <param name="AssemblyName">程序集名</param>
        /// <returns></returns>
        public abstract Assembly CompileStreamHandler(Stream stream, string AssemblyName);


        /// <summary>
        /// 文件编译流过来之后需要如何处理
        /// </summary>
        /// <param name="dllFile">编译后的文件路径</param>
        /// <param name="pdbFile">编译后的PDB文件</param>
        /// <param name="AssemblyName">程序集名</param>
        /// <returns></returns>
        public abstract Assembly CompileFileHandler(string dllFile, string pdbFile, string AssemblyName);


        /// <summary>
        /// 如何加载从文件过来的插件
        /// </summary>
        /// <param name="path">插件路径</param>
        /// <param name="excludePaths">需要排除的引用路径</param>
        /// <returns></returns>
        public abstract Assembly LoadPluginFromFile(string path, params string[] excludePaths);


        /// <summary>
        /// 如何加载从内存过来的插件
        /// </summary>
        /// <param name="path">插件路径</param>
        /// <param name="excludePaths">需要排除的引用路径</param>
        /// <returns></returns>
        public abstract Assembly LoadPluginFromStream(string path, params string[] excludePaths);


        /// <summary>
        /// 移除文件对应的引用
        /// </summary>
        /// <param name="path">文件路径，插件或者生成的DLL</param>
        public virtual void Remove(string path)
        {
            if (path != null)
            {
                if (DllAssemblies.ContainsKey(path))
                {

                    var shortName = Path.GetFileNameWithoutExtension(path);
                    while (!ReferencesFromFile.TryRemove(shortName, out _)) { }
                    Assembly assembly;
                    while (!DllAssemblies.TryRemove(path, out assembly)) { }
                    RemoveAssemblyEvent?.Invoke(assembly);
                }
            }
        }


        /// <summary>
        /// 移除程序集对应的引用
        /// </summary>
        /// <param name="assembly">程序集</param>
        public virtual void Remove(Assembly assembly)
        {
            if (assembly != null)
            {
                if (ReferencesFromStream.ContainsKey(assembly))
                {
                    while (!ReferencesFromStream.TryRemove(assembly, out _)) { }
                    RemoveAssemblyEvent?.Invoke(assembly);
                }
                else if (assembly.Location != "" && assembly.Location != default)
                {
                    Remove(assembly.Location);
                }
            }
        }


        /// <summary>
        /// 当前域引用的数量
        /// </summary>
        public int Count
        {
            get { return ReferencesFromStream.Count + ReferencesFromFile.Count; }
        }


        /// <summary>
        /// 销毁函数
        /// </summary>
        public virtual void Dispose()
        {
#if !NETSTANDARD2_0
            DependencyResolver = null;
#endif
            DllAssemblies.Clear();
            ReferencesFromStream.Clear();
            ReferencesFromFile.Clear();
        }


        /// <summary>
        /// 根据DLL路径添加单个的引用，以文件方式加载
        /// </summary>
        /// <param name="path">DLL文件路径</param>
        /// <param name="excludePaths">需要排除的依赖文件路径</param>
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


        /// <summary>
        /// 根据DLL路径添加单个的引用，以流的方式加载
        /// </summary>
        /// <param name="path">DLL文件路径</param>
        /// <param name="excludePaths">需要排除的依赖文件路径</param>
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


        /// <summary>
        /// 将程序集和内存流添加到引用缓存
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="stream">内存流</param>
        public virtual void AddReferencesFromAssemblyStream(Assembly assembly, Stream stream)
        {

            ReferencesFromStream[assembly] = MetadataReference.CreateFromStream(stream);

        }


        /// <summary>
        /// 扫描文件夹，并将文件夹下的DLL文件添加到引用
        /// </summary>
        /// <param name="path">DLL文件路径</param>
        /// <param name="subFolder">是否递归查询所有文件夹</param>
        /// <param name="excludePaths">需要排除的依赖文件路径</param>
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
        /// <summary>
        /// 解析devjson文件添加引用
        /// </summary>
        /// <param name="path">devjson文件路径</param>
        /// <param name="excludePaths">需要排除的依赖文件路径</param>
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
            Assembly assembly;
            if (Name == "Default")
            {
                assembly = Default.LoadFromAssemblyPath(path);
            }
            else
            {
                assembly = LoadFromAssemblyPath(path);
            }
            AddAssemblyEvent?.Invoke(assembly);
            return assembly;
        }


        /// <summary>
        /// 将流转换为程序集，并加载到域
        /// [手动释放]
        /// </summary>
        /// <param name="stream">外部流</param>
        /// <returns></returns>
        public virtual Assembly LoadAssemblyFromStream(Stream stream)
        {
            Assembly assembly;
            if (Name == "Default")
            {
                assembly = Default.LoadFromStream(stream);
            }
            else
            {
                assembly = LoadFromStream(stream);
            }
            AddAssemblyEvent?.Invoke(assembly);
            return assembly;
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