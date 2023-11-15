#if !NETCOREAPP3_0_OR_GREATER
/// <summary>
/// 程序集编译构建器 - 语法树相关
/// </summary>
public sealed partial class AssemblyCSharpBuilder
{
    /// <summary>
    /// 注入代码并拼接using
    /// </summary>
    /// <param name="script">脚本代码</param>
    /// <param name="usingLoadBehavior">using 拼接行为</param>
    /// <returns></returns>
    public AssemblyCSharpBuilder Add(string script, UsingLoadBehavior usingLoadBehavior)
    {
        switch (usingLoadBehavior)
        {
            case UsingLoadBehavior.WithDefault:
            case UsingLoadBehavior.WithAll:
                return AddScript(DefaultUsing.UsingScript + script);
            default:
                return AddScript(script);
        }
    }
}
#endif




