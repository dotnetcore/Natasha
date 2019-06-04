using Natasha;
using NatashaUT.Model;
using Xunit;

namespace NatashaUT
{
    [Trait("反解测试","参数")]
    public class ReverserTest
    {

        [Fact(DisplayName = "参数与类型反解 in")]
        public void TestIn()
        {
            var info = typeof(ReverserTestModel).GetMethod("Test1");
            Assert.Equal("in Rsm<GRsm>",DeclarationReverser.GetParametersType(info.GetParameters()[0]));
        }



        [Fact(DisplayName = "参数与类型反解 out")]
        public void TestOut()
        {
            var info = typeof(ReverserTestModel).GetMethod("Test2");
            Assert.Equal("out Rsm<Rsm<GRsm>[]>", DeclarationReverser.GetParametersType(info.GetParameters()[0]));
        }



        [Fact(DisplayName = "参数与类型反解 ref")]
        public void TestRef()
        {
            var info = typeof(ReverserTestModel).GetMethod("Test3");
            Assert.Equal("ref Rsm<Rsm<GRsm>[]>[]", DeclarationReverser.GetParametersType(info.GetParameters()[0]));
        }
    }
}
