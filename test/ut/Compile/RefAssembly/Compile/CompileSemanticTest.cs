using Microsoft.CodeAnalysis.CSharp;
using System;
using Xunit;

namespace RefAssembly.Script.Reverser
{
    [Trait("基础编译(REF)", "编译")]
    public class CompileSemanticTest : CompilePrepareBase
    {
        [Fact(DisplayName = "[语义过滤]编译测试")]
        public void SemanticTest1()
        {
            var code = NatashaLoadContext.DefaultContext.UsingRecorder.ToString() + "using abcde;public class A{ public string Name;}";
            AssemblyCSharpBuilder builder = new()
            {
                LoadContext = DomainManagement.Random()
            };
            builder.UseSmartMode();
            builder.EnableSemanticHandler = false;
            builder.Add(code);
            //Assert.Equal(DefaultUsing.Count, builder.SyntaxTrees[0].GetCompilationUnitRoot().Usings.Count -1);

            try
            {
                var assemly = builder.GetAssembly();
                Assert.True(false);
            }
            catch (Exception ex)
            {
                var nex = ex as NatashaException;
                Assert.NotNull(nex);
                Assert.Equal(NatashaExceptionKind.Compile, nex!.ErrorKind);
                //Assert.Equal(DefaultUsingCount, builder.SyntaxTrees[0].GetCompilationUnitRoot().Usings.Count -1);
                //Assert.Equal(DefaultUsingCount, DefaultUsing.Count);
            }
            builder.EnableSemanticHandler = true;
            var succeedAssembly = builder.GetAssembly();
            Assert.NotNull(succeedAssembly);
            Assert.Equal(builder.AssemblyName,succeedAssembly.GetName().Name);
            //Assert.Equal(DefaultUsingCount, DefaultUsing.Count);
            //Assert.NotEqual(DefaultUsingCount, builder.SyntaxTrees[0].GetCompilationUnitRoot().Usings.Count);
            Assert.Empty(builder.Compilation!.SyntaxTrees[0].GetCompilationUnitRoot().Usings);

        }
    }
}
