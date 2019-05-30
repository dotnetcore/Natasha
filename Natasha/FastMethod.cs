using Natasha.Builder;

namespace Natasha
{
    /// <summary>
    /// 快速创建一个动态方法
    /// </summary>
    public class FastMethod : OnceMethodBuilder<FastMethod>
    {
        public static FastMethod New
        {
            get { return new FastMethod(); }
        }
        public FastMethod()
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
