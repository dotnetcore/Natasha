using Natasha.CSharp.Reverser;
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
        [Fact(DisplayName = "复杂可空引用判别测试")]
        public void NullableTest()
        {
            foreach (var item in typeof(NullabelTestModel).GetFields())
            {
                Assert.True(item.IsContainsNullable());
            }
        }

        [Fact(DisplayName = "复杂非空引用判别测试")]
        public void NotNullableTest()
        {
            foreach (var item in typeof(NotNullabelTestModel).GetFields())
            {
                Assert.False(item.IsContainsNullable());
            }
        }

        [Fact(DisplayName = "复杂可空引用反解测试")]
        public void NullableStringTest()
        {
            var f1 = typeof(NullabelTestModel).GetField("DictionaryTupleArrayNullableField");
            Assert.Equal("System.Collections.Generic.Dictionary<System.String,System.ValueTuple<System.String,System.String,System.String,System.String?[][][]>>"
                , NullableMemberReverser.GetMemberNullableDevelopName(f1));

            var f2 = typeof(NullabelTestModel).GetField("ArrayDictionaryTupleNullableField");
            Assert.Equal("System.Collections.Generic.Dictionary<System.String,System.ValueTuple<System.String,System.String?>>[][][]"
                , NullableMemberReverser.GetMemberNullableDevelopName(f2));
        }

        [Fact(DisplayName = "复杂非空引用反解测试")]
        public void NotNullableStringTest()
        {
            var f1 = typeof(NotNullabelTestModel).GetField("DictionaryTupleArrayNullableField");
            Assert.Equal("System.Collections.Generic.Dictionary<System.String,System.ValueTuple<System.String,System.String,System.String,System.String[][][]>>"
                , NullableMemberReverser.GetMemberNullableDevelopName(f1));

            var f2 = typeof(NotNullabelTestModel).GetField("ArrayDictionaryTupleNullableField");
            Assert.Equal("System.Collections.Generic.Dictionary<System.String,System.ValueTuple<System.String,System.String>>[][][]"
                , NullableMemberReverser.GetMemberNullableDevelopName(f2));
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
