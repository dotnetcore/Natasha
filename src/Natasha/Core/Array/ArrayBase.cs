using Natasha.Utils;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Natasha.Core
{
    //数组操作基类
    public class ArrayBase : ComplexType
    {
        public ArrayBase(Type type) : base(type) { }
        public ArrayBase(Action loadAction, Type type) : base(loadAction, type) { }
        public ArrayBase(LocalBuilder builder, Type type) : base(builder, type) { }
        public ArrayBase(int parameterIndex, Type type) : base(parameterIndex, type) { }

        #region 基础方法
        public void StoreArray(Action actionIndex, Action valueIndex)
        {
            Load();
            actionIndex();
            valueIndex();
            il.StoreElement(ElementType);
        }
        public void StoreArray(Action actionIndex, object value)
        {
            StoreArray(actionIndex, () => { il.NoErrorLoad(value); });
        }
        public void StoreArray(object builderIndex, object value)
        {
            Load();
            il.NoErrorLoad(builderIndex);
            il.NoErrorLoad(value);
            il.StoreElement(ElementType);
        }
        public void StoreArray(object builderIndex, Action value)
        {
            StoreArray(builderIndex, (object)value);
        }
        #endregion

        

       

        //加载元素
        public void LoadArray(int index)
        {
            LoadArray(() =>
            {
                il.EmitInt(index);
            });     
        }
        public void LoadArray(Action action)
        {
            This();
            if (action!=null)
            {
                action();
            }
            il.LoadElement(ElementType);
        }
        public void LoadArray(object instance)
        {
            LoadArray(() =>
            {
                il.NoErrorLoad(instance);
            });
        }
    }
}
