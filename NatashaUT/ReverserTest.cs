using Natasha.Engine.Builder.Reverser;
using NatashaUT.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NatashaUT
{
    public class ReverserTest
    {
        [Fact(DisplayName = "参数与类型反解 in")]
        public void TestIn()
        {
            var info = typeof(ReverserTestModel).GetMethod("Test1");
            Assert.Equal("in Rsm<GRsm>",DeclarationReverser.GetDeclarationType(info.GetParameters()[0]));
        }
        [Fact(DisplayName = "参数与类型反解 out")]
        public void TestOut()
        {
            var info = typeof(ReverserTestModel).GetMethod("Test2");
            Assert.Equal("out Rsm<Rsm<GRsm>[]>", DeclarationReverser.GetDeclarationType(info.GetParameters()[0]));
        }
        [Fact(DisplayName = "参数与类型反解 ref")]
        public void TestRef()
        {
            var info = typeof(ReverserTestModel).GetMethod("Test3");
            Assert.Equal("ref Rsm<Rsm<GRsm>[]>[]", DeclarationReverser.GetDeclarationType(info.GetParameters()[0]));
        }
    }
}
