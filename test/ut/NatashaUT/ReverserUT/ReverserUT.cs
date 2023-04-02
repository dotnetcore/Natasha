using Natasha.CSharp.Reverser;
using NatashaUT.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace NatashaUT.ReverserUT
{
    [Trait("反解测试","参数")]
    public class ReverserUT : PrepareTest
    {

        [Fact(DisplayName = "参数与类型反解 in")]
        public void TestIn()
        {
            var info = typeof(MethodSpecialTestModel).GetMethod("Test1");
            Assert.Equal("in NatashaUT.Model.Rsm<NatashaUT.Model.GRsm>", DeclarationReverser.GetParametersType(info.GetParameters()[0]));
        }




        [Fact(DisplayName = "参数与类型反解 out")]
        public void TestOut()
        {
            var info = typeof(MethodSpecialTestModel).GetMethod("Test2");
            Assert.Equal("out NatashaUT.Model.Rsm<NatashaUT.Model.Rsm<NatashaUT.Model.GRsm>[]>", DeclarationReverser.GetParametersType(info.GetParameters()[0]));
        }




        [Fact(DisplayName = "参数与类型反解 ref")]
        public void TestRef()
        {
            var info = typeof(MethodSpecialTestModel).GetMethod("Test3");
            Assert.Equal("ref NatashaUT.Model.Rsm<NatashaUT.Model.Rsm<NatashaUT.Model.GRsm>[]>[]", DeclarationReverser.GetParametersType(info.GetParameters()[0]));
        }


        public class Inline<TT> { }

        [Fact(DisplayName = "多维数组解析")]
        public void TestType()
        {
            var temp = new { name="abc", age= 15 };
            var temp1 = new { name = "abc", age = 15, time=DateTime.Now };
            Assert.Equal("System.Nullable<System.Int32>", typeof(int?).GetDevelopName());
            Assert.Equal("<>f__AnonymousType0<System.String,System.Int32>", temp.GetType().GetDevelopName());
            Assert.Equal("<>f__AnonymousType1<System.String,System.Int32,System.DateTime>", temp1.GetType().GetDevelopName());
            Assert.Equal("System.ValueTuple<System.Int32,System.ValueTuple<System.Int32,System.Int32>>", typeof((int, (int,int))).GetDevelopName());
            Assert.Equal("System.ValueTuple<System.Int32,System.Int32>", typeof((int,int)).GetDevelopName());
            Assert.Equal("NatashaUT.ReverserUT.ReverserUT.Inline<TT>", typeof(Inline<>).GetDevelopName());
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




        [Fact(DisplayName = "内部类解析")]
        public void TestInnerType()
        {

            Assert.Equal("NatashaUT.Model.OopTestModel.InnerClass", typeof(OopTestModel.InnerClass).GetDevelopNameWithoutFlag());

        }



        [Fact(DisplayName = "类继承解析")]
        public void TestInheritanceType()
        {
            var a = typeof(InheritanceTest).GetInterfaces();
            var b = typeof(InheritanceTest).BaseType;
            Assert.Equal("NatashaUT.Model.OopTestModel.InnerClass", typeof(OopTestModel.InnerClass).GetDevelopNameWithoutFlag());

        }


        //[Fact(DisplayName = "方法承解析1")]
        //public void TestMethodProtected1()
        //{

        //    var a = typeof(MethodModel).GetMethod("M");
        //    Assert.Equal("virtual ", ModifierReverser.GetModifier(a));
        //    Assert.False(a.IsAbstract);

        //}
        //[Fact(DisplayName = "方法承解析2")]
        //public void TestMethodProtected2()
        //{

        //    var a = typeof(MethodModel2).GetMethod("M");
        //    Assert.Equal("abstract ", ModifierReverser.GetModifier(a));
        //    Assert.True(a.IsVirtual);

        //}
        //[Fact(DisplayName = "方法承解析3")]
        //public void TestMethodProtected3()
        //{

        //    var a = typeof(MethodModel3).GetMethod("M");
        //    Assert.Equal("override ", ModifierReverser.GetModifier(a));

        //}
        //[Fact(DisplayName = "方法承解析4")]
        //public void TestMethodProtected4()
        //{
        //    var a = typeof(IMethodModel4).GetMethod("M");
        //    Assert.Equal(null, ModifierReverser.GetModifier(a));

        //}
        //[Fact(DisplayName = "方法承解析4")]
        //public void TestMethodProtected4()
        //{

        //    var a = typeof(MethodModel4).GetMethod("M");
        //    Assert.Equal("new ", ModifierReverser.GetModifier(a));

        //}


    }
}
