﻿using Natasha.CSharp.Builder;
using System;
using Xunit;

namespace NatashaFunctionUT.Template
{

    [Trait("基础功能测试", "模板")]
    public class FieldTemplateTest:DomainPrepareBase
    {

        [Fact(DisplayName = "静态字段1")]
        public void Test1()
        {

            FieldBuilder template = new();
            var result = template
                .AttributeAppend("[Test]")
                .Access("public")
                .Modifier("static")
                .Name("Name")
                .Type<string>()
                .GetScript();

            Assert.Equal($"[Test]{Environment.NewLine}public static System.String Name;", result);

        }





        [Fact(DisplayName = "静态字段2")]
        public void Test2()
        {

            FieldBuilder template = new();
            var result = template
                .AttributeAppend("[Test][Test1]")
                .Access(AccessFlags.Public)
                .Modifier( ModifierFlags.Static)
                .Name("Age")
                .Type(typeof(int))
                .GetScript();

            Assert.Equal($"[Test][Test1]{Environment.NewLine}public static System.Int32 Age;", result);


        }




        [Fact(DisplayName = "普通字段1")]
        public void Test3()
        {

            FieldBuilder template = new();
            var result = template
                .AttributeAppend("[Test]")
                .Access(AccessFlags.Public)
                .Name("Name")
                .Type<string>()
                .GetScript();

            Assert.Equal($"[Test]{Environment.NewLine}public System.String Name;", result);


        }



        [Fact(DisplayName = "普通字段2")]
        public void Test4()
        {

            FieldBuilder template = new();
            var result = template
                .AttributeAppend<ClassDataAttribute>()
                .Access(AccessFlags.Public)
                .Name("Name")
                .Type<string>()
                .GetScript();

            Assert.Equal($"[Xunit.ClassDataAttribute]{Environment.NewLine}public System.String Name;", result);


        }


        [Fact(DisplayName = "只读静态字段2")]
        public void Test5()
        {

            FieldBuilder template = new();
            var result = template
                .AttributeAppend<ClassDataAttribute>()
                .Access(AccessFlags.Public)
                .Static()
                .ModifierAppend("readonly")
                .Name("Name")
                .Type<string>()
                .GetScript();

            Assert.Equal($"[Xunit.ClassDataAttribute]{Environment.NewLine}public static readonly System.String Name;", result);


        }

    }
}
