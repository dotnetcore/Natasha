using Natasha.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
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


        [Fact(DisplayName = "CS0433 Error1")]
        public void Test2()
        {

            var getMembers = NDelegate.RandomDomain().Func<Type, object>($@"
            var type = typeof(Int32);
            return  (
            from val in type.GetFields()
            where val.FieldType == arg
            select val).ToArray();", "System.Linq");
            Assert.NotNull(getMembers);

        }
    }
}
