using Natasha.CSharp;
using NatashaUT.Model;
using Xunit;

namespace NatashaUT.OperatorUT
{

    [Trait("伪造委托构建与编译", "")]
    public class FakeOperatorUT : PrepareTest
    {

        [Fact(DisplayName = "普通函数克隆")]
        public void MakerCode1()
        {

            var builder = FakeMethodOperator.RandomDomain();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite1")!)
                .MethodBody(@"Console.WriteLine(""hello world"");");
            Assert.Equal($@"public void ReWrite1(){{Console.WriteLine(""hello world"");}}", builder.Script);
            
        }



        [Fact(DisplayName = "静态普通函数克隆")]
        public void MakerStaticCode1()
        {

            var builder = FakeMethodOperator.RandomDomain();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite1")!)
                .StaticMethodBody(@"Console.WriteLine(""hello world"");");
            Assert.Equal($@"public static void ReWrite1(){{Console.WriteLine(""hello world"");}}", builder.Script);
            Assert.NotNull(builder.Compile());

        }




        [Fact(DisplayName = "特殊函数克隆")]
        public void MakerCode2()
        {

            var builder = FakeMethodOperator.RandomDomain();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite2")!)
                .MethodBody(@"Console.WriteLine(""hello world"");return this;");
            Assert.Equal($@"public async System.Threading.Tasks.Task<NatashaUT.Model.OopTestModel> ReWrite2(){{Console.WriteLine(""hello world"");return this;}}", builder.Script);

        }




        [Fact(DisplayName = "静态特殊函数克隆")]
        public void MakerStaticCode2()
        {
            var builder = FakeMethodOperator.RandomDomain()
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite2")!)
                .StaticMethodBody(@"Console.WriteLine(""hello world"");return default;");
            Assert.Equal($@"public static async System.Threading.Tasks.Task<NatashaUT.Model.OopTestModel> ReWrite2(){{Console.WriteLine(""hello world"");return default;}}", builder.Script);
            Assert.NotNull(builder.Compile());

        }




        [Fact(DisplayName = "虚函数克隆")]
        public void MakerCode3()
        {

            var builder = FakeMethodOperator.RandomDomain();
            builder
                .Virtrual()
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite3")!)
                .MethodBody(@"i++;temp+=i.ToString();");
            Assert.Equal($@"public virtual void ReWrite3(ref System.Int32 i,System.String temp){{i++;temp+=i.ToString();}}", builder.Script);

        }



        private delegate void TestDelegate(ref int i, string temp);
        [Fact(DisplayName = "静态虚函数克隆态")]
        public void MakerStaticCode3()
        {

            var builder = FakeMethodOperator.RandomDomain();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite3")!)
                .StaticMethodBody(@"i++;temp+=i.ToString();");
            Assert.Equal($@"public static void ReWrite3(ref System.Int32 i,System.String temp){{i++;temp+=i.ToString();}}", builder.Script);
            Assert.NotNull(builder.Compile<TestDelegate>());

        }

        private delegate ref int TestDelegate1(ref int i,out string temp);
        [Fact(DisplayName = "静态虚函数克隆态")]
        public void MakerStaticCode4()
        {

            var builder = FakeMethodOperator.RandomDomain();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite4")!)
                .StaticMethodBody(@"temp = default;return ref i;");
            Assert.Equal($@"public static ref System.Int32 ReWrite4(ref System.Int32 i,out System.String temp){{temp = default;return ref i;}}", builder.Script);
            Assert.NotNull(builder.Compile<TestDelegate1>());

        }
    }
}
