using System.Linq;
using System.Reflection;

namespace Natasha.Engine.Builder
{
    public abstract partial class BuilderStandard<LINK>
    {
        public LINK Method(MethodInfo info)
        {
            if (info!=null)
            {
                Using(info.ReturnType);
                Using(info.GetParameters().Select(item => item.ParameterType));
            }
            return _link;
        }

    }
}
