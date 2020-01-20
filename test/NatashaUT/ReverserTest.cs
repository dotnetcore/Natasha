using Natasha;
using Natasha.Reverser;
using NatashaUT.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace NatashaUT
{
    [Trait("反解测试","参数")]
    public class ReverserTest
    {

        [Fact(DisplayName = "参数与类型反解 in")]
        public void TestIn()
        {
            var info = typeof(ReverserTestModel).GetMethod("Test1");
            Assert.Equal("in Rsm<GRsm>",DeclarationReverser.GetParametersType(info.GetParameters()[0]));
        }




        [Fact(DisplayName = "参数与类型反解 out")]
        public void TestOut()
        {
            var info = typeof(ReverserTestModel).GetMethod("Test2");
            Assert.Equal("out Rsm<Rsm<GRsm>[]>", DeclarationReverser.GetParametersType(info.GetParameters()[0]));
        }




        [Fact(DisplayName = "参数与类型反解 ref")]
        public void TestRef()
        {
            var info = typeof(ReverserTestModel).GetMethod("Test3");
            Assert.Equal("ref Rsm<Rsm<GRsm>[]>[]", DeclarationReverser.GetParametersType(info.GetParameters()[0]));
        }


        public class Inline<TT> { }

        [Fact(DisplayName = "多维数组解析")]
        public void TestType()
        {
            var temp = new { name="abc", age= 15 };
            var temp1 = new { name = "abc", age = 15, time=DateTime.Now };
            Assert.Equal("Nullable<Int32>", typeof(int?).GetDevelopName());
            Assert.Equal("<>f__AnonymousType0<String,Int32>", temp.GetType().GetDevelopName());
            Assert.Equal("<>f__AnonymousType1<String,Int32,DateTime>", temp1.GetType().GetDevelopName());
            Assert.Equal("ValueTuple<Int32,ValueTuple<Int32,Int32>>", typeof((int, (int,int))).GetDevelopName());
            Assert.Equal("ValueTuple<Int32,Int32>", typeof((int,int)).GetDevelopName());
            Assert.Equal("ReverserTest.Inline<TT>", typeof(Inline<>).GetDevelopName());
            Assert.Equal("Dictionary<TKey,TValue>", typeof(Dictionary<,>).GetDevelopName());
            Assert.Equal("List<T>", typeof(List<>).GetDevelopName());
            Assert.Equal("List<Int32>", typeof(List<int>).GetDevelopName());
            Assert.Equal("List<>", typeof(List<>).GetRuntimeName());
            Assert.Equal("List<Int32>[]", typeof(List<int>[]).GetRuntimeName());
            Assert.Equal("List<Int32>[,]", typeof(List<int>[,]).GetRuntimeName());
            Assert.Equal("List<Int32>[,][][,,,,]", typeof(List<int>[,][][,,,,]).GetRuntimeName());
            Assert.Equal("Int32[,]", typeof(int[,]).GetRuntimeName());
            Assert.Equal("Int32[][]", typeof(int[][]).GetRuntimeName());
            Assert.Equal("Int32[][,,,]", typeof(int[][,,,]).GetRuntimeName());
            Assert.Equal("Dictionary<Int32[][,,,],String[,,,][]>[]", typeof(Dictionary<int[][,,,], string[,,,][]>[]).GetRuntimeName());

        }




        [Fact(DisplayName = "内部类解析")]
        public void TestInnerType()
        {

            Assert.Equal("OopTestModel.InnerClass", typeof(OopTestModel.InnerClass).GetRuntimeName());

        }



        [Fact(DisplayName = "类继承解析")]
        public void TestInheritanceType()
        {
            var a = typeof(InheritanceTest).GetInterfaces();
            var b = typeof(InheritanceTest).BaseType;
            Assert.Equal("OopTestModel.InnerClass", typeof(OopTestModel.InnerClass).GetRuntimeName());

        }


    }
}
