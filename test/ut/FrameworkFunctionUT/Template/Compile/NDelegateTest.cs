using System;
using System.Threading.Tasks;
using Xunit;

namespace FrameworkFunctionUT.Template.Compile
{
    [Trait("高级API功能测试", "方法")]
    public class NDelegateTest : DomainPrepare
    {
        [Fact(DisplayName = "委托混合测试")]
        public void CNDelegate()
        {
            var nClass = NClass.DefaultDomain();
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
        public void SNDelegate()
        {

            var func = NDelegate
                .DefaultDomain()
                .AsyncFunc<Task<string>>($"return \"hw!\";");

            var result = func().Result;
            Assert.Equal("hw!", result);

        }


        [Fact(DisplayName = "异步委托")]
        public async void RunAsyncDelegate()
        {
            var action = NDelegate.DefaultDomain().AsyncFunc<string, string, Task<string>>(@"
                            return arg1 +"" ""+ arg2;");

            string result = await action("Hello", "World1!");
            Assert.Equal("Hello World1!", result);
        }


        [Fact(DisplayName = "dynamic 异步委托1")]
        public async void RunDynamicAsyncDelegate1()
        {
            var action = NDelegate.DefaultDomain().AsyncFunc<string, dynamic, Task<string>>(@"
                            return arg1 +"" ""+ arg2;");

            string result = await action("Hello", "World1!");
            Assert.Equal("Hello World1!", result);
        }

        //[Fact(DisplayName = "dynamic 异步委托2")]
        //public async void RunDynamicAsyncDelegate2()
        //{
        //    var domain = DomainManagement.Random();
        //    var type = NClass
        //        .UseDomain(domain)
        //        .Public()
        //        .Ctor(item=>item.Public().Body("S1=\"Hello \";S2 = \"World!\";"))
        //        .PublicField<string>("S1")
        //        .PublicField<string>("S2")
        //        .GetType();
        //    var obj = Activator.CreateInstance(type);
        //    var action = NDelegate.UseDomain(domain).AsyncFunc<dynamic, Task<string>>(@"
        //                    return arg.S1 + arg.S2;");

        //    string result = await action(obj!);
        //    Assert.Equal("Hello World1!", result);
        //}



        [Fact(DisplayName = "非安全异步委托")]
        public async void RunUnsafeAsyncDelegate()
        {
            var action = NDelegate.DefaultDomain().ConfigMethod(item=>item.Summary("zhushi")).UnsafeAsyncFunc<string, string, Task<string>>(@"
                            return arg1 +"" ""+ arg2;");

            string result = await action("Hello", "World1!");
            Assert.Equal("Hello World1!", result);
        }





        [Fact(DisplayName = "自定义委托")]
        public void RunDelegate5()
        {
            var action = NDelegate.DefaultDomain().Delegate<TestDelegate>(@"
                            List<int> list = new List<int>();
                            return value.Length;");
            int result = action("Hello");
            Assert.Equal(5, result);
        }


        public delegate int TestDelegate(string value);


        [Fact(DisplayName = "非安全异步委托2")]
        public async void RunAsyncDelegate6()
        {
            var action = NDelegate.DefaultDomain().UnsafeAsyncFunc<string, string, Task<string>>(@"
                            string _AppCode=""aaaa""; string arg3 = default; string b; return arg1 +"" ""+ arg2;");

            string result = await action("Hello", "World1!");
            Assert.Equal("Hello World1!", result);
        }
    }
}