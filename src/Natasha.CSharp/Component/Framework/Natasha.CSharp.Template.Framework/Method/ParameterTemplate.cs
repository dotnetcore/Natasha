using Natasha.CSharp.Template.Reverser;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Natasha.CSharp.Template
{

    public class ParameterTemplate<T> : DefinedNameTemplate<T> where T : ParameterTemplate<T>, new()
    {


        public readonly StringBuilder ParametersScript;

        public ParameterTemplate()
        {
            ParametersScript = new StringBuilder();
        }




        /// <summary>
        /// 根据方法元数据将参数还原
        /// </summary>
        /// <param name="info">方法元数据</param>
        /// <returns></returns>
        public T Param(MethodInfo info)
        {

            var parameters = info.GetParameters().OrderBy(item => item.Position);
            foreach (var item in parameters)
            {
                Param(item);
            }
            return Link;

        }




        /// <summary>
        /// 根据参数元数据还原参数信息
        /// </summary>
        /// <param name="info">参数元数据</param>
        /// <returns></returns>
        public T Param(ParameterInfo info)
        {

            RecordUsing(info.ParameterType);
            Param(info.GetCustomAttribute(typeof(DynamicAttribute)) == null ? info.ParameterType.GetDevelopName() : "dynamic", info.Name!, DeclarationReverser.GetParametePrefix(info));
            return Link;

        }




        ///// <summary>
        ///// 根据字符串设置参数
        ///// </summary>
        ///// <param name="parameterString">参数字符串</param>
        ///// <returns></returns>
        //public T Param(string parameterString)
        //{

        //    if (parameterString!=default)
        //    {
        //        ParametersScript.Append(parameterString + ",");
        //    }
        //    return Link;

        //}





        /// <summary>
        /// 添加参数
        /// </summary>
        /// <typeparam name="S">参数类型</typeparam>
        /// <param name="paramName">参数名字</param>
        /// <returns></returns>
        public T Param<S>(string paramName, string keywords = "")
        {
            return Param(typeof(S), paramName, keywords);
        }




        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="type">参数类型</param>
        /// <param name="paramName">参数名字</param>
        /// <returns></returns>
        public virtual T Param(string type, string paramName, string keywords = "")
        {
            return Param($"{keywords}{type} {paramName}");
        }
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="type">参数类型</param>
        /// <param name="paramName">参数名字</param>
        /// <returns></returns>
        public virtual T Param(Type type, string paramName, string keywords = "")
        {


            return Param($"{keywords}{type.GetDevelopName()} {paramName}");

        }


        public virtual T Param(string script)
        {
            if (ParametersScript.Length > 0)
            {
                ParametersScript.Append(',');
            }
            ParametersScript.Append(script);
            return Link;
        }




        public override T BuilderScript()
        {
            // [comment]
            // [attribute]
            // [access] [modifier] [type] [name][{this}]  
            base.BuilderScript();
            _script.Append($"({ParametersScript})");
            return Link;

        }

    }

}
