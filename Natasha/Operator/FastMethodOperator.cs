using Natasha.Builder;

namespace Natasha
{
    /// <summary>
    /// 快速创建一个动态方法
    /// </summary>
    public class FastMethodOperator : OnceMethodBuilder<FastMethodOperator>
    {


        public static FastMethodOperator New
        {
            get { return new FastMethodOperator(); }
        }


        public FastMethodOperator()
        {
            Link = this;
            HiddenNameSpace()
                .ClassAccess(AccessTypes.Public)
                .ClassModifier(Modifiers.Static)
                .MethodAccess(AccessTypes.Public)
                .MethodModifier(Modifiers.Static);
        }
    }
}
