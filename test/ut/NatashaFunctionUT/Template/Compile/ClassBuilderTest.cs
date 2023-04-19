using Natasha.CSharp;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace NatashaFunctionUT.Template.Compile
{

    [Trait("高级API功能测试", "OOP")]
    public class ClassBuilderTest : DomainPrepare
    {
        [Fact(DisplayName = "类构建与编译1")]
        public void TestClass()
        {
            NClass builder = NClass.RandomDomain();
            var type = builder
                .NoGlobalUsing()
                .HiddenNamespace()
                .Access(AccessFlags.Public)
                .Name("EnumUT1")
                .Field(item=> { item.Public().Name("Apple").Type<int>(); })
                .Field(item => { item.Public().Name("Orange").Type<string>(); })
                .Property(item => { item.Public().Name("Banana").Type<NClass>(); })
                .GetType();
            var script = builder.AssemblyBuilder.Compilation!.SyntaxTrees[0].ToString();
            var expected = @"
public class EnumUT1
{
    public System.Int32 Apple;
    public System.String Orange;
    public NClass Banana { get; set; }
}";
            OSStringCompare.Equal(expected, script);
            Assert.NotNull(type);
        }




        [Fact(DisplayName = "类构建与编译2")]
        public void TestClass1()
        {

            NClass builder = NClass.RandomDomain();
            var type = builder
                .NoGlobalUsing()
                .HiddenNamespace()
                .Summary("zhushi")
                .Access(AccessFlags.Public)
                .Name("EnumUT1")
                .Method(item => { item.Public().Name("Apple").Type<int>().Body("return 0;"); })
                .Property(item => { item
                    .Public()
                    .Name("Banana")
                    .Type<NClass>()
                    .Setter("int a = value.ToString().Length;")
                    .Getter("return default;"); })
                .GetType();
            var script = builder.AssemblyBuilder.Compilation!.SyntaxTrees[0].ToString();
            var expected = @"
/// <summary>
/// zhushi
/// </summary>
public class EnumUT1
{
    public System.Int32 Apple()
    {
        return 0;
    }

    public NClass Banana
    {
        get
        {
            return default;
        }

        set
        {
            int a = value.ToString().Length;
        }
    }
}";
            OSStringCompare.Equal(expected, script);
            Assert.NotNull(type);

        }

        [Fact(DisplayName = "类构建与编译3")]
        public void TestClass2()
        {

            NClass builder = NClass.RandomDomain();

            builder.Method(mb=> mb.Public().Name("Apple").Type<int>().Body("return 0;"));
            var pb = builder.Property(pb => pb.Public()
            .Name("Banana")
            .Type<NClass>()
            .Setter("int a = value.ToString().Length;")
            .Getter("return default;"));


            var type = builder
                .NoGlobalUsing()
                .HiddenNamespace()
                .Access(AccessFlags.Public)
                .Name("EnumUT1")
                .GetType();


            var script = builder.AssemblyBuilder.Compilation!.SyntaxTrees[0].ToString();
            var expected = @"
public class EnumUT1
{
    public System.Int32 Apple()
    {
        return 0;
    }

    public NClass Banana
    {
        get
        {
            return default;
        }

        set
        {
            int a = value.ToString().Length;
        }
    }
}";
            OSStringCompare.Equal(expected, script);
            Assert.NotNull(type);

        }


        [Fact(DisplayName = "类构建与编译4")]
        public void TestClass3()
        {

            string expected = @"using NatashaFunctionUT.Template.Compile;

namespace NatashaDynimacSpace
{
    /// <summary>
    /// This is a test class;
    /// </summary>
    public class Nee7e202ee18c413dacae62af6b106c6e
    {
        public readonly System.Int32 ReadonlyField;
        public Nee7e202ee18c413dacae62af6b106c6e()
        {
            ReadonlyField = 10;
        }

        [MyTestAttribute]
        private System.String _name;
        [NatashaFunctionUT.Template.Compile.MyTestAttribute]
        public System.String NameProperty
        {
            get
            {
                return _name;
            }
        }

        public AnotherClass AnotherProperty { get; set; }

        public virtual async System.Threading.Tasks.Task<System.String> SetName(System.String name)
        {
            _name = name;
            return _name;
        }
    }

    public class AnotherClass
    {
    }
}";



            NClass builder = NClass.RandomDomain();

            var type = builder
                .Public()
                .Name("Nee7e202ee18c413dacae62af6b106c6e")
                .Summary("This is a test class;")

                .PublicReadonlyField<int>("ReadonlyField")
                .Ctor(item => item.Public().Body("ReadonlyField = 10;"))

                .PrivateField<string>("_name", "[MyTestAttribute]")
                .Property(item => item
                    .Public()
                    .Attribute<MyTestAttribute>()
                    .Type<string>()
                    .Name("NameProperty")
                    .OnlyGetter("return _name;"))

                .Property(item => item
                    .Public()
                    .Type("AnotherClass")
                    .Name("AnotherProperty"))

                .Method(item => item
                    .Public()
                    .Virtrual()
                    .Async()
                    .Name("SetName")
                    .Param<string>("name")
                    .Body(@"_name = name;
                            return _name;")
                    .Return<Task<string>>())

                .NamespaceBodyAppend("public class AnotherClass{}")
                .GetType();
            var actual = builder.AssemblyBuilder.Compilation!.SyntaxTrees[0].ToString();
            OSStringCompare.Equal(expected, actual);
            Assert.NotNull(type);
        }
    }

    public class MyTestAttribute : Attribute { }
}
