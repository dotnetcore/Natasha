using Natasha.Cache;
using Natasha.Debug;
using Natasha.Utils;
using System;
using System.Reflection.Emit;

namespace Natasha.Core.Base
{
    public abstract class SingleType : TypeInitiator,IOperator,IPacket
    {

        public SingleType(Type type)
            : base(type)
        {

        }
        public SingleType(LocalBuilder builder, Type type)
            : base(builder, type)
        {

        }
        public SingleType(int parameterIndex, Type type)
            : base(parameterIndex, type)
        {

        }
        public SingleType(Action loadAction, Type type)
            : base(loadAction, type)
        {

        }
        #region 基本操作
        public void Store()
        {
            if (Builder != null)
            {
                ilHandler.Emit(OpCodes.Stloc_S, Builder);
                DebugHelper.WriteLine("Stloc_S", Builder.LocalIndex);
            }
            else if (ParameterIndex != -1)
            {
                ilHandler.Emit(OpCodes.Starg_S, ParameterIndex);
                DebugHelper.WriteLine("Starg_S", ParameterIndex);
            }
        }
        public void Store(Action action)
        {
            Store((object)action);
        }
        public void Store(object value)
        {
            DataHelper.NoErrorLoad(value, ilHandler);
            Store();
        }
        public void StoreNull()
        {
            if (TypeHandler == typeof(string))
            {
                ilHandler.Emit(OpCodes.Ldnull);
                DebugHelper.WriteLine("Ldnull");
                Store();
            }
        }

        public void InStackAndPacket()
        {
            Load();
            OperatorHelper.Packet(TypeHandler);
        }
        public void InStackAndUnPacket()
        {
            Load();
            OperatorHelper.UnPacket(TypeHandler);
        }
        public void Packet()
        {
            OperatorHelper.Packet(TypeHandler);
        }
        public void UnPacket()
        {
            OperatorHelper.UnPacket(TypeHandler);
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
            DataHelper.LoadSelfObject(this.TypeHandler);
            OperatorHelper.Add();
        }
        public void SubSelf()
        {
            this.Load();
            DataHelper.LoadSelfObject(this.TypeHandler);
            OperatorHelper.Sub();
        }
        #endregion
       
    }
}
