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
public static class DefaultUsing
{

    internal readonly static HashSet<string> _defaultNamesapce;
    private static Func<AssemblyName, string?, bool> _excludeDefaultAssembliesFunc;
    private static StringBuilder _usingScriptCache;
    public static string UsingScript;
    static DefaultUsing()
    {
        UsingScript = string.Empty;
        _usingScriptCache = new StringBuilder();
        _defaultNamesapce = new HashSet<string>();
        _excludeDefaultAssembliesFunc = (_, _) => false;

    }

    public static void SetDefaultUsingFilter(Func<AssemblyName, string?, bool> excludeDefaultAssembliesFunc)
    {
        _excludeDefaultAssembliesFunc = excludeDefaultAssembliesFunc;
    }

    public static int Count { get { return _defaultNamesapce.Count; } }

    internal static void AddUsing(IEnumerable<string> usings, bool autoRebuildScript = true)
    {
        try
        {
            lock (_defaultNamesapce)
            {
                foreach (var name in usings)
                {
                    if (name.IndexOf('<') == -1)
                    {
                        _defaultNamesapce.Add(name);
                        _usingScriptCache.AppendLine($"using {name!};");
                    }
                }
                if (autoRebuildScript)
                {
                    UsingScript = _usingScriptCache.ToString();
                }
            }
        }
        catch (Exception ex)
        {
#if DEBUG
            Console.WriteLine(ex.Message);
#endif
        }
    }

    internal static void AddUsingWithoutCheck(Assembly assembly, bool autoRebuildScript = true)
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
                        if (!_excludeDefaultAssembliesFunc(default!, name))
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

                        if (!_excludeDefaultAssembliesFunc(default!, name))
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

            lock (_defaultNamesapce)
            {
                foreach (var name in tempSets)
                {
                    if (_defaultNamesapce.Add(name))
                    {
                        _usingScriptCache.AppendLine($"using {name};");
                    }
                }
                if (autoRebuildScript)
                {
                    UsingScript = _usingScriptCache.ToString();
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

    internal static void AddUsingWithoutCheckingkAndInternalUsing(Assembly assembly, bool autoRebuildScript = true)
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

                        if (!_excludeDefaultAssembliesFunc(default!, name))
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

                        if (!_excludeDefaultAssembliesFunc(default!, name))
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

            lock (_defaultNamesapce)
            {
                foreach (var name in tempSets)
                {
                    if (_defaultNamesapce.Add(name))
                    {
                        _usingScriptCache.AppendLine($"using {name};");
                    }
                }
                if (autoRebuildScript)
                {
                    UsingScript = _usingScriptCache.ToString();
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
    /// 添加引用
    /// </summary>
    /// <param name="assembly"></param>
    public static void AddUsing(Assembly assembly, bool autoRebuildScript = true)
    {
        try
        {
            var types = assembly.ExportedTypes;
            lock (_defaultNamesapce)
            {
                foreach (var type in types)
                {
                    if (type.IsNested && !type.IsNestedPublic)
                    {
                        continue;
                    }

                    var name = type.Namespace;
                    if (!string.IsNullOrEmpty(name)
                        && name.IndexOf('<') == -1
                        && !_defaultNamesapce.Contains(name)
                        )
                    {

                        if (!_excludeDefaultAssembliesFunc(default!, name))
                        {

                            _defaultNamesapce.Add(name);
                            _usingScriptCache.AppendLine($"using {name!};");

                        }
#if DEBUG
                        else
                        {

                            System.Diagnostics.Debug.WriteLine("[排除程序集]:" + name);

                        }
#endif

                    }
                }
                if (autoRebuildScript)
                {
                    UsingScript = _usingScriptCache.ToString();
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

    public static void ReBuildUsingScript()
    {
        UsingScript = _usingScriptCache.ToString();
    }

    /// <summary>
    /// 添加引用
    /// </summary>
    /// <param name="assemblyName"></param>
    internal static void AddUsing(AssemblyName assemblyName, bool autoRebuildScript = true)
    {
        try
        {
            lock (_defaultNamesapce)
            {
                var name = assemblyName.Name;
                if (!string.IsNullOrEmpty(name) && name.IndexOf('<') == -1 && !_defaultNamesapce.Contains(name))
                {
                    _defaultNamesapce.Add(name);
                    _usingScriptCache.AppendLine($"using {name};");
                    if (autoRebuildScript)
                    {
                        UsingScript = _usingScriptCache.ToString();
                    }
                }

            }
        }
        catch (Exception ex)
        {
#if DEBUG
            Console.WriteLine(assemblyName.FullName + ex.Message);
#endif
        }
    }

    public static void AddUsing(bool autoRebuildScript = true, params string[] namespaceText)
    {
        lock (_defaultNamesapce)
        {
            for (int i = 0; i < namespaceText.Length; i++)
            {
                var name = namespaceText[i];
                if (!string.IsNullOrEmpty(name) && name.IndexOf('<') == -1 && !_defaultNamesapce.Contains(name))
                {
                    _defaultNamesapce.Add(name);
                    _usingScriptCache.AppendLine($"using {name};");
                    if (autoRebuildScript)
                    {
                        UsingScript = _usingScriptCache.ToString();
                    }
                }
            }


        }
    }

    /// <summary>
    /// 查询是否存在该命名空间
    /// </summary>
    /// <param name="namespace"></param>
    /// <returns></returns>
    public static bool HasElement(string @namespace)
    {
        lock (_defaultNamesapce)
        {
            return _defaultNamesapce.Contains(@namespace);
        }
    }


    /// <summary>
    /// 移除命名空间
    /// </summary>
    /// <param name="namespace"></param>
    public static void Remove(string @namespace, bool autoRebuildScript = true)
    {
        lock (_defaultNamesapce)
        {
            if (_defaultNamesapce.Contains(@namespace))
            {
                _defaultNamesapce.Remove(@namespace);
                _usingScriptCache = _usingScriptCache.Replace($"using {@namespace};{Environment.NewLine}", string.Empty);
                if (autoRebuildScript)
                {
                    UsingScript = _usingScriptCache.ToString();
                }
            }
        }
    }
    public static void Remove(IEnumerable<string> namespaces, bool autoRebuildScript = true)
    {

        lock (_defaultNamesapce)
        {
            _defaultNamesapce.ExceptWith(namespaces);
            foreach (var item in namespaces)
            {
                _usingScriptCache = _usingScriptCache.Replace($"using {item};{Environment.NewLine}", string.Empty);
                if (autoRebuildScript)
                {
                    UsingScript = _usingScriptCache.ToString();
                }
            }
        }

    }
}
