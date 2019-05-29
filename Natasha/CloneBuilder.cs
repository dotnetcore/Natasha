using Natasha.Engine.Builder.Reverser;
using System;
using System.Collections.Concurrent;
using System.Text;

namespace Natasha
{
    public class CloneBuilder<T>
    {
        public static void CreateCloneDelegate()
        {
            DeepClone<T>.CloneDelegate = (Func<T, T>)CloneBuilder.CreateCloneDelegate(typeof(T));
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
        public static void CreateCloneDelegate<T>()
        {
            DeepClone<T>.CloneDelegate = (Func<T, T>)CreateCloneDelegate(typeof(T));
        }

        public static Delegate CreateCloneDelegate(Type type)
        {
            if (type.FullName == null)
            {
                return null;
            }
            if (CloneCache.ContainsKey(type))
            {
                return CloneCache[type];
            }
            string instanceName = AvailableNameReverser.GetName(type);

            if (type.IsArray)                                   //数组直接克隆
            {
                return CreateArrayDelegate(type);
            }
            else if (type.GetInterface("IEnumerable") != null)  //集合直接克隆
            {
                return CreateCollectionDelegate(type);
            }
            else
            {                                                   //实体类型克隆
                return CreateEntityDelegate(type);
            }
        }

        internal static Delegate CreateCollectionDelegate(Type type)
        {
            if (!type.IsInterface)
            {
                var args = type.GetGenericArguments();

                CreateCloneDelegate(args[0]);

                if (args.Length == 2)
                {
                    CreateCloneDelegate(args[1]);
                }
            }
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append(@"if(oldInstance!=null){");
            if (type.IsInterface)
            {
                scriptBuilder.Append($"return oldInstance.CloneExtension();");
            }
            else
            {
                scriptBuilder.Append($"return new {NameReverser.GetName(type)}(oldInstance.CloneExtension());");
            }
            scriptBuilder.Append("}return null;");
            var tempBuilder = FastMethod.New;
            tempBuilder.ComplierInstance.UseFileComplie();
            return CloneCache[type] = tempBuilder
                        .Using("Natasha")
                        .Using(type.GetGenericArguments())
                        .ClassName("NatashaClone" + AvailableNameReverser.GetName(type))
                        .MethodName("Clone")
                        .Param(type, "oldInstance")                //参数
                        .MethodBody(scriptBuilder.ToString())         //方法体
                        .Return(type)                              //返回类型
                        .Complie();
        }

        internal static Delegate CreateArrayDelegate(Type type)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            Type eleType = type.GetElementType();
            CreateCloneDelegate(eleType);
            scriptBuilder.Append(@"if(oldInstance!=null){");
            scriptBuilder.Append($"var newInstance = new {NameReverser.GetName(eleType)}[oldInstance.Length];");
            if (IsOnceType(eleType))
            {
                //普通类型复制
                scriptBuilder.Append($@"for (int i = 0; i < oldInstance.Length; i++){{
                                    newInstance[i] = oldInstance[i];
                            }}return newInstance;");
            }
            else
            {
                CreateCloneDelegate(eleType);
                //类的克隆
                scriptBuilder.Append($@"for (int i = 0; i < oldInstance.Length; i++){{
                                    newInstance[i] = NatashaClone{AvailableNameReverser.GetName(eleType)}.Clone(oldInstance[i]);
                            }}return newInstance;");
            }
            scriptBuilder.Append("}return null;");

