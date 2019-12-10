using Natasha;
using Natasha.Operator;
using NatashaUT.Model;
using System;
using Xunit;

namespace NatashaUT
{
    [Trait("快速构建","函数")]
    public class DynamicMethodTest
    { 

        [Fact(DisplayName = "手动强转委托")]
        public static void RunDelegate1()
        {
            var delegateAction = FastMethodOperator
                .Create()
                        .Param<string>("str1")
                        .Param<string>("str2")
                        .MethodBody(@"
                            string result = str1 +"" ""+ str2;
                            Console.WriteLine(result);
                            return result;")
                        .Return<string>()
                .Complie();

           string result = ((Func<string, string, string>)delegateAction)?.Invoke("Hello", "World1!");
           Assert.Equal("Hello World1!", result);
        }



        [Fact(DisplayName = "内部类委托")]
        public static void RunInnerDelegate()
        {
            var delegateAction = FastMethodOperator
                .Create()
                        .MethodBody(@"
                           OopTestModel.InnerClass a = new OopTestModel.InnerClass();
                            a.Name =""abc"";
                            return a;")
                        .Return<OopTestModel.InnerClass>()
                .Complie();

            var result = ((Func<OopTestModel.InnerClass>)delegateAction)?.Invoke();
            Assert.Equal("abc", result.Name);
        }




        //[Fact(DisplayName = "扩展强转委托")]
        //public static void RunDelegate3()
        //{
        //    var delegateAction = @"
        //                    string result = str1 +"" ""+ str2;
        //                    Console.WriteLine(result);
        //                    return result;"
        //                .FastOperator()
        //                .Param<string>("str1")
        //                .Param<string>("str2")
        //                .Return<string>()
        //        .Complie();

        //    string result = ((Func<string, string, string>)delegateAction)?.Invoke("Hello", "World1!");
        //    Assert.Equal("Hello World1!", result);
        //}





        [Fact(DisplayName = "NFunc委托")]
        public static void RunDelegate5()
        {
            var action = NDomain.Random().UnsafeFunc<string, string, string>(@"
                            string result = arg1 +"" ""+ arg2;
                            Console.WriteLine(result);
                            return result;");

            string result = action("Hello", "World1!");
            Assert.Equal("Hello World1!", result);
        }




        [Fact(DisplayName = "自动泛型委托")]
        public static void RunDelegate2()
        {
            var delegateAction = FastMethodOperator
                .Create()
                        .Param<string>("str1")
                        .Param<string>("str2")
                        .MethodBody(@"
                            string result = str1 +"" ""+ str2;
                            Console.WriteLine(result);
                            return result;")
                        .Return<string>()
                .Complie<Func<string, string, string>>();

            string result = delegateAction?.Invoke("Hello", "World2!");
           Assert.Equal("Hello World2!",result);
        }




        //[Fact(DisplayName = "扩展泛型委托")]
        //public static void RunDelegate4()
        //{
        //    var delegateAction = @"
        //                    string result = str1 +"" ""+ str2;
        //                    Console.WriteLine(result);
        //                    return result;"
        //                .FastOperator()
        //                .Param<string>("str1")
        //                .Param<string>("str2")
        //                .Return<string>()
        //        .Complie<Func<string, string, string>>();

        //    string result = delegateAction?.Invoke("Hello", "World2!");
        //    Assert.Equal("Hello World2!", result);
        //}




        [Fact(DisplayName = "函数克隆1")]
        public static void MakerCode1()
        {

            var builder = FakeMethodOperator.Create();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite1"))
                .MethodContent(@"Console.WriteLine(""hello world"");")
                .Builder();
           Assert.Equal($@"public void ReWrite1()
{{
Console.WriteLine(""hello world"");{Environment.NewLine}
}}", builder.MethodScript);
        }



        [Fact(DisplayName = "函数克隆1-静态")]
        public static void MakerStaticCode1()
        {

            var builder = FakeMethodOperator.Create();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite1"))
                .StaticMethodContent(@"Console.WriteLine(""hello world"");")
                .Builder();
            Assert.Equal($@"public static void ReWrite1()
{{
Console.WriteLine(""hello world"");{Environment.NewLine}
}}", builder.MethodScript);
        }




        [Fact(DisplayName = "函数克隆2")]
        public static void MakerCode2()
        {
            var builder = FakeMethodOperator.Create();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite2"))
                .MethodContent(@"Console.WriteLine(""hello world"");return this;")
                .Builder();
            Assert.Equal($@"public async Task<OopTestModel> ReWrite2()
{{
Console.WriteLine(""hello world"");return this;{Environment.NewLine}
}}", builder.MethodScript);
        }




        [Fact(DisplayName = "函数克隆2-静态")]
        public static void MakerStaticCode2()
        {
            var builder = FakeMethodOperator.Create()
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite2"))
                .StaticMethodContent(@"Console.WriteLine(""hello world"");return this;")
                .Builder();
            Assert.Equal($@"public static async Task<OopTestModel> ReWrite2()
{{
Console.WriteLine(""hello world"");return this;{Environment.NewLine}
}}", builder.MethodScript);
        }




        [Fact(DisplayName = "函数克隆3")]
        public static void MakerCode3()
        {
            var builder = FakeMethodOperator.Create();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite3"))
                .MethodContent(@"i++;temp+=i.ToString();")
                .Builder();
            Assert.Equal($@"public virtual void ReWrite3(ref Int32 i,String temp)
{{
i++;temp+=i.ToString();{Environment.NewLine}
}}", builder.MethodScript);
        }




        [Fact(DisplayName = "函数克隆3-静态")]
        public static void MakerStaticCode3()
        {
            var builder = FakeMethodOperator.Create();
            builder
                .UseMethod(typeof(OopTestModel).GetMethod("ReWrite3"))
                .StaticMethodContent(@"i++;temp+=i.ToString();")
                .Builder();
            Assert.Equal($@"public static void ReWrite3(ref Int32 i,String temp)
{{
i++;temp+=i.ToString();{Environment.NewLine}
}}", builder.MethodScript);
        }

    }

}
