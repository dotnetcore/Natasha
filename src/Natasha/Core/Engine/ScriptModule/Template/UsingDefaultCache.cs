using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Natasha.Template
{
    public static class UsingDefaultCache
    {
        public static HashSet<string> Default;

        static UsingDefaultCache()
        {

            Default = new HashSet<string>();
            var assemblyNames = DependencyContext.Default.GetDefaultAssemblyNames();
            foreach (var name in assemblyNames)
            {
                var assembly = Assembly.Load(name);
                if (assembly != default)
                {
                    var types = assembly.GetTypes();

                    foreach (var item1 in types)
                    {
                        item1.GetDevelopName();
                        if (!Default.Contains(item1.Namespace) && item1.Namespace != default)
                        {
                            Default.Add(item1.Namespace);
                        }

                    }
                }
               

            }

        }
    }
}
