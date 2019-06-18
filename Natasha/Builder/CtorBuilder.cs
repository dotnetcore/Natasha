using System;

namespace Natasha
{
    /// <summary>
    /// 初始化操作类，生成初始化委托
    /// </summary>
    public class CtorBuilder
    {
        /// <summary>
        /// 返回初始化委托
        /// </summary>
        /// <typeparam name="T">初始化类型</typeparam>
        /// <param name="type">当T为object类型时候，type为真实类型</param>
        /// <returns></returns>
        public static Func<T> NewDelegate<T>(Type type=null)
        {
            var builder = FastMethodOperator.New;
            if (type==null)
            {
                //直接使用T的类型作为初始化类型
                type = typeof(T);
            }
            else
            {
                //T为object，那么自动加载type的命名空间
                builder.Using(type);
            }


            //返回构建委托
            return builder
                .Using<T>()
                .MethodBody($@"return new {type.GetDevelopName()}();")
                .Return<T>()
                .Complie<Func<T>>();
        }




        /// <summary>
        /// 返回初始化委托
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static Delegate NewDelegate(Type type)
        {
            return FastMethodOperator.New
                .MethodBody($@"return new {type.GetDevelopName()}();")
                .Return(type)
                .Complie();
        }
    }
}
