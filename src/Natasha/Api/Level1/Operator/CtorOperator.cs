using Natasha.Builder;
using System;

namespace Natasha.Operator
{
    /// <summary>
    /// 初始化操作类，生成初始化委托
    /// </summary>
    public class CtorOperator: OnceMethodBuilder<CtorOperator>
    {
        public CtorOperator()
        {
            Link = this;
        }
        /// <summary>
        /// 返回初始化委托
        /// </summary>
        /// <typeparam name="T">初始化类型</typeparam>
        /// <param name="type">当T为object类型时候，type为真实类型</param>
        /// <returns></returns>
        public Func<T> NewDelegate<T>(Type type=null)
        {

            if (type==null)
            {

                //直接使用T的类型作为初始化类型
                type = typeof(T);

            }


            //返回构建委托
            return Public
                .Static
                .PublicMember
                .StaticMember
                .UseCustomerUsing()
                .Using(type)
                .Return<T>()
                .UseRandomOopName()
                .HiddenNameSpace()
                .MethodBody($@"return new {type.GetDevelopName()}();")
                .Complie<Func<T>>();

        }




        /// <summary>
        /// 返回初始化委托
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public Delegate NewDelegate(Type type)
        {

            return Public
                .Static
                .PublicMember
                .StaticMember
                .UseCustomerUsing()
                .Using(type)
                .Return(type)
                .UseRandomOopName()
                .HiddenNameSpace()
                .MethodBody($@"return new {type.GetDevelopName()}();")
                .Complie();

        }
    }
}
