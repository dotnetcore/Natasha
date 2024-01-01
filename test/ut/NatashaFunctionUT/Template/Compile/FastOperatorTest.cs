using Natasha.CSharp;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NatashaFunctionUT.Template.Compile
{

    [Trait("高级API功能测试", "模板")]
    public class FastOperatorTest : CompilerPrepareBase
    {

        [Fact(DisplayName = "委托构建1")]
        public void RunDelegate1()
        {
            var delegateAction = FastMethodOperator
                        .RandomDomain()
                        .ConfigBuilder(opt => opt.UseSmartMode())
                        .Param<string>("str1")
                        .Param<string>("str2")
                        .Body(@"
                            string result = str1 +"" ""+ str2;
                            return result;")
                        .Return<string>()
                .Compile<Func<string, string, string>>();

            string result = delegateAction.Invoke("Hello", "World2!");
            Assert.Equal("Hello World2!", result);
        }




        [Fact(DisplayName = "委托构建2")]
        public void RunDelegate2()
        {

            var delegateAction = FastMethodOperator.RandomDomain().ConfigBuilder(opt => opt.UseSmartMode())
                .Body(@"return arg1 +"" ""+ arg2;")
                .Compile<Func<string, string, string>>();

            string result = delegateAction.Invoke("Hello", "World2!");
            Assert.Equal("Hello World2!", result);

        }




        [Fact(DisplayName = "委托构建3")]
        public void RunDelegate3()
        {

            var delegateAction = FastMethodOperator.RandomDomain().ConfigBuilder(opt => opt.UseSmartMode())
                .Body(@"var temp = obj;")
                .Compile<Action<string>>();

            delegateAction?.Invoke("Hello");
            Assert.NotNull(delegateAction);

        }



        [Fact(DisplayName = "异步委托1")]
        public async void RunAsyncDelegate1()
        {
            var delegateAction = FastMethodOperator.DefaultDomain().ConfigBuilder(opt => opt.UseSmartMode())

                .Async()
                .Body(@"
                            await Task.Delay(100);
                            string result = arg1 +"" ""+ arg2;
                            return result;")

                .Compile<Func<string, string, Task<string>>>();

            string result = await delegateAction.Invoke("Hello", "World2!");
            Assert.Equal("Hello World2!", result);
        }



        [Fact(DisplayName = "异步委托2")]
        public async void RunAsyncDelegate2()
        {
            var delegateAction = FastMethodOperator.DefaultDomain().ConfigBuilder(opt => opt.UseSmartMode())

                        .Async()
                        .Param<string>("str1")
                        .Param<string>("str2")
                        .Body(@"
                            await Task.Delay(1000);
                            string result = str1 +"" ""+ str2;
                            return result;")
                        .Return<Task<string>>()

                .Compile();

            string result = await ((Func<string, string, Task<string>>)delegateAction).Invoke("Hello", "World1!");
            Assert.Equal("Hello World1!", result);
        }



    }

}
