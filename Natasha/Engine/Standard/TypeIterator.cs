using System;

namespace Natasha
{
    public abstract class TypeIterator
    {
        public Type CurrentType;
        public virtual void EntityHandler(Type type)
        {
            
        }
        #region ArraySingle
        public virtual void ArrayOnceTypeHandler(BuilderInfo info)
        {

        }
        public virtual void ArrayEntityHandler(BuilderInfo info)
        {

        }
        #endregion

        #region Collection
        public virtual void ICollectionHandler(BuilderInfo info)
        {

        }
        public virtual void CollectionHandler(BuilderInfo info)
        {

        }
        public virtual void IDictionaryHandler(BuilderInfo info)
        {

        }
        public virtual void DictionaryHandler(BuilderInfo info)
        {

        }
        #endregion
        public virtual void OnceTypeHandler(BuilderInfo info)
        {

        }

        public void ArrayRouter(Type type)
        {
            Type eleType = type.GetElementType();
            BuilderInfo typeInfo = new BuilderInfo();
            typeInfo.Type = type;
            typeInfo.RealType = eleType;
            typeInfo.TypeName = NameReverser.GetName(eleType);
            typeInfo.AvailableName = AvailableNameReverser.GetName(type);
            if (IsOnceType(eleType))
            {
                //普通类型处理
                ArrayOnceTypeHandler(typeInfo);
            }
            else
            {
                //复杂类型交由入口处理
                EntityHandler(eleType);
                ArrayEntityHandler(typeInfo);
            }
        }
        public void EntityRouter(Type type)
        {

            BuilderInfo typeInfo = new BuilderInfo();
            typeInfo.Type = type;
            typeInfo.TypeName = NameReverser.GetName(type);
            typeInfo.AvailableName = AvailableNameReverser.GetName(type);


            EntityStartHandler(typeInfo);

            //字段克隆
            var fields = type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                var fieldInfo = fields[i];
                //排除不能操作的类型
                if (!fieldInfo.IsStatic && !fieldInfo.IsInitOnly)
                {
                    Type fieldType = fieldInfo.FieldType;
                   
                    if (IsOnceType(fieldType))       //普通字段
                    {
                        BuilderInfo info = new BuilderInfo();
                        info.MemberName = fieldInfo.Name;
                        info.RealType = fieldType;
                        info.TypeName = NameReverser.GetName(fieldType);
                        info.AvailableName = AvailableNameReverser.GetName(fieldType);
                        FieldOnceTypeHandler(info);
                    }
                    else if (fieldType.IsArray)      //数组
                    {

                        Type eleType = fieldType.GetElementType();
                        BuilderInfo info = new BuilderInfo();
                        info.MemberName = fieldInfo.Name;
                        info.Type = eleType;
                        info.RealType = eleType;
                        info.TypeName = NameReverser.GetName(eleType);
                        info.AvailableName = AvailableNameReverser.GetName(fieldType);

                        if (IsOnceType(eleType))
                        {
                            FieldArrayOnceTypeHandler(info);
                        }
                        else
                        {
                            EntityHandler(eleType);
                            FieldArrayEntityHandler(info);
                        }
                    }
                    else if (!fieldType.IsNotPublic)
                    {

                        //检测集合
                        BuilderInfo info = new BuilderInfo();
                        info.MemberName = fieldInfo.Name;
                        info.RealType = fieldType;
                        info.TypeName = NameReverser.GetName(fieldType);
                        info.AvailableName = AvailableNameReverser.GetName(fieldType);

                        EntityHandler(fieldType);
                        if (fieldType.GetInterface("IEnumerable") != null)
                        {
                            if (fieldType.GetInterface("IDictionary") != null)
                            {
                                if (fieldType.IsInterface)
                                {
                                    FieldIDictionaryHandler(info);
                                }
                                else
                                {
                                    FieldDictionaryHandler(info);
                                }
                            }
                            else
                            {
                                if (fieldType.IsInterface)
                                {
                                    FieldICollectionHandler(info);
                                }
                                else
                                {
                                    FieldCollectionHandler(info);
                                }
                            }
                        }
                        else
                        {
                            FieldEntityHandler(info);
                        }
                    }
                }
            }

