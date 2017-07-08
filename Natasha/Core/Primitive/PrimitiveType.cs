using Natasha.Cache;

using Natasha.Utils;
using System;
using System.Reflection.Emit;

namespace Natasha.Core.Base
{
    public abstract class PrimitiveType : TypeInitiator,IOperator,IPacket
    {

        public PrimitiveType(Type type)
            : base(type)
        {

        }
        public PrimitiveType(LocalBuilder builder, Type type)
            : base(builder, type)
        {

        }
        public PrimitiveType(int parameterIndex, Type type)
            : base(parameterIndex, type)
        {

        }
        public PrimitiveType(Action loadAction, Type type)
            : base(loadAction, type)
        {

        }
        #region 基本操作
        public void Store()
        {
            if (Builder != null)
            {
                il.REmit(OpCodes.Stloc_S, Builder);
            }
            else if (ParameterIndex != -1)
            {
                il.EmitStoreArg(ParameterIndex);
            }
        }
        public void Store(Action action)
        {
            Store((object)action);
        }
        public void Store(object value)
        {
            il.NoErrorLoad(value);
            Store();
        }
        public void StoreNull()
        {
            if (TypeHandler == typeof(string))
            {
                il.REmit(OpCodes.Ldnull);
                Store();
            }
        }

        public void InStackAndPacket()
        {
            Load();
            il.Packet(TypeHandler);
        }
        public void InStackAndUnPacket()
        {
            Load();
            il.UnPacket(TypeHandler);
        }
        public void Packet()
        {
            il.Packet(TypeHandler);
        }
        public void UnPacket()
        {
            il.UnPacket(TypeHandler);
        }
        #endregion


        #region 运算接口
        public void RunCompareAction()
        {
            Load();
        }
        public void AddSelf()
        {
            this.Load();
            il.LoadOne(this.TypeHandler);
            il.Add();
        }
        public void SubSelf()
        {
            this.Load();
            il.LoadOne(this.TypeHandler);
            il.Sub();
        }
        #endregion
       
    }
}
