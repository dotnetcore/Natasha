using Natasha.Debug;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Natasha.Utils
{
    public class EmitHelper
    {
        #region Code映射
        
        public static OpCode GetLoadCode(Type type,int i=4)
        {
            if (type.IsByRef || !type.IsValueType)
            {
                if (i == 0)
                {
                    DebugHelper.WriteLine("Ldloc_0");
                    return OpCodes.Ldloc_0;
                }
                else if (i == 1)
                {
                    DebugHelper.WriteLine("Ldloc_1");
                    return OpCodes.Ldloc_1;
                }
                else if (i == 2)
                {
                    DebugHelper.WriteLine("Ldloc_2");
                    return OpCodes.Ldloc_2;
                }
                else if (i == 3)
                {
                    DebugHelper.WriteLine("Ldloc_3");
                    return OpCodes.Ldloc_3;
                }
                else
                {
                    DebugHelper.Write("Ldloc_S\t");
                    return OpCodes.Ldloc_S;
                }
            }
            else
            {
                DebugHelper.Write("Ldloca_S\t");
                return OpCodes.Ldloca_S;
            }
        }
        public static OpCode GetArgsCode(Type type, int i = 4)
        {
            if (type != null && (type.IsByRef || !type.IsValueType))
            {
                DebugHelper.Write("Ldarga_S\t");
                return OpCodes.Ldarga_S;
            }
            else
            {
                if (i == 0)
                {
                    DebugHelper.WriteLine("Ldarg_0");
                    return OpCodes.Ldarg_0;
                }
                else if (i == 1)
                {
                    DebugHelper.WriteLine("Ldarg_1");
                    return OpCodes.Ldarg_1;
                }
                else if (i == 2)
                {
                    DebugHelper.WriteLine("Ldarg_2");
                    return OpCodes.Ldarg_2;
                }
                else if (i == 3)
                {
                    DebugHelper.WriteLine("Ldarg_3");
                    return OpCodes.Ldarg_3;
                }
                else
                {
                    DebugHelper.Write("Ldarg_S\t");
                    return OpCodes.Ldarg_S;
                }
            }
        }
        public static OpCode GetCallCode(Type type, FieldInfo info)
        {
            if (type.IsValueType || info.IsStatic)
            {
                DebugHelper.Write("Call\t");
                return OpCodes.Call;
            }
            else
            {
                DebugHelper.Write("Callvirt\t");
                return OpCodes.Callvirt;
            }
        }

        #endregion
        
        //获取链式调用model
        public static EModel GetLink(Type type)
        {
            return EModel.CreateModelFromAction(null, type);
        }


        
    }
}
