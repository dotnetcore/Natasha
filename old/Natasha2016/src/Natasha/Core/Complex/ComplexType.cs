using Natasha.Cache;
using Natasha.Core.Base;
using Natasha.Core.Complex;
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
        internal Type PreCallType;
        internal LinkCallOption TempOption;

        internal string _currentMemeberName;
        internal Type _currentPrivateType;
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
        public EModel Load(string memberName)
        {
            _currentMemeberName = memberName;
            if (Struction.Fields.ContainsKey(memberName))
            {
                return LField(memberName);
            }
            else if (Struction.Properties.ContainsKey(memberName))
            {
                return LProperty(memberName);
            }
            throw new NullReferenceException("找不到" + memberName + "成员！");
        }

        public EModel LoadValue(string memberName)
        {
            _currentMemeberName = memberName;
            if (Struction.Fields.ContainsKey(memberName))
            {
                return LFieldValue(memberName);
            }
            else if (Struction.Properties.ContainsKey(memberName))
            {
                return LPropertyValue(memberName);
            }
            throw new NullReferenceException("找不到" + memberName + "成员！");
        }
        #endregion

        #region Field操作

        public void SField(string fieldName, object value)
        {
            FieldInfo info = Struction.Fields[fieldName];
            if (!info.IsPublic)
            {
                il.SetPrivateField(Load, info, value);
            }
            else
            {
                il.SetPublicField(This, info, value);
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
                il.SetPrivateProperty(Load, info, value);
            }
            else
            {
                il.SetPublicProperty(This, info, value);
            }
        }
        public void SProperty(string propertyName, Action action)
        {
            SProperty(propertyName, (object)action);
        }
        #endregion

        #region 嵌套调用接口
        public EModel LField(string fieldName, bool isLoadValue = false)
        { 
            _currentMemeberName = fieldName;
            FieldInfo info = Struction.Fields[_currentMemeberName];
            CompareType = info.FieldType;

            CurrentCallOption = GetCurrentOption(_currentMemeberName);
            switch (PrewCallOption)
            {
                case LinkCallOption.Default:
                    #region 第一次调用
                    switch (CurrentCallOption)
                    {
                        case LinkCallOption.Public_Field_Value_Call:
                            This();
                            if (isLoadValue || info.FieldType.IsPrimitive)
                            {
                                il.REmit(OpCodes.Ldfld, info);
                            }
                            else
                            {
                                il.REmit(OpCodes.Ldflda, info);
                            }
                            break;
                        case LinkCallOption.Public_Static_Field_Value_Call:
                            if (isLoadValue || info.FieldType.IsPrimitive)
                            {
                                il.REmit(OpCodes.Ldsfld, info);
                            }
                            else
                            {
                                il.REmit(OpCodes.Ldsflda, info);
                            }
                            break;
                        case LinkCallOption.Public_Field_Ref_Call:
                            This();
                            il.REmit(OpCodes.Ldfld, info);
                            break;
                        case LinkCallOption.Public_Static_Field_Ref_Call:
                            il.REmit(OpCodes.Ldsfld, info);
                            break;
                        case LinkCallOption.Private_Field_Value_Call:
                        case LinkCallOption.Private_Field_Ref_Call:
                            il.REmit(OpCodes.Ldtoken, info.DeclaringType);
                            il.REmit(OpCodes.Call, ClassCache.ClassHandle);
                            il.REmit(OpCodes.Ldstr, info.Name);
                            il.REmit(OpCodes.Ldc_I4_S, 44);
                            il.REmit(OpCodes.Callvirt, ClassCache.FieldInfoGetter);
                            Load();
                            il.Packet(info.DeclaringType);
                            il.REmit(OpCodes.Callvirt, ClassCache.FieldValueGetter);
                            il.UnPacket(info.FieldType);
                            break;
                        default:
                            break;
                    }
                    break;
                #endregion
                case LinkCallOption.Public_Ref_Call:
                case LinkCallOption.Private_Ref_Call:
                case LinkCallOption.Public_Value_Call:
                    #region 前一次非特殊类型的链式调用
                    switch (CurrentCallOption)
                    {
                        case LinkCallOption.Public_Field_Value_Call:
                            if (isLoadValue)
                            {
                                il.REmit(OpCodes.Ldfld, info);
                            }
                            else
                            {
                                il.REmit(OpCodes.Ldflda, info);
                            }
                            break;
                        case LinkCallOption.Public_Static_Field_Value_Call:
                            if (isLoadValue)
                            {
                                il.REmit(OpCodes.Ldsfld, info);
                            }
                            else
                            {
                                il.REmit(OpCodes.Ldsflda, info);
                            }
                            break;
                        case LinkCallOption.Public_Field_Ref_Call:
                            il.REmit(OpCodes.Ldfld, info);
                            break;
                        case LinkCallOption.Public_Static_Field_Ref_Call:
                            il.REmit(OpCodes.Ldsfld, info);
                            break;
                        case LinkCallOption.Private_Field_Value_Call:
                        case LinkCallOption.Private_Field_Ref_Call:
                            LocalBuilder tempBuidler = il.DeclareLocal(PreCallType);
                            il.EmitStoreBuilder(tempBuidler);
                            il.REmit(OpCodes.Ldtoken, info.DeclaringType);
                            il.REmit(OpCodes.Call, ClassCache.ClassHandle);
                            il.REmit(OpCodes.Ldstr, info.Name);
                            il.REmit(OpCodes.Ldc_I4_S, 44);
                            il.REmit(OpCodes.Callvirt, ClassCache.FieldInfoGetter);
                            il.LoadBuilder(tempBuidler, false);
                            il.Packet(info.DeclaringType);
                            il.REmit(OpCodes.Callvirt, ClassCache.FieldValueGetter);
                            il.UnPacket(info.FieldType);
                            break;
                        default:
                            break;
                    }
                    break;
                #endregion
                case LinkCallOption.Private_Value_Call:
                    #region 前一次为私有值类型的链式调用

                    if (CurrentCallOption == LinkCallOption.Private_Field_Value_Call || CurrentCallOption == LinkCallOption.Private_Field_Ref_Call)
                    {
                        LocalBuilder tempBuidler = il.DeclareLocal(PreCallType);
                        il.EmitStoreBuilder(tempBuidler);
                        il.REmit(OpCodes.Ldtoken, info.DeclaringType);
                        il.REmit(OpCodes.Call, ClassCache.ClassHandle);
                        il.REmit(OpCodes.Ldstr, info.Name);
                        il.REmit(OpCodes.Ldc_I4_S, 44);
                        il.REmit(OpCodes.Callvirt, ClassCache.FieldInfoGetter);
                        il.LoadBuilder(tempBuidler, false);
                        il.Packet(info.DeclaringType);
                        il.REmit(OpCodes.Callvirt, ClassCache.FieldValueGetter);
                        il.UnPacket(info.FieldType);
                    }
                    else
                    {
                        LocalBuilder tempBuidler = il.DeclareLocal(PreCallType);
                        il.EmitStoreBuilder(tempBuidler);
                        il.LoadBuilder(tempBuidler);
                        switch (CurrentCallOption)
                        {
                            case LinkCallOption.Public_Field_Value_Call:
                                if (isLoadValue)
                                {
                                    il.REmit(OpCodes.Ldfld, info);
                                }
                                else
                                {
                                    il.REmit(OpCodes.Ldflda, info);
                                }
                                break;
                            case LinkCallOption.Public_Static_Field_Value_Call:
                                if (isLoadValue)
                                {
                                    il.REmit(OpCodes.Ldsfld, info);
                                }
                                else
                                {
                                    il.REmit(OpCodes.Ldsflda, info);
                                }
                                break;
                            case LinkCallOption.Public_Field_Ref_Call:
                                il.REmit(OpCodes.Ldfld, info);
                                break;
                            case LinkCallOption.Public_Static_Field_Ref_Call:
                                il.REmit(OpCodes.Ldsfld, info);
                                break;
                            default:
                                break;
                        }
                    }

                    break;
                #endregion
                default:
                    break;
            }
            PreCallType = CompareType;
            if (info.FieldType == typeof(bool))
            {
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }
            if (Struction.DelegateMethods.ContainsKey(_currentMemeberName))
            {
                return il.GetLink((EModel)this, TypeHandler);
            }
            else
            {
                return il.GetLink((EModel)this, CompareType);
            }
        }

        public EModel LProperty(string propertyName, bool isLoadValue = false)
        {
            _currentMemeberName = propertyName;
            PropertyInfo pInfo = Struction.Properties[_currentMemeberName];
            MethodInfo info = pInfo.GetGetMethod(true);
            if (info == null)
            {
                throw new NullReferenceException("找不到" + propertyName + "属性的Get方法！");
            }
            CompareType = pInfo.PropertyType;
            
            CurrentCallOption = GetCurrentOption(_currentMemeberName);
            switch (PrewCallOption)
            {
                case LinkCallOption.Default:
                    #region 第一次调用
                    switch (CurrentCallOption)
                    {
                        case LinkCallOption.Public_Property_Value_Call:
                            This();
                            il.REmit(OpCodes.Call, info);
                            break;
                        case LinkCallOption.Public_Static_Property_Value_Call:
                            il.REmit(OpCodes.Call, info);
                            break;
                        case LinkCallOption.Public_Property_Ref_Call:
                            This();
                            il.REmit(OpCodes.Callvirt, info);
                            break;
                        case LinkCallOption.Public_Static_Property_Ref_Call:
                            il.REmit(OpCodes.Call, info);
                            break;
                        case LinkCallOption.Private_Property_Value_Call:
                        case LinkCallOption.Private_Property_Ref_Call:
                            il.REmit(OpCodes.Ldtoken, pInfo.DeclaringType);
                            il.REmit(OpCodes.Call, ClassCache.ClassHandle);
                            il.REmit(OpCodes.Ldstr, pInfo.Name);
                            il.REmit(OpCodes.Ldc_I4_S, 44);
                            il.REmit(OpCodes.Call, ClassCache.PropertyInfoGetter);
                            Load();
                            il.Packet(pInfo.DeclaringType);
                            il.REmit(OpCodes.Callvirt, ClassCache.PropertyValueGetter);
                            il.UnPacket(pInfo.PropertyType);
                            break;
                        default:
                            break;
                    }
                    break;
                #endregion
                case LinkCallOption.Private_Ref_Call:
                case LinkCallOption.Public_Ref_Call:
                    #region 前一次非特殊类型的链式调用
                    if (CurrentCallOption == LinkCallOption.Private_Property_Value_Call || CurrentCallOption == LinkCallOption.Private_Property_Ref_Call)
                    {
                        LocalBuilder tempBuidler = il.DeclareLocal(PreCallType);
                        il.EmitStoreBuilder(tempBuidler);
                        il.REmit(OpCodes.Ldtoken, pInfo.DeclaringType);
                        il.REmit(OpCodes.Call, ClassCache.ClassHandle);
                        il.REmit(OpCodes.Ldstr, pInfo.Name);
                        il.REmit(OpCodes.Ldc_I4_S, 44);
                        il.REmit(OpCodes.Call, ClassCache.PropertyInfoGetter);
                        il.LoadBuilder(tempBuidler, false);
                        il.Packet(pInfo.DeclaringType);
                        il.REmit(OpCodes.Callvirt, ClassCache.PropertyValueGetter);
                        il.UnPacket(pInfo.PropertyType);
                    }
                    else if (CurrentCallOption == LinkCallOption.Public_Property_Value_Call)
                    {
                        LocalBuilder tempBuidler = il.DeclareLocal(PreCallType);
                        il.EmitStoreBuilder(tempBuidler);
                        il.LoadBuilder(tempBuidler);
                        il.REmit(OpCodes.Call, info);

                    }else
                    {
                        il.REmit(OpCodes.Callvirt, info);
                    }
                    #endregion
                    break;
                case LinkCallOption.Public_Value_Call:
                    #region 前一次非特殊类型的链式调用
                    if (CurrentCallOption == LinkCallOption.Private_Property_Value_Call || CurrentCallOption == LinkCallOption.Private_Property_Ref_Call)
                    {
                        LocalBuilder tempBuidler = il.DeclareLocal(PreCallType);
                        il.EmitStoreBuilder(tempBuidler);
                        il.REmit(OpCodes.Ldtoken, pInfo.DeclaringType);
                        il.REmit(OpCodes.Call, ClassCache.ClassHandle);
                        il.REmit(OpCodes.Ldstr, pInfo.Name);
                        il.REmit(OpCodes.Ldc_I4_S, 44);
                        il.REmit(OpCodes.Call, ClassCache.PropertyInfoGetter);
                        il.LoadBuilder(tempBuidler, false);
                        il.Packet(pInfo.DeclaringType);
                        il.REmit(OpCodes.Callvirt, ClassCache.PropertyValueGetter);
                        il.UnPacket(pInfo.PropertyType);
                    }
                    else
                    {
                        il.REmit(OpCodes.Call, info);
                    }
                    break;
                #endregion
                case LinkCallOption.Private_Value_Call:
                    #region 前一次为私有值类型的链式调用

                    if (CurrentCallOption == LinkCallOption.Private_Property_Value_Call || CurrentCallOption == LinkCallOption.Private_Property_Ref_Call)
                    {
                        LocalBuilder tempBuidler = il.DeclareLocal(PreCallType);
                        il.EmitStoreBuilder(tempBuidler);
                        il.REmit(OpCodes.Ldtoken, pInfo.DeclaringType);
                        il.REmit(OpCodes.Call, ClassCache.ClassHandle);
                        il.REmit(OpCodes.Ldstr, pInfo.Name);
                        il.REmit(OpCodes.Ldc_I4_S, 44);
                        il.REmit(OpCodes.Call, ClassCache.PropertyInfoGetter);
                        il.LoadBuilder(tempBuidler, false);
                        il.Packet(pInfo.DeclaringType);
                        il.REmit(OpCodes.Callvirt, ClassCache.PropertyValueGetter);
                        il.UnPacket(pInfo.PropertyType);
                    }
                    else
                    {
                        LocalBuilder tempBuidler = il.DeclareLocal(PreCallType);
                        il.EmitStoreBuilder(tempBuidler);
                        il.LoadBuilder(tempBuidler);
                        il.REmit(OpCodes.Call, info);
                    }
                    break;
                #endregion
                default:
                    break;
            }
            PreCallType = CompareType;
            if (pInfo.PropertyType==typeof(bool))
            {
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }
           
            if (Struction.DelegateMethods.ContainsKey(_currentMemeberName))
            {
                return il.GetLink((EModel)this, TypeHandler);
            }
            else
            {
                return il.GetLink((EModel)this, CompareType);
            }
        }

        private LinkCallOption GetCurrentOption(string memberName)
        {
            if (Struction.Fields.ContainsKey(memberName))
            {
                FieldInfo info = Struction.Fields[memberName];
                if (info.IsPublic)
                {
                    if (info.IsStatic)
                    {
                        if (info.FieldType.IsValueType)
                        {
                            TempOption = LinkCallOption.Public_Value_Call;
                            return LinkCallOption.Public_Static_Field_Value_Call;
                        }
                        else
                        {
                            TempOption = LinkCallOption.Public_Ref_Call;
                            return LinkCallOption.Public_Static_Field_Ref_Call;
                        }
                    }
                    else
                    {
                        if (info.FieldType.IsValueType)
                        {
                            TempOption = LinkCallOption.Public_Value_Call;
                            return LinkCallOption.Public_Field_Value_Call;
                        }
                        else
                        {
                            TempOption = LinkCallOption.Public_Ref_Call;
                            return LinkCallOption.Public_Field_Ref_Call;
                        }
                    }
                }
                else
                {
                    if (info.IsStatic)
                    {
                        if (info.FieldType.IsValueType)
                        {
                            TempOption = LinkCallOption.Private_Value_Call;
                            return LinkCallOption.Private_Static_Field_Value_Call;
                        }
                        else
                        {
                            TempOption = LinkCallOption.Private_Ref_Call;
                            return LinkCallOption.Private_Static_Field_Ref_Call;
                        }
                    }
                    else
                    {
                        if (info.FieldType.IsValueType)
                        {
                            TempOption = LinkCallOption.Private_Value_Call;
                            return LinkCallOption.Private_Field_Value_Call;
                        }
                        else
                        {
                            TempOption = LinkCallOption.Private_Ref_Call;
                            return LinkCallOption.Private_Field_Ref_Call;
                        }
                    }
                }
            }
            else if (Struction.Properties.ContainsKey(memberName))
            {
                PropertyInfo pInfo = Struction.Properties[memberName];
                MethodInfo info = pInfo.GetGetMethod(true);
                if (info == null)
                {
                    info = pInfo.GetSetMethod(true);
                }
                if (info.IsPublic)
                {
                    if (info.IsStatic)
                    {
                        if (pInfo.DeclaringType.IsValueType)
                        {
                            TempOption = LinkCallOption.Private_Value_Call;
                            return LinkCallOption.Public_Static_Property_Value_Call;
                        }
                        else
                        {
                            TempOption = LinkCallOption.Public_Ref_Call;
                            return LinkCallOption.Public_Static_Property_Ref_Call;
                        }
                    }
                    else
                    {
                        if (pInfo.DeclaringType.IsValueType)
                        {
                            TempOption = LinkCallOption.Private_Value_Call;
                            return LinkCallOption.Public_Property_Value_Call;
                        }
                        else
                        {
                            TempOption = LinkCallOption.Public_Ref_Call;
                            return LinkCallOption.Public_Property_Ref_Call;
                        }
                    }
                }
                else
                {
                    if (info.IsStatic)
                    {
                        if (pInfo.DeclaringType.IsValueType)
                        {
                            TempOption = LinkCallOption.Private_Value_Call;
                            return LinkCallOption.Private_Static_Property_Value_Call;
                        }
                        else
                        {
                            TempOption = LinkCallOption.Private_Ref_Call;
                            return LinkCallOption.Private_Static_Property_Ref_Call;
                        }
                    }
                    else
                    {
                        if (pInfo.DeclaringType.IsValueType)
                        {
                            TempOption = LinkCallOption.Private_Value_Call;
                            return LinkCallOption.Private_Property_Value_Call;
                        }
                        else
                        {
                            TempOption = LinkCallOption.Private_Ref_Call;
                            return LinkCallOption.Private_Property_Ref_Call;
                        }
                    }
                }
            }
            throw new Exception("找不到属性或者字段");
        }

        //public EModel LProperty(string propertyName)
        //{
        //    if (_currentPrivateType != null && _currentPrivateType.IsValueType)
        //    {
        //        il.LoadStructLocalBuilder(_currentPrivateType);
        //    }
        //    _currentMemeberName = propertyName;
        //    PropertyInfo info = Struction.Properties[propertyName];
        //    CompareType = info.PropertyType;
        //    MethodInfo method = info.GetGetMethod(true);
        //    if (!method.IsPublic)
        //    {
        //        if (_currentPrivateType == null)
        //        {
        //            il.LoadPrivatePropertyValue(Load, info);
        //        }
        //        else
        //        {
        //            LocalBuilder tempBuilder = il.DeclareLocal(_currentPrivateType);
        //            il.EmitStoreBuilder(tempBuilder);
        //            il.LoadPrivatePropertyValue(() => { il.LoadBuilder(tempBuilder, false); }, info);
        //        }
        //        _currentPrivateType = CompareType;
        //    }
        //    else
        //    {
        //        //上一次调用为私有字段值类型变量
        //        if (_currentPrivateType != null && _currentPrivateType.IsValueType)
        //        {
        //            il.LoadStructLocalBuilder(_currentPrivateType);
        //        }
        //        il.LoadPublicProperty(This, info);
        //    }

        //    if (Struction.DelegateMethods.ContainsKey(_currentMemeberName))
        //    {
        //        return il.GetLink((EModel)this, TypeHandler);
        //    }
        //    else
        //    {
        //        return il.GetLink((EModel)this, CompareType);
        //    }
        //}

        public EModel LPropertyValue(string propertyName)
        {
            return LProperty(propertyName, true);
        }
        public EModel LFieldValue(string fieldName)
        {
            return LField(fieldName, true);
        }



        #endregion
    
        #region 调用标签
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
            il.Packet(CompareType);
        }

        public void UnPacket()
        {
            il.UnPacket(CompareType);
        }

        public void InStackAndPacket()
        {
            Load();
            il.Packet(TypeHandler);
        }

        public void InStackAndUnPacket()
        {
            Load();
            il.UnPacket(TypeHandler);
        }
        #endregion

        #region 延迟加载

        private List<Func<EModel, EModel>> _tempDelayFunc;
        private List<string> _selfAdd;
        private Type _delayType;

        public void Initialize()
        {
            DelayDict = new Dictionary<string, Func<EModel, string, object, EModel>>();
            DelayDict["Load"] = (model, key, value) => { return model.Load(key); };
            DelayDict["LoadValue"] = (model, key, value) => { return model.LoadValue(key); };
            DelayDict["Set"] = (model, key, value) => { model.Set(key, value); return model; };
            DelayDict["Packet"] = (model, key, value) => { model.Packet(); return model; };
            DelayDict["UnPacket"] = (model, key, value) => { model.UnPacket(); return model; };
            DelayDict["InStackAndPacket"] = (model, key, value) => { model.InStackAndPacket(); return model; };
            DelayDict["InStackAndUnPacket"] = (model, key, value) => { model.InStackAndUnPacket(); return model; };
        }
        private EModel AddDelayFunc(string key, string Name, object value)
        {
            if (_tempDelayFunc == null)
            {
                _delayType = TypeHandler;
                CompareType = TypeHandler;
                _tempDelayFunc = new List<Func<EModel, EModel>>();
            }
            if (Name!=null)
            {
                PropertyInfo propertyInfo = _delayType.GetProperty(Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
                if (propertyInfo != null)
                {
                    _delayType = propertyInfo.PropertyType;
                    CompareType = _delayType;
                }
                FieldInfo fieldInfo = _delayType.GetField(Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
                if (fieldInfo != null)
                {
                    _delayType = fieldInfo.FieldType;
                    CompareType = _delayType;
                }
            }
            _tempDelayFunc.Add((model) => { return DelayDict[key](model, Name, value); });
            return (EModel)this;
        }
        public EModel DLoadValue(string Name)
        {
            if (_selfAdd == null)
            {
                _selfAdd = new List<string>();
            }
            _selfAdd.Add(Name);
            return AddDelayFunc("LoadValue", Name, null);
        }
        public EModel DLoad(string Name)
        {
            if (_selfAdd == null)
            {
                _selfAdd = new List<string>();
            }
            _selfAdd.Add(Name);
            return AddDelayFunc("Load", Name, null);
        }
        public EModel DPacket()
        {
            return AddDelayFunc("Packet", null, null);
        }
        public EModel DUnPacket()
        {
            return AddDelayFunc("UnPacket", null, null);
        }
        public EModel DInStackAndPacket()
        {
            return AddDelayFunc("InStackAndPacket", null, null);
        }
        public EModel DInStackAndUnPacket()
        {
            return AddDelayFunc("InStackAndUnPacket", null, null);
        }
        public EModel DSet(string Name, object value)
        {
            return AddDelayFunc("Set", Name, value);
        }

        public Action DelayAction
        {
            get
            {
                if (_tempDelayFunc != null)
                {
                    _delayAction = GetDelayAction();
                }
                return _delayAction;
            }
            set { _delayAction = value; }
        }

        private Action GetDelayAction()
        {
            Func<EModel, EModel>[] funcArray = _tempDelayFunc.ToArray();
            Action action = () =>
            {
                EModel tempModel = (EModel)this;
                for (int i = 0; i < funcArray.Length; i += 1)
                {
                    tempModel = funcArray[i](tempModel);
                }
                _delayAction = null;
                DDelayAction = null;
            };
            if (_selfAdd != null)
            {
                string[] SelfOperatorArray = _selfAdd.ToArray();
                CurrentDelayAction = (code) =>
                {
                    Action setAction = () =>
                    {
                        EModel loadModel = (EModel)this;
                        for (int i = 0; i < funcArray.Length; i += 1)
                        {
                            loadModel = funcArray[i](loadModel);
                        }
                        il.LoadOne(_delayType);
                        il.REmit(code);
                    };
                    ComplexType tempModel = this;

                    for (int i = 0; i < SelfOperatorArray.Length - 1; i += 1)
                    {
                        tempModel = tempModel.Load(SelfOperatorArray[i]);
                    }
                    tempModel.Set(SelfOperatorArray[SelfOperatorArray.Length - 1], setAction);
                    DDelayAction = null;
                };
                _selfAdd = null;
            }

            _tempDelayFunc = null;
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
                    il.REmit(OpCodes.Stloc_S, Builder.LocalIndex);
                }
                else if (Builder.LocalIndex == 0)
                {
                    il.REmit(OpCodes.Stloc_0);
                }
                else if (Builder.LocalIndex == 1)
                {
                    il.REmit(OpCodes.Stloc_1);
                }
                else if (Builder.LocalIndex == 2)
                {
                    il.REmit(OpCodes.Stloc_2);
                }
                else if (Builder.LocalIndex == 3)
                {
                    il.REmit(OpCodes.Stloc_3);
                }
            }
            if (ParameterIndex != -1)
            {
                il.REmit(OpCodes.Starg_S, ParameterIndex);
            }
        }
        #endregion

        #region 委托类型执行
        public EModel ExecuteDelegate(params object[] parameters)
        {
            if (!Struction.DelegateMethods.ContainsKey(_currentMemeberName))
            {
                throw new NullReferenceException(TypeHandler.Name + "委托字典中不存在以" + _currentMemeberName + "命名的委托");
            }
            MethodInfo info = Struction.DelegateMethods[_currentMemeberName];
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i += 1)
                {
                    il.NoErrorLoad(parameters[i]);
                }
            }
            il.REmit(OpCodes.Callvirt, info);
            if (info.ReturnType != null)
            {
                return il.GetLink((EModel)this, info.ReturnType);
            }
            else
            {
                return null;
            }

        }
        #endregion

    }
}
