using Natasha.Builder;
using System;
using System.Reflection;

namespace Natasha
{
    /// <summary>
    /// 根据现有反射方法伪造一个方法，内容自己定
    /// </summary>
    public class FakeMethod : OnceMethodBuilder<FakeMethod>
    {
        public Action<MethodTemplate> Action;
        public static FakeMethod New
        {
            get { return new FakeMethod(); }
        }
        private MethodInfo _temp_info;
        public FakeMethod()
        {
            Link = this;
            HiddenNameSpace();
            ClassAccess(AccessTypes.Public);
        }




        /// <summary>
        /// 填装反射方法
        /// </summary>
        /// <param name="reflectMethodInfo">反射方法</param>
        /// <returns></returns>
        public FakeMethod UseMethod(MethodInfo reflectMethodInfo)
        {
            _temp_info = reflectMethodInfo;
            return this;
        }




        /// <summary>
        /// 指定方法内容
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public FakeMethod MethodContent(string content)
        {
            if (!HashMethodName())
            {
                MethodName(_temp_info);
            }
            MethodAccess(_temp_info)
            .MethodModifier(_temp_info)
            .Parameter(_temp_info)
            .MethodBody(content)
            .Return(_temp_info)
            .Builder();
            return this;
        }
    }
}
