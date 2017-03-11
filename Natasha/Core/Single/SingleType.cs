using Natasha.Cache;
using Natasha.Debug;
using Natasha.Utils;
using System;
using System.Reflection.Emit;

namespace Natasha.Core.Base
{
    public abstract class SingleType : TypeInitiator,IOperator
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
                DebugHelper.WriteLine("Stloc_S " + Builder.LocalIndex);
            }
            else if (ParameterIndex != -1)
            {
                ilHandler.Emit(OpCodes.Starg_S, ParameterIndex);
                DebugHelper.WriteLine("Starg_S " + ParameterIndex);
            }
        }
        public void Store(Action action)
        {
            Store((object)action);
        }
        public void Store(object value)
        {
            EData.NoErrorLoad(value, ilHandler);
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
        public void Packet()
        {
            if (TypeHandler.IsValueType)
            {
                Load();
                ilHandler.Emit(OpCodes.Box, TypeHandler);
                DebugHelper.WriteLine("Box "+ TypeHandler.Name);
            }
        }
        public void UnPacket()
        {
            if (TypeHandler.IsClass)
            {
                Load();
                ilHandler.Emit(OpCodes.Castclass, TypeHandler);
                DebugHelper.WriteLine("Castclass " + TypeHandler.Name);
            }
            else
            {
                Load();
                ilHandler.Emit(OpCodes.Unbox_Any, TypeHandler);
                DebugHelper.WriteLine("Unbox_Any " + TypeHandler.Name);
            }
        }
        #endregion

        #region 运算
        #endregion

        #region 运算接口
        public void RunCompareAction()
        {
            Load();
        }
        public void AddSelf()
        {
            this.Load();
            EData.LoadSelfObject(this.TypeHandler);
            this.ilHandler.Emit(OpCodes.Add);
            DebugHelper.WriteLine("Add");
        }
        public void SubSelf()
        {
            this.Load();
            EData.LoadSelfObject(this.TypeHandler);
            this.ilHandler.Emit(OpCodes.Sub);
            DebugHelper.WriteLine("Sub");
        }
        #endregion
       
    }
}
