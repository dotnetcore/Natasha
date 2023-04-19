using Natasha.CSharp;
using System.Runtime.InteropServices;
using Xunit;

namespace NatashaFunctionUT.Template.Compile
{

    [Trait("高级API功能测试", "OOP")]
    public class StructBuilderTest : DomainPrepare
    {

        [Fact(DisplayName = "结构体构建与编译1")]
        public void TestStruct()
        {

            NStruct builder = NStruct.RandomDomain();
           
            var type = builder
                .HiddenNamespace()
                .Access(AccessFlags.Public)
                .Name("EnumUT1")
                .Field(item => { item.Public().Name("Apple").Type<int>(); })
                .Field(item => { item.Public().Name("Orange").Type<string>(); })
                .Property(item => { item.Public().Name("Banana").Type<NClass>(); })
                .GetType();
            var script = builder.AssemblyBuilder.Compilation!.SyntaxTrees[0].ToString();
            var expected = @"
public struct EnumUT1
{
    public System.Int32 Apple;
    public System.String Orange;
    public NClass Banana { get; set; }
}";
            OSStringCompare.Equal(expected, script);
            Assert.NotNull(type);

        }


        [Fact(DisplayName = "结构体构建与编译2")]
        public void TestStruct1()
        {
            NStruct builder = NStruct.RandomDomain();
           
            var type = builder
                .HiddenNamespace()
                .AttributeAppend("[StructLayout(LayoutKind.Explicit)]")
                .Access(AccessFlags.Public)
                .Name("EnumUT1")
                .Field(item => { item.AttributeAppend<FieldOffsetAttribute>("0").Public().Name("Apple").Type<int>(); })
                .Field(item => { item.AttributeAppend<FieldOffsetAttribute>("0").Public().Name("Orange").Type<int>(); })
                .GetType();
            var script = builder.AssemblyBuilder.Compilation!.SyntaxTrees[0].ToString();
            var expected = @"using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
public struct EnumUT1
{
    [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
    public System.Int32 Apple;
    [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
    public System.Int32 Orange;
}";
            OSStringCompare.Equal(expected, script); 
            Assert.NotNull(type);

        }
    }
}
