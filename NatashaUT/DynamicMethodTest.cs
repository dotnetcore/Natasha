using Natasha;
using System;
using Xunit;

namespace NatashaUT
{
    [Trait("快速构建","函数")]
    public class DynamicMethodTest
    {


        [Fact(DisplayName = "手动强转委托")]
        public static void RunDelegate1()
        {
            var delegateAction = FastMethodOperator
                .New
                        .Param<string>("str1")
                        .Param<string>("str2")
                        .MethodBody(@"
                            string result = str1 +"" ""+ str2;
                            Console.WriteLine(result);
                            return result;")
                        .Return<string>()
                .Complie();

           string result = ((Func<string, string,string>)delegateAction)("Hello", "World1!");
           Assert.Equal("Hello World1!", result);
        }



        [Fact(DisplayName = "自动泛型委托")]
        public static void RunDelegate2()
        {
            var delegateAction = FastMethodOperator
                .New
                        .Param<string>("str1")
                        .Param<string>("str2")
                        .MethodBody(@"
                            string result = str1 +"" ""+ str2;
                            Console.WriteLine(result);
                            return result;")
                        .Return<string>()
                .Complie<Func<string, string, string>>();

            string result = delegateAction("Hello", "World2!");
           Assert.Equal("Hello World2!",result);
        }



        [Fact(DisplayName = "函数克隆1")]
        public static void MakerCode1()
        {

            var builder = FakeMethodOperator.New;
            builder
                .UseMethod(typeof(OopTest).GetMethod("ReWrite1"))
                .MethodContent(@"Console.WriteLine(""hello world"");")
                .Builder();
           Assert.Equal(@"public void ReWrite1(){Console.WriteLine(""hello world"");}", builder.MethodScript);
        }



        [Fact(DisplayName = "函数克隆2")]
        public static void MakerCode2()
        {
            var builder = FakeMethodOperator.New;
            builder
                .UseMethod(typeof(OopTest).GetMethod("ReWrite2"))
                .MethodContent(@"Console.WriteLine(""hello world"");return this;")
                .Builder();
            Assert.Equal(@"public OopTest ReWrite2(){Console.WriteLine(""hello world"");return this;}", builder.MethodScript);
        }



        [Fact(DisplayName = "函数克隆3")]
        public static void MakerCode3()
        {
            var builder = FakeMethodOperator.New;
            builder
                .UseMethod(typeof(OopTest).GetMethod("ReWrite3"))
                .MethodContent(@"i++;temp+=i.ToString();")
                .Builder();
            Assert.Equal(@"public void ReWrite3(ref Int32 i,String temp){i++;temp+=i.ToString();}", builder.MethodScript);
        }
    }

    public class OopTest
    {
        public void ReWrite1()
        {

        }

        public OopTest ReWrite2()
        {
            return this;
        }

        public void ReWrite3(ref int i,string temp)
        {

        }
    }

}
