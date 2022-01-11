using System;
using Xunit;

namespace NatashaFunctionUT.Special
{
    [Trait("高级API功能测试", "其他")]
    public class DeconstructTest : DomainPrepare
    {
        [Fact(DisplayName ="解构功能测试")]
        public void Test()
        {
            using (DomainManagement.Random().CreateScope())
            {

                (var assembly, var log) = "public class A{ public A(){ Age = 1;} public int Age; }";

                Assert.NotNull(assembly);
                Assert.False(log.HasError);


                var delegateMaker = new NDelegate();
                var func = delegateMaker.Func<int>("return (new A()).Age;");

                Assert.Equal(1, func());
                Assert.NotEqual("Default", delegateMaker.AssemblyBuilder.Domain.Name);

            }
            
        }
    }
}
