using Natasha.CSharp;
using Natasha.Error.Model;
using Xunit;

namespace NatashaUT
{

    [Trait("异常捕获","")]
    public class ClassExceptionTest : PrepareTest
    {

        [Fact(DisplayName = "类构造-程序集异常")]
        public void Test1()
        {
            NClass classBuilder = new NClass();
            classBuilder.AssemblyBuilder.SyntaxErrorBehavior = ExceptionBehavior.None;
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
            Assert.Null(type);
            Assert.Equal(ExceptionKind.Syntax, classBuilder.Exception.ErrorFlag);
        }




        [Fact(DisplayName = "结构体构造-程序集异常")]
        public void Test2()
        {
            OopOperator builder = new OopOperator();
            builder.AssemblyBuilder.SyntaxErrorBehavior = ExceptionBehavior.None;
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
            
            Assert.Null(type);
            Assert.Equal(ExceptionKind.Syntax, builder.Exception.ErrorFlag);

        }




        [Fact(DisplayName = "函数构造-程序集异常")]
        public void Test3()
        {

            var builder = FastMethodOperator.DefaultDomain();
            builder.AssemblyBuilder.SyntaxErrorBehavior = ExceptionBehavior.None;
            var delegateAction = builder
                       .Param<string>("str1")
                       .Param<string>("str2")
                       .Body(@"
                            string result = str1 +"" ""+ str2;
                            Console.WriteLine(result)1;
                            return result;")
                       .Return<string>()
               .Compile();

            Assert.Null(delegateAction);
            Assert.Equal(ExceptionKind.Syntax, builder.Exception.ErrorFlag);
            
        }

    }

}
