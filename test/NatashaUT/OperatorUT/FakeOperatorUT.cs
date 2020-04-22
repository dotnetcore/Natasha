using Natasha.CSharp.Operator;
using NatashaUT.Model;
using Xunit;

namespace NatashaUT.OperatorUT
{

    [Trait("伪造委托构建与编译", "")]
    public class FakeOperatorUT
    {

        [Fact(DisplayName = "普通函数克隆")]
        public static void MakerCode1()
        {

            var builder = FakeMethodOperator.RandomDomain();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite1"))
                .Methodbody(@"Console.WriteLine(""hello world"");");
            Assert.Equal($@"public void ReWrite1(){{Console.WriteLine(""hello world"");}}", builder.Script);
            
        }



        [Fact(DisplayName = "静态普通函数克隆")]
        public static void MakerStaticCode1()
        {

            var builder = FakeMethodOperator.RandomDomain();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite1"))
                .StaticMethodBody(@"Console.WriteLine(""hello world"");");
            Assert.Equal($@"public static void ReWrite1(){{Console.WriteLine(""hello world"");}}", builder.Script);
            Assert.NotNull(builder.Compile());

        }




        [Fact(DisplayName = "特殊函数克隆")]
        public static void MakerCode2()
        {

            var builder = FakeMethodOperator.RandomDomain();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite2"))
                .Methodbody(@"Console.WriteLine(""hello world"");return this;");
            Assert.Equal($@"public async System.Threading.Tasks.Task<NatashaUT.Model.OopTestModel> ReWrite2(){{Console.WriteLine(""hello world"");return this;}}", builder.Script);

        }




        [Fact(DisplayName = "静态特殊函数克隆")]
        public static void MakerStaticCode2()
        {
            var builder = FakeMethodOperator.RandomDomain()
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite2"))
                .StaticMethodBody(@"Console.WriteLine(""hello world"");return default;");
            Assert.Equal($@"public static async System.Threading.Tasks.Task<NatashaUT.Model.OopTestModel> ReWrite2(){{Console.WriteLine(""hello world"");return default;}}", builder.Script);
            Assert.NotNull(builder.Compile());

        }




        [Fact(DisplayName = "虚函数克隆")]
        public static void MakerCode3()
        {

            var builder = FakeMethodOperator.RandomDomain();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite3"))
                .Methodbody(@"i++;temp+=i.ToString();");
            Assert.Equal($@"public virtual void ReWrite3(ref System.Int32 i,System.String temp){{i++;temp+=i.ToString();}}", builder.Script);

        }



        private delegate void TestDelegate(ref int i, string temp);
        [Fact(DisplayName = "静态虚函数克隆态")]
        public static void MakerStaticCode3()
        {

            var builder = FakeMethodOperator.RandomDomain();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite3"))
                .StaticMethodBody(@"i++;temp+=i.ToString();");
            Assert.Equal($@"public static void ReWrite3(ref System.Int32 i,System.String temp){{i++;temp+=i.ToString();}}", builder.Script);
            Assert.NotNull(builder.Compile<TestDelegate>());

        }

    }
}
