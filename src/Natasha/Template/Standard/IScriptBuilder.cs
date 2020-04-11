namespace Natasha.CSharp
{
    /// <summary>
    /// 构建脚本接口
    /// </summary>
    public interface IScriptBuilder<T>:IScript
    {

        T BuilderScript();

    }

}
