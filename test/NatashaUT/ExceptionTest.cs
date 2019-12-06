using Natasha;
using Natasha.Operator;
using Xunit;

namespace NatashaUT
{

    [Trait("异常捕获","")]
    public class ClassExceptionTest
    {

        [Fact(DisplayName = "类构造-程序集异常")]
        public void Test1()
        {
            OopOperator builder = new OopOperator();
            builder
                .Public
                .Static
                .Using<OopBuildTest>()
                .Namespace("TestNamespace")
                .OopName("TestExceptionUt1")
                .OopBody(@"public static void 1 Test(){}")
                .PublicStaticField<string>("Name")
                .PrivateStaticField<int>("_age")
                .Builder();
            var type = builder.GetType();
            Assert.Null(type);
            Assert.Equal(ComplieError.Syntax, builder.Complier.ComplieException.ErrorFlag);
        }




        [Fact(DisplayName = "结构体构造-程序集异常")]
        public void Test2()
        {
            OopOperator builder = new OopOperator();
            builder
                .Public
                .Static
                .Using<OopBuildTest>()
                .Namespace("TestNamespace")
                .ChangeToStruct()
                .OopName("TestExceptionUt2")
                .OopBody(@"public static void 1 Test(){}")
                .PublicStaticField<string>("Name")
                .PrivateStaticField<int>("_age")
                .Builder();
            var type = builder.GetType();
            Assert.Equal(OopType.Struct ,builder.OopTypeEnum);
            Assert.Null(type);
            Assert.Equal(ComplieError.Syntax, builder.Complier.ComplieException.ErrorFlag);
            Assert.Equal(ComplieError.Syntax, builder.Complier.SyntaxExceptions[0].ErrorFlag);
        }




        [Fact(DisplayName = "函数构造-程序集异常")]
        public void Test3()
        {
            var builder = FastMethodOperator.Create();
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
            Assert.Equal(ComplieError.Syntax, builder.Complier.ComplieException.ErrorFlag);
            Assert.Equal(ComplieError.Syntax, builder.Complier.SyntaxExceptions[0].ErrorFlag);
        }

    }

}
