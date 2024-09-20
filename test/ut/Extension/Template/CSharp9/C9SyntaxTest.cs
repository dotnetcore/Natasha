#if NET5_0_OR_GREATER

using Natasha.CSharp;
using System;
using System.Runtime.CompilerServices;
using Xunit;

namespace NatashaUT
{


    [Trait("CSharp9测试", "语法")]
    public class C9SyntaxTest : PrepareTest
    {
        [Fact(DisplayName = "Recoder 编译")]
        public void Test1()
        {

            string script = @"public record Person
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
}";
            AssemblyCSharpBuilder builder = new();
            builder.Domain = DomainManagement.Random;
            builder.Add(script);
            var type = builder.GetType();
            Assert.NotNull(type);

        }

        [Fact(DisplayName = "Recoder 脚本")]
        public void Test2()
        {

            var script = NRecord
                .RandomDomain()
                .Name("TRecord")
                .HiddenNamespace()
                .Public()
                .RecordProperty<string>("P1")
                .RecordProperty<int>("P2")
                .Script;
            Assert.Contains($@"public record TRecord{{{Environment.NewLine}public System.String P1{{{Environment.NewLine}get;{Environment.NewLine}init;{Environment.NewLine}}}{Environment.NewLine}public System.Int32 P2{{{Environment.NewLine}get;{Environment.NewLine}init;{Environment.NewLine}}}{Environment.NewLine}}}", script);

        }


        [Fact(DisplayName = "NoInit 脚本")]
        public void Test3()
        {

            var script = NRecord
                .RandomDomain()
                .Name("TRecord")
                .HiddenNamespace()
                .SkipInit()
                .Public()
                .RecordProperty<string>("P1")
                .RecordProperty<int>("P2")
                .Script;
            Assert.Contains(@$"SkipLocalsInit", script);

        }
        [Fact(DisplayName = "NoInit 类编译")]
        public void Test5()
        {

            var type = NClass
                .RandomDomain()
                .Name("TRecord")
                .HiddenNamespace()
                .SkipInit()
                .Public()
                .RecordProperty<string>("P1")
                .RecordProperty<int>("P2")
                .GetType();
            Assert.NotNull(type);

        }
        [Fact(DisplayName = "NoInit 方法编译")]
        public void Test4()
        {

            var func = NDelegate
                .RandomDomain()
                .SetMethod(item=>item.SkipInit())
                .Func<int>("return 1;");
            Assert.Equal(1, func());

        }


    }

    public interface a { }

    
    public record P2:a
    {
        private readonly int test;
        public string? FirstName { get; }
        public string? LastName { get; init; }

        public string? Set()
        {
            return LastName;
        }
    }
    public class P3
    {
        private readonly int test = 0;
        private string? _name;
        public string? FirstName { get { return _name; } init { _name = value; } }
        
    }

}
#endif