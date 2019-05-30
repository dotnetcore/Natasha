using System;
using System.Collections.Generic;

namespace Natasha
{
    /// <summary>
    /// 快速构建一个方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OnceMethodTemplate<T> : OnceMethodContentTemplate<T>, IMethodPackage
    {

        private Type DelegateType;


        /// <summary>
        /// 打包脚本,获取委托信息等
        /// </summary>
        /// <returns>(函数名，涉及到的类，脚本体, 委托类型)</returns>
        public (string Flag, IEnumerable<Type> Types, string Script, Type Delegate) Package()
        {
            //未扣减脚本则构建脚本
            if (Script.Length == 0){Builder();}


            //获取动态方法委托类型
            DelegateType = DelegateBuilder.GetDelegate(ParametersTypes.ToArray(), ReturnType);


            //返回包装函数信息
            return (MethodNameScript, UsingRecoder.Types, Script.ToString(), DelegateType);
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
