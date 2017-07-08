using Natasha.Cache;

using Natasha.Utils;
using System;
using System.Reflection.Emit;

namespace Natasha
{
    //操作默认值
    public partial class EDefault
    {
        public static void LoadDefault(Type type)
        {
            ILGenerator il = ThreadCache.GetIL();
            il.EmitDefault(type);
        }
    }
}
