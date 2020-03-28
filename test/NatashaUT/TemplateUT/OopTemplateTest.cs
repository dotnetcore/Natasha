using Natasha;
using Natasha.Builder;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NatashaUT
{
    [Trait("对象构建", "")]
    public class OopTemplateTest
    {

        //[Fact(DisplayName = "构建类")]
        //public void TestBuilder1()
        //{
        //    OopBuilder<OopTemplateTest> builder = new OopBuilder();
        //    var script = builder
        //        .Access(Natasha.Reverser.Model.AccessTypes.Public)
        //        .Modifier(Natasha.Reverser.Model.Modifiers.Static)
        //        .Class()
        //        .CurstomeUsing()
        //        .Using<OopTemplateTest>()
        //        .Using<int>()
        //        .Namespace("TestNamespace")
        //        .DefinedName("TestUt1")
        //        .Body("public static void Test(){}")
        //        .Script;
        //    Assert.Equal($@"using NatashaUT;{Environment.NewLine}using System;{Environment.NewLine}namespace TestNamespace{{public static class TestUt1{{{Environment.NewLine}public static void Test(){{}}{Environment.NewLine}}}}}", script);

        //}




        //[Fact(DisplayName = "结构体构建")]
        //public void TestBuilder2()
        //{

        //    OopBuilder builder = new OopBuilder();
        //    var script = builder
        //        .Access(Natasha.Reverser.Model.AccessTypes.Internal)
        //        .Modifier(Natasha.Reverser.Model.Modifiers.Abstract)
        //        .Struct()
        //        .CurstomeUsing()
        //        .Using<OopTemplateTest>()
        //        .Namespace("TestNamespace")
        //        .DefinedName("TestUt1")
        //        .Body("public static void Test(){}")
        //        .Script;
        //    Assert.Equal($@"using NatashaUT;{Environment.NewLine}namespace TestNamespace{{internal abstract struct TestUt1{{{Environment.NewLine}public static void Test(){{}}{Environment.NewLine}}}}}", script);

        //}



        //[Fact(DisplayName = "接口构建")]
        //public void TestBuilder3()
        //{

        //    OopBuilder builder = new OopBuilder();
        //    var script = builder
        //        .Access(Natasha.Reverser.Model.AccessTypes.Private)
        //        .Interface()
        //        .CurstomeUsing()
        //        .Using<OopTemplateTest>()
        //        .Namespace("TestNamespace")
        //        .DefinedName("TestUt1")
        //        .Body("public static void Test(){}")
        //        .Script;
        //    Assert.Equal($@"using NatashaUT;{Environment.NewLine}namespace TestNamespace{{private interface TestUt1{{{Environment.NewLine}public static void Test(){{}}{Environment.NewLine}}}}}", script);

        //}




        //[Fact(DisplayName = "枚举构建")]
        //public void TestBuilder4()
        //{

        //    OopBuilder builder = new OopBuilder();
        //    var script = builder
        //        .Access(Natasha.Reverser.Model.AccessTypes.None)
        //        .Enum()
        //        .CurstomeUsing()
        //        .Using<OopTemplateTest>()
        //        .Namespace("TestNamespace")
        //        .DefinedName("TestUt1")
        //        .Body("public static void Test(){}")
        //        .Script;
        //    Assert.Equal($@"using NatashaUT;{Environment.NewLine}namespace TestNamespace{{internal enum TestUt1{{{Environment.NewLine}public static void Test(){{}}{Environment.NewLine}}}}}", script);

        //}



        //[Fact(DisplayName = "复合构建1")]
        //public void TestBuilder5()
        //{

        //    OopBuilder builder = new OopBuilder();
        //    var script = builder
        //        .Access(Natasha.Reverser.Model.AccessTypes.None)
        //        .Enum()
        //        .CurstomeUsing()
        //        .Using<OopTemplateTest>()
        //        .Namespace("TestNamespace")
        //        .DefinedName("TestUt1")
        //        .Field(item =>
        //        {

        //            item

        //                .NoUseAccess()
        //                .DefinedType<string>()
        //                .DefinedName("name");
        //                //System.String name;
        //        })
        //        .Property(item =>
        //        {

        //            item

        //                .Abstract()
        //                .DefinedType<string>()
        //                .DefinedName("address");
        //                //abstract System.String address{get;set;}

        //        })
        //        .Method(item =>
        //        {

        //            item

        //                .Access(Natasha.Reverser.Model.AccessTypes.Protected)
        //                .Async()
        //                .Return<Task<int>>()
        //                .DefinedName("testMethod")
        //                .Param<int>("age")
        //                .Body("return age;");
        //                //protected async System.Threading.TasksTask<System.Int32>testMethod(System.Int32 age){return age;}

        //        })
        //        .Body("public static void Test(){}")
        //        .Script;
        //    Assert.Equal($@"using NatashaUT;{Environment.NewLine}namespace TestNamespace{{internal enum TestUt1{{{Environment.NewLine}System.String name;{Environment.NewLine}abstract System.String address{{{Environment.NewLine}get;{Environment.NewLine}set;{Environment.NewLine}}}{Environment.NewLine}{Environment.NewLine}protected async System.Threading.Tasks.Task<System.Int32> testMethod(System.Int32 age){{return age;}}{Environment.NewLine}{Environment.NewLine}public static void Test(){{}}{Environment.NewLine}}}}}", script);

        //}

    }

}
