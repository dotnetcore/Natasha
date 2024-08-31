using Natasha.CSharp;
using NatashaUT.Model;
using System;
using Xunit;


namespace NatashaUT
{

    [Trait("快速构建", "完整类")]
    public class SyntaxFormartTest : PrepareTest
    {


       
        [Fact(DisplayName = "字符串格式化测试1")]
        public void Formart2()
        {

            var content = @"unsafe class C
{
    delegate * < int,  int> functionPointer;
}";

            var expected = @"unsafe class C
{
    delegate*<int, int> functionPointer;
}";


            NatashaCSharpSyntax syntax = new NatashaCSharpSyntax();
            syntax.AddTreeToCache(content);
            var result = syntax.TreeCache[expected].ToString();
            Assert.Equal(expected, result);
        }
        [Fact(DisplayName = "字符串格式化测试2")]
        public void Formart3()
        {

            var content = @"class A
            {        
int             i               =               20          ;           int             j           =           1           +           2       ;
                        T           .               S           =           Test            (           10              )           ;
                        }";

            var expected = $"class A{Environment.NewLine}{{{Environment.NewLine}    int i = 20;{Environment.NewLine}    int j = 1 + 2;{Environment.NewLine}    T.S =  Test( 10);{Environment.NewLine}}}";

            NatashaCSharpSyntax syntax = new NatashaCSharpSyntax();
            syntax.AddTreeToCache(content);
            var result = syntax.TreeCache[expected].ToString();
            Assert.Equal(expected, result);
        }




        //        [Fact(DisplayName = "字符串格式化测试3")]
        //        public void RunClassName7()
        //        {

        //            var initial =
        //@"using A = B;
        //using C;
        //using D = E;
        //using F;";

        //            var final =
        //    @"using C;
        //using F;
        //using A = B;
        //using D = E;
        //";

        //            NatashaCSharpSyntax syntax = new NatashaCSharpSyntax();
        //            syntax.AddTreeToCache(initial);
        //            var result = syntax.TreeCache[final].ToString();
        //            Assert.Equal(final, result);
        //        }


        [Fact(DisplayName = "字符串格式化测试4")]
        public void Formart4()
        {

            var initial = "int i=0 ; var t=new{Name=\"\"};";

            var final = $"int i = 0;{Environment.NewLine}var t = new{Environment.NewLine}{{{Environment.NewLine}Name = \"\"{Environment.NewLine}}}{Environment.NewLine}{Environment.NewLine};";

            NatashaCSharpSyntax syntax = new NatashaCSharpSyntax();
            syntax.AddTreeToCache(initial);
            var result = syntax.TreeCache[final].ToString();
            Assert.Equal(final, result);
        }

        [Fact(DisplayName = "字符串格式化测试5")]
        public void Formart5()
        {
            string pre = @"
using ReferenceTest50;
 public class A {   
    /// <summary>
    ///  comment will raise a error.
    /// </summary>
    public string Name{get;set;}
    /// <summary>
    ///  comment will raise a error.
    /// </summary>
    public Test test;
}";

            string after = @"using ReferenceTest50;

public class A
{
    /// <summary>
    ///  comment will raise a error.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///  comment will raise a error.
    /// </summary>
    public Test test;
}";

            NatashaCSharpSyntax syntax = new NatashaCSharpSyntax();
            syntax.AddTreeToCache(pre);
            var result = syntax.TreeCache[after].ToString();
            Assert.Equal(after, result);
        }
    }
}
