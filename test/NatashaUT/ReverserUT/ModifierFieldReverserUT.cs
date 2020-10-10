using NatashaUT.Model;
using Xunit;

namespace NatashaUT.ReverserUT
{

    [Trait("反解器", "字段修饰符")]
    public class ModifierFieldReverserUT
    {



        [Fact(DisplayName = "虚方法元数据判别")]
        public void TestMethod1()
        {

            var constA = typeof(FFieldModel).GetField("constA");
            var readonlyA = typeof(FFieldModel).GetField("readonlyA");
            Assert.False(constA.IsInitOnly);
            Assert.True(constA.IsLiteral);
            Assert.True(readonlyA.IsInitOnly);
            Assert.False(readonlyA.IsLiteral);
            
        }

    }
}
