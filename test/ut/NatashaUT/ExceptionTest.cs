using Natasha.CSharp;
using Natasha.Error;
using Natasha.Error.Model;
using Xunit;

namespace NatashaUT
{

    [Trait("异常捕获","")]
    public class ClassExceptionTest : PrepareTest
    {
        /*
        [Fact(DisplayName = "类构造-程序集异常")]
        public void Test1()
        {
            try
            {
                NClass classBuilder = new NClass();
                classBuilder
                    .Public()
                    .Static()
                    .Using<ClassExceptionTest>()
                    .Namespace("TestNamespace")
                    .Name("TestExceptionUt1")
                    .BodyAppend(@"public static void 1 Test(){}")
                    .PublicStaticField<string>("Name")
                    .PrivateStaticField<int>("_age")
                    .BuilderScript();
                var type = classBuilder.GetType();
            }
            catch (CompileWrapperExceptions ex)
            {

                Assert.Equal(ExceptionKind.Syntax, ex.ErrorFlag);
            }
           
        }




        [Fact(DisplayName = "结构体构造-程序集异常")]
        public void Test2()
        {
            try
            {
                OopOperator builder = new OopOperator();
                builder
                    .Public()
                    .Static()
                    .Using<ClassExceptionTest>()
                    .Namespace("TestNamespace")
                    .Struct()
                    .Name("TestExceptionUt2")
                    .BodyAppend(@"public static void 1 Test(){}")
                    .PublicStaticField<string>("Name")
                    .PrivateStaticField<int>("_age")
                    .BuilderScript();
                var type = builder.GetType();
            }
            catch (CompileWrapperExceptions ex)
            {
                Assert.Equal(ExceptionKind.Syntax, ex.ErrorFlag);

            }
        }




        [Fact(DisplayName = "函数构造-程序集异常")]
        public void Test3()
        {

            var builder = FastMethodOperator.DefaultDomain();
            try
            {
                var delegateAction = builder
                      .Param<string>("str1")
                      .Param<string>("str2")
                      .Body(@"
                            string result = str1 +"" ""+ str2;
                            Console.WriteLine(result)1;
                            return result;")
                      .Return<string>()
              .Compile();
            }
            catch (CompileWrapperExceptions ex)
            {
                Assert.Equal(ExceptionKind.Syntax, ex.ErrorFlag);
            }
           
        }*/

    }

}
