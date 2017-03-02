using Natasha.Utils;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Natasha.Core
{
    //数组操作基类
    public class ArrayBase : ComplexType
    {
        public static Dictionary<Type, OpCode> LoadDict;
        public static Dictionary<Type, OpCode> StoreDict;
        public static Dictionary<Type, Type> ArrayTypeDict;

        public OpCode LoadArrayCode;
        public OpCode StoreArrayCode;

        private Type _baseType;
        public bool ArrayIsStruct;

        //根据元素类型获取加载填充的OpCode
        public Type BaseType
        {
            get { return _baseType; }
            set
            {
                _baseType = value;
                if (_baseType.IsValueType && !_baseType.IsPrimitive)
                {
                    if (_baseType.IsEnum)
                    {
                        ArrayIsStruct = false;
                        LoadArrayCode = OpCodes.Ldelem_I4;
                        StoreArrayCode = OpCodes.Stelem_I4;
                    }
                    else
                    {
                        ArrayIsStruct = true;
                        LoadArrayCode = OpCodes.Ldelema;
                        StoreArrayCode = OpCodes.Stelem;
                    }
                }
            }
        }

        //静态初始化，填充OpCode缓存
        static ArrayBase()
        {
            LoadDict = new Dictionary<Type, OpCode>();
            StoreDict = new Dictionary<Type, OpCode>();
            ArrayTypeDict = new Dictionary<Type, Type>();

            ArrayTypeDict[typeof(byte[])] = typeof(byte);
            LoadDict[typeof(byte)] = OpCodes.Ldc_I4;
            LoadDict[typeof(byte[])] = OpCodes.Ldelem_I1;
            StoreDict[typeof(byte[])] = OpCodes.Stelem_I1;

            ArrayTypeDict[typeof(short[])] = typeof(short);
            LoadDict[typeof(short)] = OpCodes.Ldc_I4;
            LoadDict[typeof(short[])] = OpCodes.Ldelem_I2;
            StoreDict[typeof(short[])] = OpCodes.Stelem_I2;

            ArrayTypeDict[typeof(ushort[])] = typeof(ushort);
            LoadDict[typeof(ushort)] = OpCodes.Ldc_I4;
            LoadDict[typeof(ushort[])] = OpCodes.Ldelem_I2;
            StoreDict[typeof(ushort[])] = OpCodes.Stelem_I2;

            ArrayTypeDict[typeof(int[])] = typeof(int);
            LoadDict[typeof(int)] = OpCodes.Ldc_I4;
            LoadDict[typeof(int[])] = OpCodes.Ldelem_I4;
            StoreDict[typeof(int[])] = OpCodes.Stelem_I4;

            ArrayTypeDict[typeof(uint[])] = typeof(uint);
            LoadDict[typeof(uint)] = OpCodes.Ldc_I4;
            LoadDict[typeof(uint[])] = OpCodes.Ldelem_I4;
            StoreDict[typeof(uint[])] = OpCodes.Stelem_I4;

            ArrayTypeDict[typeof(long[])] = typeof(long);
            LoadDict[typeof(long)] = OpCodes.Ldc_I8;
            LoadDict[typeof(long[])] = OpCodes.Ldelem_I8;
            StoreDict[typeof(long[])] = OpCodes.Stelem_I8;

            ArrayTypeDict[typeof(ulong[])] = typeof(ulong);
            LoadDict[typeof(ulong)] = OpCodes.Ldc_I8;
            LoadDict[typeof(ulong[])] = OpCodes.Ldelem_I8;
            StoreDict[typeof(ulong[])] = OpCodes.Stelem_I8;

            ArrayTypeDict[typeof(float[])] = typeof(float);
            LoadDict[typeof(float)] = OpCodes.Ldc_R4;
            LoadDict[typeof(float[])] = OpCodes.Ldelem_R4;
            StoreDict[typeof(float[])] = OpCodes.Stelem_R4;

            ArrayTypeDict[typeof(double[])] = typeof(double);
            LoadDict[typeof(double)] = OpCodes.Ldc_R8;
            LoadDict[typeof(double[])] = OpCodes.Ldelem_R8;
            StoreDict[typeof(double[])] = OpCodes.Stelem_R8;

            ArrayTypeDict[typeof(string[])] = typeof(string);
            LoadDict[typeof(string)] = OpCodes.Ldstr;
            LoadDict[typeof(string[])] = OpCodes.Ldelem_Ref;
            StoreDict[typeof(string[])] = OpCodes.Stelem_Ref;

            ArrayTypeDict[typeof(object[])] = typeof(object);
            LoadDict[typeof(object)] = OpCodes.Ldobj;
            LoadDict[typeof(object[])] = OpCodes.Ldelem_Ref;
            StoreDict[typeof(object[])] = OpCodes.Stelem_Ref;

            ArrayTypeDict[typeof(bool[])] = typeof(bool);
            LoadDict[typeof(bool)] = OpCodes.Ldc_I4;
            LoadDict[typeof(bool[])] = OpCodes.Ldelem_I1;
            StoreDict[typeof(bool[])] = OpCodes.Stelem_I1;
        }
        public ArrayBase(Type type) : base(type) { InitializeArray(); }
        public ArrayBase(Action loadAction, Type type) : base(loadAction, type) { InitializeArray(); }
        public ArrayBase(LocalBuilder builder, Type type) : base(builder, type) { InitializeArray(); }
        public ArrayBase(int parameterIndex, Type type) : base(parameterIndex, type) { InitializeArray(); }

        //初始化OpCode
        public void InitializeArray()
        {
            if (LoadDict.ContainsKey(TypeHandler))
            {
                LoadArrayCode = LoadDict[TypeHandler];
            }
            else
            {
                LoadArrayCode = OpCodes.Ldelem_Ref;
            }
            if (StoreDict.ContainsKey(TypeHandler))
            {
                StoreArrayCode = StoreDict[TypeHandler];
            }
            else
            {
                StoreArrayCode = OpCodes.Stelem_Ref;
            }
            BaseType = TypeHandler.GetElementType();
        }
        #region 基础方法
        public void StoreArray(Action actionIndex, Action valueIndex)
        {

            Load();
            actionIndex();
            valueIndex();
            //枚举以及结构体填充方式
            if (BaseType != null && ArrayIsStruct)
            {
               
                ilHandler.Emit(StoreArrayCode, _baseType);
            }
            else
            {
                ilHandler.Emit(StoreArrayCode);
            }
        }
        public void StoreArray(Action actionIndex, object value)
        {
            StoreArray(actionIndex, () => { EData.NoErrorLoad(value,ilHandler); });
        }
        public void StoreArray(object builderIndex, object value)
        {
            Load();
            EData.NoErrorLoad(builderIndex,ilHandler);
            EData.NoErrorLoad(value, ilHandler);
            //枚举以及结构体填充方式
            if (BaseType != null && ArrayIsStruct)
            {

                ilHandler.Emit(StoreArrayCode, _baseType);
            }
            else
            {
                ilHandler.Emit(StoreArrayCode);
            }

        }
        public void StoreArray(object builderIndex, Action value)
        {
            StoreArray(builderIndex, (object)value);
        }
        #endregion

        

       

        //加载元素
        public void LoadArray(int index)
        {
            LoadAddress();
            ilHandler.Emit(OpCodes.Ldc_I4, index);
            if (ArrayIsStruct)
            {
                ilHandler.Emit(LoadArrayCode, _baseType);
            }
            else
            {
                ilHandler.Emit(LoadArrayCode);
            }
        }
        public void LoadArray(Action action)
        {
            LoadAddress();
            if (action!=null)
            {
                action();
            }
            if (ArrayIsStruct)
            {
                ilHandler.Emit(LoadArrayCode, _baseType);
            }
            else
            {
                ilHandler.Emit(LoadArrayCode);
            }
        }
        public void LoadArray(ILoadInstance instance)
        {
            LoadAddress();
            instance.Load();
            if (ArrayIsStruct)
            {
                ilHandler.Emit(LoadArrayCode, _baseType);
            }
            else
            {
                ilHandler.Emit(LoadArrayCode);
            }
        }
    }
}
