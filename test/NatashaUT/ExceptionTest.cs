using Natasha;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NatashaUT
{

    [Trait("异常捕获","")]
    public class ClassExceptionTest
    {

        [Fact(DisplayName = "类构造-程序集异常")]
        public void Test1()
        {
            ClassBuilder builder = new ClassBuilder();
            var script = builder
                .Using<DynamicBuilderTest>()
                .Namespace("TestNamespace")
                .ClassAccess(AccessTypes.Public)
                .ClassModifier(Modifiers.Static)
                .ClassName("TestExceptionUt1")
                .ClassBody(@"public static void 1 Test(){}")
                .PublicStaticField<string>("Name")
                .PrivateStaticField<int>("_age")
                .Builder()
                .Script;
            var type = builder.GetType();
            Assert.Null(type);
            Assert.Equal(ComplieError.Assembly, builder.Complier.Exception.ErrorFlag);
        }




        [Fact(DisplayName = "结构体构造-程序集异常")]
        public void Test2()
        {
            ClassBuilder builder = new ClassBuilder();
            var script = builder
                .Using<DynamicBuilderTest>()
                .Namespace("TestNamespace")
                .ChangeToStruct()
                .ClassAccess(AccessTypes.Public)
                .ClassModifier(Modifiers.Static)
                .ClassName("TestExceptionUt2")
                .ClassBody(@"public static void 1 Test(){}")
                .PublicStaticField<string>("Name")
                .PrivateStaticField<int>("_age")
                .Builder()
                .Script;
            var type = builder.GetType();
            Assert.True(builder.IsStruct);
            Assert.Null(type);
            Assert.Equal(ComplieError.Assembly, builder.Complier.Exception.ErrorFlag);
        }




        [Fact(DisplayName = "函数构造-程序集异常")]
        public void Test3()
        {
            var builder = FastMethodOperator.New;
            var delegateAction = builder
                       .Param<string>("str1")
                       .Param<string>("str2")
                       .MethodBody(@"
                            string result = str1 +"" ""+ str2;
                            Console.WriteLine(result)1;
                            return result;")
                       .Return<string>()
               .Complie();

            Assert.Null(delegateAction);
            Assert.Equal(ComplieError.Assembly, builder.Complier.Exception.ErrorFlag);
        }

    }

}
