using Natasha.CSharp.Builder;
using Natasha.CSharp.Template;
using Natasha.CSharp.Template.Reverser;
using System.Reflection;

namespace Natasha.CSharp
{
    /// <summary>
    /// 根据现有反射方法伪造一个方法，内容自己定
    /// </summary>
    public sealed class FakeMethodOperator : MethodBuilder<FakeMethodOperator>
    {
        private static readonly MethodInfo _init_method;
        static FakeMethodOperator()
        {
            _init_method = typeof(FakeMethodOperator).GetMethod("Init")!;
        }


        private MethodInfo _method_info;
        public FakeMethodOperator()
        {
            _method_info = _init_method;
            Link = this;

        }



        public override void Init()
        {

            ClassOptions(item => item
            .Modifier(ModifierFlags.Static)
            .Class()
            .UseRandomName()
            .HiddenNamespace()
            .Access(AccessFlags.Public)
            );

        }







        /// <summary>
        /// 填装反射方法
        /// </summary>
        /// <param name="reflectMethodInfo">反射方法</param>
        /// <returns></returns>
        public FakeMethodOperator UseMethod(MethodInfo reflectMethodInfo)
        {

            _method_info = reflectMethodInfo;
            return this;

        }
        public FakeMethodOperator UseMethod<T>(string methodName)
        {

            _method_info = typeof(T).GetMethod(methodName)!;
            return this;

        }



        /// <summary>
        /// 指定方法内容
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private FakeMethodOperator MethodCopy()
        {

            if (NameScript == string.Empty)
            {

                Name(_method_info);

             }

            return Access(AccessFlags.Public)
                .Param(_method_info)
            .Return(_method_info);

        }


        public FakeMethodOperator MethodBody(string body)
        {

            MethodCopy();
            ModifierAppend(AsyncReverser.GetAsync(_method_info));
            return Body(body);
            
        }


        /// <summary>
        /// 指定方法内容
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public FakeMethodOperator StaticMethodBody(string body)
        {

            MethodCopy();
            Modifier(ModifierFlags.Static);
            ModifierAppend(AsyncReverser.GetAsync(_method_info));
            return Body(body);

        }

    }

}
