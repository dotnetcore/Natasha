#if MULTI
/// <summary>
/// 程序集编译构建器 - 语法树相关
/// </summary>
public sealed partial class AssemblyCSharpBuilder
{
    /// <summary>
    /// 脚本前自动拼接全局的 using 引用
    /// </summary>
    /// <param name="script">脚本代码</param>
    /// <returns></returns>
    public AssemblyCSharpBuilder AddWithFullUsing(string script)
    {
        if (Domain.Name == "Default")
        {
            return AddWithDefaultUsing(script);
        }
        else
        {
            return AddWithDefaultUsing(Domain.UsingRecorder.ToString() + script);
        }

    }
}
#endif




