using System;

namespace Natasha
{
    public abstract class TypeIterator
    {
        public Type CurrentType;
        public bool IncludeStatic;
        public bool IncludeCanWrite;
        public bool IncludeCanRead;


        /// <summary>
        /// 单独处理复杂类型，例如类、接口，不包括集合、字典
        /// </summary>
        /// <param name="type"></param>
        public virtual void EntityHandler(Type type)
        {
            
        }

        
        #region ArraySingle
        /// <summary>
        /// 一次性赋值的类型，比如int,string,枚举这类的
        /// </summary>
        /// <param name="info">构建信息</param>
        public virtual void ArrayOnceTypeHandler(BuilderInfo info)
        {

        }


        /// <summary>
        /// 复杂类型，比如类、接口、集合类型等
        /// </summary>
        /// <param name="info"></param>
        public virtual void ArrayEntityHandler(BuilderInfo info)
        {

        }
        #endregion


        #region Collection 
        /// <summary>
        /// 集合接口类型，IEnumable<T>
        /// </summary>
        /// <param name="info"></param>
        public virtual void ICollectionHandler(BuilderInfo info)
        {

        }


        /// <summary>
        /// 集合实体类型，比如List<T>
        /// </summary>
        /// <param name="info"></param>
        public virtual void CollectionHandler(BuilderInfo info)
        {

        }


        /// <summary>
        /// 字典接口类型，IDictionary<TKey,TValue>
        /// </summary>
        /// <param name="info"></param>
        public virtual void IDictionaryHandler(BuilderInfo info)
        {

        }


        /// <summary>
        /// 字典实体类型，Dictionary<TKey,TValue>
        /// </summary>
        /// <param name="info"></param>
        public virtual void DictionaryHandler(BuilderInfo info)
        {

        }
        #endregion


        /// <summary>
        /// 一次性类型处理，比如int,string,枚举这类的
        /// </summary>
        /// <param name="info"></param>
        public virtual void OnceTypeHandler(BuilderInfo info)
        {

        }


        /// <summary>
        /// 数组分配方法，数组类型会被拆分，例如：class[]或者int[]
        /// </summary>
        /// <param name="type"></param>
        public void ArrayRouter(Type type)
        {
            Type eleType = type.GetElementType();
            if (eleType.IsOnceType())
            {
                //普通类型处理
                ArrayOnceTypeHandler(type);
            }
            else
            {
                //复杂类型交由入口处理
                EntityHandler(eleType);
                ArrayEntityHandler(type);
            }
        }


