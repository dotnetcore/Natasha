using System;
using System.Collections.Generic;

namespace Natasha
{
    /// <summary>
    /// 方法模板
    /// </summary>
    public class MethodTemplate : MethodContentTemplate<MethodTemplate>, IMethodPackage
    {
        private Type DelegateType;
        public MethodTemplate() => Link = this;


        /// <summary>
        /// 打包脚本,获取委托信息等
        /// </summary>
        /// <returns>(函数名，涉及到的类，脚本体, 委托类型)</returns>
        public (string Flag, IEnumerable<Type> Types, string Script, Type Delegate) Package()
        {
            //脚本未构建，则自动构建
            if (Script.Length==0){Builder();}


            //获取方法委托
            DelegateType = DelegateBuilder.GetDelegate(ParametersTypes.ToArray(), ReturnType);


            //返回包装结果
            return (NameScript, UsingRecoder.Types, Script.ToString(), DelegateType);
        }




        /// <summary>
        /// 构建脚本
        /// </summary>
        /// <returns></returns>
        public override string Builder()
        {
            return base.Builder();
        }
    }
}
