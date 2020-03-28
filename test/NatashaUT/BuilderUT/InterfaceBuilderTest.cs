using Natasha;
using System;
using Xunit;

namespace NatashaUT.BuilderUT
{

    [Trait("接口构建与编译", "接口")]
    public class InterfaceBuilderTest
    {

        [Fact(DisplayName = "接口构建与编译1")]
        public void TestInterface()
        {

            var builder = NInterface.Random();
            var script = builder
                .CurstomeUsing()
                .HiddenNamespace()
                .Access(Natasha.Reverser.Model.AccessTypes.Public)
                .DefinedName("Interface1")
                .Property(item=>item.DefinedType<string>().DefinedName("Abc"))
                .Method(item=>item.DefinedName("Test").Param<string>("p").Return<int>())
                .Script;

            Assert.Equal($"using System;{Environment.NewLine}public interface Interface1{{{Environment.NewLine}System.String Abc{{{Environment.NewLine}get;{Environment.NewLine}set;{Environment.NewLine}}}{Environment.NewLine}System.Int32 Test(System.String p);{Environment.NewLine}}}", script);
            Assert.NotNull(builder.GetType());

        }

    }

}
