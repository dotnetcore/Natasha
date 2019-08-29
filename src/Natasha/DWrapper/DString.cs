using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Natasha
{
    public static class DString
    {


#if !NETSTANDARD2_0
        public static void Deconstruct(
            this string path,
            out Assembly Assembly,
            out ConcurrentDictionary<string,Type> TypeCache)
        {

            if (File.Exists(path))
            {
                var domain = (AssemblyDomain)(AssemblyLoadContext.CurrentContextualReflectionContext);
                Assembly = domain.LoadFile(path);
                TypeCache = new ConcurrentDictionary<string, Type>();
                foreach (var item in Assembly.GetTypes())
                {
                    TypeCache[item.GetDevelopName()] = item;
                }
            }
            else
            {
                throw new Exception("Can't find file.");
            }
            
        }




        public static void Deconstruct(
           this string script,
           out Assembly Assembly,
           out Type[] Types,
           out CompilationException Error)
        {

            AssemblyComplier assembly = new AssemblyComplier();
            assembly.Add(script);
            Assembly = assembly.GetAssembly();
            Types = Assembly.GetTypes();
            Error = assembly.Exception;

        }
#endif

    }

}
