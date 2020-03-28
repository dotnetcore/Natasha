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
            NClass builder = new NClass();
            builder
                .Public()
                .Static()
                .Using<OopBuildTest>()
                .Namespace("TestNamespace")
                .DefinedName("TestExceptionUt1")
                .Body(@"public static void 1 Test(){}")
                .PublicStaticField<string>("Name")
                .PrivateStaticField<int>("_age")
                .Builder();
            var type = builder.GetType();
            Assert.Null(type);
            Assert.Equal(CompileError.Syntax, builder.Compiler.CompileException.ErrorFlag);
        }




        [Fact(DisplayName = "结构体构造-程序集异常")]
        public void Test2()
        {
            OopOperator builder = new OopOperator();
            builder
                .Public()
                .Static()
                .Using<OopBuildTest>()
                .Namespace("TestNamespace")
                .Struct()
                .DefinedName("TestExceptionUt2")
                .Body(@"public static void 1 Test(){}")
                .PublicStaticField<string>("Name")
                .PrivateStaticField<int>("_age")
                .Builder();
            var type = builder.GetType();
            
            Assert.Null(type);
            Assert.Equal(CompileError.Syntax, builder.Compiler.CompileException.ErrorFlag);
            Assert.Equal(CompileError.Syntax, builder.Compiler.SyntaxExceptions[0].ErrorFlag);
        }




        [Fact(DisplayName = "函数构造-程序集异常")]
        public void Test3()
        {
            var builder = FastMethodOperator.Default();
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
            Assert.Equal(CompileError.Syntax, builder.Compiler.CompileException.ErrorFlag);
            Assert.Equal(CompileError.Syntax, builder.Compiler.SyntaxExceptions[0].ErrorFlag);
        }

    }

}
