using Natasha.Cache;
using Natasha.Core;
using Natasha.Debug;
using Natasha.Utils;
using System;
using System.Reflection.Emit;

namespace Natasha
{
    //拆装箱操作
    public class EPacket
    {
        #region 装箱
        //对指定类型装箱
        public static void Packet(Type type)
        {
            Packet(type, loadAction:null);
        }
        //对普通变量进行装箱
        public static void Packet(EVar varHandler)
        {
            Packet(varHandler.TypeHandler, ()=> { varHandler.This(); });
        }
        //指定类型，对指定操作类的变量进行装箱
        public static void Packet(Type type,  object value)
        {
            Packet(type, () => { EData.NoErrorLoad(value, ThreadCache.GetIL()); });
        }
        public static void Packet(Type type,Action loadAction)
        {
            ILGenerator ilHandler = ThreadCache.GetIL();
            if (loadAction!=null)
            {
                loadAction();
            }
            if (type.IsValueType)
            {
                ilHandler.Emit(OpCodes.Box, type);
                DebugHelper.WriteLine("Box "+type.Name);
            }
        }
        #endregion

        #region 拆箱
        public static void UnPacket(Type type)
        {
            UnPacket(type,loadAction:null);
        }
        public static void UnPacket(Type type, object value)
        {
            UnPacket(type, () => { EData.NoErrorLoad(value, ThreadCache.GetIL()); });
        }

        public static void UnPacket(Type type,Action loadAction)
        {
            ILGenerator ilHandler = ThreadCache.GetIL();
            if (loadAction != null)
            {
                loadAction();
            }
            if (type.IsClass && type!=typeof(string) && type != typeof(object))
            {
                ilHandler.Emit(OpCodes.Castclass, type);
                DebugHelper.WriteLine("Castclass " + type.Name);
            }
            else
            {
                ilHandler.Emit(OpCodes.Unbox_Any, type);
                DebugHelper.WriteLine("Unbox_Any " + type.Name);
            }
        }
        #endregion
    }
}
