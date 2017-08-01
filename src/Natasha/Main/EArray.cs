using Natasha.Core;
using Natasha.Utils;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Natasha
{
    //数组操作
    public class EArray : ArrayBase,IIterator
    {

        #region 初始化
        private EArray(Type type) : base(type) { }
        private EArray(Action loadAction, Type type) : base(loadAction, type) { }
        private EArray(LocalBuilder builder, Type type) : base(builder, type) { }
        private EArray(int parameterIndex, Type type) : base(parameterIndex, type) { }

        #region 创建本地变量数组
        public static EArray CreateArrayFromBuilder<T>(LocalBuilder builder)
        {
            return CreateArrayFromBuilder(builder, typeof(T[]));
        }
        public static EArray CreateArrayFromBuilder(LocalBuilder builder, Type type)
        {
            EArray instance = new EArray(builder, type);
            return instance;
        }
        #endregion
        #region 创建参数类型数组
        public static EArray CreateArrayFromParameter<T>(int parameterIndex)
        {
            return CreateArrayFromParameter(parameterIndex, typeof(T[]));
        }
        public static EArray CreateArrayFromParameter(int parameterIndex, Type type)
        {
            EArray instance = new EArray(parameterIndex, type);
            return instance;
        }
        #endregion
        #region 创建指定长度的数组
        public static EArray CreateArray<T>()
        {
            return CreateArray(typeof(T[]));
        }
        public static EArray CreateArray(Type type)
        {
            EArray instance = new EArray(type);
            return instance;
        }
        public static EArray CreateArraySpecifiedLength<T>(int length)
        {
            return CreateArraySpecifiedLength(typeof(T[]), length);
        }
        public static EArray CreateArraySpecifiedLength(Type aType, int length)
        {
            EArray model = new EArray(aType);
            model.Length = length;
            if (length != -1)
            {
                model.il.REmit(OpCodes.Ldc_I4, length);
                model.il.REmit(OpCodes.Newarr, model.ElementType);
                model.il.REmit(OpCodes.Stloc, model.Builder);
            }
            return model;
        }
        #endregion
        #region 创建用委托加载的参数变量 
        public static EArray CreateArrayFromAction<T>(Action loadAction)
        {
            return CreateArrayFromAction(loadAction, typeof(T[]));
        }
        public static EArray CreateArrayFromAction(Action loadAction, Type type)
        {
            EArray instance = new EArray(loadAction, type);
            return instance;
        }
        #endregion
        #region 创建来自于类和结构体的数组 
        public static EArray CreateArrayFromRuntimeArray<T>(params T[] loadElements)
        {
            Type type = typeof(T);
            EArray instance = CreateArraySpecifiedLength<T>(loadElements.Length);
            for (int i = 0; i < loadElements.Length; i += 1)
            {
                instance.StoreArray(i, loadElements[i]);
            }
            return instance;
        }
        #endregion
        #endregion

        #region 迭代器属性和方法(不支持)
        public MethodInfo MoveNext
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public MethodInfo Current
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public MethodInfo Dispose
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public LocalBuilder TempEnumerator
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }


        #endregion

        #region 迭代器属性和方法(支持)
        public new void Initialize()
        {
            
        }
        
        public int Length
        {
            get; set;
        }
        public void LoadCurrentElement(LocalBuilder currentBuilder)
        {
            LoadArray(currentBuilder);
        }
        #endregion
       
        #region 隐式转换
        public static implicit operator EArray(object[] value)
        {
            EArray instance = CreateArraySpecifiedLength(value.GetType(), value.Length);
            for (int i = 0; i < value.Length; i += 1)
            {
                instance.StoreArray(i, value[i]);
            }
            return instance;
        }

        public static implicit operator EArray(int[] value)
        {
            EArray instance = CreateArraySpecifiedLength<int>(value.Length);
            for (int i = 0; i < value.Length; i += 1)
            {
                instance.StoreArray(i, value[i]);
            }
            return instance;
        }
        public static implicit operator EArray(uint[] value)
        {
            EArray instance = CreateArraySpecifiedLength<uint>(value.Length);
            for (int i = 0; i < value.Length; i += 1)
            {
                instance.StoreArray(i, value[i]);
            }
            return instance;
        }
        public static implicit operator EArray(short[] value)
        {
            EArray instance = CreateArraySpecifiedLength<short>(value.Length);
            for (int i = 0; i < value.Length; i += 1)
            {
                instance.StoreArray(i, value[i]);
            }
            return instance;
        }
        public static implicit operator EArray(ushort[] value)
        {
            EArray instance = CreateArraySpecifiedLength<ushort>(value.Length);
            for (int i = 0; i < value.Length; i += 1)
            {
                instance.StoreArray(i, value[i]);
            }
            return instance;
        }
        public static implicit operator EArray(long[] value)
        {
            EArray instance = CreateArraySpecifiedLength<long>(value.Length);
            for (int i = 0; i < value.Length; i += 1)
            {
                instance.StoreArray(i, value[i]);
            }
            return instance;
        }
        
        public static implicit operator EArray(ulong[] value)
        {
            EArray instance = CreateArraySpecifiedLength<ulong>(value.Length);
            for (int i = 0; i < value.Length; i += 1)
            {
                instance.StoreArray(i, value[i]);
            }
            return instance;
        }
        public static implicit operator EArray(float[] value)
        {
            EArray instance = CreateArraySpecifiedLength<float>(value.Length);
            for (int i = 0; i < value.Length; i += 1)
            {
                instance.StoreArray(i, value[i]);
            }
            return instance;
        }
        public static implicit operator EArray(double[] value)
        {
            EArray instance = CreateArraySpecifiedLength<double>(value.Length);
            for (int i = 0; i < value.Length; i += 1)
            {
                instance.StoreArray(i, value[i]);
            }
            return instance;
        }
        public static implicit operator EArray(decimal[] value)
        {
            EArray instance = CreateArraySpecifiedLength<decimal>(value.Length);
            for (int i = 0; i < value.Length; i += 1)
            {
                instance.StoreArray(i, value[i]);
            }
            return instance;
        }
        public static implicit operator EArray(byte[] value)
        {
            EArray instance = CreateArraySpecifiedLength<byte>(value.Length);
            for (int i = 0; i < value.Length; i += 1)
            {
                instance.StoreArray(i, value[i]);
            }
            return instance;
        }
        public static implicit operator EArray(string[] value)
        {
            EArray instance = CreateArraySpecifiedLength<string>(value.Length);
            for (int i = 0; i < value.Length; i += 1)
            {
                instance.StoreArray(i, value[i]);
            }
            return instance;
        }
        public static implicit operator EArray(char[] value)
        {
            EArray instance = CreateArraySpecifiedLength<char>(value.Length);
            for (int i = 0; i < value.Length; i += 1)
            {
                instance.StoreArray(i, value[i]);
            }
            return instance;
        }
        public static implicit operator EArray(bool[] value)
        {
            EArray instance = CreateArraySpecifiedLength<bool>(value.Length);
            for (int i = 0; i < value.Length; i += 1)
            {
                instance.StoreArray(i, value[i]);
            }
            return instance;
        }
        #endregion
    }
}
