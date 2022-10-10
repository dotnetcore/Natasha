using System;
using Xunit;

namespace FrameworkFunctionUT.Special
{
    [Trait("高级API功能测试", "其他")]
    public class DeconstructTest : DomainPrepare
    {
        [Fact(DisplayName = "解构功能测试")]
        public void Test()
        {


            (var assembly, var log) = "public class A1{ public A1(){ Age = 1;} public int Age; }";

            Assert.NotNull(assembly);
            Assert.False(log.HasError);


            var delegateMaker = new NDelegate();
            var func = delegateMaker.Func<int>("return (new A1()).Age;");

            Assert.Equal(1, func());


        }
    }
}
