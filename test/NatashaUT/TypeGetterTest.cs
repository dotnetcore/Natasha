using Natasha;
using NatashaUT.Model;
using System;
using System.Collections.Generic;
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


        [Fact(DisplayName = "获取所有类型")]
        public void TestBuilder3()
        {
            var types = new HashSet<Type>(typeof(Dictionary<String, FieldCloneNormalModel[]>[]).GetAllTypes());
            Assert.Contains(typeof(FieldCloneNormalModel), types);
            Assert.Contains(typeof(FieldCloneNormalModel[]), types);
            Assert.Contains(typeof(String), types);
            Assert.Contains(typeof(Dictionary<String, FieldCloneNormalModel[]>[]), types);
            Assert.Contains(typeof(Dictionary<String, FieldCloneNormalModel[]>), types);
        }


        
    }
}
