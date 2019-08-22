using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
using Natasha.Template;
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
        public readonly HashSet<PortableExecutableReference> NewReferences;
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

                NewReferences = new HashSet<PortableExecutableReference>(_ref);
            }
            else
            {
                NewReferences = new HashSet<PortableExecutableReference>();
            }


            this.Unloading += AssemblyDomain_Unloading;

        }




        private void AssemblyDomain_Unloading(AssemblyLoadContext obj)
        {
            //throw new NotImplementedException();
        }




        public void RemoveReference(string name)
        {
            if (NameMapping.ContainsKey(name))
            {
                NewReferences.Remove(NameMapping[name]);
            }
            
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
            for (int i = 0; i < types.Length; i++)
            {

                TypeMapping[types[i].Name] = assembly;
                NameMapping[types[i].Name] = metadata;

            }

            if (stream != null)
            {

                NewReferences.Add(metadata);

            }

        }




        public void LoadFile(string path)
        {

            if (!OutfileMapping.ContainsKey(path))
            {

                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    CacheAssembly(LoadFromStream(stream), stream);
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
