namespace Natasha
{
    /// <summary>
    /// 构建脚本接口
    /// </summary>
    public interface IScriptBuilder<T>
    {
        T Builder();
    }
}
