using Natasha.Debug;
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

            LoadDict[typeof(byte)] = OpCodes.Ldc_I4;
            LoadDict[typeof(byte[])] = OpCodes.Ldelem_I1;
            StoreDict[typeof(byte[])] = OpCodes.Stelem_I1;

            LoadDict[typeof(short)] = OpCodes.Ldc_I4;
            LoadDict[typeof(short[])] = OpCodes.Ldelem_I2;
            StoreDict[typeof(short[])] = OpCodes.Stelem_I2;

            LoadDict[typeof(ushort)] = OpCodes.Ldc_I4;
            LoadDict[typeof(ushort[])] = OpCodes.Ldelem_I2;
            StoreDict[typeof(ushort[])] = OpCodes.Stelem_I2;

            LoadDict[typeof(int)] = OpCodes.Ldc_I4;
            LoadDict[typeof(int[])] = OpCodes.Ldelem_I4;
            StoreDict[typeof(int[])] = OpCodes.Stelem_I4;

            LoadDict[typeof(uint)] = OpCodes.Ldc_I4;
            LoadDict[typeof(uint[])] = OpCodes.Ldelem_I4;
            StoreDict[typeof(uint[])] = OpCodes.Stelem_I4;

            LoadDict[typeof(long)] = OpCodes.Ldc_I8;
            LoadDict[typeof(long[])] = OpCodes.Ldelem_I8;
            StoreDict[typeof(long[])] = OpCodes.Stelem_I8;

            LoadDict[typeof(ulong)] = OpCodes.Ldc_I8;
            LoadDict[typeof(ulong[])] = OpCodes.Ldelem_I8;
            StoreDict[typeof(ulong[])] = OpCodes.Stelem_I8;

            LoadDict[typeof(float)] = OpCodes.Ldc_R4;
            LoadDict[typeof(float[])] = OpCodes.Ldelem_R4;
            StoreDict[typeof(float[])] = OpCodes.Stelem_R4;

            LoadDict[typeof(double)] = OpCodes.Ldc_R8;
            LoadDict[typeof(double[])] = OpCodes.Ldelem_R8;
            StoreDict[typeof(double[])] = OpCodes.Stelem_R8;

            LoadDict[typeof(string)] = OpCodes.Ldstr;
            LoadDict[typeof(string[])] = OpCodes.Ldelem_Ref;
            StoreDict[typeof(string[])] = OpCodes.Stelem_Ref;

            LoadDict[typeof(object)] = OpCodes.Ldobj;
            LoadDict[typeof(object[])] = OpCodes.Ldelem_Ref;
            StoreDict[typeof(object[])] = OpCodes.Stelem_Ref;

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
                DebugHelper.WriteLine(StoreArrayCode.Name+"", _baseType.Name);
            }
            else
            {
                ilHandler.Emit(StoreArrayCode);
                DebugHelper.WriteLine(StoreArrayCode.Name);
            }
        }
        public void StoreArray(Action actionIndex, object value)
        {
            StoreArray(actionIndex, () => { DataHelper.NoErrorLoad(value,ilHandler); });
        }
        public void StoreArray(object builderIndex, object value)
        {
            Load();
            DataHelper.NoErrorLoad(builderIndex,ilHandler);
            DataHelper.NoErrorLoad(value, ilHandler);
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
            This();
            LoadArray(() =>
            {
                if (index < 255)
                {
                    ilHandler.Emit(OpCodes.Ldc_I4_S, index);
                    DebugHelper.WriteLine("Ldc_I4_S", index);
                }
                else
                {
                    ilHandler.Emit(OpCodes.Ldc_I4, index);
                    DebugHelper.WriteLine("Ldc_I4", index);
                }
            });     
        }
        public void LoadArray(Action action)
        {
            This();
            if (action!=null)
            {
                action();
            }
            if (ArrayIsStruct)
            {
                ilHandler.Emit(LoadArrayCode, _baseType);
                DebugHelper.WriteLine(StoreArrayCode.Name + "", _baseType.Name);
            }
            else
            {
                ilHandler.Emit(LoadArrayCode);
                DebugHelper.WriteLine(LoadArrayCode.Name);
            }
        }
        public void LoadArray(object instance)
        {
            This();
            LoadArray(() =>
            {
                DataHelper.NoErrorLoad(instance,ilHandler);
            });
        }
    }
}
