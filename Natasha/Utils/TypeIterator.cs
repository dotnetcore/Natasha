using System;

namespace Natasha
{
    public abstract class TypeIterator
    {
        public void TypeHandler(Type type)
        {
            //无效类PASS
            if (type.FullName == null){ return;}

            BuilderInfo typeInfo = new BuilderInfo();
            typeInfo.MemberType = type;
            typeInfo.ClassName = NameReverser.GetName(type);
            typeInfo.AvailableName = AvailableNameReverser.GetName(type);

            if (type.IsArray)                                   //数组直接克隆
            {
                MemberArrayHandler(typeInfo);
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
        /// <summary>
        /// 创建实体类委托
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public void EntityHandler(Type type)
        {
            
            BuilderInfo typeInfo = new BuilderInfo();
            typeInfo.MemberType = type;
            typeInfo.ClassName = NameReverser.GetName(type);
            typeInfo.AvailableName = AvailableNameReverser.GetName(type);
            StartHandler(typeInfo);


            var fields = type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {

                //排除不能操作的类型
                if (!fields[i].IsStatic && !fields[i].IsInitOnly)
                {

                    Type fieldType = fields[i].FieldType;


                    BuilderInfo fieldInfo = new BuilderInfo();
                    fieldInfo.Name = fields[i].Name;
                    fieldInfo.MemberType = fieldType;
                    fieldInfo.ClassName = NameReverser.GetName(fieldType);
                    fieldInfo.AvailableName = AvailableNameReverser.GetName(fieldType);


                    if (IsOnceType(fieldType))       //普通字段
                    {
                        FieldHandler(fieldInfo);
                    }
                    else if (fieldType.IsArray)      //数组
                    {
                        Type eleType = fieldType.GetElementType();
                        fieldInfo.MemberType = eleType;
                        fieldInfo.ClassName = NameReverser.GetName(eleType);
                        fieldInfo.AvailableName = AvailableNameReverser.GetName(eleType);


                        if (IsOnceType(eleType))
                        {
                            FieldArrayHandler(fieldInfo);
                        }
                        else
                        {
                            FieldArrayEntitiesHandler(fieldInfo);
                        }
                    }
                    else if (!fieldType.IsNotPublic)
                    {

                        //检测集合
                        Type collectionType = fieldType.GetInterface("IEnumerable");

                        if (collectionType != null)
                        {
                            //创建集合克隆
                            FieldCollectionHandler(fieldInfo);
                        }
                        else
                        {
                            FieldEntitiesHandler(fieldInfo);
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
                    propInfo.Name = fields[i].Name;
                    propInfo.MemberType = propertyType;
                    propInfo.ClassName = NameReverser.GetName(propertyType);
                    propInfo.AvailableName = AvailableNameReverser.GetName(propertyType);


                    if (IsOnceType(propertyType))               //普通属性
                    {
                        PropertyHandler(propInfo);
                    }
                    else if (propertyType.IsArray)               //数组
                    {

                        Type eleType = propertyType.GetElementType();
                        propInfo.MemberType = eleType;
                        propInfo.ClassName = NameReverser.GetName(eleType);
                        propInfo.AvailableName = AvailableNameReverser.GetName(eleType);


                        if (IsOnceType(eleType))
                        {
                            PropertyArrayHandler(propInfo);
                        }
                        else
                        {
                            PropertyArrayEntitiesHandler(propInfo);
                        }
                    }
                    else if (!propertyType.IsNotPublic)
                    {

                        Type collectionType = propertyType.GetInterface("IEnumerable");


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

    public class BuilderInfo
    {
        public string Name;
        public string ClassName;
        public Type MemberType;
        public string AvailableName;
    }
}
