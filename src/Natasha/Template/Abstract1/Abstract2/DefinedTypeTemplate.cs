using System;
using System.Reflection;

namespace Natasha.Template
{

    public class DefinedTypeTemplate<T> : ModifierTemplate<T> where T: DefinedTypeTemplate<T>, new()
    {

        public string TypeScript;
        internal Type _type;




        /// <summary>
        /// 不使用该层定义
        /// </summary>
        /// <returns></returns>
        public T NoUseDefinedType()
        {
            return DefinedType(typeString: default);
        }




        /// <summary>
        /// 用户自定义类型字符串
        /// </summary>
        /// <param name="typeString">类型字符串</param>
        /// <returns></returns>
        public T DefinedType(string typeString)
        {

            TypeScript = ScriptWrapper(typeString);
            return Link;

        }




        /// <summary>
        /// 根据外部类型来创建定义类型
        /// </summary>
        /// <param name="type">外部类型</param>
        public T DefinedType(Type type)
        {

            RecoderType(type);
            if (type==typeof(void))
            {
                DefinedType("void");
            }


            _type = type;
            if (type!=default)
            {

                DefinedType(type.GetDevelopName());

            }
            return Link;

        }
        public T DefinedType<S>()
        {
            return DefinedType(typeof(S));
        }




        /// <summary>
        /// 根据成员元数据来反射定义类型
        /// </summary>
        /// <param name="typeInfo">成员元数据</param>
        /// <returns></returns>
        public T DefinedType(MemberInfo memberInfo)
        {

            switch (memberInfo.MemberType)
            {

                case MemberTypes.Field:
                    return DefinedType(((FieldInfo)memberInfo).FieldType);

                case MemberTypes.Method:
                    return DefinedType(((MethodInfo)memberInfo).ReturnType);

                case MemberTypes.Property:
                    return DefinedType(((PropertyInfo)memberInfo).PropertyType);

                default:
                    return Link;

            }

        }



        
        public override T Builder()
        {

            // [Attribute]
            // [access] [modifier] [{this}]  
            base.Builder();
            _script.Append(TypeScript);
            return Link;

        }


    }

}
