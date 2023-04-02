using Natasha.CSharp;
using System;
using Xunit;

namespace NatashaUT.BuilderUT
{

    [Trait("类构建与编译", "类")]
    public class ClassBuilderTest : PrepareTest
    {
        [Fact(DisplayName = "类构建与编译1")]
        public void TestClass()
        {
            NClass builder = NClass.RandomDomain();
            var script = builder
                .CustomUsing()
                .HiddenNamespace()
                .Access(AccessFlags.Public)
                .Name("EnumUT1")
                .Field(item=> { item.Public().Name("Apple").Type<int>(); })
                .Field(item => { item.Public().Name("Orange").Type<string>(); })
                .Property(item => { item.Public().Name("Banana").Type<NClass>(); })
                .Script;

            Assert.Equal($"using System;{Environment.NewLine}using Natasha.CSharp;{Environment.NewLine}public class EnumUT1{{{Environment.NewLine}public System.Int32 Apple;{Environment.NewLine}public System.String Orange;{Environment.NewLine}public Natasha.CSharp.NClass Banana{{{Environment.NewLine}get;{Environment.NewLine}set;{Environment.NewLine}}}{Environment.NewLine}}}", script);
            Assert.NotNull(builder.GetType());
        }




        [Fact(DisplayName = "类构建与编译2")]
        public void TestClass1()
        {

            NClass builder = NClass.RandomDomain();
            var script = builder
                .CustomUsing()
                .HiddenNamespace()
                .Access(AccessFlags.Public)
                .Name("EnumUT1")
                .Method(item => { item.Public().Name("Apple").Type<int>().Body("return 0;"); })
                .Property(item => { item
                    .Public()
                    .Name("Banana")
                    .Type<NClass>()
                    .Setter("int a = value.ToString().Length;")
                    .Getter("return default;"); })
                .Script;

            Assert.Equal($"using System;{Environment.NewLine}using Natasha.CSharp;{Environment.NewLine}public class EnumUT1{{{Environment.NewLine}public System.Int32 Apple(){{return 0;}}{Environment.NewLine}public Natasha.CSharp.NClass Banana{{{Environment.NewLine}get{{return default;}}{Environment.NewLine}set{{int a = value.ToString().Length;}}{Environment.NewLine}}}{Environment.NewLine}}}", script);
            Assert.NotNull(builder.GetType());

        }

        [Fact(DisplayName = "类构建与编译3")]
        public void TestClass2()
        {

            NClass builder = NClass.RandomDomain();

            var mb = builder.GetMethodBuilder();
            mb.Public().Name("Apple").Type<int>().Body("return 0;");
            var pb = builder.GetPropertyBuilder();
            pb.Public()
            .Name("Banana")
            .Type<NClass>()
            .Setter("int a = value.ToString().Length;")
            .Getter("return default;");


            var script = builder
                .CustomUsing()
                .HiddenNamespace()
                .Access(AccessFlags.Public)
                .Name("EnumUT1")
                .Script;
            
            Assert.Equal($"using System;{Environment.NewLine}using Natasha.CSharp;{Environment.NewLine}public class EnumUT1{{{Environment.NewLine}public System.Int32 Apple(){{return 0;}}{Environment.NewLine}public Natasha.CSharp.NClass Banana{{{Environment.NewLine}get{{return default;}}{Environment.NewLine}set{{int a = value.ToString().Length;}}{Environment.NewLine}}}{Environment.NewLine}}}", script);
            Assert.Equal($"using System;{Environment.NewLine}using Natasha.CSharp;{Environment.NewLine}public class EnumUT1{{{Environment.NewLine}public System.Int32 Apple(){{return 0;}}{Environment.NewLine}public Natasha.CSharp.NClass Banana{{{Environment.NewLine}get{{return default;}}{Environment.NewLine}set{{int a = value.ToString().Length;}}{Environment.NewLine}}}{Environment.NewLine}}}", script);

            Assert.NotNull(builder.GetType());

        }
    }
}
