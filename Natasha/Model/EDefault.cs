using Natasha.Cache;
using Natasha.Debug;
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
            if (type == typeof(int))
            {
                il.Emit(OpCodes.Ldc_I4_0);
                DebugHelper.WriteLine("Ldc_I4_0");
            }
            else if (type == typeof(double))
            {
                il.Emit(OpCodes.Ldc_R8, 0.00);
                DebugHelper.WriteLine("Ldc_R8 0.00");
            }
            else if (type == typeof(long) || type == typeof(ulong))
            {
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Conv_I8);
                DebugHelper.WriteLine("Ldc_I4_0");
                DebugHelper.WriteLine("Conv_I8");
            }
            else if (type == typeof(float))
            {
                il.Emit(OpCodes.Ldc_R4, 0.0);
                DebugHelper.WriteLine("Ldc_R4 0.0");
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4_0);
                DebugHelper.WriteLine("Ldc_I4_0");
            }
        }
    }
}
