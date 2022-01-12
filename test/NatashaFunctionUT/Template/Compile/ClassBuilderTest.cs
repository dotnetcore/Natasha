using Natasha.CSharp;
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
            var script = builder.AssemblyBuilder.SyntaxTrees[0].ToString();
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
            var script = builder.AssemblyBuilder.SyntaxTrees[0].ToString();
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


            var script = builder.AssemblyBuilder.SyntaxTrees[0].ToString();
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
    }
}
