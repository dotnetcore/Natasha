using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
#if DEBUG
            Stopwatch stopwatch = new();
            stopwatch.Start();
#endif


            IEnumerable<string> paths = DependencyContext
                .Default
                .CompileLibraries.SelectMany(cl => cl.ResolveReferencePaths());

            var domain = DomainManagement.Random;
            foreach (var item in paths)
            {
                try
                {
                    Assembly? assembly = domain.LoadFromAssemblyPath(item);
                    //#if DEBUG
                    //                    Console.WriteLine($"[内部加载]:{item}");
                    //#endif
                    if (assembly != null)
                    {
                        try
                        {
                            var types = assembly.GetTypes();
                            foreach (var type in types)
                            {
                                if (!string.IsNullOrEmpty(type.Namespace))
                                {
                                    DefaultNamesapce.Add(type.Namespace);
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                }
                catch (Exception ex)
                {

                    //                    Assembly? assembly = Assembly.LoadFrom(item);
                    //                    if (assembly != null)
                    //                    {
                    //#if DEBUG
                    //                        Console.WriteLine($"[外部加载]:{item}");
                    //#endif
                    //                        try
                    //                        {
                    //                            var types = assembly.GetTypes();
                    //                            foreach (var type in types)
                    //                            {
                    //                                if (!string.IsNullOrEmpty(type.Namespace))
                    //                                {
                    //                                    DefaultNamesapce.Add(type.Namespace);
                    //                                }
                    //                            }
                    //                        }
                    //                        catch
                    //                        {

                    //                        }
                    //                    }
                }

            }
            domain.Dispose();
            foreach (var @using in DefaultNamesapce)
            {
                DefaultScript.AppendLine($"using {@using};");
            }
#if DEBUG
            stopwatch.StopAndShowCategoreInfo("[DefaultUsing]", "全部 Using 准备", 1);
#endif

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
