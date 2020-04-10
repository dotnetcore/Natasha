using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
using Natasha.Core;
using Natasha.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Natasha
{
    public class AssemblyDomain : DomainBase
    {
        public readonly ShortNameReference ShortReferences;

        public AssemblyDomain()
        {
        }

        static AssemblyDomain()
        {
            var defaultDomain = DomainManagement.RegisterDefault<AssemblyDomain>();

            foreach (var asm in DependencyContext
                .Default
                .CompileLibraries
                .SelectMany(cl => cl.ResolveReferencePaths()))
            {
                defaultDomain.ShortReferences.Add(asm);
            }
        }

        /// <summary>
        /// 让父类通过此方法获取当前类型域
        /// </summary>
        /// <returns></returns>
        public override DomainBase GetInstance(string name)
        {
            return new AssemblyDomain(name);
        }

        /// <summary>
        /// 从外部文件获取程序集，并添加引用信息
        /// </summary>
        /// <param name="path">文件路径</param>
        public Assembly AddFromFile(string path)
        {
            var assembly = GetFromFile(path);
            if (assembly != null)
            {
                AssemblyReferences[assembly] = MetadataReference.CreateFromFile(path);
            }
            return assembly;
        }

        /// <summary>
        /// 从外部文件以流的方式获取程序集，并添加引用信息
        /// [自动释放]
        /// </summary>
        /// <param name="path">外部文件</param>
        public Assembly AddFromStream(string path)
        {
            return AddFromStream(new FileStream(path, FileMode.Open, FileAccess.Read));
        }

        /// <summary>
        /// 以流的方式获取程序集，并添加引用信息
        /// [自动释放]
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns></returns>
        public Assembly AddFromStream(Stream stream)
        {
            using (stream)
            {
                var assembly = GetFromStream(stream);
                if (assembly != null)
                {
                    stream.Position = 0;
                    AssemblyReferences[assembly] = MetadataReference.CreateFromStream(stream);
                }
                return assembly;
            }
        }

        /// <summary>
        /// 将文件转换为程序集，并加载到域
        /// </summary>
        /// <param name="path">外部文件</param>
        /// <returns></returns>
        internal Assembly GetFromFile(string path)
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
        internal Assembly GetFromStream(Stream stream)
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
        internal Assembly GetFromStream(string path)
        {
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return GetFromStream(stream);
            }
        }

        /// <summary>
        /// 获取编译所需的引用库
        /// </summary>
        /// <returns></returns>
        public override HashSet<PortableExecutableReference> GetCompileReferences()
        {
            var sets = base.GetCompileReferences();
            if (Name != "Default")
            {
                //拿到系统域
                var @default = DomainManagement.Default;
                //将系统域的短名引用缓存起来
                var dict = new Dictionary<string, PortableExecutableReference>(((AssemblyDomain)@default).ShortReferences.References);
                foreach (var item in ShortReferences.References)
                {
                    //用当前域的引用集合覆盖系统域的集合
                    dict[item.Key] = item.Value;
                }
                //将系统域的程序集引用加载进来
                sets.UnionWith(@default.GetCompileReferences());
                //将处理后的名引用加载进来
                sets.UnionWith(dict.Values);
            }
            else
            {
                //如果是系统域则直接拼接自己的引用库
                sets.UnionWith(ShortReferences.References.Values);
            }
            return sets;
        }

        /// <summary>
        /// 当拿到动态编译生成的文件时调用
        /// </summary>
        /// <param name="dllFile">dll文件位置</param>
        /// <param name="pdbFile">pdb文件位置</param>
        /// <param name="AssemblyName">程序集名</param>
        /// <returns></returns>
        public override Assembly CompileFileHandler(string dllFile, string pdbFile, string AssemblyName)
        {
            return AddFromFile(dllFile);
        }

        /// <summary>
        /// 当拿到动态编译生成的流时调用
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="AssemblyName">程序集名</param>
        /// <returns></returns>
        public override Assembly CompileStreamHandler(Stream stream, string AssemblyName)
        {
            return AddFromStream(stream);
        }

        /// <summary>
        /// 移除对应的短名程序集引用，移除插件方式加载的引用才会用到该方法
        /// </summary>
        /// <param name="path">文件路径</param>
        public override void Remove(string path)
        {
            if (path != null)
            {
                ShortReferences.Remove(path);
            }
        }

        /// <summary>
        /// 移除程序集对应的引用
        /// 优先：移除动态编译的引用
        /// 其次：移除插件加载方式的引用
        /// </summary>
        /// <param name="assembly"></param>
        public override void Remove(Assembly assembly)
        {
            if (assembly != null)
            {
                if (AssemblyReferences.ContainsKey(assembly))
                {
                    while (!AssemblyReferences.TryRemove(assembly, out _)) { }
                }
                else if (assembly.Location != "" && assembly.Location != default)
                {
                    Remove(assembly.Location);
                }
            }
        }

        public AssemblyDomain(string key) : base(key)
        {
#if !NETSTANDARD2_0
            _load_resolver = new AssemblyDependencyResolver(AppDomain.CurrentDomain.BaseDirectory);
#endif

            ShortReferences = new ShortNameReference();
            DomainManagement.Add(key, this);
        }

        public override void Dispose()
        {
#if !NETSTANDARD2_0
            _load_resolver = null;
#endif
            ShortReferences.Dispose();
            base.Dispose();
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
                return GetFromStream(assemblyPath);
            }
#endif
            return null;
        }

        public override Assembly Default_Resolving(AssemblyLoadContext arg1, AssemblyName arg2)
        {
            return Load(arg2);
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
#if !NETSTANDARD2_0
            string libraryPath = _load_resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }
#endif
            return IntPtr.Zero;
        }

        public override IntPtr Default_ResolvingUnmanagedDll(Assembly arg1, string arg2)
        {
            return LoadUnmanagedDll(arg2);
        }

        /// <summary>
        /// 使用外部文件加载程序集
        /// </summary>
        /// <param name="path">dll文件路径</param>
        /// <returns></returns>
        public override Assembly LoadPluginFromFile(string path, params string[] excludePaths)
        {
            AddNewShortNameReference(path, excludePaths);
            return GetFromFile(path);
        }

        /// <summary>
        /// 使用外部文件流加载程序集
        /// [自动释放]
        /// </summary>
        /// <param name="path"></param>
        /// <param name="excludePaths"></param>
        /// <returns></returns>
        public override Assembly LoadPluginFromStream(string path, params string[] excludePaths)
        {
            AddNewShortNameReference(path, excludePaths);
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return GetFromStream(stream);
            }
        }

        /// <summary>
        /// 使用外部文件路径添加引用
        /// </summary>
        /// <param name="path"></param>
        /// <param name="excludePaths"></param>
        public void AddNewShortNameReference(string path, params string[] excludePaths)
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

            _load_resolver = new AssemblyDependencyResolver(path);
            var newMapping = GetDictionary(_load_resolver);

            foreach (var item in newMapping)
            {
                if (!exclude.Contains(item.Value))
                {
                    ShortReferences.Add(item.Value);
                }
            }

#else

            var dllPath = Path.GetDirectoryName(path);
            string[] dllFiles = Directory.GetFiles(dllPath, "*.dll", SearchOption.AllDirectories);
            for (int i = 0; i < dllFiles.Length; i++)
            {
                if (!exclude.Contains(dllFiles[i]))
                {
                    ShortReferences.Add(dllFiles[i]);
                }
            }
#endif
        }
    }
}