            //属性克隆
            var properties = type.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {

                var propertyInfo = properties[i];

                //排除不能操作的属性
                if (propertyInfo.CanRead && propertyInfo.CanWrite && !propertyInfo.GetGetMethod(true).IsStatic)
                {
                    Type propertyType = propertyInfo.PropertyType;


                    if (IsOnceType(propertyType))               //普通属性
                    {
                        BuilderInfo info = new BuilderInfo();
                        info.MemberName = propertyInfo.Name;
                        info.RealType = propertyType;
                        info.TypeName = NameReverser.GetName(propertyType);
                        info.AvailableName = AvailableNameReverser.GetName(propertyType);
                        PropertyOnceTypeHandler(info);
                    }
                    else if (propertyType.IsArray)               //数组
                    {

                        Type eleType = propertyType.GetElementType();

                        BuilderInfo info = new BuilderInfo();
                        info.MemberName = propertyInfo.Name;
                        info.Type = propertyType;
                        info.RealType = eleType;
                        info.TypeName = NameReverser.GetName(eleType);
                        info.AvailableName = AvailableNameReverser.GetName(propertyType);

                        if (IsOnceType(eleType))
                        {
                            PropertyArrayOnceTypeHandler(info);
                        }
                        else
                        {
                            EntityHandler(eleType);
                            PropertyArrayEntityHandler(info);
                        }
                    }
                    else if (!propertyType.IsNotPublic)
                    {
                        //检测集合
                        BuilderInfo info = new BuilderInfo();
                        info.MemberName = propertyInfo.Name;
                        info.RealType = propertyType;
                        info.TypeName = NameReverser.GetName(propertyType);
                        info.AvailableName = AvailableNameReverser.GetName(propertyType);


                        EntityHandler(propertyType);


                        if (propertyType.GetInterface("IEnumerable") != null)
                        {
                            if (propertyType.GetInterface("IDictionary") != null)
                            {
                                if (propertyType.IsInterface)
                                {
                                    PropertyIDictionaryHandler(info);
                                }
                                else
                                {
                                    PropertyDictionaryHandler(info);
                                }
                            }
                            else
                            {
                                if (propertyType.IsInterface)
                                {
                                    PropertyICollectionHandler(info);
                                }
                                else
                                {
                                    PropertyCollectionHandler(info);
                                }
                            }
                        }
                        else
                        {
                            PropertyEntityHandler(info);
                        }
                    }
                }
            }

            EntityReturnHandler(typeInfo);

        }
        public void CollectionRouter(Type type)
        {           

            BuilderInfo typeInfo = new BuilderInfo();
            typeInfo.RealType = type;
            typeInfo.TypeName = NameReverser.GetName(type);
            typeInfo.AvailableName = AvailableNameReverser.GetName(type);


            if (!type.IsGenericType)
            {
                Type[] args = type.GetGenericArguments();
                EntityHandler(args[0]);
            }


            if (type.IsInterface)
            {
                ICollectionHandler(typeInfo);
            }
            else
            {
                CollectionHandler(typeInfo);
            }
        }
        public void DictionaryRouter(Type type)
        {

            BuilderInfo typeInfo = new BuilderInfo();
            typeInfo.RealType = type;
            typeInfo.TypeName = NameReverser.GetName(type);
            typeInfo.AvailableName = AvailableNameReverser.GetName(type);


            if (!type.IsGenericType)
            {
                Type[] args = type.GetGenericArguments();
                EntityHandler(args[0]);
                EntityHandler(args[1]);
            }


            if (type.IsInterface)
            {
                IDictionaryHandler(typeInfo);
            }
            else
            {
                DictionaryHandler(typeInfo);
            }
        }
        public void OnceTypeRouter(Type type)
        {
            BuilderInfo typeInfo = new BuilderInfo();
            typeInfo.RealType = type;
            typeInfo.TypeName = NameReverser.GetName(type);
            typeInfo.AvailableName = AvailableNameReverser.GetName(type);
            OnceTypeHandler(typeInfo);
        }

