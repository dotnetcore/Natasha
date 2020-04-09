using Natasha.Reverser;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Natasha.Template
{

    public class ParameterTemplate<T> : DefinedNameTemplate<T> where T : ParameterTemplate<T>, new()
    {


        public readonly StringBuilder ParametersScript;

        public ParameterTemplate()
        {
            ParametersScript = new StringBuilder();
            ParametersScript.Append('(');
        }




        /// <summary>
        /// 根据方法元数据将参数还原
        /// </summary>
        /// <param name="info">方法元数据</param>
        /// <returns></returns>
        public T Param(MethodInfo info)
        {

            var parameters = info.GetParameters().OrderBy(item=>item.Position);
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
            
            Param(info.ParameterType, info.Name, DeclarationReverser.GetParametePrefix(info));
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
        public T Param<S>(string paramName, string keywords = default)
        {
            return Param(typeof(S), paramName, keywords);
        }




        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="type">参数类型</param>
        /// <param name="paramName">参数名字</param>
        /// <returns></returns>
        public virtual T Param(Type type, string paramName, string keywords = default)
        {

            RecoderType(type);
            ParametersScript.Append($"{keywords}{type.GetDevelopName()} {paramName},");
            return Link;

        }




        public override T BuilderScript()
        {

            // [Attribute]
            // [access] [modifier] [type] [Name][{this}]  
            if (ParametersScript.Length > 1)
            {
                ParametersScript.Length -= 1;
            }
            ParametersScript.Append(')');

            base.BuilderScript();
            _script.Append(ParametersScript);
            return Link;

        }

    }

}
