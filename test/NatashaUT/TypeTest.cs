using Natasha;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NatashaUT
{
    [Trait("类型测试", "")]
    public class TypeTest
    {
        [Fact(DisplayName = "简单类")]
        public void Test1()
        {
            
            Assert.True(typeof(Func<int>).IsSimpleType());
            Assert.True(typeof(Type).IsSimpleType());
            Assert.True(typeof(MulticastDelegate).IsSimpleType());
            Assert.True(typeof(int).IsSimpleType());
            Assert.True(typeof(string).IsSimpleType());
            Assert.True(typeof(ValueTuple).IsSimpleType());
            Assert.True(typeof(Delegate).IsSimpleType());

        }
    }
}
