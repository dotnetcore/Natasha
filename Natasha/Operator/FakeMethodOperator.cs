using Natasha.Builder;
using System;
using System.Reflection;

namespace Natasha
{
    /// <summary>
    /// 根据现有反射方法伪造一个方法，内容自己定
    /// </summary>
    public class FakeMethodOperator : OnceMethodBuilder<FakeMethodOperator>
    {


        public Action<MethodTemplate> Action;
        private MethodInfo _temp_info;
        public static FakeMethodOperator New
        {
            get { return new FakeMethodOperator(); }
        }
      

        public FakeMethodOperator()
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
        public FakeMethodOperator UseMethod(MethodInfo reflectMethodInfo)
        {
            _temp_info = reflectMethodInfo;
            return this;
        }
        public FakeMethodOperator UseMethod<T>(string methodName)
        {
            _temp_info = typeof(T).GetMethod(methodName);
            return this;
        }



        /// <summary>
        /// 指定方法内容
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public FakeMethodOperator MethodContent(string content)
        {
            if (!HashMethodName())
            {
                MethodName(_temp_info);
            }
            if (OnceModifierScript==string.Empty)
            {
                MethodModifier(_temp_info);
            }
            MethodAccess(_temp_info)
            .Parameter(_temp_info)
            .MethodBody(content)
            .Return(_temp_info);
            return this;
        }



        /// <summary>
        /// 指定方法内容
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public FakeMethodOperator StaticMethodContent(string content)
        {
            if (!HashMethodName())
            {
                MethodName(_temp_info);
            }
            ClassModifier(Modifiers.Static)
            .MethodAccess(_temp_info)
            .MethodModifier(Modifiers.Static)
            .Parameter(_temp_info)
            .MethodBody(content)
            .Return(_temp_info);
            return this;
        }
    }
}
