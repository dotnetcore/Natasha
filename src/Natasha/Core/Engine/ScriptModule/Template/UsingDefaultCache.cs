using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha.Template
{

    public static class UsingDefaultCache
    {

        public readonly static HashSet<string> DefaultNamesapce;
        public readonly static StringBuilder DefaultScript;

        static UsingDefaultCache()
        {

            DefaultScript = new StringBuilder();
            DefaultNamesapce = new HashSet<string>();
            var assemblyNames = DependencyContext.Default.GetDefaultAssemblyNames();
            foreach (var name in assemblyNames)
            {

                var assembly = Assembly.Load(name);
                if (assembly != default)
                {

                    var types = assembly.ExportedTypes;
                    foreach (var item in types)
                    {
                        
                        if (!DefaultNamesapce.Contains(item.Namespace) && item.Namespace != default)
                        {
                            DefaultNamesapce.Add(item.Namespace);
                        }

                    }
                }

            }

            var entryTypes = Assembly.GetEntryAssembly().GetTypes();
            foreach (var item in entryTypes)
            {

                if (!DefaultNamesapce.Contains(item.Namespace) && item.Namespace != default)
                {
                    DefaultNamesapce.Add(item.Namespace);
                }

            }

            foreach (var @using in DefaultNamesapce)
            {
                DefaultScript.AppendLine($"using {@using};");
            }

           
        }


        public static void Remove(string @namespace)
        {
            lock (DefaultNamesapce)
            {
                if (DefaultNamesapce.Contains(@namespace))
                {
                    DefaultNamesapce.Remove(@namespace);
                    DefaultScript.Replace($"using {@namespace};", string.Empty);
                }
            }
        }

    }

}
