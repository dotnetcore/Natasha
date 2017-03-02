using System;
using System.Reflection;
using System.Reflection.Emit;
using Natasha.Cache;
using Natasha.Core.Base;
using System.Collections.Generic;
using Natasha.Utils;

namespace Natasha.Core
{
    //这里提供了Model初始化的方法
    public class TypeInitiator : TypeStructionAnalyzer, ILoadInstance
    {
        public LocalBuilder Builder;
        public int ParameterIndex;
        public Action LoadAction;

        #region 初始化
        public TypeInitiator(Type parameter_Type) : base(parameter_Type)
        {
            ParameterIndex = -1;

            //处理抽象类以及接口
            if (TypeHandler.IsAbstract && TypeHandler.IsInterface)
            {
                Builder = null;
                return;
            }

            Builder = ilHandler.DeclareLocal(TypeHandler);

            if (IsStuct)  
            {
                ilHandler.Emit(OpCodes.Ldloca_S, Builder);
                ilHandler.Emit(OpCodes.Initobj, TypeHandler);
            }

        }
        public TypeInitiator(Action action,Type parameter_Type) : base(parameter_Type)
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

        #region 加载
        public void Load()
        {
            //加载委托
            //加载本地变量 
            //加载函数参数
            if (LoadAction != null)
            {
                LoadAction();
            }
            else if (ParameterIndex == -1)
            {
                if (Builder != null)
                {
                    ilHandler.Emit(OpCodes.Ldloc, Builder);
                }
            }
            else if (ParameterIndex == 0)
            {
                ilHandler.Emit(OpCodes.Ldarg_0);
            }
            else if (ParameterIndex == 1)
            {
                ilHandler.Emit(OpCodes.Ldarg_1);
            }
            else if (ParameterIndex == 2)
            {
                ilHandler.Emit(OpCodes.Ldarg_2);
            }
            else if (ParameterIndex == 3)
            {
                ilHandler.Emit(OpCodes.Ldarg_3);
            }
            else
            {
                ilHandler.Emit(OpCodes.Ldarg_S, ParameterIndex);
            }
        }
        public void LoadAddress()
        {
            if (LoadAction != null)
            {
                LoadAction();
            }
            else
            {
                if (IsStuct)
                {
                    if (ParameterIndex == -1)
                    {
                        if (Builder != null)
                        {
                            //加载结构体本地变量
                            ilHandler.Emit(OpCodes.Ldloca_S, Builder);
                        }
                    }
                    else
                    {
                        ilHandler.Emit(OpCodes.Ldarga_S, ParameterIndex);
                    }
                }
                else
                {
                    Load();
                }
            }
        }
        #endregion

        


     


       
    }
}

