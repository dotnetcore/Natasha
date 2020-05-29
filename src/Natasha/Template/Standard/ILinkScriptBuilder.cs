using Natasha.Template;

namespace Natasha.CSharp
{
    /// <summary>
    /// 构建脚本接口
    /// </summary>
    public interface ILinkScriptBuilder<T> : IScriptBuilder
    {

        T BuilderScript();

    }

}
