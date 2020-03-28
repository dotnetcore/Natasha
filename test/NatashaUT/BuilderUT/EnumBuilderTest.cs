using Natasha;
using System;
using Xunit;

namespace NatashaUT
{

    [Trait("枚举构建与编译", "枚举")]
    public class EnumBuilderTest
    {

        [Fact(DisplayName = "枚举构建与编译1")]
        public void TestEnum()
        {
            NEnum builder = NEnum.Random();
            var script = builder
                .CurstomeUsing()
                .HiddenNamespace()
                .Access(Natasha.Reverser.Model.AccessTypes.Public)
                .DefinedName("EnumUT1")
                .EnumField("Apple", 1)
                .EnumField("Orange", 2)
                .EnumField("Banana", 4)
                .Script;

            Assert.Equal($"public enum EnumUT1{{{Environment.NewLine}Apple=1,{Environment.NewLine}Orange=2,{Environment.NewLine}Banana=4}}", script);
            Assert.NotNull(builder.GetType());
        }




        [Fact(DisplayName = "枚举构建与编译2")]
        public void TestEnum1()
        {
            NEnum builder = NEnum.Random();
            var script = builder
                .CurstomeUsing()
                .Namespace("haha")
                .Access(Natasha.Reverser.Model.AccessTypes.Public)
                .DefinedName("EnumUT1")
                .EnumField("Apple", 1)
                .EnumField("Orange", 2)
                .EnumField("Banana", 4)
                .Script;

            Assert.Equal($"namespace haha{{public enum EnumUT1{{{Environment.NewLine}Apple=1,{Environment.NewLine}Orange=2,{Environment.NewLine}Banana=4}}}}", script);
            Assert.NotNull(builder.GetType());
        }

    }

}
