using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;


/// <summary>
/// 全局 using, 初始化时会通过 DependencyModel 获取主域所需的引用 using 字符串.
/// </summary>
public static class DefaultUsing
{

    private readonly static HashSet<string> _defaultNamesapce;
    public static StringBuilder UsingScriptCache;
    static DefaultUsing()
    {

        UsingScriptCache = new StringBuilder();
        _defaultNamesapce = new HashSet<string>();

    }

    public static int Count { get { return _defaultNamesapce.Count; } }


    /// <summary>
    /// 添加引用
    /// </summary>
    /// <param name="assembly"></param>
    internal static void AddUsing(Assembly assembly)
    {
        try
        {
            lock (_defaultNamesapce)
            {
                var types = assembly.GetTypes();
                foreach (var t in types)
                {
                    var name = t.Namespace;
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (!_defaultNamesapce.Contains(name))
                        {
                            _defaultNamesapce.Add(name!);
                            UsingScriptCache.AppendLine($"using {name!};");
                        }

                    }

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
    /// <param name="assemblyName"></param>
    internal static void AddUsing(AssemblyName assemblyName)
    {
        try
        {
            lock (_defaultNamesapce)
            {
                var name = assemblyName.Name;
                if (!string.IsNullOrEmpty(name))
                {
                    if (!_defaultNamesapce.Contains(name))
                    {
                        _defaultNamesapce.Add(name!);
                        UsingScriptCache.AppendLine($"using {name!};");
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
    public static void Remove(string @namespace)
    {
        lock (_defaultNamesapce)
        {
            if (_defaultNamesapce.Contains(@namespace))
            {
                _defaultNamesapce.Remove(@namespace);
                UsingScriptCache = UsingScriptCache.Replace($"using {@namespace};{Environment.NewLine}", string.Empty);
            }
        }
    }
    public static void Remove(IEnumerable<string> namespaces)
    {

        lock (_defaultNamesapce)
        {
            _defaultNamesapce.ExceptWith(namespaces);
            foreach (var item in namespaces)
            {
                UsingScriptCache = UsingScriptCache.Replace($"using {item};{Environment.NewLine}", string.Empty);
            }
        }

    }
}