        /// <summary>
        /// 实体分配方法，会遍历实体类中的所有成员，不包括方法
        /// </summary>
        /// <param name="type"></param>
        public void EntityRouter(Type type)
        {

            EntityStartHandler(type);


            //字段克隆
            var fields = type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                var fieldInfo = fields[i];
                //排除不能操作的类型
                if ((!fieldInfo.IsStatic | IncludeStatic) && !fieldInfo.IsInitOnly)
                {
                    Type fieldType = fieldInfo.FieldType;
                    if (fieldType.IsOnceType())       //普通字段
                    {
                        FieldOnceTypeHandler(fieldInfo);
                    }
                    else if (fieldType.IsArray)      //数组
                    {
                        Type eleType = fieldType.GetElementType();
                        if (eleType.IsOnceType())
                        {
                            FieldArrayOnceTypeHandler(fieldInfo);
                        }
                        else
                        {
                            EntityHandler(eleType);
                            FieldArrayEntityHandler(fieldInfo);
                        }
                    }
                    else if (!fieldType.IsNotPublic)
                    {
                        //检测集合
                        EntityHandler(fieldType);
                        if (fieldType.GetInterface("IEnumerable") != null)
                        {
                            if (fieldType.GetInterface("IDictionary") != null)
                            {
                                if (fieldType.IsInterface)
                                {
                                    FieldIDictionaryHandler(fieldInfo);
                                }
                                else
                                {
                                    FieldDictionaryHandler(fieldInfo);
                                }
                            }
                            else
                            {
                                if (fieldType.IsInterface)
                                {
                                    FieldICollectionHandler(fieldInfo);
                                }
                                else
                                {
                                    FieldCollectionHandler(fieldInfo);
                                }
                            }
                        }
                        else
                        {
                            FieldEntityHandler(fieldInfo);
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
                if ((propertyInfo.CanRead | !IncludeCanRead) && (propertyInfo.CanWrite | !IncludeCanWrite) && (!propertyInfo.GetGetMethod(true).IsStatic | IncludeStatic))
                {
                    Type propertyType = propertyInfo.PropertyType;

                    if (propertyType.IsOnceType())               //普通属性
                    {
                        PropertyOnceTypeHandler(propertyInfo);
                    }
                    else if (propertyType.IsArray)               //数组
                    {

                        Type eleType = propertyType.GetElementType();

                        if (eleType.IsOnceType())
                        {
                            PropertyArrayOnceTypeHandler(propertyInfo);
                        }
                        else
                        {
                            EntityHandler(eleType);
                            PropertyArrayEntityHandler(propertyInfo);
                        }
                    }
                    else if (!propertyType.IsNotPublic)
                    {
                        //检测集合
                        EntityHandler(propertyType);


                        if (propertyType.GetInterface("IEnumerable") != null)
                        {
                            if (propertyType.GetInterface("IDictionary") != null)
                            {
                                if (propertyType.IsInterface)
                                {
                                    PropertyIDictionaryHandler(propertyInfo);
                                }
                                else
                                {
                                    PropertyDictionaryHandler(propertyInfo);
                                }
                            }
                            else
                            {
                                if (propertyType.IsInterface)
                                {
                                    PropertyICollectionHandler(propertyInfo);
                                }
                                else
                                {
                                    PropertyCollectionHandler(propertyInfo);
                                }
                            }
                        }
                        else
                        {
                            PropertyEntityHandler(propertyInfo);
                        }
                    }
                }
            }

            EntityReturnHandler(type);

        }


        /// <summary>
        /// 集合分配方法，将集合拆分处理，例如，IEnumable<T>或List<T>
        /// </summary>
        /// <param name="type"></param>
        public void CollectionRouter(Type type)
        {           

            if (!type.IsGenericType)
            {
                Type[] args = type.GetGenericArguments();
                EntityHandler(args[0]);
            }


            if (type.IsInterface)
            {
                ICollectionHandler(type);
            }
            else
            {
                CollectionHandler(type);
            }
        }


        /// <summary>
        /// 字典分配方法，将字典拆分处理，例如，IDictionary<TKey,TValue>或Dictionary<TKey,TValue>
        /// </summary>
        /// <param name="type"></param>
        public void DictionaryRouter(Type type)
        {

            if (!type.IsGenericType)
            {
                Type[] args = type.GetGenericArguments();
                EntityHandler(args[0]);
                EntityHandler(args[1]);
            }


            if (type.IsInterface)
            {
                IDictionaryHandler(type);
            }
            else
            {
                DictionaryHandler(type);
            }
        }


        /// <summary>
        /// 一次性类型分配方法
        /// </summary>
        /// <param name="type"></param>
        public void OnceTypeRouter(Type type)
        {
            OnceTypeHandler(type);
        }

        #region StartAndEnd
        /// <summary>
        /// 复杂类型刚开始时需要处理的，比如初始化
        /// </summary>
        /// <param name="info"></param>
        public virtual void EntityStartHandler(BuilderInfo info)
        {

        }


        /// <summary>
        /// 复杂类型结束时需要处理的，比如返回一个实例
        /// </summary>
        /// <param name="info"></param>
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
        /// <summary>
        /// 复杂类型中，一次性类型的成员操作,
        /// 例如 public int Age; public string Name{get;set;}
        /// </summary>
        /// <param name="info"></param>
        public virtual void MemberOnceTypeHandler(BuilderInfo info)
        {

        }


        /// <summary>
        /// 复杂类型中，一次性数组类型的成员操作，
        /// 例如public int[] Scores;public string[] Names{get;set;}
        /// </summary>
        /// <param name="info"></param>
        public virtual void MemberArrayOnceTypeHandler(BuilderInfo info)
        {

        }


        /// <summary>
        /// 复杂类型中，复杂类型数组的成员操作，
        /// 例如public ClassA[] values;
        /// </summary>
        /// <param name="info"></param>
        public virtual void MemberArrayEntityHandler(BuilderInfo info)
        {

        }


        /// <summary>
        /// 复杂类型中，接口集合的成员操作，
        /// 例如public IEnumables<T> list;
        /// </summary>
        /// <param name="info"></param>
        public virtual void MemberICollectionHandler(BuilderInfo info)
        {

        }


        /// <summary>
        /// 复杂类型中，接口集合的成员操作，
        /// 例如public List<T> list;
        /// </summary>
        /// <param name="info"></param>
        public virtual void MemberCollectionHandler(BuilderInfo info)
        {

        }


        /// <summary>
        /// 复杂类型中，复杂类型的成员操作，
        /// 例如public ClassA SubClass;
        /// </summary>
        /// <param name="info"></param>
        public virtual void MemberEntityHandler(BuilderInfo info)
        {

        }


        /// <summary>
        /// 复杂类型中，接口集合的成员操作，
        /// 例如public IDictionary<TKey,TValue> dict;
        /// </summary>
        /// <param name="info"></param>
        public virtual void MemberIDictionaryHandler(BuilderInfo info)
        {

        }


        /// <summary>
        /// 复杂类型中，接口集合的成员操作，
        /// 例如public Dictionary<TKey,TValue> dict;
        /// </summary>
        /// <param name="info"></param>
        public virtual void MemberDictionaryHandler(BuilderInfo info)
        {

        }
        #endregion




        /// <summary>
        /// 获取克隆委托
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool TypeRouter(Type type)
        {
            //无效类PASS
            if (type.FullName == null)
            {
                return false;
            }

            if (type.IsOnceType())
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
    }    
}

