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
                        ComplexType tempModel = this;
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
        public void Set(string memberName, Action action)
        {
            if (Struction.Fields.ContainsKey(memberName))
            {
                SField(memberName, action);
            }
            else if (Struction.Properties.ContainsKey(memberName))
            {
                SProperty(memberName, action);
            }
        }
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
        public EModel LoadStruct(string memberName)
        {
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
            else if (Struction.Properties.ContainsKey(memberName))
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
                EmitHelper.SetPrivateField(Load, info, value);
            }
            else
            {
                EmitHelper.SetPublicField(This, info, value);
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
                EmitHelper.SetPrivateProperty(Load, info, value);
            }
            else
            {
                EmitHelper.SetPublicProperty(This,info,value);
            }
        }
        public void SProperty(string propertyName, Action action)
        {
            SProperty(propertyName, (object)action);
        }
        #endregion

        #region Method操作
       
        public ComplexType EMethod(MethodInfo methodInfo, object[] parameters)
        {
            if (!methodInfo.IsStatic)
            {
                This();
            }
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i += 1)
                {
                    EData.NoErrorLoad(parameters[i], ilHandler);
                }
            }

            EmitHelper.CallMethod(TypeHandler, methodInfo);

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
            CompareType = info.FieldType;
            //私有字段
            if (!info.IsPublic)
            {
                EmitHelper.LoadPrivateField(Load, info);
            }
            //公有字段
            else
            {
                if (info.FieldType.IsValueType && !info.FieldType.IsPrimitive)
                {
                    EmitHelper.LoadPublicStructField(This, info);
                }
                else
                {
                    EmitHelper.LoadPublicField(This, info);
                }
            }
            //如果单独加载了bool类型的值
            if (CompareType == typeof(bool))
            {
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }
            return ELink.GetLink(CompareType);
        }
        public EModel LProperty(string propertyName)
        {
            PropertyInfo info = Struction.Properties[propertyName];
            CompareType = info.PropertyType;
            MethodInfo method = info.GetGetMethod(true);
            if (!method.IsPublic)
            {
                EmitHelper.LoadPrivateProperty(Load, info);
            }
            else
            {
                EmitHelper.LoadPublicProperty(This, info);
            }
            if (CompareType == typeof(bool))
            {
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }
            return ELink.GetLink(CompareType);
        }
        public EModel LField(string fieldName)
        {
            FieldInfo info = Struction.Fields[fieldName];
            CompareType = info.FieldType;
            //私有字段
            if (!info.IsPublic)
            {
                EmitHelper.LoadPrivateField(Load, info);
            }
            //公有字段
            else
            {
                 EmitHelper.LoadPublicField(This, info);
            }
            //如果单独加载了bool类型的值
            if (CompareType == typeof(bool))
            {
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }
            return ELink.GetLink(CompareType);
        }

        #endregion


        #region 调用属性
        private string MemberName;

        public ComplexType ALoad(string memberName)
        {
            MemberName = memberName;
            return this;
        }
        public T GetAttribute<T>(string attributeName) where T : Attribute
        {
            attributeName += "Attribute";
            if (Struction.AttributeTree.ContainsKey(MemberName))
            {
                if (Struction.AttributeTree[MemberName].ContainsKey(attributeName))
                {
                    return (T)Struction.AttributeTree[MemberName][attributeName];
                }
            }
            return null;
        }
        public EModel GetAttributeModel(string attributeName)
        {
            attributeName += "Attribute";
            if (Struction.AttributeTree.ContainsKey(MemberName))
            {
                if (Struction.AttributeTree[MemberName].ContainsKey(attributeName))
                {
                    object value = Struction.AttributeTree[MemberName][attributeName];
                    return EModel.CreateModelFromObject(value, value.GetType());
                }
            }
            return null;
        }
        public IEnumerable<object> GetAttributes(string memberName)
        {
            if (Struction.AttributeTree.ContainsKey(memberName))
            {
                return Struction.AttributeTree[MemberName].Values;
            }
            return null;
        }
        #endregion
    }
}
