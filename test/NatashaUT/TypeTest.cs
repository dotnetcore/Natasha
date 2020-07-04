using Natasha;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NatashaUT
{
    [Trait("类型测试", "")]
    public class TypeTest : PrepareTest
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

        [Fact(DisplayName = "继承类")]
        public void Test2()
        {

            Assert.True(typeof(object).IsAssignableFrom(typeof(List<>)));
            Assert.False(typeof(IList<>).IsAssignableFrom(typeof(List<>)));

        }
    }
}
