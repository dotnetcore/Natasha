using Natasha.CSharp;
using Xunit;

namespace NatashaFunctionUT.Template.Compile
{

    [Trait("高级API功能测试", "OOP")]
    public class EnumBuilderTest : DomainPrepare
    {

        [Fact(DisplayName = "枚举构建与编译1")]
        public void TestEnum()
        {
            NEnum builder = NEnum.RandomDomain();

             var type = builder
                .NoGlobalUsing()
                .HiddenNamespace()
                .Access(AccessFlags.Public)
                .Name("EnumUT1")
                .EnumField("Apple", 1,"苹果")
                .EnumField("Orange", 2)
                .EnumField("Banana", 4)
                .GetType();
            var script = builder.AssemblyBuilder.SyntaxTrees[0].ToString();
            var expected = @"public enum EnumUT1
{
    /// <summary>
    /// 苹果
    /// </summary>
    Apple = 1,
    Orange = 2,
    Banana = 4
}";
            OSStringCompare.Equal(expected, script);
            Assert.NotNull(type);
        }




        [Fact(DisplayName = "枚举构建与编译2")]
        public void TestEnum1()
        {
            NEnum builder = NEnum.RandomDomain();
            var type = builder
                .NoGlobalUsing()
                .Namespace("haha")
                .Access(AccessFlags.Public)
                .Name("EnumUT1")
                .EnumField("Apple", 1)
                .EnumField("Orange", 2)
                .EnumField("Banana", 4)
                .GetType();
            var script = builder.AssemblyBuilder.SyntaxTrees[0].ToString();
            var expected = @"namespace haha
{
    public enum EnumUT1
    {
        Apple = 1,
        Orange = 2,
        Banana = 4
    }
}";
            OSStringCompare.Equal(expected, script);
            Assert.NotNull(type);
        }

    }

}
