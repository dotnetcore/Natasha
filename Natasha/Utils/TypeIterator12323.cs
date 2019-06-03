using System;

namespace Natasha
{
    public abstract class TypeIterator1
    {

        public virtual void ArrayOnceTypeHandler(BuilderInfo builderInfo)
        {

        }
        public virtual void ArrayEntityTypeHandler(BuilderInfo builderInfo)
        {

        }
        public void TypeHandler(Type type)
        {
            //无效类PASS
            if (type.FullName == null){ return;}

            BuilderInfo typeInfo = new BuilderInfo();
            typeInfo.Type = type;
            typeInfo.TypeName = NameReverser.GetName(type);
            typeInfo.AvailableName = AvailableNameReverser.GetName(type);

            if (type.IsArray)                                   //数组直接克隆
            {
                Type eleType = type.GetElementType();
                TypeHandler(eleType);
                typeInfo.Type = eleType;
                typeInfo.TypeName = NameReverser.GetName(eleType);
                typeInfo.AvailableName = AvailableNameReverser.GetName(eleType);

                if (IsOnceType(eleType))
                {
                    ArrayOnceTypeHandler(typeInfo);
                }
                else
                {
                    ArrayEntityTypeHandler(typeInfo);
                }
                
            }
            else if (type.GetInterface("IEnumerable") != null)  //集合直接克隆
            {
                MemberCollectionHandler(typeInfo);
            }
            else
            {                                                   //实体类型克隆
                MemberHandler(typeInfo);
            }
        }

        public virtual void StartHandler(BuilderInfo buildInfo)
        {

        }
        
        
        public virtual void FieldICollectionEntitiesHandler(BuilderInfo collectionBuilder)
        {
            MemberICollectionEntitiesHandler(collectionBuilder);
        }
        public virtual void FieldICollectionHandler(BuilderInfo collectionBuilder)
        {
            MemberICollectionHandler(collectionBuilder);
        }
        public virtual void FieldCollectionEntitiesHandler(BuilderInfo collectionBuilder)
        {
            MemberCollectionEntitiesHandler(collectionBuilder);
        }
        public virtual void FieldCollectionHandler(BuilderInfo collectionBuilder)
        {
            MemberCollectionHandler(collectionBuilder);
        }
        public virtual void FieldArrayEntitiesHandler(BuilderInfo buildInfo)
        {
            MemberArrayEntitiesHandler(buildInfo);
        }
        public virtual void FieldArrayHandler(BuilderInfo buildInfo)
        {
            MemberArrayHandler(buildInfo);
        }
        public virtual void FieldIEntitiesHandler(BuilderInfo buildInfo)
        {
            MemberIEntitiesHandler(buildInfo);
        }
        public virtual void FieldEntitiesHandler(BuilderInfo buildInfo)
        {
            MemberEntitiesHandler(buildInfo);
        }
        public virtual void FieldHandler(BuilderInfo buildInfo)
        {
            MemberHandler(buildInfo);
        }


        public virtual void PropertyICollectionEntitiesHandler(BuilderInfo collectionBuilder)
        {
            MemberICollectionEntitiesHandler(collectionBuilder);
        }
        public virtual void PropertyICollectionHandler(BuilderInfo collectionBuilder)
        {
            MemberICollectionHandler(collectionBuilder);
        }
        public virtual void PropertyCollectionEntitiesHandler(BuilderInfo collectionBuilder)
        {
            MemberCollectionEntitiesHandler(collectionBuilder);
        }
        public virtual void PropertyCollectionHandler(BuilderInfo collectionBuilder)
        {
            MemberCollectionHandler(collectionBuilder);
        }
        public virtual void PropertyArrayEntitiesHandler(BuilderInfo buildInfo)
        {
            MemberArrayEntitiesHandler(buildInfo);
        }
        public virtual void PropertyArrayHandler(BuilderInfo buildInfo)
        {
            MemberArrayHandler(buildInfo);
        }
        public virtual void PropertyIEntitiesHandler(BuilderInfo buildInfo)
        {
            MemberIEntitiesHandler(buildInfo);
        }
        public virtual void PropertyEntitiesHandler(BuilderInfo buildInfo)
        {
            MemberEntitiesHandler(buildInfo);
        }
        public virtual void PropertyHandler(BuilderInfo buildInfo)
        {
            MemberHandler(buildInfo);
        }
        public virtual void ReturnHandler(BuilderInfo buildInfo)
        {

        }