            var tempBuilder = FastMethod.New;
            tempBuilder.ComplierInstance.UseFileComplie();
            tempBuilder.Using(eleType);
            return CloneCache[type] = tempBuilder
                        .Using("Natasha")
                        .ClassName("NatashaClone" + AvailableNameReverser.GetName(type))
                        .MethodName("Clone")
                        .Param(type, "oldInstance")                //参数
                        .MethodBody(scriptBuilder.ToString())         //方法体
                        .Return(type)                              //返回类型
                        .Complie();
        }

        internal static Delegate CreateEntityDelegate(Type type)
        {

            var builder = FastMethod.New;
            StringBuilder sb = new StringBuilder();
            string className = NameReverser.GetName(type);
            sb.Append($"{className} newInstance = new {className}();");

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



                    if (IsOnceType(fieldType))
                    {
                        //普通字段
                        sb.Append($"{newField} = {oldField};");
                    }
                    else if (fieldType.IsArray)
                    {
                        //数组
                        Type eleType = fieldType.GetElementType();

                        //初始化新对象数组长度
                        sb.Append($@"if({oldField}!=null){{");
                        sb.Append($"{newField} = new {NameReverser.GetName(eleType)}[{oldField}.Length];");
                        if (IsOnceType(eleType))
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
                                    {newField}[i] = NatashaClone{AvailableNameReverser.GetName(eleType)}.Clone({oldField}[i]);
                            }}");
                        }
                        sb.Append('}');
                        builder.Using(eleType);
                    }
                    else if (!fieldType.IsNotPublic)
                    {
                        Type spacielType = fieldType.GetInterface("IEnumerable");
                        if (spacielType != null)
                        {
                            CreateCollectionDelegate(fieldType);
                            sb.Append($@"if({oldField}!=null){{");
                            if (fieldType.IsInterface)
                            {
                                sb.Append($"{newField} = {oldField}.CloneExtension();");
                            }
                            else
                            {
                                sb.Append($"{newField} = new {fieldClassName}({oldField}.CloneExtension());");
                            }
                            sb.Append('}');
                            builder.Using(fieldType);
                            builder.Using("Natasha");
                        }
                        else
                        {
                            CreateCloneDelegate(fieldType);
                            sb.Append($"if({oldField}!=null){{");
                            sb.Append($"{newField} = NatashaClone{AvailableNameReverser.GetName(fieldType)}.Clone({oldField});");
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



                    if (IsOnceType(propertyType))
                    {
                        //普通属性
                        sb.Append($"{newProp} = {oldProp};");
                    }
                    else if (propertyType.IsArray)
                    {
                        //数组
                        Type eleType = propertyType.GetElementType();

                        //初始化新对象数组长度
                        sb.Append($@"if({oldProp}!=null){{");
                        sb.Append($"{newProp} = new {NameReverser.GetName(eleType)}[{oldProp}.Length];");
                        if (IsOnceType(eleType))
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
                                    {newProp}[i] = NatashaClone{AvailableNameReverser.GetName(eleType)}.Clone({oldProp}[i]);
                            }}");
                        }
                        sb.Append('}');
                        builder.Using(eleType);
                    }
                    else if (!propertyType.IsNotPublic)
                    {
                        Type spacielType = propertyType.GetInterface("IEnumerable");
                        if (spacielType != null)
                        {
                            CreateCollectionDelegate(propertyType);
                            sb.Append($@"if({oldProp}!=null){{");
                            if (propertyType.IsInterface)
                            {
                                sb.Append($"{newProp} = {oldProp}.CloneExtension();");
                            }
                            else
                            {
                                sb.Append($"{newProp} = new  {propClassName}({oldProp}.CloneExtension());");
                            }
                            sb.Append('}');
                            builder.Using(propertyType);
                            builder.Using("Natasha");
                        }
                        else
                        {
                            CreateCloneDelegate(propertyType);
                            sb.Append($"if({oldProp}!=null){{");
                            sb.Append($"{newProp} = NatashaClone{AvailableNameReverser.GetName(propertyType)}.Clone({oldProp});");
                            sb.Append('}');
                        }
                    }
                }
            }
            sb.Append($"return newInstance;");//使用文件编译方式常驻程序集
            builder.ComplierInstance.UseFileComplie();
            var @delegate = builder
                        .ClassName("NatashaClone" + AvailableNameReverser.GetName(type))
                        .MethodName("Clone")
                        .Param(type, "oldInstance")                //参数
                        .MethodBody(sb.ToString())                 //方法体
                        .Return(type)                              //返回类型
                       .Complie();
            CloneCache[type] = @delegate;
            return @delegate;
        }
        public static bool IsOnceType(Type type)
        {
            return type.IsPrimitive
                            || type == typeof(string)
                            || !type.IsClass
                            || type.IsEnum;
        }
    }
}

