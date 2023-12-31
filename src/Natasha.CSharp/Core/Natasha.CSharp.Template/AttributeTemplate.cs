using System;
using System.Text;

namespace Natasha.CSharp.Template
{
    public class AttributeTemplate<T> : CommentTemplate<T> where T : AttributeTemplate<T>, new()
    {

        public readonly StringBuilder AttributeScript;
        public AttributeTemplate() => AttributeScript = new StringBuilder();


        public T Attribute(string? attrInfo = default)
        {
            AttributeScript.Clear();
            AttributeScript.AppendLine(attrInfo);
            return Link;
        }

        // <summary>
        /// 根据类型设置特性，参数是特性的参数 如 [type( {ctorInfo} )]
        /// </summary>
        /// <typeparam name="A">特性的类型</typeparam>
        /// <param name="ctorInfo">类型的构造参数字符串</param>
        /// <returns></returns>
        public T Attribute<A>(string? ctorInfo = default)
        {

            return Attribute(typeof(A), ctorInfo);

        }


        /// <summary>
        /// 根据类型设置特性，参数是特性的参数 如 [type({ctorInfo})]
        /// </summary>
        /// <typeparam name="type">特性的类型</typeparam>
        /// <param name="ctorInfo">类型的构造参数字符串</param>
        /// <returns></returns>
        public T Attribute(Type type, string? ctorInfo = default)
        {

            RecordUsing(type);
            if (ctorInfo != default)
            {
                Attribute($"[{type.GetDevelopName()}({ctorInfo})]");
            }
            else
            {
                Attribute($"[{type.GetDevelopName()}]");
            }
            return Link;

        }


        /// <summary>
        /// 直接设置特性字符串
        /// </summary>
        /// <param name="attrInfo">特性字符串</param>
        /// <returns></returns>
        public T AttributeAppend(string attrInfo)
        {
            AttributeScript.AppendLine(attrInfo);
            return Link;

        }




        /// <summary>
        /// 根据类型设置特性，参数是特性的参数 如 [type( {ctorInfo} )]
        /// </summary>
        /// <typeparam name="A">特性的类型</typeparam>
        /// <param name="ctorInfo">类型的构造参数字符串</param>
        /// <returns></returns>
        public T AttributeAppend<A>(string? ctorInfo = default)
        {

            return AttributeAppend(typeof(A), ctorInfo);

        }




        /// <summary>
        /// 根据类型设置特性，参数是特性的参数 如 [type({ctorInfo})]
        /// </summary>
        /// <typeparam name="type">特性的类型</typeparam>
        /// <param name="ctorInfo">类型的构造参数字符串</param>
        /// <returns></returns>
        public T AttributeAppend(Type type, string? ctorInfo = default)
        {

            RecordUsing(type);
            if (ctorInfo != default)
            {
                AttributeAppend($"[{type.GetDevelopName()}({ctorInfo})]");
            }
            else
            {
                AttributeAppend($"[{type.GetDevelopName()}]");
            }
            return Link;

        }


        public override T BuilderScript()
        {
            // [comment]
            // [{this}]
            // [access] [modifier]
            base.BuilderScript();
            if (AttributeScript != default)
            {
                _script.Append(AttributeScript);
            }
            return Link;

        }

    }

}
