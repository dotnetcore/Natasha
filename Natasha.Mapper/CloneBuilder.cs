using Natasha.Engine.Builder.Reverser;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Natasha
{
    public class CloneBuilder<T>
    {
        public static void CreateCloneDelegate()
        {
            DeepClone<T>.Clone = (Func<T,T>)CloneBuilder.CreateCloneDelegate(typeof(T));
        }
    }
    public class CloneBuilder
    {
        public static ConcurrentDictionary<Type, Delegate> CloneCache;

        static CloneBuilder()
        {
            CloneCache = new ConcurrentDictionary<Type, Delegate>();
        }
        public CloneBuilder()
        {
            
        }

        public static void CreateCloneDelegate<T>() {
            DeepClone<T>.Clone = (Func<T, T>)CreateCloneDelegate(typeof(T));
        }
        public static Delegate CreateCloneDelegate(Type type)
        {
            if (CloneCache.ContainsKey(type))
            {
                return CloneCache[type];
            }

            var builder = FastMethod.New;
            StringBuilder sb = new StringBuilder();
            string instanceName = NameReverser.GetName(type);
            sb.Append($"{instanceName} newInstance = new {instanceName}();");

            //字段克隆
            var fields = type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                if (!fields[i].IsStatic && !fields[i].IsInitOnly)
                {
                    string oldField = $"oldInstance.{fields[i].Name}";
                    string newField = $"newInstance.{fields[i].Name}";
                    string fieldClassName = NameReverser.GetName(fields[i].FieldType);
                    Type fieldType = fields[i].FieldType;



                    if (fieldType.IsPrimitive
                        || fieldType == typeof(string)
                        || !fieldType.IsClass
                        || fieldType.IsEnum)
                    {
                        //普通字段
                        sb.Append($"{newField} = {oldField};");
                    }
                    else if (fieldType.IsArray)
                    {
                        //数组
                        Type eleType = fieldType.GetElementType();
                        string eleName = NameReverser.GetName(eleType);
                        

                        //初始化新对象数组长度
                        sb.Append($"{newField} = new {eleName}[{oldField}.Length];");
                        if (eleType.IsPrimitive
                        || eleType == typeof(string)
                        || !eleType.IsClass)
                        {
                            //结构体复制
                            sb.Append($@"for (int i = 0; i < {oldField}.Length; i++){{
                                    {newField}[i] = {oldField}[i];
                            }}");
                        }
                        else
                        {
                            CreateCloneDelegate(eleType);
                            //类走克隆
                            sb.Append($@"for (int i = 0; i < {oldField}.Length; i++){{
                                    {newField}[i] = NatashaClone{eleName}.Clone({oldField}[i]);
                            }}");
                        }

                        builder.ClassTemplate.Using(eleType);
                    }
                    else if (!fieldType.IsNotPublic)
                    {
                        //是集合则视为最小单元
                        Type spacielType = fieldType.GetInterface("IEnumerable");
                        if (spacielType!=null)
                        {
                            sb.Append($"{newField} = {oldField};");
                        }
                        else
                        {
                            CreateCloneDelegate(fieldType);
                            sb.Append($"if({oldField}!=null){{");
                            sb.Append($"{newField} = NatashaClone{fieldClassName}.Clone({oldField});");
                            sb.Append('}');
                        }
                    }
                }
            }

            //属性克隆
            var properties = type.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                var info = properties[i].GetGetMethod(true);

                if (properties[i].CanRead && properties[i].CanWrite && !info.IsStatic)
                {
                    string oldProp = $"oldInstance.{properties[i].Name}";
                    string newProp = $"newInstance.{properties[i].Name}";
                    string propClassName = NameReverser.GetName(properties[i].PropertyType);
                    Type propertyType = properties[i].PropertyType;



                    if (propertyType.IsPrimitive
                        || propertyType == typeof(string)
                        || !propertyType.IsClass
                        || propertyType.IsEnum)
                    {
                        //普通属性
                        sb.Append($"{newProp} = {oldProp};");
                    }
                    else if (propertyType.IsArray)
                    {
                        //数组
                        Type eleType = propertyType.GetElementType();
                        string eleName = NameReverser.GetName(eleType);


                        //初始化新对象数组长度
                        sb.Append($"{newProp} = new {eleName}[{oldProp}.Length];");
                        if (eleType.IsPrimitive
                        || eleType == typeof(string)
                        || !eleType.IsClass)
                        {
                            //结构体复制
                            sb.Append($@"for (int i = 0; i < {oldProp}.Length; i++){{
                                    {newProp}[i] = {oldProp}[i];
                            }}");
                        }
                        else
                        {
                            CreateCloneDelegate(eleType);
                            //类走克隆
                            sb.Append($@"for (int i = 0; i < {oldProp}.Length; i++){{
                                    {newProp}[i] = NatashaClone{eleName}.Clone({oldProp}[i]);
                            }}");
                        }

                        builder.ClassTemplate.Using(eleType);
                    }
                    else if (!propertyType.IsNotPublic)
                    {
                        //是集合则视为最小单元
                        Type spacielType = propertyType.GetInterface("IEnumerable");
                        if (spacielType != null)
                        {
                            sb.Append($"{newProp} = {oldProp};");
                        }
                        else
                        {
                            CreateCloneDelegate(propertyType);
                            sb.Append($"if({oldProp}!=null){{");
                            sb.Append($"{newProp} = NatashaClone{propClassName}.Clone({oldProp});");
                            sb.Append('}');
                        }
                    }
                }
            }
            sb.Append($"return newInstance;");//使用文件编译方式常驻程序集
            builder.UseFileComplie();
            var @delegate = builder
                       .UseClassTemplate(t=>t
                        .Access(AccessTypes.Public)
                        .ClassName("NatashaClone" + instanceName)) 
                       .UseBodyTemplate(t=>t
                        .MethodName("Clone")
                        .Param(type, "oldInstance")                //参数
                        .Body(sb.ToString())                       //方法体
                        .Return(type)                              //返回类型
                       )
                       .Create();
            CloneCache[type] = @delegate;
            return @delegate;
        }
    }
}

