using System;
using System.Reflection;

namespace Natasha.CSharp.Template
{

    public class DefinedTypeTemplate<T> : ModifierTemplate<T> where T: DefinedTypeTemplate<T>, new()
    {

        public string TypeScript;
        internal Type? _type;

        public DefinedTypeTemplate()
        {
            TypeScript = string.Empty;
        }
        /// <summary>
        /// 不使用该层定义
        /// </summary>
        /// <returns></returns>
        public T NoUseType()
        {
            TypeScript = string.Empty;
            return Link;
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

            RecordUsing(type);
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

            return memberInfo.MemberType switch
            {
                MemberTypes.Field => Type(((FieldInfo)memberInfo).FieldType),
                MemberTypes.Method => Type(((MethodInfo)memberInfo).ReturnType),
                MemberTypes.Property => Type(((PropertyInfo)memberInfo).PropertyType),
                MemberTypes.Event => Type(((EventInfo)memberInfo).EventHandlerType!),
                _ => Link,
            };
        }




        public override T BuilderScript()
        {
            // [comment]
            // [attribute]
            // [access] [modifier] [{this}]  
            base.BuilderScript();
            if (TypeScript != default)
            {
                _script.Append(TypeScript);
            }
            return Link;

        }


    }

}
