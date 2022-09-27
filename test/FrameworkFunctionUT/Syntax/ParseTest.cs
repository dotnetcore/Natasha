using System;
using Xunit;

namespace FrameworkFunctionUT.Syntax
{
    [Trait("基础功能测试", "语法")]
    public class ParseTest
    {
        private NatashaException? CatchTreeError(string code)
        {
            try
            {
                AssemblyCSharpBuilder builder = new();
                builder.Add(code);
            }
            catch (Exception ex)
            {
                return ex as NatashaException;
            }
            return new NatashaException("a");
        }


        [Fact(DisplayName = "语法树异常")]
        public void Formart1()
        {

            var source = @"unsafe class C
{
    delegate * < int,  int> functionPointer 1;
}";

            var expected = @"unsafe class C
{
    delegate*<int, int> functionPointer 1 ;
}";

            var ex = CatchTreeError(source)!;
            Assert.NotNull(ex);
            OSStringCompare.Equal(expected, ex.Formatter);
            Assert.Equal(NatashaExceptionKind.Syntax, ex.ErrorKind);
        }
    }
}
