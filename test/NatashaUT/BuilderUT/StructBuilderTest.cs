using Natasha.CSharp;
using System;
using System.Runtime.InteropServices;
using Xunit;

namespace NatashaUT.BuilderUT
{

    [Trait("结构体构建与编译", "结构体")]
    public class StructBuilderTest
    {

        [Fact(DisplayName = "结构体构建与编译1")]
        public void TestStruct()
        {

            NStruct builder = NStruct.Random();
            var script = builder
                .CurstomeUsing()
                .HiddenNamespace()
                .Access(Natasha.Reverser.Model.AccessTypes.Public)
                .DefinedName("EnumUT1")
                .Field(item => { item.Public().DefinedName("Apple").DefinedType<int>(); })
                .Field(item => { item.Public().DefinedName("Orange").DefinedType<string>(); })
                .Property(item => { item.Public().DefinedName("Banana").DefinedType<NClass>(); })
                .Script;

            Assert.Equal($"using System;{Environment.NewLine}using Natasha.CSharp;{Environment.NewLine}public struct EnumUT1{{{Environment.NewLine}public System.Int32 Apple;{Environment.NewLine}public System.String Orange;{Environment.NewLine}public Natasha.CSharp.NClass Banana{{{Environment.NewLine}get;{Environment.NewLine}set;{Environment.NewLine}}}{Environment.NewLine}}}", script);
            Assert.NotNull(builder.GetType());

        }


        [Fact(DisplayName = "结构体构建与编译1")]
        public void TestStruct1()
        {
            
            NStruct builder = NStruct.Random();
            var script = builder
                .CurstomeUsing()
                .HiddenNamespace()
                .Attribute("[StructLayout(LayoutKind.Explicit)]")
                .Access(Natasha.Reverser.Model.AccessTypes.Public)
                .DefinedName("EnumUT1")
                .Field(item => { item.Attribute<FieldOffsetAttribute>("0").Public().DefinedName("Apple").DefinedType<int>(); })
                .Field(item => { item.Attribute<FieldOffsetAttribute>("0").Public().DefinedName("Orange").DefinedType<int>(); })
                .Script;

            Assert.Equal($"using System.Runtime.InteropServices;{Environment.NewLine}using System;{Environment.NewLine}[StructLayout(LayoutKind.Explicit)]{Environment.NewLine}public struct EnumUT1{{{Environment.NewLine}[System.Runtime.InteropServices.FieldOffsetAttribute(0)]{Environment.NewLine}public System.Int32 Apple;{Environment.NewLine}[System.Runtime.InteropServices.FieldOffsetAttribute(0)]{Environment.NewLine}public System.Int32 Orange;{Environment.NewLine}}}", script);
            Assert.NotNull(builder.GetType());

        }
    }
}
