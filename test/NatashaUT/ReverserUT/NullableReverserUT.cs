﻿using Natasha.CSharp.Reverser;
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


        [Fact(DisplayName = "自定义方法参数可空类型反解测试")]
        public void ParameterInfoNullableTest()
        {
            foreach (var item in typeof(NullabelTestModel).GetMethods())
            {
                if (item.Name=="Show1")
                {
                    var parameters = item.GetParameters().OrderBy(item => item.Position).ToArray();
                    Assert.Equal("NatashaUT.ReverserUT.NullableReverserUT.NullabelTestModel?", parameters[0].GetMemberNullableDevelopName());
                    Assert.Equal("System.Nullable<System.Int32>", parameters[1].GetMemberNullableDevelopName());
                    Assert.Equal("System.String?", parameters[2].GetMemberNullableDevelopName());
                }
                else if (item.Name=="Show2")
                {
                    var parameters = item.GetParameters().OrderBy(item => item.Position).ToArray();
                    Assert.Equal("NatashaUT.ReverserUT.NullableReverserUT.NullabelTestModel", parameters[0].GetMemberNullableDevelopName());
                    Assert.Equal("System.Nullable<System.Int32>", parameters[1].GetMemberNullableDevelopName());
                    Assert.Equal("System.String?", parameters[2].GetMemberNullableDevelopName());
                }
                else if (item.Name == "Show3")
                {
                    var parameters = item.GetParameters().OrderBy(item => item.Position).ToArray();
                    Assert.Equal("NatashaUT.ReverserUT.NullableReverserUT.NullabelTestModel", parameters[0].GetMemberNullableDevelopName());
                    Assert.Equal("System.Int32", parameters[1].GetMemberNullableDevelopName());
                    Assert.Equal("System.String?", parameters[2].GetMemberNullableDevelopName());
                }

            }
        }

        [Fact(DisplayName = "系统委托参数可空类型反解测试1")]
        public void SystemParameterInfoNullableTest1()
        {
            var m1 = typeof(Action<NullabelTestModel>).GetMethod("Invoke");
            var parameters = m1.GetParameters().OrderBy(item => item.Position).ToArray();
            Assert.Equal("NatashaUT.ReverserUT.NullableReverserUT.NullabelTestModel?", parameters[0].GetMemberNullableDevelopName());
           
        }

        [Fact(DisplayName = "系统委托参数可空类型反解测试2")]
        public void SystemParameterInfoNullableTest2()
        {
            var m1 = typeof(Action<NullabelTestModel, NullabelTestModel?>).GetMethod("Invoke");
            var parameters = m1.GetParameters().OrderBy(item => item.Position).ToArray();
            Assert.False(parameters[0].IsContainsNullable());
            Assert.True(parameters[1].IsContainsNullable());
            Assert.Equal("NatashaUT.ReverserUT.NullableReverserUT.NullabelTestModel", parameters[0].GetMemberNullableDevelopName());
            Assert.Equal("NatashaUT.ReverserUT.NullableReverserUT.NullabelTestModel?", parameters[1].GetMemberNullableDevelopName());
            

        }

        [Fact(DisplayName = "系统委托参数可空类型反解测试3")]
        public void SystemParameterInfoNullableTest3()
        {
            var m1 = typeof(Action<NullabelTestModel, NullabelTestModel?,int?>).GetMethod("Invoke");
            var parameters = m1.GetParameters().OrderBy(item => item.Position).ToArray();
            Assert.Equal("NatashaUT.ReverserUT.NullableReverserUT.NullabelTestModel", parameters[0].GetMemberNullableDevelopName());
            Assert.Equal("NatashaUT.ReverserUT.NullableReverserUT.NullabelTestModel?", parameters[1].GetMemberNullableDevelopName());
            Assert.Equal("System.Nullable<System.Int32>", parameters[2].GetMemberNullableDevelopName());
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

            public void Show1(NullabelTestModel? a, int? b, string? c) { }
            public void Show2(NullabelTestModel a, int? b, string? c) { }
            public void Show3(NullabelTestModel a, int b, string? c) { }
        }
    }
}
