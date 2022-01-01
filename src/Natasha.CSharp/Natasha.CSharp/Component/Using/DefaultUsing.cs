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
    public static StringBuilder DefaultScript;
    static DefaultUsing()
    {

        DefaultScript = new StringBuilder();
        _defaultNamesapce = new HashSet<string>();

    }
    private static bool _completed;

    public static int Count { get { return _defaultNamesapce.Count; } }

    public static bool InitCompleted { get { return _completed; } }
    

    /// <summary>
    /// 添加引用
    /// </summary>
    /// <param name="path"></param>
    internal static void AddUsing(AssemblyName assemblyName)
    {
        try
        {
            if (assemblyName.Name != null)
            {
                lock (_defaultNamesapce)
                {
                    _defaultNamesapce.Add(assemblyName.Name);
                }
            }
        }
        catch(Exception ex)
        {
#if DEBUG
            Console.WriteLine(assemblyName.FullName + ex.Message);
#endif
        }
    }

    /// <summary>
    /// 添加完成
    /// </summary>
    internal static void UsingGenerator()
    {
        _completed = true;
        foreach (var @using in _defaultNamesapce)
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
                DefaultScript = DefaultScript.Replace($"using {@namespace};{Environment.NewLine}", string.Empty);
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
                DefaultScript.Replace($"using {item};{Environment.NewLine}", string.Empty);
            }
        }

    }
}
