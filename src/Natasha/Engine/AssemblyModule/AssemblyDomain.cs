using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
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

        public readonly ConcurrentDictionary<string, Assembly> TypeMapping;
        public readonly ConcurrentDictionary<string, Assembly> OutfileMapping;
        public readonly ConcurrentDictionary<string, PortableExecutableReference> NameMapping;
        public readonly HashSet<PortableExecutableReference> References;
        public bool CanCover;



        public AssemblyDomain(string key)
#if NETCOREAPP3_0
            : base(isCollectible: true,name:key)
#endif

        {


            TypeMapping = new ConcurrentDictionary<string, Assembly>();
            OutfileMapping = new ConcurrentDictionary<string, Assembly>();
            NameMapping = new ConcurrentDictionary<string, PortableExecutableReference>();
            CanCover = true;


            if (key == "Default")
            {
                var _ref = DependencyContext.Default.CompileLibraries
                                .SelectMany(cl => cl.ResolveReferencePaths())
                                .Select(asm => MetadataReference.CreateFromFile(asm));

                References = new HashSet<PortableExecutableReference>(_ref);
            }
            else
            {
                References = new HashSet<PortableExecutableReference>();
            }


            this.Unloading += AssemblyDomain_Unloading;

        }




        private void AssemblyDomain_Unloading(AssemblyLoadContext obj)
        {
            //throw new NotImplementedException();
        }




        public bool RemoveReferenceByClassName(string name)
        {

            if (TypeMapping.ContainsKey(name))
            {

                return RemoveReferenceByAssembly(TypeMapping[name]);

            }

            return false;
            
        }




        public bool RemoveReferenceByAssembly(Assembly assembly)
        {

            if (assembly == default)
            {
                throw new NullReferenceException("This method can't be passed a null instance.");
            }


            if (NameMapping.ContainsKey(assembly.FullName))
            {

                References.Remove(NameMapping[assembly.FullName]);
                foreach (var item in assembly.GetTypes())
                {
                    TypeMapping.TryRemove(item.Name, out Assembly result);
                }
               
                return true;

            }

            return false;

        }




        public void Dispose()
        {

#if NETCOREAPP3_0
            NewReferences.Clear();
#endif
            TypeMapping.Clear();
            OutfileMapping.Clear();

        }




        protected override Assembly Load(AssemblyName assemblyName)
        {

            return null;

        }





        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {

            return IntPtr.Zero;

        }




        /// <summary>
        /// 缓存表的原子操作，缓存程序集，并写入引用表
        /// </summary>
        /// <param name="assembly">新程序集</param>
        /// <param name="stream">程序集流</param>
        public void CacheAssembly(Assembly assembly,Stream stream = null)
        {

            if (stream !=null)
            {

                //生成引用表
                stream.Position = 0;
                var metadata = MetadataReference.CreateFromStream(stream);


                //添加引用表
                NameMapping[assembly.FullName] = metadata;
                References.Add(metadata);


                //添加类型-程序集映射
                var types = assembly.GetTypes();
                for (int i = 0; i < types.Length; i++)
                {

                    TypeMapping[types[i].Name] = assembly;

                }

            }

        }




        /// <summary>
        /// 使用外部文件加载程序集
        /// </summary>
        /// <param name="path">dll文件路径</param>
        /// <param name="isCover">是否覆盖原有的同路径的dll</param>
        /// <returns></returns>
        public Assembly LoadFile(string path, bool  isCover = false)
        {

            Assembly assembly = default;
            if (!OutfileMapping.ContainsKey(path))
            {

                //缓存中没有加载该路劲的文件
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    assembly = LoadFromStream(stream);
                    CacheAssembly(LoadFromStream(stream), stream);
                }

            }
            else if (isCover)
            {
                
                //覆盖引用表，使用新的程序集
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    assembly = LoadFromStream(stream);
                    RemoveReferenceByAssembly(assembly);
                    CacheAssembly(assembly, stream);
                }

            }

            return assembly;

        }




        public Assembly GetDynamicAssembly(string className)
        {

            if (TypeMapping.ContainsKey(className))
            {

                return TypeMapping[className];

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

            if (TypeMapping.ContainsKey(name))
            {

                return TypeMapping[name].GetTypes().First(item => item.Name == name);

            }
            return null;
            
        }



#if NETCOREAPP3_0
        public T Execute<T>(Func<T,T> action) where T: TemplateRecoder<T>, new()
        {

            return action?.Invoke(new T()).InDomain(this);

        }
#endif

    }

}
