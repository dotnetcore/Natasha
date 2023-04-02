using Natasha.CSharp;
using Natasha.CSharp.Builder;
using NatashaFunctionUT.Reverser;
using System;
using System.Collections.Generic;
using Xunit;

namespace NatashaFunctionUT.Template
{
    [Trait("基础功能测试", "模板")]
    public class OopTemplateTest 
    {

        [Fact(DisplayName = "构建类")]
        public void TestBuilder1()
        {

            OopBuilder builder = new OopBuilder();
            var script = builder
                .Access(AccessFlags.Public)
                .Modifier(ModifierFlags.Static)
                .Class()
                .NoGlobalUsing()
                .NotLoadDomainUsing()
                .Using<OopTemplateTest>()
                .Using<int>()
                .InheritanceAppend<int>()
                .Namespace("TestNamespace")
                .Name("TestUt1<T>")
                .Constraint("where T : class")
                .Body("public static void Test(){}")
                .GetScript();
            Assert.Equal($@"using NatashaFunctionUT.Template;{Environment.NewLine}using System;{Environment.NewLine}namespace TestNamespace{{public static class TestUt1<T> : System.Int32 where T : class{{{Environment.NewLine}public static void Test(){{}}}}}}", script);

        }


        [Fact(DisplayName = "构建类2")]
        public void TestBuilder2()
        {

            OopBuilder builder = new OopBuilder();
            var script = builder
                .Access(AccessFlags.Public)
                .Modifier(ModifierFlags.Static)
                .Class()
                .NoGlobalUsing()
                .NotLoadDomainUsing()
                .Using<OopTemplateTest>()
                .Using<int>()
                .InheritanceAppend<int>()
                .Namespace("TestNamespace")
                .Name("TestUt1<T>")
                .ConstraintAppendFrom(typeof(List<>))
                .Body("public static void Test(){}")
                .GetScript();

#if NET5_0_OR_GREATER
    Assert.Equal($@"using NatashaFunctionUT.Template;{Environment.NewLine}using System;{Environment.NewLine}namespace TestNamespace{{public static class TestUt1<T> : System.Int32 where T : notnull {{{Environment.NewLine}public static void Test(){{}}}}}}", script);
#else
            Assert.Equal($@"using NatashaFunctionUT.Template;{Environment.NewLine}using System;{Environment.NewLine}namespace TestNamespace{{public static class TestUt1<T> : System.Int32 where T : notnull {{{Environment.NewLine}public static void Test(){{}}}}}}", script);
#endif


        }

        [Fact(DisplayName = "构建类3")]
        public void TestBuilder3()
        {

            var type = typeof(G1).GetMethod("Test");
            var p = type.GetParameters();
            var a = p[0].ParameterType.GetDevelopName();
           

            OopBuilder builder = new OopBuilder();
            var script = builder
                .Access(AccessFlags.Public)
                .Modifier(ModifierFlags.Static)
                .Class()
                .NoGlobalUsing()
                .NotLoadDomainUsing()
                .Using<OopTemplateTest>()
                .Using<int>()
                .InheritanceAppend<int>()
                .Namespace("TestNamespace")
                .Name("TestUt1<T>")
                .ConstraintAppendFrom(typeof(InOutInterfaceT<,>))
                .Body("public static void Test(){}")
                .GetScript();
            Assert.Equal($@"using NatashaFunctionUT.Template;{Environment.NewLine}using System;{Environment.NewLine}namespace TestNamespace{{public static class TestUt1<T> : System.Int32 where T : NatashaFunctionUT.Reverser.G2, NatashaFunctionUT.Reverser.G3, NatashaFunctionUT.Reverser.G4, new() where S : NatashaFunctionUT.Reverser.G2, NatashaFunctionUT.Reverser.G3, NatashaFunctionUT.Reverser.G4, new() {{{Environment.NewLine}public static void Test(){{}}}}}}", script);

        }

        [Fact(DisplayName = "构建类4")]
        public void TestBuilder4()
        {

            OopBuilder builder = new OopBuilder();
            var script = builder
                .Access(AccessFlags.Public)
                .Modifier(ModifierFlags.Static)
                .Class()
                .NoGlobalUsing()
                .NotLoadDomainUsing()
                .Using<OopTemplateTest>()
                .Using<int>()
                .InheritanceAppend<int>()
                .Namespace("TestNamespace")
                .Name("TestUt1<T>")
                .Constraint(item=>
                    item
                    .SetType("T")
                    .Constraint(ConstraintFlags.New)
                    .Constraint<Int32>()
                    .Constraint(typeof(string))
                    .Constraint(ConstraintFlags.Class))
                .Body("public static void Test(){}")
                .GetScript();
            Assert.Equal($@"using NatashaFunctionUT.Template;{Environment.NewLine}using System;{Environment.NewLine}namespace TestNamespace{{public static class TestUt1<T> : System.Int32 where T : class,new(),System.Int32,System.String{{{Environment.NewLine}public static void Test(){{}}}}}}", script);

        }

    }

}
