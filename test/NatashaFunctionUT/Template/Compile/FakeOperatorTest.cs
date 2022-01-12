using Natasha.CSharp;
using System.Threading.Tasks;
using Xunit;

namespace NatashaFunctionUT.Template.Compile
{

    [Trait("高级API功能测试", "模板")]
    public class FakeOperatorTest : DomainPrepare
    {

        [Fact(DisplayName = "普通函数克隆")]
        public void MakerCode1()
        {

            var builder = FakeMethodOperator.RandomDomain();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite1")!)
                .MethodBody(@"Console.WriteLine(""hello world"");");
            OSStringCompare.Equal($@"public void ReWrite1(){{Console.WriteLine(""hello world"");}}", builder.ScriptCache);
            
        }



        [Fact(DisplayName = "静态普通函数克隆")]
        public void MakerStaticCode1()
        {

            var builder = FakeMethodOperator.RandomDomain();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite1")!)
                .StaticMethodBody(@"Console.WriteLine(""hello world"");");
            Assert.NotNull(builder.Compile());
            OSStringCompare.Equal($@"public static void ReWrite1(){{Console.WriteLine(""hello world"");}}", builder.ScriptCache);
            

        }




        [Fact(DisplayName = "特殊函数克隆")]
        public void MakerCode2()
        {

            var builder = FakeMethodOperator.RandomDomain();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite2")!)
                .MethodBody(@"Console.WriteLine(""hello world"");return this;");
            OSStringCompare.Equal($@"public async System.Threading.Tasks.Task<NatashaFunctionUT.Template.Compile.OopTestModel> ReWrite2(){{Console.WriteLine(""hello world"");return this;}}", builder.ScriptCache);

        }




        [Fact(DisplayName = "静态特殊函数克隆")]
        public void MakerStaticCode2()
        {
            var builder = FakeMethodOperator.RandomDomain()
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite2")!)
                .StaticMethodBody(@"Console.WriteLine(""hello world"");return default;");
            Assert.NotNull(builder.Compile());
            OSStringCompare.Equal($@"public static async System.Threading.Tasks.Task<NatashaFunctionUT.Template.Compile.OopTestModel> ReWrite2(){{Console.WriteLine(""hello world"");return default;}}", builder.ScriptCache);


        }




        [Fact(DisplayName = "虚函数克隆")]
        public void MakerCode3()
        {

            var builder = FakeMethodOperator.RandomDomain();
            builder
                .Virtrual()
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite3")!)
                .MethodBody(@"i++;temp+=i.ToString();");
            OSStringCompare.Equal($@"public virtual void ReWrite3(ref System.Int32 i,System.String temp){{i++;temp+=i.ToString();}}", builder.ScriptCache);

        }



        private delegate void TestDelegate(ref int i, string temp);
        [Fact(DisplayName = "静态虚函数克隆态")]
        public void MakerStaticCode3()
        {

            var builder = FakeMethodOperator.RandomDomain();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite3")!)
                .StaticMethodBody(@"i++;temp+=i.ToString();");
            Assert.NotNull(builder.Compile<TestDelegate>());
            OSStringCompare.Equal($@"public static void ReWrite3(ref System.Int32 i,System.String temp){{i++;temp+=i.ToString();}}", builder.ScriptCache);


        }

        private delegate ref int TestDelegate1(ref int i,out string temp);
        [Fact(DisplayName = "静态虚函数克隆态")]
        public void MakerStaticCode4()
        {

            var builder = FakeMethodOperator.RandomDomain();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite4")!)
                .StaticMethodBody(@"temp = default;return ref i;");
            Assert.NotNull(builder.Compile<TestDelegate1>());
            OSStringCompare.Equal($@"public static ref System.Int32 ReWrite4(ref System.Int32 i,out System.String temp){{temp = default;return ref i;}}", builder.ScriptCache);

        }
    }

    public class OopTestModel
    {

        public void ReWrite1()
        {

        }
        public async Task<OopTestModel> ReWrite2()
        {
            return this;
        }

        public virtual void ReWrite3(ref int i, string temp)
        {

        }


        public virtual ref int ReWrite4(ref int i, out string temp)
        {
            temp = default!;
            return ref i;
        }

        public class InnerClass
        {

            public string? Name;

        }


    }
}
