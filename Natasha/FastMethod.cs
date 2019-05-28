using Natasha.Builder;
using System;

namespace Natasha
{
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
