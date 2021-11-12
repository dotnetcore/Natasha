using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NatashaUT.ReverserUT
{
    [Trait("反解器", "可空引用")]
    public class NullableReverserUT
    {
        [Fact(DisplayName = "复杂可空引用测试")]
        public void NullableComplexTest()
        {
            foreach (var item in typeof(NullabelTestModel).GetFields())
            {
                Assert.True(item.IsContainsNullable());
            }
        }

        [Fact(DisplayName = "复杂非空引用测试")]
        public void NullableSimpleTest()
        {
            foreach (var item in typeof(NotNullabelTestModel).GetFields())
            {
                Assert.False(item.IsContainsNullable());
            }
        }

        public class NotNullabelTestModel
        {
            public string[][][] ArrayNullableField;
            public Dictionary<string, string> DictionaryNullableField;
            public Dictionary<string, string[][][]> DictionaryArrayNullableField;
            public Dictionary<string, string>[][][] ArrayDictionaryNullableField;
            public (string, string, string) TupleNullableField;
            public (string, string, string, string[][][]) TupleArrayNullableField;
            public Dictionary<string, (string, string, string, string[][][])> DictionaryTupleArrayNullableField;
            public Dictionary<string, (string, string)>[][][] ArrayDictionaryTupleNullableField;
        }

        public class NullabelTestModel
        {
            public string?[][][] ArrayNullableField;
            public Dictionary<string, string?> DictionaryNullableField;
            public Dictionary<string, string?[][][]> DictionaryArrayNullableField;
            public Dictionary<string, string?>[][][] ArrayDictionaryNullableField;
            public (string, string?, string) TupleNullableField;
            public (string, string, string, string?[][][]) TupleArrayNullableField;
            public Dictionary<string, (string, string, string, string?[][][])> DictionaryTupleArrayNullableField;
            public Dictionary<string, (string,string?)>[][][] ArrayDictionaryTupleNullableField;
        }
    }
}
