using Natasha.Cache;
using Natasha.Utils;
using System;
using System.Reflection.Emit;

namespace Natasha.Core.Base
{
    public abstract class SingleType : TypeInitiator, ILoadInstance,IOperator
    {
        public object Value;

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
                ilHandler.Emit(OpCodes.Stloc, Builder);
            }
            else if (ParameterIndex != -1)
            {
                ilHandler.Emit(OpCodes.Starg_S, ParameterIndex);
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
                Store();
            }
        }
        public void Packet()
        {
            if (TypeHandler.IsValueType)
            {
                Load();
                ilHandler.Emit(OpCodes.Box, TypeHandler);
            }
        }
        public void UnPacket()
        {
            if (TypeHandler.IsClass)
            {
                Load();
                ilHandler.Emit(OpCodes.Castclass, TypeHandler);
            }
            else
            {
                Load();
                ilHandler.Emit(OpCodes.Unbox_Any, TypeHandler);
            }
        }
        #endregion

       

        #region 运算

        /*
        public static Action operator +(IOperator source, object dest)
        {
            return () =>
            {
                source.RunCompareAction();
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    EData.LoadObject(dest);
                }
                ThreadCache.GetIL().Emit(OpCodes.Add);
            };
        }
        //public static Action operator -(SingleType source, SingleType dest)
        //{
        //    return () =>
        //    {
        //        source.Load();
        //        dest.Load();
        //        source.ilHandler.Emit(OpCodes.Sub);
        //    };
        //}
        public static Action operator -(SingleType source, object dest)
        {
            return () =>
            {
                source.Load();
                EData.LoadObject(dest);
                source.ilHandler.Emit(OpCodes.Sub);
            };
        }
        public static Action operator *(SingleType source, SingleType dest)
        {
            return () =>
            {
                source.Load();
                dest.Load();
                source.ilHandler.Emit(OpCodes.Mul);
            };
        }
        public static Action operator *(SingleType source, object dest)
        {
            return () =>
            {
                source.Load();
                EData.LoadObject(dest);
                source.ilHandler.Emit(OpCodes.Mul);
            };
        }
        public static Action operator /(SingleType source, SingleType dest)
        {
            return () =>
            {
                source.Load();
                dest.Load();
                source.ilHandler.Emit(OpCodes.Div);
            };
        }
        public static Action operator /(SingleType source, object dest)
        {
            return () =>
            {
                source.Load();
                EData.LoadObject(dest);
                source.ilHandler.Emit(OpCodes.Div);
            };
        }
        public static Action operator %(SingleType source, SingleType dest)
        {
            return () =>
            {
                source.Load();
                dest.Load();
                source.ilHandler.Emit(OpCodes.Rem);
            };
        }
        public static Action operator %(SingleType source, object dest)
        {
            return () =>
            {
                source.Load();
                EData.LoadObject(dest);
                source.ilHandler.Emit(OpCodes.Rem);
            };
        }
        public static Action operator >>(SingleType source, int dest)
        {
            return () =>
            {
                source.Load();
                source.ilHandler.Emit(OpCodes.Ldc_I4, dest);
                source.ilHandler.Emit(OpCodes.Ldc_I4_S, 31);
                source.ilHandler.Emit(OpCodes.And);
                source.ilHandler.Emit(OpCodes.Shr);
            };
        }
        public static Action operator <<(SingleType source, int dest)
        {
            return () =>
            {
                source.Load();
                source.ilHandler.Emit(OpCodes.Ldc_I4, dest);
                source.ilHandler.Emit(OpCodes.Ldc_I4_S, 31);
                source.ilHandler.Emit(OpCodes.And);
                source.ilHandler.Emit(OpCodes.Shl);
            };
        }
        public static Action operator |(SingleType source, SingleType dest)
        {
            return () =>
            {
                source.Load();
                dest.Load();
                source.ilHandler.Emit(OpCodes.Or);
            };
        }
        public static Action operator |(SingleType source, object dest)
        {
            return () =>
            {
                source.Load();
                EData.LoadObject(dest);
                source.ilHandler.Emit(OpCodes.Or);
            };
        }
        public static Action operator &(SingleType source, SingleType dest)
        {
            return () =>
            {
                source.Load();
                dest.Load();
                source.ilHandler.Emit(OpCodes.And);
            };
        }
        public static Action operator &(SingleType source, object dest)
        {
            return () =>
            {
                source.Load();
                EData.LoadObject(dest);
                source.ilHandler.Emit(OpCodes.And);
            };
        }
        public static SingleType operator >(SingleType source, SingleType dest)
        {
            ThreadCache.SetCompareAction1(() =>
            {
                source.Load();
            });
            ThreadCache.SetCompareAction2(() =>
            {
                dest.Load();
            });
            ThreadCache.SetJudgeCode(OpCodes.Ble_S);
            return source;
        }
        public static SingleType operator >(SingleType source, object dest)
        {
            ThreadCache.SetCompareAction1(() =>
            {
                source.Load();
            });
            ThreadCache.SetCompareAction2(() =>
            {
                EData.LoadObject(dest);
            });
            ThreadCache.SetJudgeCode(OpCodes.Ble_S);
            return source;
        }
        public static SingleType operator <(SingleType source, SingleType dest)
        {
            ThreadCache.SetCompareAction1(() =>
            {
                source.Load();
            });
            ThreadCache.SetCompareAction2(() =>
            {
                dest.Load();
            });
            ThreadCache.SetJudgeCode(OpCodes.Bge_S);
            return source;
        }
        public static SingleType operator <(SingleType source, object dest)
        {
            ThreadCache.SetCompareAction1(() =>
            {
                source.Load();
            });
            ThreadCache.SetCompareAction2(() =>
            {
                EData.LoadObject(dest);
            });
            ThreadCache.SetJudgeCode(OpCodes.Bge_S);
            return source;
        }
        public static SingleType operator <=(SingleType source, SingleType dest)
        {
            ThreadCache.SetCompareAction1(() =>
            {
                source.Load();
            });
            ThreadCache.SetCompareAction2(() =>
            {
                dest.Load();
            });
            ThreadCache.SetJudgeCode(OpCodes.Bgt_S);
            return source;
        }
        public static SingleType operator <=(SingleType source, object dest)
        {
            ThreadCache.SetCompareAction1(() =>
            {
                source.Load();
            });
            ThreadCache.SetCompareAction2(() =>
            {
                EData.LoadObject(dest);
            });
            ThreadCache.SetJudgeCode(OpCodes.Bgt_S);
            return source;
        }
        public static SingleType operator >=(SingleType source, SingleType dest)
        {
            ThreadCache.SetCompareAction1(() =>
            {
                source.Load();
            });
            ThreadCache.SetCompareAction2(() =>
            {
                dest.Load();
            });
            ThreadCache.SetJudgeCode(OpCodes.Blt_S);
            return source;
        }
        public static SingleType operator >=(SingleType source, object dest)
        {
            ThreadCache.SetCompareAction1(() =>
            {
                source.Load();
            });
            ThreadCache.SetCompareAction2(() =>
            {
                EData.LoadObject(dest);
            });
            ThreadCache.SetJudgeCode(OpCodes.Blt_S);
            return source;
        }
        public static SingleType operator ==(SingleType source, SingleType dest)
        {
            ThreadCache.SetCompareAction1(() =>
            {
                source.Load();
            });
            ThreadCache.SetCompareAction2(() =>
            {
                dest.Load();
            });
            if (source.TypeHandler == typeof(string))
            {
                source.ilHandler.Emit(OpCodes.Call, ClassCache.StringCompare);
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }
            else
            {
                ThreadCache.SetJudgeCode(OpCodes.Bne_Un_S);
            }
            return source;
        }
        public static SingleType operator ==(SingleType source, object dest)
        {
            ThreadCache.SetCompareAction1(() =>
            {
                source.Load();
            });
            ThreadCache.SetCompareAction2(() =>
            {
                EData.LoadObject(dest);
            });
            if (source.TypeHandler == typeof(string))
            {
                source.ilHandler.Emit(OpCodes.Call, ClassCache.StringCompare);
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }
            else
            {
                ThreadCache.SetJudgeCode(OpCodes.Bne_Un_S);
            }
            return source;
        }
        public static SingleType operator !=(SingleType source, SingleType dest)
        {
            ThreadCache.SetCompareAction1(() =>
            {
                source.Load();
            });
            ThreadCache.SetCompareAction2(() =>
            {
                dest.Load();
            });

            if (source.TypeHandler == typeof(string))
            {
                source.ilHandler.Emit(OpCodes.Call, ClassCache.StringCompare);
                ThreadCache.SetJudgeCode(OpCodes.Brtrue_S);
            }
            else
            {
                ThreadCache.SetJudgeCode(OpCodes.Beq_S);
            }
            return source;
        }
        public static SingleType operator !=(SingleType source, object dest)
        {
            ThreadCache.SetCompareAction1(() =>
            {
                source.Load();
            });
            ThreadCache.SetCompareAction2(() =>
            {
                EData.LoadObject(dest);
            });
            if (source.TypeHandler == typeof(string))
            {
                source.ilHandler.Emit(OpCodes.Call, ClassCache.StringCompare);
                ThreadCache.SetJudgeCode(OpCodes.Brtrue_S);
            }
            else
            {
                ThreadCache.SetJudgeCode(OpCodes.Beq_S);
            }
            return source;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }*/
        #endregion

        public new void Load()
        {
            LoadAddress();
        }
        public new void LoadAddress()
        {
            if (LoadAction != null)
            {
                LoadAction();

            }else if (Builder != null)
            {
                ilHandler.Emit(OpCodes.Ldloc, Builder);
            }
            else if (ParameterIndex != -1)
            {
                if (ParameterIndex == 0)
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
            else
            {
                if (TypeHandler.IsEnum)
                {
                    ilHandler.Emit(OpCodes.Ldc_I4, (int)Value);

                }
                else if (TypeHandler == typeof(int))
                {
                    int result;
                    if (int.TryParse(Value.ToString(), out result))
                    {
                        ilHandler.Emit(OpCodes.Ldc_I4, result);
                    }
                }
                else if (TypeHandler == typeof(string))
                {
                    string result = Value.ToString();
                    ilHandler.Emit(OpCodes.Ldstr, result);
                }
                else if (TypeHandler == typeof(double))
                {
                    double result;
                    if (double.TryParse(Value.ToString(), out result))
                    {
                        ilHandler.Emit(OpCodes.Ldc_R8, result);
                    }
                }
                else if (TypeHandler == typeof(bool))
                {
                    if ((bool)Value)
                    {
                        ilHandler.Emit(OpCodes.Ldc_I4_1);
                    }
                    else
                    {
                        ilHandler.Emit(OpCodes.Ldc_I4_0);
                    }
                    ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
                }
                else if (TypeHandler == typeof(float))
                {
                    float result;
                    if (float.TryParse(Value.ToString(), out result))
                    {
                        ilHandler.Emit(OpCodes.Ldc_R4, result);
                    }
                }
                else if (TypeHandler == typeof(byte))
                {
                    int result;
                    if (int.TryParse(Value.ToString(), out result))
                    {
                        ilHandler.Emit(OpCodes.Ldc_I4, result);
                    }
                }
                else if (TypeHandler == typeof(short))
                {
                    short result;
                    if (short.TryParse(Value.ToString(), out result))
                    {
                        ilHandler.Emit(OpCodes.Ldc_I4, result);
                    }
                }
                else if (TypeHandler == typeof(ushort))
                {
                    ushort result;
                    if (ushort.TryParse(Value.ToString(), out result))
                    {
                        ilHandler.Emit(OpCodes.Ldc_I4, result);
                    }
                }
                else if (TypeHandler == typeof(long))
                {
                    long result;
                    if (long.TryParse(Value.ToString(), out result))
                    {
                        ilHandler.Emit(OpCodes.Ldc_I4, result);
                        ilHandler.Emit(OpCodes.Conv_I8);
                    }
                }
                else if (TypeHandler == typeof(ulong))
                {
                    ulong result;
                    if (ulong.TryParse(Value.ToString(), out result))
                    {
                        ilHandler.Emit(OpCodes.Ldc_I4, result);
                        ilHandler.Emit(OpCodes.Conv_I8);
                    }
                }
                else if (TypeHandler == typeof(decimal))
                {
                    decimal result;
                    if (decimal.TryParse(Value.ToString(), out result))
                    {
                        ilHandler.Emit(OpCodes.Ldc_I4, (int)result);
                    }
                }
            }
        }


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
        }
        public void SubSelf()
        {
            this.Load();
            EData.LoadSelfObject(this.TypeHandler);
            this.ilHandler.Emit(OpCodes.Sub);
        }
        #endregion
       
    }
}
