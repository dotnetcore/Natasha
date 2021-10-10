using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha
{
    /// <summary>
    /// 全局 using, 初始化时会通过 DependencyModel 获取主域所需的引用 using 字符串.
    /// </summary>
    public static class DefaultUsing
    {

        private readonly static HashSet<string> DefaultNamesapce;
        public static StringBuilder DefaultScript;

        static DefaultUsing()
        {

            DefaultScript = new StringBuilder();
            DefaultNamesapce = new HashSet<string>();
            var assemblyNames = DependencyContext.Default.GetDefaultAssemblyNames();
            foreach (var name in assemblyNames)
            {

                var assembly = Assembly.Load(name);
                if (assembly != default)
                {

                    try
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
                    catch (Exception)
                    {

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


        /// <summary>
        /// 查询是否存在该命名空间
        /// </summary>
        /// <param name="namespace"></param>
        /// <returns></returns>
        public static bool HasElement(string @namespace)
        {
            lock (DefaultNamesapce)
            {
                return DefaultNamesapce.Contains(@namespace);
            }
        }


        /// <summary>
        /// 移除命名空间
        /// </summary>
        /// <param name="namespace"></param>
        public static void Remove(string @namespace)
        {
            lock (DefaultNamesapce)
            {
                if (DefaultNamesapce.Contains(@namespace))
                {
                    DefaultNamesapce.Remove(@namespace);
                    DefaultScript = DefaultScript.Replace($"using {@namespace};{Environment.NewLine}", string.Empty);
                }
            }
        }
        public static void Remove(IEnumerable<string> namespaces)
        {

            if (namespaces!=null)
            {
                lock (DefaultNamesapce)
                {
                    DefaultNamesapce.ExceptWith(namespaces);
                    foreach (var item in namespaces)
                    {
                        DefaultScript.Replace($"using {item};{Environment.NewLine}", string.Empty);
                    }
                }
            }
            
        }
    }

}