        #region StartAndEnd
        public virtual void EntityStartHandler(BuilderInfo info)
        {

        }
        public virtual void EntityReturnHandler(BuilderInfo info)
        {

        }
        #endregion

        #region Field
        public virtual void FieldOnceTypeHandler(BuilderInfo info)
        {
            MemberOnceTypeHandler(info);
        }
        public virtual void FieldArrayOnceTypeHandler(BuilderInfo info)
        {
            MemberArrayOnceTypeHandler(info);
        }
        public virtual void FieldArrayEntityHandler(BuilderInfo info)
        {
            MemberArrayEntityHandler(info);
        }
        public virtual void FieldICollectionHandler(BuilderInfo info)
        {
            MemberICollectionHandler(info);
        }
        public virtual void FieldCollectionHandler(BuilderInfo info)
        {
            MemberCollectionHandler(info);
        }
        public virtual void FieldEntityHandler(BuilderInfo info)
        {
            MemberEntityHandler(info);
        }
        public virtual void FieldIDictionaryHandler(BuilderInfo info)
        {
            MemberIDictionaryHandler(info);
        }
        public virtual void FieldDictionaryHandler(BuilderInfo info)
        {
            MemberDictionaryHandler(info);
        }
        #endregion

        #region Property
        public virtual void PropertyOnceTypeHandler(BuilderInfo info)
        {
            MemberOnceTypeHandler(info);
        }
        public virtual void PropertyArrayOnceTypeHandler(BuilderInfo info)
        {
            MemberArrayOnceTypeHandler(info);
        }
        public virtual void PropertyArrayEntityHandler(BuilderInfo info)
        {
            MemberArrayEntityHandler(info);
        }
        public virtual void PropertyICollectionHandler(BuilderInfo info)
        {
            MemberICollectionHandler(info);
        }
        public virtual void PropertyCollectionHandler(BuilderInfo info)
        {
            MemberCollectionHandler(info);
        }
        public virtual void PropertyEntityHandler(BuilderInfo info)
        {
            MemberEntityHandler(info);
        }
        public virtual void PropertyIDictionaryHandler(BuilderInfo info)
        {
            MemberIDictionaryHandler(info);
        }
        public virtual void PropertyDictionaryHandler(BuilderInfo info)
        {
            MemberDictionaryHandler(info);
        }
        #endregion

        #region Member
        public virtual void MemberOnceTypeHandler(BuilderInfo info)
        {

        }
        public virtual void MemberArrayOnceTypeHandler(BuilderInfo info)
        {

        }
        public virtual void MemberArrayEntityHandler(BuilderInfo info)
        {

        }
        public virtual void MemberICollectionHandler(BuilderInfo info)
        {

        }
        public virtual void MemberCollectionHandler(BuilderInfo info)
        {

        }
        public virtual void MemberEntityHandler(BuilderInfo info)
        {

        }
        public virtual void MemberIDictionaryHandler(BuilderInfo info)
        {

        }
        public virtual void MemberDictionaryHandler(BuilderInfo info)
        {

        }
        #endregion




        /// <summary>
        /// 获取克隆委托
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool TypeHandler(Type type)
        {
            //无效类PASS
            if (type.FullName == null)
            {
                return false;
            }

            if (IsOnceType(type))
            {
                OnceTypeRouter(type);
                return true;
            }

            if (type.IsArray)                                   //数组直接
            {
                ArrayRouter(type);
            }
            else if (type.GetInterface("IEnumerable") != null)  //集合直接
            {
                if (type.GetInterface("IDictionary")!=null)
                {
                    DictionaryRouter(type);
                }
                else
                {
                    CollectionRouter(type);
                }
            }
            else
            {
                //实体类型
                EntityRouter(type);
            }
            return true;
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
                            || (!type.IsClass && !type.IsInterface)
                            || type.IsEnum
                            || type == typeof(Delegate);
        }
    }

    public class BuilderInfo
    {
        public string MemberName;
        public string TypeName;
        public Type Type;
        public Type RealType;
        public string AvailableName;
    }
}

