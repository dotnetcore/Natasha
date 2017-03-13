using Natasha.Cache;
using Natasha.Core.Base;
using Natasha.Debug;
using Natasha.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Natasha.Core
{
    //对类和结构体的操作
    public class ComplexType : TypeInitiator, IOperator, IPacket, IDelayOperator
    {
        private Dictionary<string, Func<EModel, string, object, EModel>> DelayDict;
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
            return EmitHelper.GetLink(null);
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
            return EmitHelper.GetLink(null);
        }
        #endregion

        #region Field操作

        public void SField(string fieldName, object value)
        {
            FieldInfo info = Struction.Fields[fieldName];
            if (!info.IsPublic)
            {
                MemberHelper.SetPrivateField(Load, info, value);
            }
            else
            {
                MemberHelper.SetPublicField(This, info, value);
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
                MemberHelper.SetPrivateProperty(Load, info, value);
            }
            else
            {
                MemberHelper.SetPublicProperty(This, info, value);
            }
        }
        public void SProperty(string propertyName, Action action)
        {
            SProperty(propertyName, (object)action);
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
                MemberHelper.LoadPrivateField(Load, info);
            }
            //公有字段
            else
            {
                if (info.FieldType.IsValueType && !info.FieldType.IsPrimitive)
                {
                    MemberHelper.LoadPublicStructField(This, info);
                }
                else
                {
                    MemberHelper.LoadPublicField(This, info);
                }
            }
            //如果单独加载了bool类型的值
            if (CompareType == typeof(bool))
            {
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }
            return EmitHelper.GetLink(CompareType);
        }
        public EModel LProperty(string propertyName)
        {
            PropertyInfo info = Struction.Properties[propertyName];
            CompareType = info.PropertyType;
            MethodInfo method = info.GetGetMethod(true);
            if (!method.IsPublic)
            {
                MemberHelper.LoadPrivateProperty(Load, info);
            }
            else
            {
                MemberHelper.LoadPublicProperty(This, info);
            }
            if (CompareType == typeof(bool))
            {
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }
            return EmitHelper.GetLink(CompareType);
        }
        public EModel LField(string fieldName)
        {
            FieldInfo info = Struction.Fields[fieldName];
            CompareType = info.FieldType;
            //私有字段
            if (!info.IsPublic)
            {
                MemberHelper.LoadPrivateField(Load, info);
            }
            //公有字段
            else
            {
                MemberHelper.LoadPublicField(This, info);
            }
            //如果单独加载了bool类型的值
            if (CompareType == typeof(bool))
            {
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }
            return EmitHelper.GetLink(CompareType);
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

        #region 拆装箱


        public void Packet()
        {
            OperatorHelper.Packet(CompareType);
        }

        public void UnPacket()
        {
            OperatorHelper.UnPacket(CompareType);
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
        #endregion

        #region 延迟加载

        private List<Func<EModel, EModel>> tempDelayFunc;
        private List<string> SelfAdd;

        public void Initialize()
        {
            DelayDict = new Dictionary<string, Func<EModel, string, object, EModel>>();
            DelayDict["LoadStruct"] = (model, key, value) => { return model.LoadStruct(key); };
            DelayDict["Load"] = (model, key, value) => { return model.Load(key); };
            DelayDict["Set"] = (model, key, value) => { model.Set(key, value); return model; };
            DelayDict["Packet"] = (model, key, value) => { model.Packet(); return model; };
            DelayDict["UnPacket"] = (model, key, value) => { model.UnPacket(); return model; };
            DelayDict["InStackAndPacket"] = (model, key, value) => { model.InStackAndPacket(); return model; };
            DelayDict["InStackAndUnPacket"] = (model, key, value) => { model.InStackAndUnPacket(); return model; };
        }
        private EModel AddDelayFunc(string key, string Name, object value)
        {
            if (tempDelayFunc == null)
            {
                tempDelayFunc = new List<Func<EModel, EModel>>();
            }
            tempDelayFunc.Add((model) => { return DelayDict[key](model, Name, value); });
            return (EModel)this;
        }
        public EModel DLoad(string Name)
        {
            if (SelfAdd == null)
            {
                SelfAdd = new List<string>();
            }
            SelfAdd.Add(Name);
            return AddDelayFunc("Load", Name, null);
        }
        public EModel DLoadStruct(string Name)
        {
            return AddDelayFunc("LoadStruct", Name, null);
        }
        public EModel DPacket(string Name, object value)
        {
            return AddDelayFunc("Packet", Name, null);
        }
        public EModel DUnPacket(string Name, object value)
        {
            return AddDelayFunc("UnPacket", Name, null);
        }
        public EModel DInStackAndPacket(string Name, object value)
        {
            return AddDelayFunc("InStackAndPacket", Name, null);
        }
        public EModel DInStackAndUnPacket(string Name, object value)
        {
            return AddDelayFunc("InStackAndUnPacket", Name, null);
        }
        public EModel DSet(string Name, object value)
        {
            return AddDelayFunc("Set", Name, value);
        }

        public Action DelayAction
        {
            get
            {
                if (tempDelayFunc != null)
                {
                    _delayAction = GetDelayAction();
                }
                return _delayAction;
            }
            set { _delayAction = value; }
        }

        private Action GetDelayAction()
        {
            Func<EModel, EModel>[] funcArray = tempDelayFunc.ToArray();
            Action action = () =>
            {
                EModel tempModel = (EModel)this;
                for (int i = 0; i < funcArray.Length; i += 1)
                {
                    tempModel = funcArray[i](tempModel);
                }
                _delayAction = null;
            };
            string[] SelfOperatorArray = SelfAdd.ToArray();
            CurrentDelayAction = (code) =>
            {
                Action setAction = () =>
                {
                    EModel loadModel = (EModel)this;
                    for (int i = 0; i < funcArray.Length; i += 1)
                    {
                        loadModel = funcArray[i](loadModel);
                    }
                    DataHelper.LoadSelfObject(TypeHandler);
                    ilHandler.Emit(code);
                    DebugHelper.WriteLine(code.Name);
                };
                ComplexType tempModel = this;

                for (int i = 0; i < SelfOperatorArray.Length - 1; i += 1)
                {
                    tempModel = tempModel.LoadStruct(SelfOperatorArray[i]);
                }
                tempModel.Set(SelfOperatorArray[SelfOperatorArray.Length - 1], setAction);
            };
            SelfAdd = null;
            tempDelayFunc = null;
            return action;
        }
        public EModel LoadEnd()
        {
            Action action = GetDelayAction();
            _delayAction = action;
            if (DDelayAction == null)
            {
                DDelayAction = action;
            }
            else
            {
                SDelayAction = action;
            }
            return (EModel)this;
        }

        private Action DDelayAction;

        private Action SDelayAction;

        private Action<OpCode> CurrentDelayAction;

        public Type CompareType;

        private Action _delayAction;

        public void AddSelf()
        {
            CurrentDelayAction(OpCodes.Add);
        }
        public void SubSelf()
        {
            CurrentDelayAction(OpCodes.Sub);
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
        #endregion

        #region 数据填充
        public void Store(Action action)
        {
            if (action != null)
            {
                action();
            }
            Store();
        }
        public void Store()
        {
            if (Builder != null)
            {
                if (Builder.LocalIndex > 3)
                {
                    ilHandler.Emit(OpCodes.Stloc_S, Builder.LocalIndex);
                    DebugHelper.WriteLine("Stloc_S", Builder.LocalIndex);
                }
                else if (Builder.LocalIndex == 0)
                {
                    ilHandler.Emit(OpCodes.Stloc_0);
                    DebugHelper.WriteLine("Stloc_0");
                }
                else if (Builder.LocalIndex == 1)
                {
                    ilHandler.Emit(OpCodes.Stloc_1);
                    DebugHelper.WriteLine("Stloc_1");
                }
                else if (Builder.LocalIndex == 2)
                {
                    ilHandler.Emit(OpCodes.Stloc_2);
                    DebugHelper.WriteLine("Stloc_2");
                }
                else if (Builder.LocalIndex == 3)
                {
                    ilHandler.Emit(OpCodes.Stloc_3);
                    DebugHelper.WriteLine("Stloc_3");
                }
            }
            if (ParameterIndex != -1)
            {
                ilHandler.Emit(OpCodes.Starg_S, ParameterIndex);
                DebugHelper.WriteLine("Starg_S", ParameterIndex);
            }
        }
        #endregion
    }
}
