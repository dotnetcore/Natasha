using Natasha;
using Natasha.CSharp;
using System;
using Xunit;

namespace NatashaUT.BuilderUT
{

    [Trait("类构建与编译", "类")]
    public class ClassBuilderTest
    {
        [Fact(DisplayName = "类构建与编译1")]
        public void TestClass()
        {
            NClass builder = NClass.RandomDomain();
            var script = builder
                .CustomUsing()
                .HiddenNamespace()
                .Access(Natasha.Reverser.Model.AccessTypes.Public)
                .DefinedName("EnumUT1")
                .Field(item=> { item.Public().DefinedName("Apple").DefinedType<int>(); })
                .Field(item => { item.Public().DefinedName("Orange").DefinedType<string>(); })
                .Property(item => { item.Public().DefinedName("Banana").DefinedType<NClass>(); })
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
                .Access(Natasha.Reverser.Model.AccessTypes.Public)
                .DefinedName("EnumUT1")
                .Method(item => { item.Public().DefinedName("Apple").DefinedType<int>().Body("return 0;"); })
                .Property(item => { item
                    .Public()
                    .DefinedName("Banana")
                    .DefinedType<NClass>()
                    .Setter("int a = value.ToString().Length;")
                    .Getter("return default;"); })
                .Script;

            Assert.Equal($"using System;{Environment.NewLine}using Natasha.CSharp;{Environment.NewLine}public class EnumUT1{{{Environment.NewLine}public System.Int32 Apple(){{return 0;}}{Environment.NewLine}public Natasha.CSharp.NClass Banana{{{Environment.NewLine}get{{return default;}}{Environment.NewLine}set{{int a = value.ToString().Length;}}{Environment.NewLine}}}{Environment.NewLine}}}", script);
            Assert.NotNull(builder.GetType());

        }
    }
}
