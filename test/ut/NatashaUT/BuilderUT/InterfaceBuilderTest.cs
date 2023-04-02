using Natasha.CSharp;
using System;
using Xunit;

namespace NatashaUT.BuilderUT
{

    [Trait("接口构建与编译", "接口")]
    public class InterfaceBuilderTest : PrepareTest
    {

        [Fact(DisplayName = "接口构建与编译1")]
        public void TestInterface()
        {

            var builder = NInterface.RandomDomain();
            var script = builder
                .CustomUsing()
                .HiddenNamespace()
                .Access(AccessFlags.Public)
                .Name("Interface1")
                .Property(item=>item.Type<string>().Name("Abc"))
                .Method(item=>item.Name("Test").Param<string>("p").Return<int>())
                .Script;

            Assert.Equal($"using System;{Environment.NewLine}public interface Interface1{{{Environment.NewLine}System.String Abc{{{Environment.NewLine}get;{Environment.NewLine}set;{Environment.NewLine}}}{Environment.NewLine}System.Int32 Test(System.String p);{Environment.NewLine}}}", script);
            Assert.NotNull(builder.GetType());

        }

    }

}
