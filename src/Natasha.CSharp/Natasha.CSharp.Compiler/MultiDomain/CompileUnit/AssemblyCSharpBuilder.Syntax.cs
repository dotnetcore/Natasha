#if NETCOREAPP3_0_OR_GREATER
using System.Text;
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
    public AssemblyCSharpBuilder Add(string script, UsingLoadBehavior usingLoadBehavior = UsingLoadBehavior.WithCurrent)
    {
        switch (usingLoadBehavior)
        {
            case UsingLoadBehavior.WithDefault:
                return AddScript(DefaultUsing.UsingScript + script);
            case UsingLoadBehavior.WithCurrent:
                if (Domain == NatashaReferenceDomain.DefaultDomain)
                {
                    return AddScript(DefaultUsing.UsingScript + script);
                }
                return AddScript(Domain.UsingRecorder + script);
            case UsingLoadBehavior.WithAll:
                if (Domain == NatashaReferenceDomain.DefaultDomain)
                {
                    return AddScript(DefaultUsing.UsingScript + script);
                }
                StringBuilder usingBuilder = new();
                foreach (var item in Domain.UsingRecorder._usings)
                {
                    if (!DefaultUsing.HasElement(item))
                    {
                        usingBuilder.AppendLine($"using {item};");
                    }
                }
                return AddScript(DefaultUsing.UsingScript + usingBuilder + script);
            default:
                return AddScript(script);
        }
    }
}
#endif




