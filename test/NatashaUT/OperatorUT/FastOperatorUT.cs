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



    }

}
