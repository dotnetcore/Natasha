using System;
using System.Reflection.Emit;
using Natasha.Utils;
using Natasha.Core.Complex;

namespace Natasha.Core
{
    //这里提供了Model初始化的方法
    public class TypeInitiator : TypeStructionAnalyzer, ILoadInstance
    {
        public LocalBuilder Builder;
        public int ParameterIndex;
        public Action LoadAction;
        public bool IsRef;
        
        #region 初始化
        public TypeInitiator(Type parameter_Type) : base(parameter_Type)
        {
            ParameterIndex = -1;
            Builder = il.DeclareLocal(TypeHandler);

        }
        public TypeInitiator(Action action, Type parameter_Type) : base(parameter_Type)
        {
            ParameterIndex = -1;
            Builder = null;
            LoadAction = action;
        }
        public TypeInitiator(LocalBuilder builder, Type parameter_Type) : base(parameter_Type)
        {
            ParameterIndex = -1;
            Builder = builder;
        }
        public TypeInitiator(int parameterIndex, Type parameter_Type) : base(parameter_Type)
        {
            ParameterIndex = parameterIndex;
            Builder = null;
        }
        #endregion

        public void UseRef()
        {
            IsRef = true;
        }


        #region 加载
        public void Load()
        {
            if (LoadAction != null)
            {
                LoadAction();
            }
            else if (ParameterIndex == -1)
            {
                if (Builder != null)
                {
                    il.LoadBuilder(Builder,false);
                }
                else if (TypeHandler != null && TypeHandler.IsEnum)
                {
                    int value = (int)Value;
                    il.EmitInt(value);
                }
                else if (TypeHandler !=null && (TypeHandler.IsPrimitive || TypeHandler == typeof(string) || TypeHandler== typeof(decimal)))
                {
                    il.LoadObject(Value, TypeHandler);
                }
            }
            else {
                il.EmitLoadArg(ParameterIndex);
            }
            if (IsRef)
            {
                il.EmitLoadValueIndirect(TypeHandler);
            }
        }

        public void This()
        {
            if (LoadAction != null)
            {
                LoadAction();
            }
            else
            {
                if (ParameterIndex == -1)
                {
                    if (Builder != null)
                    {
                        il.LoadBuilder(Builder);
                    }
                    else if (TypeHandler != null && TypeHandler.IsEnum)
                    {
                        int value = (int)Value;
                        il.EmitInt(value);
                    }
                    else if (TypeHandler != null && (TypeHandler.IsPrimitive || TypeHandler == typeof(string)))
                    {
                       il.LoadObject(Value, TypeHandler);
                    }
                }
                else
                {
                    il.LoadParameter(TypeHandler, ParameterIndex);
                }
            }
            if (IsRef)
            {
                il.EmitLoadValueIndirect(TypeHandler);
            }
        }
        #endregion
    }
}

