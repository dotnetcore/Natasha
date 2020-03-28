using Natasha;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NatashaUT.OperatorUT
{

    [Trait("静态委托构建与编译","")]
    public class FastOperatorUT
    {

        [Fact(DisplayName = "委托构建1")]
        public static void RunDelegate1()
        {
            var delegateAction = FastMethodOperator
                .Random()
                        .Param<string>("str1")
                        .Param<string>("str2")
                        .Body(@"
                            string result = str1 +"" ""+ str2;
                            return result;")
                        .Return<string>()
                .Compile<Func<string, string, string>>();

            string result = delegateAction?.Invoke("Hello", "World2!");
            Assert.Equal("Hello World2!", result);
        }




        [Fact(DisplayName = "委托构建2")]
        public static void RunDelegate2()
        {

            var delegateAction = FastMethodOperator.Random()
                .Body(@"return arg1 +"" ""+ arg2;")
                .Compile<Func<string, string, string>>();

            string result = delegateAction?.Invoke("Hello", "World2!");
            Assert.Equal("Hello World2!", result);

        }




        [Fact(DisplayName = "委托构建3")]
        public static void RunDelegate3()
        {

            var delegateAction = FastMethodOperator.Random()
                .Body(@"var temp = obj;")
                .Compile<Action<string>>();

            delegateAction?.Invoke("Hello");
            Assert.NotNull(delegateAction);

        }



        [Fact(DisplayName = "异步委托1")]
        public static async void RunAsyncDelegate1()
        {
            var delegateAction = FastMethodOperator.Default()

                .Async()
                .Body(@"
                            await Task.Delay(100);
                            string result = arg1 +"" ""+ arg2;
                            return result;")

                .Compile<Func<string, string, Task<string>>>();

            string result = await delegateAction?.Invoke("Hello", "World2!");
            Assert.Equal("Hello World2!", result);
        }



        [Fact(DisplayName = "异步委托2")]
        public static async void RunAsyncDelegate2()
        {
            var delegateAction = FastMethodOperator.Default()

                        .Async()
                        .Param<string>("str1")
                        .Param<string>("str2")
                        .Body(@"
                            await Task.Delay(1000);
                            string result = str1 +"" ""+ str2;
                            return result;")
                        .Return<Task<string>>()

                .Compile();

            string result = await ((Func<string, string, Task<string>>)delegateAction)?.Invoke("Hello", "World1!");
            Assert.Equal("Hello World1!", result);
        }



        [Fact(DisplayName = "异步委托")]
        public static async void RunAsyncDelegate3()
        {
            var action = NDelegate.Random().AsyncFunc<string, string, Task<string>>(@"
                            return arg1 +"" ""+ arg2;");

            string result = await action("Hello", "World1!");
            Assert.Equal("Hello World1!", result);
        }




        [Fact(DisplayName = "非安全异步委托")]
        public static async void RunAsyncDelegate4()
        {
            var action = NDelegate.Random().UnsafeAsyncFunc<string, string, Task<string>>(@"
                            return arg1 +"" ""+ arg2;");

            string result = await action("Hello", "World1!");
            Assert.Equal("Hello World1!", result);
        }



        [Fact(DisplayName = "自定义委托")]
        public static void RunDelegate5()
        {
            var action = NDelegate.Random().Delegate<TestDelegate>(@"
                            return value.Length;");
            int result = action("Hello");
            Assert.Equal(5, result);
        }

        public delegate int TestDelegate(string value);


}
     
}
