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
    [Trait("Share库引用测试","")]
    public class CS0433Test:PrepareTest
    {
        [Fact(DisplayName = "CS0433 ConcurrentDictionary")]
        public void Test()
        {
            var func = NDelegate
                .RandomDomain(item=>item.UseShareLibraries = true)
                .Func<int>("return (new ConcurrentDictionary<string,string>()).Count;");
            Assert.Equal(0, func());
            func = NDelegate
                .RandomDomain()
                .Func<int>("return (new ConcurrentDictionary<string,string>()).Count;");
            Assert.Equal(0, func());
        }

        [Fact(DisplayName = "CS0433 ConcurrentQueue")]
        public void Test1()
        {
            var func = NDelegate
                .RandomDomain(item => item.UseShareLibraries = true)
                .Func<int>("return (new ConcurrentQueue<string>()).Count;");
            Assert.Equal(0, func());
            func = NDelegate
                .RandomDomain()
                .Func<int>("return (new ConcurrentQueue<string>()).Count;");
            Assert.Equal(0, func());
        }

        [Fact(DisplayName = "CS0433 Dictionary")]
        public void Test2()
        {
            var func = NDelegate
                .RandomDomain(item => item.UseShareLibraries = true)
                .Func<int>("return (new Dictionary<string,string>()).Count;");
            Assert.Equal(0, func());
            func = NDelegate
                .RandomDomain()
                .Func<int>("return (new Dictionary<string,string>()).Count;");
            Assert.Equal(0, func());
        }

        [Fact(DisplayName = "CS0433 List")]
        public void Test3()
        {
            var func = NDelegate
                .RandomDomain(item => item.UseShareLibraries = true)
                .Func<int>("return (new List<string>()).Count;");
            Assert.Equal(0, func());
            func = NDelegate
                .RandomDomain()
                .Func<int>("return (new List<string>()).Count;");
            Assert.Equal(0, func());
        }



        //[Fact(DisplayName = "CS1705 未知错误")]
        //public void TestUnknow()
        //{

        //    var getMembers = NDelegate.RandomDomain().Func<Type, object>($@"
        //    var type = typeof(Int32);
        //    return  (
        //    from val in type.GetFields()
        //    where val.FieldType == arg
        //    select val).ToArray();");
        //    Assert.NotNull(getMembers);

        //}
    }
}
