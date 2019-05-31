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

        static CloneBuilder() => CloneCache = new ConcurrentDictionary<Type, Delegate>();

        public static Action<Type, Delegate> StaticMapping;




        /// <summary>
        /// 根据委托强类型获取强类型
        /// </summary>
        /// <typeparam name="T">强类型</typeparam>
        public static void CreateCloneDelegate<T>()
        {
            DeepClone<T>.CloneDelegate = (Func<T, T>)CreateCloneDelegate(typeof(T));
        }




        /// <summary>
        /// 获取克隆委托
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Delegate CreateCloneDelegate(Type type)
        {
            //无效类PASS
            if (type.FullName == null)
            {
                return null;
            }


            //缓存有则跳过
            if (CloneCache.ContainsKey(type))
            {
                return CloneCache[type];
            }


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




        /// <summary>
        /// 获取集合委托
        /// </summary>
        /// <param name="type">集合类型</param>
        /// <returns></returns>
        internal static Delegate CreateCollectionDelegate(Type type)
        {
            //泛型集合参数
            Type[] args = null;


            //检测接口
            if (!type.IsInterface)
            {

                //获取泛型参数
                args = type.GetGenericArguments();


                //创建参数的委托
                CreateCloneDelegate(args[0]);


                //如果存在第二个泛型参数，例如字典，同样进行克隆处理
                if (args.Length == 2)
                {
                    CreateCloneDelegate(args[1]);
                }
            }


            //生成方法体
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


            //创建委托
            var tempBuilder = FastMethod.New;
            tempBuilder.Using(args);
            tempBuilder.Using("Natasha");
            tempBuilder.ComplierInstance.UseFileComplie();
            return CloneCache[type] = tempBuilder
                        .Using("Natasha")
                        .Using(GenericTypeOperator.GetTypes(type))
                        .ClassName("NatashaClone" + AvailableNameReverser.GetName(type))
                        .MethodName("Clone")
                        .Param(type, "oldInstance")                 //参数
                        .MethodBody(scriptBuilder.ToString())       //方法体
                        .Return(type)                               //返回类型
                        .Complie();
        }




        /// <summary>
        /// 创建数组委托
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static Delegate CreateArrayDelegate(Type type)
        {

            Type eleType = type.GetElementType();
            CreateCloneDelegate(eleType);


            StringBuilder scriptBuilder = new StringBuilder();
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
                //类的克隆
                CreateCloneDelegate(eleType);
                scriptBuilder.Append($@"for (int i = 0; i < oldInstance.Length; i++){{
                                    newInstance[i] = NatashaClone{AvailableNameReverser.GetName(eleType)}.Clone(oldInstance[i]);
                            }}return newInstance;");
            }
            scriptBuilder.Append("}return null;");


            //创建委托
            var tempBuilder = FastMethod.New;
            tempBuilder.ComplierInstance.UseFileComplie();
            tempBuilder.Using(eleType);
            return CloneCache[type] = tempBuilder
                        .Using("Natasha")
                        .ClassName("NatashaClone" + AvailableNameReverser.GetName(type))
                        .MethodName("Clone")
                        .Param(type, "oldInstance")                 //参数
                        .MethodBody(scriptBuilder.ToString())       //方法体
                        .Return(type)                               //返回类型
                        .Complie();
        }




        /// <summary>
        /// 创建实体类委托
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static Delegate CreateEntityDelegate(Type type)
        {

            var builder = FastMethod.New;
            string className = NameReverser.GetName(type);


            StringBuilder sb = new StringBuilder();
            sb.Append($"if(oldInstance==null){{return null;}}");
            sb.Append($"{className} newInstance = new {className}();");

            //字段克隆
            var fields = type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                
                //排除不能操作的类型
                if (!fields[i].IsStatic && !fields[i].IsInitOnly)
                {


                    string oldField = $"oldInstance.{fields[i].Name}";
                    string newField = $"newInstance.{fields[i].Name}";
                    string fieldClassName = NameReverser.GetName(fields[i].FieldType);
                    Type fieldType = fields[i].FieldType;


                    if (IsOnceType(fieldType))       //普通字段
                    {
                        sb.Append($"{newField} = {oldField};");
                    }
                    else if (fieldType.IsArray)      //数组
                    {
                        Type eleType = fieldType.GetElementType();


                        //初始化新对象数组长度
                        sb.Append($@"if({oldField}!=null){{");
                        sb.Append($"{newField} = new {NameReverser.GetName(eleType)}[{oldField}.Length];");


                        if (IsOnceType(eleType))
                        {
                            //普通类复制
                            sb.Append($@"for (int i = 0; i < {oldField}.Length; i++){{
                                    {newField}[i] = {oldField}[i];
                            }}");
                        }
                        else
                        {
                            //类复制
                            CreateCloneDelegate(eleType);
                            sb.Append($@"for (int i = 0; i < {oldField}.Length; i++){{
                                    {newField}[i] = NatashaClone{AvailableNameReverser.GetName(eleType)}.Clone({oldField}[i]);
                            }}");
                        }
                        sb.Append('}');


                        //使用数组类型的命名空间
                        builder.Using(eleType);
                    }
                    else if (!fieldType.IsNotPublic)
                    {
                        
                        //检测集合
                        Type collectionType = fieldType.GetInterface("IEnumerable");


                        if (collectionType != null)
                        {

                            //创建集合克隆
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


                //排除不能操作的属性
                if (properties[i].CanRead && properties[i].CanWrite && !info.IsStatic)
                {

                    string oldProp = $"oldInstance.{properties[i].Name}";
                    string newProp = $"newInstance.{properties[i].Name}";
                    string propClassName = NameReverser.GetName(properties[i].PropertyType);


                    Type propertyType = properties[i].PropertyType;

                    if (IsOnceType(propertyType))               //普通属性
                    {
                        
                        sb.Append($"{newProp} = {oldProp};");
                    }
                    else if (propertyType.IsArray)               //数组
                    {

                        Type eleType = propertyType.GetElementType();


                        //初始化新对象数组长度
                        sb.Append($@"if({oldProp}!=null){{");
                        sb.Append($"{newProp} = new {NameReverser.GetName(eleType)}[{oldProp}.Length];");


                        if (IsOnceType(eleType))
                        {
                            //普通类型复制
                            sb.Append($@"for (int i = 0; i < {oldProp}.Length; i++){{
                                    {newProp}[i] = {oldProp}[i];
                            }}");
                        }
                        else
                        {
                            //类复制
                            CreateCloneDelegate(eleType);
                            sb.Append($@"for (int i = 0; i < {oldProp}.Length; i++){{
                                    {newProp}[i] = NatashaClone{AvailableNameReverser.GetName(eleType)}.Clone({oldProp}[i]);
                            }}");
                        }
                        sb.Append('}');
                        builder.Using(eleType);
                    }
                    else if (!propertyType.IsNotPublic)
                    {

                        Type collectionType = propertyType.GetInterface("IEnumerable");


                        if (collectionType != null)
                        {
                            //集合复制
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


            //创建委托
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




        /// <summary>
        /// 判断是否为普通类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        internal static bool IsOnceType(Type type)
        {
            return type.IsPrimitive
                            || type == typeof(string)
                            || !type.IsClass
                            || type.IsEnum;
        }
    }
}

