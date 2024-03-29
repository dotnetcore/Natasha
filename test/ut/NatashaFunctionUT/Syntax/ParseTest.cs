﻿using System;
using Xunit;

namespace NatashaFunctionUT.Syntax
{
    [Trait("基础功能测试", "语法")]
    public class ParseTest : DomainPrepareBase
    {
        private NatashaException? CatchTreeError(string code)
        {
            try
            {
                AssemblyCSharpBuilder builder = new();
                builder.WithoutCombineUsingCode();
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

//        [Fact(DisplayName = "访问性检查")]
//        public void AccessTest()
//        {

//            var source = @"
//public class A{ private string Name =""1"";}
//public class B{ private string Show(){ return (new A()).Name; }}
//";

//            AssemblyCSharpBuilder cSharpBuilder = new();
//            cSharpBuilder.UseRandomDomain();
//            cSharpBuilder.ConfigCompilerOption(item => item.WithCompilerFlag(Natasha.CSharp.Compiler.CompilerBinderFlags.None));
//            cSharpBuilder.WithSemanticCheck();
//            cSharpBuilder.AddReference(typeof(object));
//            cSharpBuilder.WithAnalysisAccessibility();
//            cSharpBuilder.Add(source);
//            var asm = cSharpBuilder.GetAssembly();
//            Assert.NotNull(asm);
//            //Assert.Equal(NatashaExceptionKind.Syntax, ex.ErrorKind);
//        }
    }
}
