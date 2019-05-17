using Natasha;
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
            ScriptBuilder maker = new ScriptBuilder();

            var delegateAction = maker
                .Namespace(typeof(Console))
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
        [Fact(DisplayName = "手动强转委托")]
        public static void RunDelegate2()
        {

            ScriptBuilder maker2 = new ScriptBuilder();
            var delegateAction2 = maker2
                .Namespace(typeof(Console))
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

    }
}
