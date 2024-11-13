using MemAssembly.Compile.Semantic.Utils;

namespace MemAssembly.Compile.Semantic
{
    [Trait("基础编译(REF)", "语义过滤")]
    public class SemanticTest : CompilePrepareBase
    {
        [Fact(DisplayName = "CS8019")]
        public void SemanticTest1()
        {
            var script = "using System.IO; public class A{}";
            var expect = @"
public class A
{
}";
            var result = SemanticHelper.GetScript(script);
            OSStringCompare.Equal(expect, result.script);
            Assert.Equal("CS8019", result.id);
        }

        [Fact(DisplayName = "CS0246")]
        public void SemanticTest2()
        {
            var script = "using abc; public class A{}";
            var expect = @"
public class A
{
}";
            var result = SemanticHelper.GetScript(script);
            OSStringCompare.Equal(expect, result.script);
            Assert.Equal("CS0246", result.id);
        }

        [Fact(DisplayName = "CS0234")]
       public void SemanticTest3()
        {
            var script = "using System.IO1; public class A{}";
            var expect = @"
public class A
{
}";
            var result = SemanticHelper.GetScript(script);
            OSStringCompare.Equal(expect, result.script);
            Assert.Equal("CS0234", result.id);
        }
    }
}
