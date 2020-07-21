using Natasha;
using Natasha.CSharp;
using NatashaUT.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;

namespace NatashaUT
{
    [Trait("新科技测试", "私有调用")]
    public class PrivateTest : PrepareTest
    {

        [Fact(DisplayName = "私有成员调用")]
        public void Test()
        {
            TPropertyClass test = new TPropertyClass();
            var action = NDelegate
                .RandomDomain()
                .SetClass(item=>item.AllowPrivate<TPropertyClass>())
                .Action<TPropertyClass>("obj.publicA = 2; obj.privateA=1;");
            action(test);
            Assert.Equal(1, test.GetD);
        }

        [Fact(DisplayName = "私有成员调用2")]
        public void Test1()
        {

            var action = NDelegate
                .RandomDomain(item => item.UseShareLibraries = true)
                .SetClass(item => item.AllowPrivate<List<int>>())
                .Func<int>("return (new List<int>())._size;");
            Assert.Equal(0,action());

        }


        [Fact(DisplayName = "只读成员调用2")]
        public void Test9()
        {

            var action = NDelegate
                .RandomDomain(item => item.UseShareLibraries = true)
                .SetClass(item => item.AllowPrivate<List<int>>())
                .Func<int>("return (new List<int>())._size;");
            Assert.Equal(0, action());

        }

    }
}
