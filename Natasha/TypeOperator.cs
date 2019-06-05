using System;
using System.Reflection;

namespace Natasha
{
    public class TypeOperator
    {


        private readonly Type _type;
        public TypeOperator(Type type)
        {
            _type = type;
        }




        /// <summary>
        /// 静态加类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static TypeOperator Loader(Type type)
        {
            return new TypeOperator(type);
        }




        /// <summary>
        /// 根据索引值获取程序集中的类
        /// </summary>
        /// <param name="className">成员名</param>
        /// <returns></returns>
        public MemberInfo this[string memberName]
        {
            get { return _type.GetMember(memberName)[0]; }
        }
    }
}
