using Natasha;
using Natasha.Operator;
using System;
using System.Collections.Generic;
using System.Text;
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
           var method =new MethodOperator<OopTest>();
           var result = method["ReWrite1"].SetMethod(@"Console.WriteLine(""hello world"");");
           Assert.Equal(@"public void ReWrite1(){Console.WriteLine(""hello world"");}", result);
        }

        [Fact(DisplayName = "函数克隆2")]
        public static void MakerCode2()
        {
            var method = new MethodOperator<OopTest>();
            var result = method["ReWrite2"].SetMethod(@"Console.WriteLine(""hello world"");return this;");
            Assert.Equal(@"public OopTest ReWrite2(){Console.WriteLine(""hello world"");return this;}", result);
        }

        [Fact(DisplayName = "函数克隆3")]
        public static void MakerCode3()
        {
            var method = new MethodOperator<OopTest>();
            var result = method["ReWrite3"].SetMethod(@"i++;temp+=i.ToString();");
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
