using Natasha;
using NatashaUT.Model;
using System;
using Xunit;

namespace NatashaUT
{
    [Trait("类型获取", "")]
    public class TypeGetterTest
    {
        [Fact(DisplayName = "获取类型1")]
        public void TestBuilder2()
        {
            var result = NewMethod.Create<Func<Type>>(builder => builder
                .Using("NatashaUT.Model")
                .MethodBody($@"return typeof(FieldSelfLinkModel);")
             );

            Assert.NotNull(result.Method());
            Assert.Equal(typeof(FieldSelfLinkModel),result.Method());
        }
    }
}
