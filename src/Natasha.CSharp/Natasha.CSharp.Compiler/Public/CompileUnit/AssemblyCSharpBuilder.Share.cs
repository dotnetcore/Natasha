using System;

public sealed partial class AssemblyCSharpBuilder
{
    /// <summary>
    /// 清空编译信息, 下次编译重组 Compilation 和语法树.
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder ClearCompilationCache()
    {
        _compilation = null;
        return this;
    }

    /// <summary>
    /// 清空所有记录,包括编译信息和脚本记录,以及程序集名称.
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder Clear()
    {
        _compilation = null;
        SyntaxTrees.Clear();
        AssemblyName = string.Empty;
        return this;
    }

    /// <summary>
    /// 自动使用 GUID 作为程序集名称.
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithRandomAssenblyName()
    {
        AssemblyName = Guid.NewGuid().ToString("N");
        return this;
    }


}

