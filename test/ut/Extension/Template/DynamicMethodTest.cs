using Natasha.CSharp;
using NatashaUT.Model;
using System;
using Xunit;

namespace NatashaUT
{
    [Trait("快速构建","函数")]
    public class DynamicMethodTest : PrepareTest
    { 

        [Fact(DisplayName = "手动强转委托")]
        public void RunDelegate1()
        {
            Delegate delegateAction = FastMethodOperator
                .DefaultDomain()
                        .Param<string>("str1")
                        .Param<string>("str2")
                        .Body(@"
                            string result = str1 +"" ""+ str2;
                            Console.WriteLine(result);
                            return result;")
                        .Return<string>()
                .Compile();

           string result = ((Func<string, string, string>)delegateAction)("Hello", "World1!");
           Assert.Equal("Hello World1!", result);
        }



        [Fact(DisplayName = "内部类委托")]
        public static void RunInnerDelegate()
        {
            Delegate delegateAction = FastMethodOperator
                .DefaultDomain()
                        .Body(@"
                           OopTestModel.InnerClass a = new OopTestModel.InnerClass();
                            a.Name =""abc"";
                            return a;")
                        .Return<OopTestModel.InnerClass>()
                .Compile();
            var action = (Func<OopTestModel.InnerClass>)delegateAction;
            var result = action();
            Assert.Equal("abc", result.Name);
        }



        [Fact(DisplayName = "NFunc委托")]
        public void RunDelegate5()
        {

            NSucceedLog.Enabled = true;
            Func<string, string, string> action = NDelegate.RandomDomain(builder=>builder.UseNatashaFileOut()).UnsafeFunc<string, string, string>(@"
                            string result = arg1 +"" ""+ arg2;
                            Console.WriteLine(result);
                            return result;");

            Func<string, string, string> action2 = NDelegate.RandomDomain(builder => builder.UseNatashaFileOut()).UnsafeFunc<string, string, string>(@"
                            string result = arg1 + "" "" + arg2 + ""1"";
                            Console.WriteLine(result);
                            return result;");

            Func<string, string, string> action3 = (s1, s2) => s1 + s2;


            string result = action("Hello", "World1!");
            Assert.Equal("Hello World1!", result);


            Assert.NotEqual(action3.GetHashCode(), action2.GetHashCode());
            Assert.NotEqual(action.Method.GetHashCode(), action2.Method.GetHashCode());
            Assert.Equal(action.GetHashCode(), action2.GetHashCode());

        }




        [Fact(DisplayName = "自动泛型委托")]
        public void RunDelegate2()
        {
            Func<string, string, string> delegateAction = FastMethodOperator
                .DefaultDomain()
                        .Param<string>("str1")
                        .Param<string>("str2")
                        .Body(@"
                            string result = str1 +"" ""+ str2;
                            Console.WriteLine(result);
                            return result;")
                        .Return<string>()
                .Compile<Func<string, string, string>>();

           string result = delegateAction("Hello", "World2!");
           Assert.Equal("Hello World2!",result);
        }

    }

}
