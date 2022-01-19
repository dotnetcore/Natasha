using System.Threading.Tasks;
using Xunit;

namespace NatashaFunctionUT.Template.Compile
{
    [Trait("高级API功能测试", "方法")]
    public class NDelegateTest : DomainPrepare
    {
        [Fact(DisplayName = "委托混合测试")]
        public void CNDelegate()
        {
            var nClass = NClass.RandomDomain();
            nClass
                .Public()
                .Namespace("Test")
                .PublicField<string>("Name")
                .Ctor(item => item.Public().Body("Name=\"hw!\";"));

            var type = nClass.GetType();
            Assert.NotNull(type.Name);

            var func = nClass
                .DelegateHandler
                .AsyncFunc<Task<string>>($"return (new {type.Name}()).Name;");

            var result = func().Result;
            Assert.Equal("hw!", result);

        }

        [Fact(DisplayName = "委托简单测试")]
        public void SNDelegate1()
        {

            var func = NDelegate
                .RandomDomain()
                .AsyncFunc<Task<string>>($"return \"hw!\";");

            var result = func().Result;
            Assert.Equal("hw!", result);

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
                            List<int> list = new List<int>();
                            return value.Length;");
            int result = action("Hello");
            Assert.Equal(5, result);
        }


        public delegate int TestDelegate(string value);


        [Fact(DisplayName = "非安全异步委托2")]
        public async void RunAsyncDelegate6()
        {
            var action = NDelegate.RandomDomain().UnsafeAsyncFunc<string, string, Task<string>>(@"
                            string _AppCode=""aaaa""; string arg3 = default; string b; return arg1 +"" ""+ arg2;");

            string result = await action("Hello", "World1!");
            Assert.Equal("Hello World1!", result);
        }
    }
}