
using Natasha.Cache;
using Natasha.Core.Base;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Natasha.Core
{
    //注：这里Model为类与结构体的总称
    //该父类为Model提供类结构的存储
    public class TypeStructionAnalyzer : ILGeneratorBase
    {
        public ClassStruction Struction;

        public Type TypeHandler;

        public bool IsStuct;

        public object Instance;

        public EMethod MethodHandler;
        public TypeStructionAnalyzer(Type parameter_Handler) : base()
        {
            if (parameter_Handler==null)
            {
                return;
            }
            TypeHandler = parameter_Handler;
            MethodHandler = parameter_Handler;
            //判断是否为结构体
            if (TypeHandler.IsValueType && !TypeHandler.IsPrimitive && !TypeHandler.IsEnum)
            {
                IsStuct = true;
            }
            else
            {
                IsStuct = false;
            }
            //填充类结构缓冲
            if (ClassCache.ClassInfoDict.ContainsKey(TypeHandler.Name))
            {
                Struction = ClassCache.ClassInfoDict[TypeHandler.Name];
            }
            else
            {
                //创建类结构
                Struction = new ClassStruction();
                Struction.Name = TypeHandler.Name;
                Struction.TypeHandler = TypeHandler;
                Struction.IsStruct = IsStuct;
                ClassCache.ClassInfoDict[TypeHandler.Name] = Struction;

                Type[] types = TypeHandler.GetInterfaces();
                HashSet<Type> hashTypes = new HashSet<Type>(types);

                #region 此处保留，因为具体操作的时候有些接口是没必要用的，可以在这里做过滤
                Type type = TypeHandler.GetInterface("IDictionary");
                if (type != null)
                {
                    for (int i = 0; i < types.Length; i += 1)
                    {
                        if (types[i].Name.Contains("ICollection") || !types[i].IsGenericType)
                        {
                            hashTypes.Remove(types[i]);
                        }
                    }
                }
                #endregion

                hashTypes.Add(TypeHandler);
                foreach (var item in hashTypes)
                {
                    Type tempType = item;

                    //获取方法结构
                    MethodInfo[] methods = tempType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    for (int j = 0; j < methods.Length; j += 1)
                    {
                        Struction.Methods[methods[j].Name] = methods[j];
                    }

                    //获取属性结构
                    PropertyInfo[] properties = tempType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    for (int j = 0; j < properties.Length; j += 1)
                    {
                        //内部私有类的数组不做操作  
                        if (properties[j].PropertyType.IsArray)
                        {
                            if (properties[j].PropertyType.GetElementType().GetTypeInfo().IsNestedPrivate)
                            {
                               continue;
                            }
                        }
                        //内部私有类 不做操作
                        if (properties[j].PropertyType.GetTypeInfo().IsNestedPrivate)
                        {
                            continue;
                        }
                        Struction.Properties[properties[j].Name] = properties[j];
                        
                    }

                    //获取字段结构
                    FieldInfo[] fields = tempType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    for (int j = 0; j < fields.Length; j += 1)
                    {
                        //内部私有类的数组不做操作  
                        if (fields[j].FieldType.IsArray)
                        {
                            if (fields[j].FieldType.GetElementType().GetTypeInfo().IsNestedPrivate)
                            {
                                continue;
                            }
                        }

                        //readonly const 内部私有类 不做操作
                        if (fields[j].IsInitOnly ||( fields[j].IsLiteral && fields[j].IsStatic) || fields[j].FieldType.GetTypeInfo().IsNestedPrivate)
                        {
                            continue;
                        }
                        Struction.Fields[fields[j].Name] = fields[j];
                    }
                }
            }
        }
    }
}
