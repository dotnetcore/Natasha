using Natasha;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NatashaUT
{

    [Trait("快速构建", "异步方法")]
    public class AsyncMethoddTest
    {
        [Fact(DisplayName = "手动强转异步委托1")]
        public static async void RunAsyncDelegate1()
        {
            var delegateAction = FastMethodOperator
                .New
                        .UseAsync()
                        .Param<string>("str1")
                        .Param<string>("str2")
                        .MethodBody(@"
                            string result = str1 +"" ""+ str2;
                            Console.WriteLine(result);
                            return result;")
                        .Return<Task<string>>()
                .Complie();

            string result =await ((Func<string, string, Task<string>>)delegateAction)?.Invoke("Hello", "World1!");
            Assert.Equal("Hello World1!", result);
        }




        [Fact(DisplayName = "手动强转异步委托2")]
        public static async void RunAsyncDelegate2()
        {
            var delegateAction = FastMethodOperator.New

                        .UseAsync()
                        .Param<string>("str1")
                        .Param<string>("str2")
                        .MethodBody(@"
                            await Task.Delay(1000);
                            string result = str1 +"" ""+ str2;
                            Console.WriteLine(result);
                            return result;")
                        .Return<Task<string>>()

                .Complie();

            string result = await ((Func<string, string, Task<string>>)delegateAction)?.Invoke("Hello", "World1!");
            Assert.Equal("Hello World1!", result);
        }




        [Fact(DisplayName = "自动泛型异步委托1")]
        public static async void RunAsyncDelegate3()
        {
            var delegateAction = FastMethodOperator.New

                .UseAsync()
                .MethodBody(@"
                            string result = arg1 +"" ""+ arg2;
                            Console.WriteLine(result);
                            return result;")

                .Complie<Func<string, string, Task<string>>>();

            string result =await delegateAction?.Invoke("Hello", "World2!");
            Assert.Equal("Hello World2!", result);
        }




        [Fact(DisplayName = "自动泛型异步委托2")]
        public static async void RunAsyncDelegate4()
        {
            var delegateAction = FastMethodOperator.New

                .UseAsync()
                .MethodBody(@"
                            await Task.Delay(100);
                            string result = arg1 +"" ""+ arg2;
                            Console.WriteLine(result);
                            return result;")

                .Complie<Func<string, string, Task<string>>>();

            string result = await delegateAction?.Invoke("Hello", "World2!");
            Assert.Equal("Hello World2!", result);
        }

    }

}
