#if NETCOREAPP3_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 全局 using, 初始化时会通过 DependencyModel 获取主域所需的引用 using 字符串.
/// </summary>
public static class GlobalUsingHelper
{
    private static Func<AssemblyName?, string?, bool>? _excludeDefaultAssembliesFunc;

    public static void SetDefaultUsingFilter(Func<AssemblyName?, string?, bool>? excludeDefaultAssembliesFunc)
    {
        _excludeDefaultAssembliesFunc = excludeDefaultAssembliesFunc;
    }
    /// <summary>
    /// for reference assembly
    /// </summary>
    /// <param name="domain"></param>
    /// <param name="assembly"></param>
    internal static void AddUsingWithoutCheck(NatashaReferenceDomain domain, Assembly assembly)
    {
        try
        {
            var tempSets = new HashSet<string>();
            var types = assembly.ExportedTypes;
            if (types.Count() > 16)
            {
                var result = Parallel.ForEach(types, type =>
                {

                    if (type.IsNested && !type.IsNestedPublic)
                    {
                        return;
                    }

                    var name = type.Namespace;
                    lock (tempSets)
                    {
                        if (tempSets.Contains(name))
                        {
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(name)
                    && name.IndexOf('<') == -1)
                    {
                        if (_excludeDefaultAssembliesFunc == null || !_excludeDefaultAssembliesFunc(null, name))
                        {
                            lock (tempSets)
                            {
                                tempSets.Add(name);
                            }
                        }
#if DEBUG
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("[排除程序集]:" + name);
                        }
#endif

                    }
                });
                while (!result.IsCompleted)
                {
                    Thread.Sleep(100);
                }
            }
            else
            {
                foreach (var type in types)
                {

                    if (type.IsNested && !type.IsNestedPublic)
                    {
                        continue;
                    }

                    var name = type.Namespace;


                    if (!string.IsNullOrEmpty(name)
                        && !tempSets.Contains(name)
                        && name.IndexOf('<') == -1)
                    {

                        if (_excludeDefaultAssembliesFunc == null || !_excludeDefaultAssembliesFunc(null, name))
                        {

                            tempSets.Add(name);
                        }
#if DEBUG
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("[排除程序集]:" + name);
                        }
#endif
                    }
                }
            }

            //*/

            lock (domain.UsingRecorder)
            {
                foreach (var name in tempSets)
                {
                    domain.UsingRecorder.Using(name);
                }
            }

        }
        catch (Exception ex)
        {
#if DEBUG
            Console.WriteLine(assembly.FullName + ex.Message);
#endif
        }
    }
    /// <summary>
    /// for runtime assembly
    /// </summary>
    /// <param name="domain"></param>
    /// <param name="assembly"></param>
    internal static void AddUsingWithoutCheckingkAndInternalUsing(NatashaReferenceDomain domain, Assembly assembly)
    {
        try
        {
            var tempSets = new HashSet<string>();
            var types = assembly.ExportedTypes;
            if (types.Count() > 16)
            {
                var result = Parallel.ForEach(types, type =>
                {

                    if (type.IsNested && !type.IsNestedPublic)
                    {
                        return;
                    }

                    var name = type.Namespace;

                    lock (tempSets)
                    {
                        if (tempSets.Contains(name))
                        {
                            return;
                        }
                    }

                    if (!string.IsNullOrEmpty(name)
                    && !name.StartsWith("Internal")
                    && name.IndexOf('<') == -1)
                    {

                        if (_excludeDefaultAssembliesFunc == null || !_excludeDefaultAssembliesFunc(null, name))
                        {
                            lock (tempSets)
                            {
                                tempSets.Add(name);
                            }
                        }
#if DEBUG
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("[排除程序集]:" + name);
                        }
#endif
                    }
                });
                while (!result.IsCompleted)
                {
                    Thread.Sleep(100);
                }
            }
            else
            {
                foreach (var type in types)
                {

                    if (type.IsNested && !type.IsNestedPublic)
                    {
                        continue;
                    }

                    var name = type.Namespace;
                    if (!string.IsNullOrEmpty(name)
                        && !name.StartsWith("Internal")
                        && !tempSets.Contains(name)
                        && name.IndexOf('<') == -1)
                    {

                        if (_excludeDefaultAssembliesFunc == null || !_excludeDefaultAssembliesFunc(null, name))
                        {
                            tempSets.Add(name);
                        }
#if DEBUG
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("[排除程序集]:" + name);
                        }
#endif
                    }
                }
            }

            //*/

            lock (domain.UsingRecorder)
            {
                foreach (var name in tempSets)
                {
                    domain.UsingRecorder.Using(name);
                }
            }

        }
        catch (Exception ex)
        {
#if DEBUG
            Console.WriteLine(assembly.FullName + ex.Message);
#endif
        }
    }

}
#endif