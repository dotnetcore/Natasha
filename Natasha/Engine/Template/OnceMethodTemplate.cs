using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha
{
    public class OnceMethodTemplate<T> : OnceMethodContentTemplate<T>, IMethodPackage
    {
        private Type DelegateType;
        /// <summary>
        /// 打包脚本,获取委托信息等
        /// </summary>
        /// <returns>(函数名，涉及到的类，脚本体, 委托类型)</returns>
        public (string Flag, IEnumerable<Type> Types, string Script, Type Delegate) Package()
        {
            if (Script.Length == 0)
            {
                Builder();
            }
            DelegateType = DelegateBuilder.GetDelegate(ParametersTypes.ToArray(), ReturnType);
            return (MethodNameScript, UsingRecoder.Types, Script.ToString(), DelegateType);
        }
        public override string Builder()
        {
            return base.Builder();
        }
    }
}
