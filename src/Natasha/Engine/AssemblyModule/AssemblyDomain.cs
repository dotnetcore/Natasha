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

                return RemoveReferenceByClassName(TypeMapping[name]);

            }

            return false;
            
        }




        public bool RemoveReferenceByClassName(Assembly assembly)
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




        public void CacheAssembly(Assembly assembly,Stream stream = null)
        {

            var types = assembly.GetTypes();
            stream.Position = 0;
            var metadata = MetadataReference.CreateFromStream(stream);
            NameMapping[assembly.FullName] = metadata;
            for (int i = 0; i < types.Length; i++)
            {

                TypeMapping[types[i].Name] = assembly;

            }


            if (stream != null)
            {

                References.Add(metadata);

            }

        }




        public void LoadFile(string path, bool  isCover = false)
        {

            if (!OutfileMapping.ContainsKey(path))
            {

                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    CacheAssembly(LoadFromStream(stream), stream);
                }

            }
            else if (isCover)
            {
                
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    var assembly = LoadFromStream(stream);
                    CacheAssembly(assembly, stream);
                }

            }


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

            return TypeMapping[name].GetTypes().First(item => item.Name == name);

        }



#if NETCOREAPP3_0
        public T Execute<T>(Func<T,T> action) where T: TemplateRecoder<T>, new()
        {

            return action?.Invoke(new T()).InDomain(this);

        }
#endif

    }

}
