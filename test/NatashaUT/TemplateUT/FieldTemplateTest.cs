using Natasha.CSharp.Builder;
using System;
using Xunit;

namespace NatashaUT
{

    [Trait("字段构建", "")]
    public class FieldTemplateTest
    {

        [Fact(DisplayName = "静态字段1")]
        public void Test1()
        {

            FieldBuilder template = new FieldBuilder();
            var result = template
                .Attribute("[Test]")
                .Access("public")
                .Modifier("static")
                .DefinedName("Name")
                .DefinedType<string>()
                .Script;

            Assert.Equal($"[Test]{Environment.NewLine}public static System.String Name;", result);

        }





        [Fact(DisplayName = "静态字段2")]
        public void Test2()
        {

            FieldBuilder template = new FieldBuilder();
            var result = template
                .Attribute("[Test][Test1]")
                .Access(Natasha.Reverser.Model.AccessTypes.Public)
                .Modifier( Natasha.Reverser.Model.Modifiers.Static)
                .DefinedName("Age")
                .DefinedType(typeof(int))
                .Script;

            Assert.Equal($"[Test][Test1]{Environment.NewLine}public static System.Int32 Age;", result);


        }




        [Fact(DisplayName = "普通字段1")]
        public void Test3()
        {

            FieldBuilder template = new FieldBuilder();
            var result = template
                .Attribute("[Test]")
                .Access(Natasha.Reverser.Model.AccessTypes.Public)
                .DefinedName("Name")
                .DefinedType<string>()
                .Script;

            Assert.Equal($"[Test]{Environment.NewLine}public System.String Name;", result);


        }



        [Fact(DisplayName = "普通字段2")]
        public void Test4()
        {

            FieldBuilder template = new FieldBuilder();
            var result = template
                .Attribute<ClassDataAttribute>()
                .Access(Natasha.Reverser.Model.AccessTypes.Public)
                .DefinedName("Name")
                .DefinedType<string>()
                .Script;

            Assert.Equal($"[Xunit.ClassDataAttribute]{Environment.NewLine}public System.String Name;", result);


        }

    }
}
