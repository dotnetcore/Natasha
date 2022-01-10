using Natasha.CSharp.Builder;
using System;
using Xunit;

namespace NatashaFunctionUT.Template
{

    [Trait("基础功能测试", "模板")]
    public class MethodTemplateTest
    {

        [Fact(DisplayName = "静态方法1")]
        public void Test1()
        {

            MethodBuilder template = MethodBuilder.RandomDomain();
            var result = template
                .AttributeAppend("[Test]")
                .Access("public")
                .Modifier("static")
                .Name("Name")
                .Type<string>()
                .GetScript();

            Assert.Equal($"[Test]{Environment.NewLine}public static System.String Name(){{}}", result);
            Assert.Equal(typeof(Func<string>), template.DelegateType);

        }





        [Fact(DisplayName = "静态方法2")]
        public void Test2()
        {

            MethodBuilder template = MethodBuilder.RandomDomain();
            var result = template
                .AttributeAppend("[Test][Test1]")
                .Access(AccessFlags.Public)
                .Modifier(ModifierFlags.Static)
                .Name("Age")
                .Param<int>("p")
                .Return<int>()
                .Body("return 1;")
                .GetScript();

            Assert.Equal($"[Test][Test1]{Environment.NewLine}public static System.Int32 Age(System.Int32 p){{return 1;}}", result);
            Assert.Equal(typeof(Func<int,int>), template.DelegateType);

        }




        [Fact(DisplayName = "普通方法1")]
        public void Test3()
        {

            MethodBuilder template = MethodBuilder.RandomDomain();
            var result = template
                .AttributeAppend("[Test]")
                .Async()
                .Access(AccessFlags.Public)
                .Name("Name")
                .Return(typeof(MethodBuilder))
                .Body("int a = 0;")
                .BodyAppend("return \"xm\";")
                .GetScript();

            Assert.Equal($"[Test]{Environment.NewLine}public async Natasha.CSharp.Builder.MethodBuilder Name(){{int a = 0;return \"xm\";}}", result);
            Assert.Equal(typeof(Func<MethodBuilder>), template.DelegateType);

        }



        [Fact(DisplayName = "普通方法2")]
        public void Test4()
        {

            MethodBuilder template = MethodBuilder.RandomDomain();
            var result = template
                .AttributeAppend<ClassDataAttribute>()
                .Unsafe()
                .Async()
                .Constraint("where T : class")
                .Access(AccessFlags.Public)
                .Name("Name")
                .GetScript();

            Assert.Equal($"[Xunit.ClassDataAttribute]{Environment.NewLine}public unsafe async void Name() where T : class{{}}", result);
            Assert.Equal(typeof(Action), template.DelegateType);

        }



        [Fact(DisplayName = "初始化方法1")]
        public void Test5()
        {

            MethodBuilder template = MethodBuilder.RandomDomain();
            var result = template
                .Access(AccessFlags.Public)
                .Name("Name")
                .NoUseType()
                .GetScript();

            Assert.Equal($"public Name(){{}}", result);
            Assert.Equal(typeof(Action), template.DelegateType);

        }

        [Fact(DisplayName = "静态初始化方法1")]
        public void Test6()
        {

            MethodBuilder template = MethodBuilder.RandomDomain();
            var result = template
                .Modifier(ModifierFlags.Static)
                .Name("Name")
                .NoUseType()
                .GetScript();

            Assert.Equal($"static Name(){{}}", result);
            Assert.Equal(typeof(Action), template.DelegateType);

        }

        [Fact(DisplayName = "静态初带参数的始化方法1")]
        public void Test7()
        {

            MethodBuilder template = MethodBuilder.RandomDomain();
            var result = template
                .Modifier(ModifierFlags.Static)
                .Name("Name")
                .NoUseType()
                .Param<int>("age")
                .Param<string>("name")
                .GetScript();

            Assert.Equal($"static Name(System.Int32 age,System.String name){{}}", result);
            Assert.Equal(typeof(Action<int,string>), template.DelegateType);

        }

        [Fact(DisplayName = "泛型方法")]
        public void Test8()
        {

            MethodBuilder template = MethodBuilder.RandomDomain();
            var result = template
                .Modifier(ModifierFlags.Static)
                .Name("Name<T,S>")
                .Type("(T key,S value)")
                .Param("T t")
                .Param("S s")
                .GetScript();

            Assert.Equal($"static (T key,S value) Name<T,S>(T t,S s){{}}", result);
            

        }

    }

}
