using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NatashaFunctionUT.Reverser
{
    [Trait("基础功能测试", "反解器")]
    public class TypeNameReverserTest
    {
        public class Inline<TT> { }
        [Fact(DisplayName = "类名反解测试")]
        public void TestType()
        {
            var temp = new { name = "abc", age = 15 };
            var temp1 = new { name = "abc", age = 15, time = DateTime.Now };
            Assert.Equal("System.Nullable<System.Int32>", typeof(int?).GetDevelopName());
            Assert.Equal("<>f__AnonymousType0<System.String,System.Int32>", temp.GetType().GetDevelopName());
            Assert.Equal("<>f__AnonymousType1<System.String,System.Int32,System.DateTime>", temp1.GetType().GetDevelopName());
            Assert.Equal("System.ValueTuple<System.Int32,System.ValueTuple<System.Int32,System.Int32>>", typeof((int, (int, int))).GetDevelopName());
            Assert.Equal("System.ValueTuple<System.Int32,System.Int32>", typeof((int, int)).GetDevelopName());
            Assert.Equal("NatashaFunctionUT.Reverser.TypeNameReverserTest.Inline<TT>", typeof(Inline<>).GetDevelopName());
            Assert.Equal("System.Collections.Generic.Dictionary<TKey,TValue>", typeof(Dictionary<,>).GetDevelopName());
            Assert.Equal("System.Collections.Generic.List<T>", typeof(List<>).GetDevelopName());
            Assert.Equal("System.Collections.Generic.List<System.Int32>", typeof(List<int>).GetDevelopName());
            Assert.Equal("System.Collections.Generic.Dictionary<System.Collections.Generic.List<System.Int32>[],System.Int32>", typeof(Dictionary<List<int>[], int>).GetDevelopNameWithoutFlag());
            Assert.Equal("System.Collections.Generic.List<>", typeof(List<>).GetDevelopNameWithoutFlag());
            Assert.Equal("System.Collections.Generic.List<System.Int32>[]", typeof(List<int>[]).GetDevelopNameWithoutFlag());
            Assert.Equal("System.Collections.Generic.List<System.Int32>[,]", typeof(List<int>[,]).GetDevelopNameWithoutFlag());
            Assert.Equal("System.Collections.Generic.List<System.Int32>[,][][,,,,]", typeof(List<int>[,][][,,,,]).GetDevelopNameWithoutFlag());
            Assert.Equal("System.Int32[,]", typeof(int[,]).GetDevelopNameWithoutFlag());
            Assert.Equal("System.Int32[][]", typeof(int[][]).GetDevelopNameWithoutFlag());
            Assert.Equal("System.Int32[][,,,]", typeof(int[][,,,]).GetDevelopNameWithoutFlag());
            Assert.Equal("System.Collections.Generic.Dictionary<System.Int32[][,,,],System.String[,,,][]>[]", typeof(Dictionary<int[][,,,], string[,,,][]>[]).GetDevelopNameWithoutFlag());
        }
    }
}
