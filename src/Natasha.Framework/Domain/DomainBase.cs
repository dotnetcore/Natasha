using Microsoft.CodeAnalysis;
using Natasha.Framework.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace Natasha.Framework
{

    /// <summary>
    /// 程序域的基础类，需要继承实现
    /// </summary>
    public abstract class DomainBase : AssemblyLoadContext, IDisposable
    {

        public static DomainBase DefaultDomain;
        public event Action<Assembly>? AddAssemblyEvent;
        public event Action<Assembly>? RemoveAssemblyEvent;

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
        public readonly ConcurrentDictionary<Assembly, PortableExecutableReference> AssemblyReferencesCache;


        /// <summary>
        /// 存放文件过来的程序集与引用
        /// </summary>
        public readonly ConcurrentDictionary<string, PortableExecutableReference> OtherReferencesFromFile;


        /// <summary>
        /// 当前域引用的数量
        /// </summary>
        public int Count
        {
            get { return AssemblyReferencesCache.Count + OtherReferencesFromFile.Count; }
        }


        #region 共享库引用
        /// <summary>
        /// 共享库引用缓存
        /// </summary>
        private static readonly ConcurrentQueue<string> _shareLibraries;

        protected HashSet<string> ExcludeAssemblies = new HashSet<string>();
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

        public readonly static Func<AssemblyDependencyResolver, Dictionary<string, string>> GetDictionary;
        static DomainBase()
        {
            //从依赖中拿到所有的路径，该属性未开放

            DefaultDomain = new FakerDomain("init_fake_domain");
            var methodInfo = typeof(AssemblyDependencyResolver).GetField("_assemblyPaths", BindingFlags.NonPublic | BindingFlags.Instance);
            GetDictionary = item => (Dictionary<string, string>)(methodInfo!.GetValue(item)!);
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
        #endregion

        /// <summary>
        /// 构造函数，需要给当前域传key，兼容standard2.0
        /// </summary>
        /// <param name="key"></param>
        public DomainBase(string key)
            : base(isCollectible: true, name: key)

        {

            AssemblyReferencesCache = new ConcurrentDictionary<Assembly, PortableExecutableReference>();
            OtherReferencesFromFile = new ConcurrentDictionary<string, PortableExecutableReference>();
            if (key == "Default")
            {
                if (DefaultDomain.Name == "init_fake_domain")
                {
                    DefaultDomain = this;
                    Default.Resolving += Default_Resolving;
                    Default.ResolvingUnmanagedDll += Default_ResolvingUnmanagedDll;
                }
                else
                {
                    throw new Exception("已经存在默认域,请勿重新创建默认域!");
                }


            }

        }

        #region 功能重载
        /// <summary>
        /// 加载插件
        /// </summary>
        /// <param name="path">插件路径</param>
        /// <param name="excludeAssemblies">需要排除的路径</param>
        /// <returns></returns>
        public abstract Assembly LoadPlugin(string path, params string[] excludeAssemblies);


        /// <summary>
        /// 移除插件
        /// </summary>
        /// <param name="path">插件路径</param>
        public abstract void RemovePlugin(string path);


        /// <summary>
        /// 获取当前域内的插件程序集
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<Assembly> GetPluginAssemblies();


        /// <summary>
        /// 系统域加载事件
        /// </summary>
        /// <param name="arg1">当前域</param>
        /// <param name="arg2">被加载的程序集名</param>
        /// <returns></returns>
        protected abstract Assembly? Default_Resolving(AssemblyLoadContext context, AssemblyName name);


        /// <summary>
        /// 系统域非托管程序集加载事件
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        protected abstract IntPtr Default_ResolvingUnmanagedDll(Assembly arg1, string arg2);


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
        public abstract Assembly CompileStreamCallback(string dllFile, string pdbFile, Stream stream, string AssemblyName);

        #endregion

        #region 移除引用
        /// <summary>
        /// 移除文件对应的引用
        /// </summary>
        /// <param name="path">文件路径，插件或者生成的DLL</param>
        public virtual void RemoveReference(string path)
        {

            if (OtherReferencesFromFile.ContainsKey(path))
            {
                var shortName = Path.GetFileNameWithoutExtension(path);
                OtherReferencesFromFile!.Remove(shortName);
            }
        }



        /// <summary>
        /// 移除程序集对应的引用
        /// </summary>
        /// <param name="assembly">程序集</param>
        public virtual void RemoveReference(Assembly assembly)
        {

            if (AssemblyReferencesCache.ContainsKey(assembly))
            {
                AssemblyReferencesCache!.Remove(assembly);
                RemoveAssemblyEvent?.Invoke(assembly);
            }
            else if (assembly.Location != "" && assembly.Location != default)
            {
                RemoveReference(assembly.Location);
            }

        }
        #endregion

        #region 添加引用

        /// <summary>
        /// 添加该程序集的引用
        /// </summary>
        /// <param name="assembly"></param>
        public virtual void AddReferencesFromAssembly(Assembly assembly)
        {
            if (!assembly.IsDynamic && !string.IsNullOrEmpty(assembly.Location))
            {
                AddReferencesFromDllFile(assembly.Location);
            }
        }


        /// <summary>
        /// 根据DLL路径添加单个引用
        /// </summary>
        /// <param name="path">DLL文件路径</param>
        /// <param name="excludePaths">需要排除的依赖文件路径</param>
        public virtual void AddReferencesFromDllFile(string path, params string[] excludePaths)
        {

            if (excludePaths.Length == 0)
            {
                OtherReferencesFromFile[Path.GetFileNameWithoutExtension(path)] = MetadataReference.CreateFromFile(path);
            }
            else
            {
                HashSet<string> exclude = new(excludePaths);
                if (!exclude.Contains(path))
                {
                    OtherReferencesFromFile[Path.GetFileNameWithoutExtension(path)] = MetadataReference.CreateFromFile(path);
                }
            }

        }


        /// <summary>
        /// 根据DLL路径添加单个引用，以流的方式加载
        /// </summary>
        /// <param name="path">DLL文件路径</param>
        public virtual unsafe void AddReferencesFromImage(Assembly assembly)
        {

            if (assembly.TryGetRawMetadata(out var bytePtr, out var length))
            {
                AssemblyReferencesCache[assembly] = MetadataReference.CreateFromImage((new Span<byte>(bytePtr, length)).ToArray());
            }

        }


        /// <summary>
        /// 根据DLL路径添加单个引用，以流的方式加载
        /// </summary>
        /// <param name="path">DLL文件路径</param>
        /// <param name="excludePaths">需要排除的依赖文件路径</param>
        public virtual void AddReferencesFromFileStream(string path, params string[] excludePaths)
        {

            if (excludePaths.Length == 0)
            {
                OtherReferencesFromFile[Path.GetFileNameWithoutExtension(path)] = MetadataReference.CreateFromStream(new FileStream(path, FileMode.Open, FileAccess.Read));
            }
            else
            {
                HashSet<string> exclude = new(excludePaths);
                if (!exclude.Contains(path))
                {
                    OtherReferencesFromFile[Path.GetFileNameWithoutExtension(path)] = MetadataReference.CreateFromStream(new FileStream(path, FileMode.Open, FileAccess.Read));
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

            AssemblyReferencesCache[assembly] = MetadataReference.CreateFromStream(stream);

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
                string[] dllFiles = Directory.GetFiles(dllPath!, "*.dll", subFolder ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                for (int i = 0; i < dllFiles.Length; i++)
                {

                    AddReferencesFromDllFile(dllFiles[i]);
                }
            }
            else
            {

                HashSet<string> exclude = new(excludePaths);

                var dllPath = Path.GetDirectoryName(path);
                string[] dllFiles = Directory.GetFiles(dllPath!, "*.dll", subFolder ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                for (int i = 0; i < dllFiles.Length; i++)
                {
                    if (!exclude.Contains(dllFiles[i]))
                    {
                        AddReferencesFromDllFile(dllFiles[i]);
                    }
                }

            }



        }



        public AssemblyDependencyResolver? DependencyResolver;

        /// <summary>
        /// 解析devjson文件添加引用
        /// </summary>
        /// <param name="path">devjson文件路径</param>
        /// <param name="excludePaths">需要排除的依赖文件路径</param>
        public virtual void AddReferencesFromDepsJsonFile(string path, params string[] excludePaths)
        {

            if (excludePaths.Length == 0)
            {

                DependencyResolver = new AssemblyDependencyResolver(path);
                var newMapping = GetDictionary(DependencyResolver);

                foreach (var item in newMapping)
                {
                    AddReferencesFromDllFile(item.Value);
                }

            }
            else
            {
                HashSet<string> exclude = new(excludePaths);
                DependencyResolver = new AssemblyDependencyResolver(path);
                var newMapping = GetDictionary(DependencyResolver);

                foreach (var item in newMapping)
                {
                    if (!exclude.Contains(item.Value))
                    {
                        AddReferencesFromDllFile(item.Value);
                    }
                }
            }
        }

        #endregion

        #region 程序集加载

        /// <summary>
        /// 将文件转换为程序集，并加载到域
        /// </summary>
        /// <param name="path">外部文件</param>
        /// <returns></returns>
        public virtual Assembly? LoadAssemblyFromFile(string path)
        {

#if DEBUG
            Debug.WriteLine($"加载路径:{path}.");
#endif
            Assembly assembly;
            if (Name == "Default")
            {
                assembly = Default.LoadFromAssemblyPath(path);
            }
            else
            {
                assembly = LoadFromAssemblyPath(path);
            }

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                AddReferencesFromAssemblyStream(assembly, stream);
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
            using (stream)
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


#if DEBUG
                Debug.WriteLine($"已加载程序集 {assembly.FullName}!");
#endif
                stream.Seek(0, SeekOrigin.Begin);
                AddReferencesFromAssemblyStream(assembly, stream);
                AddAssemblyEvent?.Invoke(assembly);
                return assembly;

            }
        }

        /// <summary>
        /// 对程序集上下文的重载函数，注：系统规定需要重载
        /// </summary>
        /// <param name="assemblyName">程序集名</param>
        /// <returns></returns>
        protected override Assembly? Load(AssemblyName assemblyName)
        {
#if DEBUG

            Console.WriteLine($"[解析]程序集:{assemblyName.Name},全名:{assemblyName.FullName}");
#endif
            if (!string.IsNullOrEmpty(assemblyName.Name))
            {
                if (ExcludeAssemblies.Contains(assemblyName.Name))
                {
#if DEBUG
                    Console.WriteLine($"[排除]程序集:{assemblyName.Name},全名:{assemblyName.FullName}");
#endif
                    return null;
                }
            }
            if (DependencyResolver != null)
            {
                string? assemblyPath = DependencyResolver!.ResolveAssemblyToPath(assemblyName);
                if (assemblyPath != null)
                {
#if DEBUG
                    Console.WriteLine($"[加载依赖]程序集:{assemblyName.Name},全名:{assemblyName.FullName}");
#endif
                    return LoadAssemblyFromStream(assemblyPath);
                }
            }
            
            return null;

        }


        /// <summary>
        /// 对程序集上下文非托管插件的函数重载，注：系统规定需要重载
        /// </summary>
        /// <param name="unmanagedDllName">路径</param>
        /// <returns></returns>
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {

            string? libraryPath = DependencyResolver!.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }
            return IntPtr.Zero;
        }


        /// <summary>
        /// 将文件流转换为程序集，并加载到域
        /// [自动释放]
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual Assembly LoadAssemblyFromStream(string path)
        {
#if DEBUG
            Debug.WriteLine($"加载路径:{path}.");
#endif
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return LoadAssemblyFromStream(stream);
            }
        }

        #endregion

        /// <summary>
        /// 销毁函数
        /// </summary>
        public virtual void Dispose()
        {
            DependencyResolver = null;
            AssemblyReferencesCache.Clear();
            OtherReferencesFromFile.Clear();
        }

    }
}