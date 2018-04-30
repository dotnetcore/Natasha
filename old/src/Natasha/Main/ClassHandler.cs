using Natasha.Cache;
using Natasha.Core;
using System;
using System.Reflection.Emit;

namespace Natasha
{
    //保留：自定义类的操作类 EModel有同样的功能
    public class ClassHandler : ComplexType
    {
        public ClassHandler(Type type):base(type)
        {
        }
        public ClassHandler(LocalBuilder builder, Type type) : base(builder, type)
        {
        }
        public ClassHandler(int parameterIndex, Type type) : base(parameterIndex,type)
        {
        }

        public static ClassHandler CreateInstance(string classKey)
        {
            ClassHandler instance = null;
            if (ClassCache.DynamicClassDict.ContainsKey(classKey))
            {
                Type type = ClassCache.DynamicClassDict[classKey];
                instance = new ClassHandler(type);
            }
            return instance;
        }
        public static ClassHandler CreateInstance(string classKey, LocalBuilder builder)
        {
            ClassHandler instance = null;
            if (ClassCache.DynamicClassDict.ContainsKey(classKey))
            {
                Type type = ClassCache.DynamicClassDict[classKey];
                instance = new ClassHandler(builder,type);
            }
            return instance;
        }
        public static ClassHandler CreateInstance(string classKey, int parameterIndex)
        {
            ClassHandler instance = null;

            if (ClassCache.DynamicClassDict.ContainsKey(classKey))
            {
                Type type = ClassCache.DynamicClassDict[classKey];
                instance = new ClassHandler(parameterIndex,type);
            }
            return instance;
        }
    }
}