        public virtual void MemberEntitiesHandler(BuilderInfo buildInfo)
        {

        }
        public virtual void MemberArrayEntitiesHandler(BuilderInfo buildInfo)
        {

        }
        public virtual void MemberArrayHandler(BuilderInfo buildInfo)
        {

        }
        public virtual void MemberHandler(BuilderInfo buildInfo)
        {

        }
        public virtual void ICollectionHandler(BuilderInfo buildInfo)
        {

        }
        public virtual void CollectionHandler(BuilderInfo collectionBuilder)
        {

        }
        public virtual void MemberIEntitiesHandler(BuilderInfo buildInfo)
        {

        }
        public virtual void MemberICollectionHandler(BuilderInfo buildInfo)
        {

        }
        public virtual void MemberCollectionHandler(BuilderInfo collectionBuilder)
        {

        }
        public virtual void MemberICollectionEntitiesHandler(BuilderInfo collectionBuilder)
        {

        }
        public virtual void MemberCollectionEntitiesHandler(BuilderInfo collectionBuilder)
        {

        }

        /*
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
        internal Delegate ArrayHandler(Type type)
        {

        }
    */


        /// <summary>
        /// 创建实体类委托
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public void EntityHandler(Type type)
        {
            
            BuilderInfo typeInfo = new BuilderInfo();
            typeInfo.Type = type;
            typeInfo.TypeName = NameReverser.GetName(type);
            typeInfo.AvailableName = AvailableNameReverser.GetName(type);
            StartHandler(typeInfo);


            var fields = type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {

                //排除不能操作的类型
                if (!fields[i].IsStatic && !fields[i].IsInitOnly)
                {

                    Type fieldType = fields[i].FieldType;


                    BuilderInfo info = new BuilderInfo();
                    info.MemberName = fields[i].Name;
                    info.Type = fieldType;
                    info.TypeName = NameReverser.GetName(fieldType);
                    info.AvailableName = AvailableNameReverser.GetName(fieldType);


                    if (IsOnceType(fieldType))       //普通字段
                    {
                        FieldHandler(info);
                    }
                    else if (fieldType.IsArray)      //数组
                    {
                        Type eleType = fieldType.GetElementType();
                        info.Type = eleType;
                        info.TypeName = NameReverser.GetName(eleType);
                        info.AvailableName = AvailableNameReverser.GetName(eleType);


                        if (IsOnceType(eleType))
                        {
                            FieldArrayHandler(info);
                        }
                        else
                        {
                            TypeHandler(eleType);
                            FieldArrayEntitiesHandler(info);
                        }
                    }
                    else if (!fieldType.IsNotPublic)
                    {

                        //检测集合
                        Type collectionType = fieldType.GetInterface("IEnumerable");
                        TypeHandler(fieldType);
                        if (collectionType != null)
                        {
                            //创建集合克隆
                            FieldCollectionHandler(info);
                        }
                        else
                        {
                            FieldEntitiesHandler(info);
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

                    Type propertyType = properties[i].PropertyType;


                    BuilderInfo propInfo = new BuilderInfo();
                    propInfo.MemberName = fields[i].Name;
                    propInfo.Type = propertyType;
                    propInfo.TypeName = NameReverser.GetName(propertyType);
                    propInfo.AvailableName = AvailableNameReverser.GetName(propertyType);


                    if (IsOnceType(propertyType))               //普通属性
                    {
                        PropertyHandler(propInfo);
                    }
                    else if (propertyType.IsArray)               //数组
                    {

                        Type eleType = propertyType.GetElementType();
                        propInfo.Type = eleType;
                        propInfo.TypeName = NameReverser.GetName(eleType);
                        propInfo.AvailableName = AvailableNameReverser.GetName(eleType);


                        if (IsOnceType(eleType))
                        {
                            PropertyArrayHandler(propInfo);
                        }
                        else
                        {
                            TypeHandler(eleType);
                            PropertyArrayEntitiesHandler(propInfo);
                        }
                    }
                    else if (!propertyType.IsNotPublic)
                    {

                        Type collectionType = propertyType.GetInterface("IEnumerable");

                        TypeHandler(propertyType);
                        if (collectionType != null)
                        {
                            if (collectionType.IsInterface)
                            {
                                PropertyICollectionHandler(propInfo);
                            }
                            else
                            {
                                PropertyCollectionHandler(propInfo);
                            }
                            
                        }
                        else
                        {
                            if (collectionType.IsInterface)
                            {
                                PropertyIEntitiesHandler(propInfo);
                            }
                            else
                            {
                                TypeHandler(propertyType);
                                PropertyEntitiesHandler(propInfo);
                            }
                        }
                    }
                }
            }

            ReturnHandler(typeInfo);
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
