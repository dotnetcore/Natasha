using System;
using Xunit;

namespace FrameworkFunctionUT.Syntax
{
    [Trait("基础功能测试", "语法")]
    public class FormartTest
    {
        private string FormartString(string code)
        {
            AssemblyCSharpBuilder builder = new();
            builder.Add(code);
            return builder.SyntaxTrees[0].ToString();

        }


        [Fact(DisplayName = "字符串格式化测试1")]
        public void Formart1()
        {

            var source = @"unsafe class C
{
    delegate * < int,  int> functionPointer;
}";

            var expected = @"unsafe class C
{
    delegate*<int, int> functionPointer;
}";


            OSStringCompare.Equal(expected, FormartString(source));
        }
        [Fact(DisplayName = "字符串格式化测试2")]
        public void Formart2()
        {

            var source = @"class A
            {        
int             i               =               20          ;           int             j           =           1           +           2       ;
                        int                     Test            (           int   g              )   {       
return
g
;
}
                        }";

            var expected = @"class A
{
    int i = 20;
    int j = 1 + 2;
    int Test(int g)
    {
        return g;
    }
}";
            OSStringCompare.Equal(expected, FormartString(source));

        }


        [Fact(DisplayName = "字符串格式化测试3")]
        public void Formart3()
        {

            var source = "int i=0 ; var t=new{Name=\"\"};";

            var expected = $"int i = 0;{Environment.NewLine}var t = new{Environment.NewLine}{{{Environment.NewLine}    Name = \"\"{Environment.NewLine}}};";

            OSStringCompare.Equal(expected, FormartString(source));

        }

        [Fact(DisplayName = "字符串格式化测试4")]
        public void Formart4()
        {
            string source = @"
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

            OSStringCompare.Equal(after, FormartString(source));
        }
    }
}
