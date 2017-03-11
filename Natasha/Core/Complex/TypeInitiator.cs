using System;
using System.Reflection;
using System.Reflection.Emit;
using Natasha.Cache;
using Natasha.Core.Base;
using System.Collections.Generic;
using Natasha.Utils;
using Natasha.Debug;

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
                DebugHelper.WriteLine("Ldloca_S " + Builder.LocalIndex);
                EmitHelper.InitObject(TypeHandler);
            }

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

        #region 加载
        public void Load()
        {
            if (LoadAction != null)
            {
                LoadAction();
            }
            else if (TypeHandler.IsEnum)
            {
                int value = (int)Value;
                if (value < 255)
                {
                    ilHandler.Emit(OpCodes.Ldc_I4_S, value);
                    DebugHelper.WriteLine("Ldc_I4_S" + value);
                }
                else
                {
                    ilHandler.Emit(OpCodes.Ldc_I4, value);
                    DebugHelper.WriteLine("Ldc_I4 " + value);
                }
            }
            else if (ParameterIndex == -1)
            {
                if (Builder != null)
                {
                    ilHandler.Emit(OpCodes.Ldloc_S, Builder.LocalIndex);
                    DebugHelper.WriteLine("Ldloc_S " + Builder.LocalIndex);

                }
                else if (TypeHandler.IsPrimitive || TypeHandler == typeof(string))
                {
                    EData.LoadObject(Value);
                }
            }
            else if (ParameterIndex == 0)
            {
                ilHandler.Emit(OpCodes.Ldarg_0);
                DebugHelper.WriteLine("Ldarg_0");
            }
            else if (ParameterIndex == 1)
            {
                ilHandler.Emit(OpCodes.Ldarg_1);
                DebugHelper.WriteLine("Ldarg_1");
            }
            else if (ParameterIndex == 2)
            {
                ilHandler.Emit(OpCodes.Ldarg_2);
                DebugHelper.WriteLine("Ldarg_2");
            }
            else if (ParameterIndex == 3)
            {
                ilHandler.Emit(OpCodes.Ldarg_3);
                DebugHelper.WriteLine("Ldarg_3");
            }
            else
            {
                ilHandler.Emit(OpCodes.Ldarg_S, ParameterIndex);
                DebugHelper.WriteLine("Ldarg_S " + ParameterIndex);
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
                if (TypeHandler.IsPrimitive || TypeHandler == typeof(string))
                {
                    EData.LoadObject(Value);
                }
                else
                {
                    if (TypeHandler.IsEnum)
                    {
                        int value = (int)Value;
                        if (value < 255)
                        {
                            ilHandler.Emit(OpCodes.Ldc_I4_S, value);
                            DebugHelper.WriteLine("Ldc_I4_S " + value);
                        }
                        else
                        {
                            ilHandler.Emit(OpCodes.Ldc_I4, value);
                            DebugHelper.WriteLine("Ldc_I4 " + value);
                        }
                    }
                    else if (ParameterIndex == -1)
                    {
                        if (Builder != null)
                        {
                            ilHandler.Emit(EmitHelper.GetLoadCode(TypeHandler), Builder);
                            DebugHelper.WriteLine(Builder.LocalIndex.ToString());
                        }
                    }
                    else
                    {
                        if (ParameterIndex > 3)
                        {
                            ilHandler.Emit(EmitHelper.GetArgsCode(TypeHandler), ParameterIndex);
                            DebugHelper.WriteLine(ParameterIndex.ToString());
                        }
                        else
                        {
                            ilHandler.Emit(EmitHelper.GetArgsCode(TypeHandler));
                            DebugHelper.WriteLine(EmitHelper.GetArgsCode(TypeHandler).Name);
                        }
                    }
                }
            }
        }
        #endregion








    }
}

