using Natasha.CSharp.Reverser;
using System;
using System.Reflection;

namespace Natasha.CSharp.Template
{
    public class AccessTemplate<T> : AttributeTemplate<T> where T : AccessTemplate<T>, new()
    {

        public string AccessScript;




        /// <summary>
        /// 不使用访问级别定义
        /// </summary>
        /// <returns></returns>
        public T NoUseAccess()
        {

            AccessScript = default;
            return Link;

        }



        
        /// <summary>
        /// 根据类型反射得到保护级别
        /// </summary>
        /// <param name="type">外部类型</param>
        /// <returns></returns>
        public T Access(Type type)
        {

            AccessScript = AccessReverser.GetAccess(type);
            return Link;

        }




        /// <summary>
        /// 设置访问级别
        /// </summary>
        /// <param name="accessInfo">根据方法元数据反射出对应的访问级别</param>
        /// <returns></returns>
        public T Access(MethodInfo accessInfo)
        {

            AccessScript = AccessReverser.GetAccess(accessInfo);
            return Link;

        }




        /// <summary>
        /// 通过枚举来设置访问级别
        /// </summary>
        /// <param name="accessEnum">访问的枚举</param>
        /// <returns></returns>
        public T Access(AccessFlags accessEnum = AccessFlags.Internal)
        {

            AccessScript = AccessReverser.GetAccess(accessEnum);
            return Link;

        }




        /// <summary>
        /// 自定义保护级别字符串如 : public / internal / private ... 
        /// </summary>
        /// <param name="accessString">保护级别字符串</param>
        /// <returns></returns>
        public T Access(string accessString)
        {

            AccessScript = ScriptWrapper(accessString);
            return Link;

        }




        public T Public()
        {

            return Access(AccessFlags.Public);

        }
        public T Private()
        {

            return Access(AccessFlags.Private);

        }
        public T Protected()
        {

            return Access(AccessFlags.Protected);

        }
        public T Internal()
        {

            return Access(AccessFlags.Internal);

        }
        public T InternalAndProtected()
        {

            return Access(AccessFlags.InternalAndProtected);

        }



        public override T BuilderScript()
        {

            // [Attribute]
            // [{this}]
            base.BuilderScript();
            _script.Append(AccessScript);
            return Link;


        }



    }
}
