using Natasha.CSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NatashaUT
{
    [Trait("错误测试","")]
    public class CS0433Test:PrepareTest
    {
        [Fact(DisplayName ="CS0433 Error")]
        public void Test()
        {
            var func = NDelegate
                .RandomDomain()
                .Func<int>("return (new ConcurrentDictionary<string,string>()).Count;");
            Assert.Equal(0, func());
        }
    }
}
