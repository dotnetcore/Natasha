using Natasha.CSharp;
using Xunit;

namespace FrameworkFunctionUT.Template.Compile
{

    [Trait("高级API功能测试", "OOP")]
    public class InterfaceBuilderTest : DomainPrepare
    {

        [Fact(DisplayName = "接口构建与编译")]
        public void TestInterface()
        {

            var builder = NInterface.DefaultDomain();
            builder.AssemblyBuilder.DisableSemanticCheck();
            var type = builder
                .NoGlobalUsing()
                .HiddenNamespace()
                .Access(AccessFlags.Public)
                .Name("Interface1")
                .Property(item=>item.Type<string>().Name("Abc"))
                .Method(item=>item.Name("Test").Param<string>("p").Return<int>())
                .GetType();
            var script = builder.AssemblyBuilder.SyntaxTrees[0].ToString();

            string expected = @"using System;

public interface Interface1
{
    System.String Abc { get; set; }

    System.Int32 Test(System.String p);
}";
            //语法树转换的BUG 多了一个\r\n
            OSStringCompare.Equal(expected, script);
            Assert.NotNull(type);

        }

    }

}