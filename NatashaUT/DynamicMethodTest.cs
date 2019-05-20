using Natasha;
using System;
using Xunit;

namespace NatashaUT
{
    public class DynamicMethodTest
    {
        [Fact(DisplayName = "手动强转委托")]
        public static void RunDelegate1()
        {
            var delegateAction = MethodBuilder.NewMethod
                .Using(typeof(Console))
                .Param<string>("str1")
                .Param<string>("str2")
                .Body(@"
                    string result = str1 +"" ""+ str2;
                    Console.WriteLine(result);
                    return result;
                                               ")
                .Return<string>()
                .Create();

           string result = ((Func<string, string,string>)delegateAction)("Hello", "World1!");
           Assert.Equal("Hello World1!", result);
        }

        [Fact(DisplayName = "自动泛型委托")]
        public static void RunDelegate2()
        {
            var delegateAction2 = MethodBuilder.NewMethod
                .Using(typeof(Console))
                .Param<string>("str1")
                .Param<string>("str2")
                .Body(@"
                    string result = str1 +"" ""+ str2;
                    Console.WriteLine(result);
                    return result;
                                               ")
                .Return<string>()
                .Create<Func<string, string, string>>();

           string result = delegateAction2("Hello", "World2!");
           Assert.Equal("Hello World2!",result);
        }

        [Fact(DisplayName = "函数克隆1")]
        public static void MakerCode1()
        {
            var result = MethodBuilder.NewMethod.From(typeof(OopTest).GetMethod("ReWrite1")).Body(@"Console.WriteLine(""hello world"");").GetMethodString(false);
           Assert.Equal(@"public void ReWrite1(){Console.WriteLine(""hello world"");}", result);
        }

        [Fact(DisplayName = "函数克隆2")]
        public static void MakerCode2()
        {
            var result = MethodBuilder.NewMethod.From(typeof(OopTest).GetMethod("ReWrite2")).Body(@"Console.WriteLine(""hello world"");return this;").GetMethodString(false);
            Assert.Equal(@"public OopTest ReWrite2(){Console.WriteLine(""hello world"");return this;}", result);
        }

        [Fact(DisplayName = "函数克隆3")]
        public static void MakerCode3()
        {
            var result = MethodBuilder.NewMethod.From(typeof(OopTest).GetMethod("ReWrite3")).Body(@"i++;temp+=i.ToString();").GetMethodString(false);
            Assert.Equal(@"public void ReWrite3(ref Int32 i,String temp){i++;temp+=i.ToString();}", result);
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
