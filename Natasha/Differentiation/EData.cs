using Natasha.Cache;
using Natasha.Core;
using Natasha.Core.Base;
using Natasha.Debug;
using System;
using System.Reflection.Emit;
using System.Threading;

namespace Natasha.Utils
{
    //神器的数据操作类
    public class EData
    {
        //各种数据给我就能入栈
        public static void NoErrorLoad(object value,ILGenerator ilHandler)
        {
            ENull enull = value as ENull;
            if (value != null)
            {
                Type type = value.GetType();
                ILoadInstance instance = value as ILoadInstance;
                LocalBuilder builder = value as LocalBuilder;
                IOperator modelOperator = value as IOperator;
                if (value is Action)
                {
                    ((Action)value)();
                }
                else if (instance!=null)
                {
                    instance.Load();
                }
                else if (builder != null)
                {
                    ilHandler.Emit(OpCodes.Ldloc_S, builder.LocalIndex);
                    DebugHelper.WriteLine("Ldloc_S " + builder.LocalIndex);
                }
                else if (enull != null)
                {
                    ilHandler.Emit(OpCodes.Ldnull);
                    DebugHelper.WriteLine("Ldnull");
                }
                else if (type.IsClass && type != typeof(string))
                {
                    EModel model = EModel.CreateModelFromObject(value, type);
                    model.Load();
                }
                else if (type.IsEnum)
                {
                    ilHandler.Emit(OpCodes.Ldc_I4, (int)value);
                    DebugHelper.WriteLine("Ldc_I4 "+ (int)value);
                }
                else if (type.IsValueType && !type.IsPrimitive)
                {
                    EModel model;
                    if (EStruckCheck.IsDefaultStruct(value, type))
                    {
                        model = EModel.CreateModel(type);
                    }
                    else
                    {
                        model = EModel.CreateModelFromObject(value, type);
                    }
                    model.Load();
                }
                else
                {
                    EData.LoadObject(value);
                }
            }
            else
            {
                ilHandler.Emit(OpCodes.Ldnull);
                DebugHelper.WriteLine("Ldnull");
            }
        }
      

        //给我一个普通变量我就入栈
        public static void LoadObject(object value)
        {
            if (value == null)
            {
                return;
            }
            ILGenerator il = ThreadCache.GetIL();
            Type EType = value.GetType();
            if (EType == typeof(int))
            {
                int result;
                if (int.TryParse(value.ToString(), out result))
                {
                    if (result < 255)
                    {
                        il.Emit(OpCodes.Ldc_I4_S, result);
                        DebugHelper.WriteLine("Ldc_I4_S "+result);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4, result);
                        DebugHelper.WriteLine("Ldc_I4 " + result);
                    }
                }
            } 
            else if (EType == typeof(string))
            {
                string result = value.ToString();
                il.Emit(OpCodes.Ldstr, result);
                DebugHelper.WriteLine("Ldstr " + result);
            }
            else if (EType == typeof(double))
            {
                double result;
                if (double.TryParse(value.ToString(), out result))
                {
                    il.Emit(OpCodes.Ldc_R8, result);
                    DebugHelper.WriteLine("Ldc_R8 " + result);
                }
            }
            else if (EType == typeof(float))
            {
                float result;
                if (float.TryParse(value.ToString(), out result))
                {
                    il.Emit(OpCodes.Ldc_R4, result);
                    DebugHelper.WriteLine("Ldc_R4 " + result);
                }
            }
            else if (EType == typeof(bool))
            {
                if ((bool)value)
                {
                    il.Emit(OpCodes.Ldc_I4_1);
                    DebugHelper.WriteLine("Ldc_I4_1");
                }
                else
                {
                    il.Emit(OpCodes.Ldc_I4_0);
                    DebugHelper.WriteLine("Ldc_I4_0");
                }
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }
            else if (value is Type)
            {
                il.Emit(OpCodes.Ldtoken, (Type)value);
                il.Emit(OpCodes.Call, ClassCache.ClassHandle);

                DebugHelper.WriteLine("Ldtoken "+((Type)value).ToString());
                DebugHelper.WriteLine("Call " + ClassCache.ClassHandle.Name);
            }
            else if (EType == typeof(long) || EType == typeof(ulong))
            {
                long result;
                if (long.TryParse(value.ToString(), out result))
                {
                    if (result < 255)
                    {
                        il.Emit(OpCodes.Ldc_I4_S, result);
                        DebugHelper.WriteLine("Ldc_I4_S " + result);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4, result);
                        DebugHelper.WriteLine("Ldc_I4 " + result);
                    }
                    il.Emit(OpCodes.Conv_I8);
                    DebugHelper.WriteLine("Conv_I8");
                }
            }
            else
            {
                int result;
                if (int.TryParse(value.ToString(), out result))
                {
                    if (result<255)
                    {
                        il.Emit(OpCodes.Ldc_I4_S, result);
                        DebugHelper.WriteLine("Ldc_I4_S " + result);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4, result);
                        DebugHelper.WriteLine("Ldc_I4 " + result);
                    }
                }
            }
        }


        //自增量入栈
        public static void LoadSelfObject(Type type)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (type == typeof(int))
            {
                il.Emit(OpCodes.Ldc_I4_1);
                DebugHelper.WriteLine("Ldc_I4_1");
            }
            else if (type == typeof(double))
            {
                il.Emit(OpCodes.Ldc_R8, 1.00);
                DebugHelper.WriteLine("Ldc_R8 1.00");
            }
            else if (type == typeof(long) || type == typeof(ulong))
            {
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Conv_I8);
                DebugHelper.WriteLine("Ldc_I4_1");
                DebugHelper.WriteLine("Conv_I8");
            }
            else if (type == typeof(float))
            {
                il.Emit(OpCodes.Ldc_R4, 1.0);
                DebugHelper.WriteLine("Ldc_R4 1.0");
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4_1);
                DebugHelper.WriteLine("Ldc_I4_1");
            }
        }
    }
}
