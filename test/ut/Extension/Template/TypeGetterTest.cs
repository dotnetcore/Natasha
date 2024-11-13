using NatashaUT.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace NatashaUT
{
    [Trait("类型获取", "")]
    public class TypeGetterTest
    {


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
