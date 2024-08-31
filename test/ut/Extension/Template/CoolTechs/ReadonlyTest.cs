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
    [Trait("新科技测试", "只读调用")]
    public class ReadonlyTest : PrepareTest
    {


        [Fact(DisplayName = "私有只读成员动态赋值")]
        public void Test()
        {

            var action = NDelegate
                .RandomDomain()
                .SetClass(item => item.AllowPrivate<ReadonlyModel>())
                .Action<ReadonlyModel>($"{"obj.@interface".ReadonlyScript()} = new DefaultReadolyInterface();");

            ReadonlyModel model = new ReadonlyModel();
            action(model);
            
            Assert.NotNull(model.GetInterface());

        }

    }
}
