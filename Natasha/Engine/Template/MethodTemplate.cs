using System;
using System.Collections.Generic;

namespace Natasha
{
    public class MethodTemplate : MethodContentTemplate<MethodTemplate>, IMethodPackage
    {
        public MethodTemplate()
        {
            Link = this;
        }

        private Type DelegateType;
        /// <summary>
        /// 打包脚本,获取委托信息等
        /// </summary>
        /// <returns>(函数名，涉及到的类，脚本体, 委托类型)</returns>
        public (string Flag, IEnumerable<Type> Types, string Script, Type Delegate) Package()
        {
            if (Script.Length==0)
            {
                Builder();
            }
            DelegateType = DelegateBuilder.GetDelegate(ParametersTypes.ToArray(), ReturnType);
            return (NameScript, UsingRecoder.Types, Script.ToString(), DelegateType);
        }
        public override string Builder()
        {
            return base.Builder();
        }
    }
}
