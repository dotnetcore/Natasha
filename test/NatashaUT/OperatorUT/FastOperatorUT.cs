using Natasha.CSharp;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NatashaUT.OperatorUT
{

    [Trait("静态委托构建与编译", "")]
    public class FastOperatorUT : PrepareTest
    {

        [Fact(DisplayName = "委托构建1")]
        public void RunDelegate1()
        {
            var delegateAction = FastMethodOperator
                .RandomDomain()
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
        public void RunDelegate2()
        {

            var delegateAction = FastMethodOperator.RandomDomain()
                .Body(@"return arg1 +"" ""+ arg2;")
                .Compile<Func<string, string, string>>();

            string result = delegateAction?.Invoke("Hello", "World2!");
            Assert.Equal("Hello World2!", result);

        }




        [Fact(DisplayName = "委托构建3")]
        public void RunDelegate3()
        {

            var delegateAction = FastMethodOperator.RandomDomain()
                .Body(@"var temp = obj;")
                .Compile<Action<string>>();

            delegateAction?.Invoke("Hello");
            Assert.NotNull(delegateAction);

        }



        [Fact(DisplayName = "异步委托1")]
        public async void RunAsyncDelegate1()
        {
            var delegateAction = FastMethodOperator.DefaultDomain()

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
        public async void RunAsyncDelegate2()
        {
            var delegateAction = FastMethodOperator.DefaultDomain()

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
        public async void RunAsyncDelegate3()
        {
            var action = NDelegate.RandomDomain().AsyncFunc<string, string, Task<string>>(@"
                            return arg1 +"" ""+ arg2;");

            string result = await action("Hello", "World1!");
            Assert.Equal("Hello World1!", result);
        }




        [Fact(DisplayName = "非安全异步委托")]
        public async void RunAsyncDelegate4()
        {
            var action = NDelegate.RandomDomain().UnsafeAsyncFunc<string, string, Task<string>>(@"
                            return arg1 +"" ""+ arg2;");

            string result = await action("Hello", "World1!");
            Assert.Equal("Hello World1!", result);
        }



        [Fact(DisplayName = "自定义委托")]
        public void RunDelegate5()
        {
            var action = NDelegate.RandomDomain().Delegate<TestDelegate>(@"
                            return value.Length;");
            int result = action("Hello");
            Assert.Equal(5, result);
        }

        public delegate int TestDelegate(string value);


    }

}
