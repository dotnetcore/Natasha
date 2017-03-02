using Natasha.Cache;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Natasha
{
    //操作DBNull 待更新
    public partial class EDBNull
    {
        static FieldInfo Value;

        static EDBNull()
        {
            Value = typeof(DBNull).GetField("Value");
        }

        public static void LoadValue()
        {
            ThreadCache.GetIL().Emit(OpCodes.Ldsfld, Value);
        }
    }
}
