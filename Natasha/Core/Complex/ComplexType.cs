using Natasha.Cache;
using Natasha.Core.Base;
using Natasha.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Natasha.Core
{
    //对类和结构体的操作
    public class ComplexType : TypeInitiator, IOperator
    {
        public ComplexType(Type parameter_Type)
            : base(parameter_Type)
        {
        }
        public ComplexType(Action action, Type parameter_Type)
            : base(action, parameter_Type)
        {
        }
        public ComplexType(LocalBuilder builder, Type parameter_Type)
            : base(builder, parameter_Type)
        {
        }
        public ComplexType(int parameterIndex, Type parameter_Type)
            : base(parameterIndex, parameter_Type)
        {
        }



        #region 运算接口
        private List<string> DelayList;

        private Action DDelayAction;

        private Action SDelayAction;

        public Action<OpCode> CurrentDelayAction;

        public Type CompareType;

        private Action _delayAction;
        public Action DelayAction
        {
            get
            {
                if (DelayList != null)
                {
                    string[] DelayArray = new string[DelayList.Count];
                    int length = DelayList.Count;
                    for (int i = 0; i < length; i += 1)
                    {
                        DelayArray[i] = DelayList[i];
                    }

                    Action action = () =>
                    {
                        ComplexType tempModel = (ComplexType)this;
                        for (int i = 0; i < DelayArray.Length - 1; i += 1)
                        {
                            tempModel = tempModel.LoadStruct(DelayArray[i]);
                        }
                        tempModel.Load(DelayArray[DelayArray.Length - 1]);
                        _delayAction = null;
                    };
                    DelayList = null;
                    _delayAction = action;
                }
                return _delayAction;
            }
            set { _delayAction = value; }
        }
        public void AddSelf()
        {
            CurrentDelayAction(OpCodes.Add);
        }
        public void SubSelf()
        {
            CurrentDelayAction(OpCodes.Sub);
        }
        public EModel LoadEnd()
        {
            string[] DelayArray = new string[DelayList.Count];
            int length = DelayList.Count;
            for (int i = 0; i < length; i += 1)
            {
                DelayArray[i] = DelayList[i];
            }

            Action action = () =>
            {
                ComplexType tempModel = this;
                for (int i = 0; i < DelayArray.Length - 1; i += 1)
                {
                    tempModel = tempModel.LoadStruct(DelayArray[i]);
                }
                tempModel.Load(DelayArray[DelayArray.Length - 1]);
            };
            DelayAction = action;

            CurrentDelayAction = (code) =>
            {
                Action setAction = () =>
                {
                    action();
                    EData.LoadSelfObject(TypeHandler);
                    ilHandler.Emit(code);
                };
                ComplexType tempModel = this;
                for (int i = 0; i < DelayArray.Length - 1; i += 1)
                {
                    tempModel = tempModel.LoadStruct(DelayArray[i]);
                }
                tempModel.Set(DelayArray[DelayArray.Length - 1], setAction);
            };
            if (DDelayAction == null)
            {
                DDelayAction = action;
            }
            else
            {
                SDelayAction = action;
            }
            DelayList = null;
            return (EModel)this;
        }

        public void RunCompareAction()
        {
            if (DDelayAction != null)
            {
                DDelayAction();
                DDelayAction = null;
                return;
            }
            if (SDelayAction != null)
            {
                SDelayAction();
                SDelayAction = null;
                return;
            }
        }
        public EModel DLoad(string Name)
        {
            if (DelayList == null)
            {
                DelayList = new List<string>();
            }
            DelayList.Add(Name);
            return (EModel)this;
        }

        #endregion

        #region Field、Property操作
        public void Set(string memberName, object value)
        {
            if (Struction.Fields.ContainsKey(memberName))
            {
                SField(memberName, value);
            }
            else if (Struction.Properties.ContainsKey(memberName))
            {
                SProperty(memberName, value);
            }
        }
        public EModel LoadStruct(string memberName) {
            if (Struction.Fields.ContainsKey(memberName))
            {
                return LFieldStructr(memberName);
            }
            else if (Struction.Properties.ContainsKey(memberName))
            {
                return LProperty(memberName);
            }
            return ELink.GetLink(null);
        }

        public EModel Load(string memberName)
        {
            if (Struction.Fields.ContainsKey(memberName))
            {
                return LField(memberName);
            }
            else if(Struction.Properties.ContainsKey(memberName))
            {
                return LProperty(memberName);
            }
            return ELink.GetLink(null);
        }
        #endregion

        #region Field操作

        public void SField(string fieldName, object value)
        {
            FieldInfo info = Struction.Fields[fieldName];
            if (!info.IsPublic)
            {
                //加载SetValue的第一个参数 即FieldInfo
                ilHandler.Emit(OpCodes.Ldtoken, TypeHandler);
                ilHandler.Emit(OpCodes.Call, ClassCache.ClassHandle);
                ilHandler.Emit(OpCodes.Ldstr, info.Name);
                ilHandler.Emit(OpCodes.Ldc_I4_S, 44);
                ilHandler.Emit(OpCodes.Callvirt, ClassCache.FieldInfoGetter);

                //加载SetValue的第二个参数

                Load();
                EPacket.Packet(TypeHandler);

                //加载SetValue的第三个参数
                EData.NoErrorLoad(value, ilHandler);
                if (value != null)
                {
                    EPacket.Packet(value.GetType());
                }
                //调用SetValue
                ilHandler.Emit(OpCodes.Callvirt, ClassCache.FieldValueSetter);
            }
            else
            {

                if (info.IsStatic)
                {
                    //填充静态字段
                    EData.NoErrorLoad(value, ilHandler);
                    ilHandler.Emit(OpCodes.Stsfld, info);
                }
                else
                {
                    //如果是结构体需要加载地址
                    LoadAddress();
                    EData.NoErrorLoad(value, ilHandler);
                    ilHandler.Emit(OpCodes.Stfld, info);
                }
            }
        }

        public void SField(string fieldName, Action action)
        {
            SField(fieldName, (object)action);
        }
        #endregion

        #region Property操作
        public void SProperty(string propertyName, object value)
        {
            PropertyInfo info = Struction.Properties[propertyName];
            MethodInfo method = info.GetSetMethod(true);
            if (!method.IsPublic)
            {
                //加载SetValue的第一个参数 即PropertyInfo
                ilHandler.Emit(OpCodes.Ldtoken, TypeHandler);
                ilHandler.Emit(OpCodes.Call, ClassCache.ClassHandle);
                ilHandler.Emit(OpCodes.Ldstr, info.Name);
                ilHandler.Emit(OpCodes.Ldc_I4_S, 44);
                ilHandler.Emit(OpCodes.Call, ClassCache.PropertyInfoGetter);

                //加载SetValue的第二个参数

                Load();
                EPacket.Packet(TypeHandler);


                //加载SetValue的第三个参数
                EData.NoErrorLoad(value, ilHandler);
                if (value != null)
                {
                    EPacket.Packet(value.GetType());
                }

                //调用SetValue
                ilHandler.Emit(OpCodes.Callvirt, ClassCache.PropertyValueSetter);
            }
            else
            {
                //静态属性
                if (!method.IsStatic)
                {
                    LoadAddress();
                }

                EData.NoErrorLoad(value, ilHandler);

                if (IsStuct || method.IsStatic)
                {
                    ilHandler.Emit(OpCodes.Call, method);
                }
                else
                {
                    ilHandler.Emit(OpCodes.Callvirt, method);
                }
            }
        }
        public void SProperty(string propertyName, Action action)
        {
            SProperty(propertyName, (object)action);
        }
        #endregion

        #region Method操作
        public ComplexType EMethod(string methodName, params object[] parameters)
        {
            return EMethod(MethodHandler.GetMethodInfo(methodName), parameters);
        }
        public ComplexType EMethod<T1>(string methodName, params object[] parameters)
        {
            return EMethod(MethodHandler.GetMethodInfo<T1>(methodName), parameters);
        }
        public ComplexType EMethod<T1, T2>(string methodName, params object[] parameters)
        {
            return EMethod(MethodHandler.GetMethodInfo<T1, T2>(methodName), parameters);
        }
        public ComplexType EMethod<T1, T2, T3>(string methodName, params object[] parameters)
        {
            return EMethod(MethodHandler.GetMethodInfo<T1, T2, T3>(methodName), parameters);
        }
        public ComplexType EMethod<T1, T2, T3, T4>(string methodName, params object[] parameters)
        {
            return EMethod(MethodHandler.GetMethodInfo<T1, T2, T3, T4>(methodName), parameters);
        }
        public ComplexType EMethod<T1, T2, T3, T4, T5>(string methodName, params object[] parameters)
        {
            return EMethod(MethodHandler.GetMethodInfo<T1, T2, T3, T4, T5>(methodName), parameters);
        }
        public ComplexType EMethod<T1, T2, T3, T4, T5, T6>(string methodName, params object[] parameters)
        {
            return EMethod(MethodHandler.GetMethodInfo<T1, T2, T3, T4, T5, T6>(methodName), parameters);
        }
        public ComplexType EMethod<T1, T2, T3, T4, T5, T6, T7>(string methodName, params object[] parameters)
        {
            return EMethod(MethodHandler.GetMethodInfo<T1, T2, T3, T4, T5, T6, T7>(methodName), parameters);
        }
        public ComplexType EMethod<T1, T2, T3, T4, T5, T6, T7, T8>(string methodName, params object[] parameters)
        {
            return EMethod(MethodHandler.GetMethodInfo<T1, T2, T3, T4, T5, T6, T7, T8>(methodName), parameters);
        }
        public ComplexType EMethod(MethodInfo methodInfo, object[] parameters)
        {
            if (!methodInfo.IsStatic)
            {
                LoadAddress();
            }
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i += 1)
                {
                    EData.NoErrorLoad(parameters[i], ilHandler);
                }

            }
            if (IsStuct || methodInfo.IsStatic)
            {
                ilHandler.Emit(OpCodes.Call, methodInfo);
            }
            else
            {
                ilHandler.Emit(OpCodes.Callvirt, methodInfo);
            }
            if (methodInfo.ReturnType == typeof(bool))
            {
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }
            return ELink.GetLink(methodInfo.ReturnType);
        }
        #endregion

        #region 嵌套调用接口
        public EModel LFieldStructr(string fieldName)
        {
            FieldInfo info = Struction.Fields[fieldName];
            Type type = info.FieldType;
            CompareType = type;
            //私有字段
            if (!info.IsPublic)
            {
                //加载GetValue的第一个参数 即FieldInfo
                ilHandler.Emit(OpCodes.Ldtoken, TypeHandler);
                ilHandler.Emit(OpCodes.Call, ClassCache.ClassHandle);
                ilHandler.Emit(OpCodes.Ldstr, fieldName);
                ilHandler.Emit(OpCodes.Ldc_I4_S, 44);
                ilHandler.Emit(OpCodes.Callvirt, ClassCache.FieldInfoGetter);

                //加载GetValue的第二个参数
                Load();
                EPacket.Packet(TypeHandler);

                //调用GetValue
                ilHandler.Emit(OpCodes.Callvirt, ClassCache.FieldValueGetter);

                //拆箱
                if (type.IsClass && type != typeof(string) && type != typeof(object))
                {
                    ilHandler.Emit(OpCodes.Castclass, type);
                }
                else
                {
                    ilHandler.Emit(OpCodes.Unbox_Any, type);
                }
            }
            //公有字段
            else
            {
                //静态字段
                if (info.IsStatic)
                {
                    //加载地址
                    if (type.IsValueType && !type.IsPrimitive)
                    {
                        ilHandler.Emit(OpCodes.Ldsflda, info);
                    }
                    else
                    {
                        ilHandler.Emit(OpCodes.Ldsfld, info);
                    }
                }
                else
                {
                    //如果是结构体那么加载结构体地址
                    LoadAddress();

                    if (type.IsValueType && !type.IsPrimitive)
                    {
                        ilHandler.Emit(OpCodes.Ldflda, info);
                    }
                    else
                    {
                        ilHandler.Emit(OpCodes.Ldfld, info);
                    }
                }
            }
            //如果单独加载了bool类型的值
            if (type == typeof(bool))
            {
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }
            return ELink.GetLink(type);
        }
        public EModel LProperty(string propertyName)
        {
            PropertyInfo info = Struction.Properties[propertyName];
            Type type = info.PropertyType;
            CompareType = type;
            MethodInfo method = info.GetGetMethod(true);
            if (!method.IsPublic)
            {
                //加载GetValue的第一个参数 即PropertyInfo
                ilHandler.Emit(OpCodes.Ldtoken, TypeHandler);
                ilHandler.Emit(OpCodes.Call, ClassCache.ClassHandle);
                ilHandler.Emit(OpCodes.Ldstr, propertyName);
                ilHandler.Emit(OpCodes.Ldc_I4_S, 44);
                ilHandler.Emit(OpCodes.Callvirt, ClassCache.PropertyInfoGetter);

                //加载GetValue的第二个参数
                Load();
                EPacket.Packet(TypeHandler);

                //调用GetValue
                ilHandler.Emit(OpCodes.Callvirt, ClassCache.ProeprtyValueGetter);

                //拆箱
                if (type.IsClass && type != typeof(string) && type != typeof(object))
                {
                    ilHandler.Emit(OpCodes.Castclass, type);
                }
                else
                {
                    ilHandler.Emit(OpCodes.Unbox_Any, type);
                }
            }
            else
            {
                //静态属性
                if (method.IsStatic)
                {
                    ilHandler.Emit(OpCodes.Call, method);
                }
                else
                {
                    LoadAddress();
                    if (IsStuct)
                    {
                        ilHandler.Emit(OpCodes.Call, method);
                    }
                    else
                    {
                        ilHandler.Emit(OpCodes.Callvirt, method);
                    }
                }
            }

            if (type == typeof(bool))
            {
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }
            return ELink.GetLink(type);
        }
        public EModel LField(string fieldName)
        {
            FieldInfo info = Struction.Fields[fieldName];
            Type type = info.FieldType;
            CompareType = type;
            //私有字段
            if (!info.IsPublic)
            {
                //加载GetValue的第一个参数 即FieldInfo
                ilHandler.Emit(OpCodes.Ldtoken, TypeHandler);
                ilHandler.Emit(OpCodes.Call, ClassCache.ClassHandle);
                ilHandler.Emit(OpCodes.Ldstr, fieldName);
                ilHandler.Emit(OpCodes.Ldc_I4_S, 44);
                ilHandler.Emit(OpCodes.Callvirt, ClassCache.FieldInfoGetter);

                //加载GetValue的第二个参数
                Load();
                EPacket.Packet(TypeHandler);

                //调用GetValue
                ilHandler.Emit(OpCodes.Callvirt, ClassCache.FieldValueGetter);

                //拆箱
                if (type.IsClass && type != typeof(string) && type != typeof(object))
                {
                    ilHandler.Emit(OpCodes.Castclass, type);
                }
                else
                {
                    ilHandler.Emit(OpCodes.Unbox_Any, type);
                }
            }
            //公有字段
            else
            {
                //静态字段
                if (info.IsStatic)
                {
                    ilHandler.Emit(OpCodes.Ldsfld, info);
                }
                else
                {
                    Load();
                    ilHandler.Emit(OpCodes.Ldfld, info);
                }
            }
            //如果单独加载了bool类型的值
            if (type == typeof(bool))
            {
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }
            return ELink.GetLink(type);
        }

        #endregion



    }
}
