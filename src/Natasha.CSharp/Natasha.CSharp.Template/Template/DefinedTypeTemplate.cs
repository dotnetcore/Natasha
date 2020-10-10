using System;
using System.Reflection;

namespace Natasha.CSharp.Template
{

    public class DefinedTypeTemplate<T> : ModifierTemplate<T> where T: DefinedTypeTemplate<T>, new()
    {


        public string TypeScript;
        internal Type _type;




        /// <summary>
        /// 不使用该层定义
        /// </summary>
        /// <returns></returns>
        public T NoUseType()
        {
            return Type(typeString: default);
        }




        /// <summary>
        /// 用户自定义类型字符串
        /// </summary>
        /// <param name="typeString">类型字符串</param>
        /// <returns></returns>
        public T Type(string typeString)
        {

            TypeScript = ScriptWrapper(typeString);
            return Link;

        }


        /// <summary>
        /// 根据外部类型来创建定义类型
        /// </summary>
        /// <param name="type">外部类型</param>
        public T Type(Type type)
        {

            RecoderType(type);
            if (type==typeof(void))
            {
                Type("void");
            }


            _type = type;
            if (type!=default)
            {

                Type(type.GetDevelopName());

            }
            return Link;

        }
        public T Type<S>()
        {
            return Type(typeof(S));
        }




        /// <summary>
        /// 根据成员元数据来反射定义类型
        /// </summary>
        /// <param name="typeInfo">成员元数据</param>
        /// <returns></returns>
        public T Type(MemberInfo memberInfo)
        {

            switch (memberInfo.MemberType)
            {

                case MemberTypes.Field:
                    return Type(((FieldInfo)memberInfo).FieldType);

                case MemberTypes.Method:
                    return Type(((MethodInfo)memberInfo).ReturnType);

                case MemberTypes.Property:
                    return Type(((PropertyInfo)memberInfo).PropertyType);

                default:
                    return Link;

            }

        }



        
        public override T BuilderScript()
        {

            // [Attribute]
            // [access] [modifier] [{this}]  
            base.BuilderScript();
            _script.Append(TypeScript);
            return Link;

        }


    }

}
