using Natasha.CSharp.Compiler;
using System;
using System.Collections.Generic;
using System.Text;

namespace RefAssembly.Compile.DuplicatedTypes
{
    [Trait("基础编译(REF) TODO", "重复类型")]
    public class DuplicatedTypesTest : CompilePrepareBase
    {
        //TODO: 重复类型测试
        [Fact(DisplayName = "禁用")]
        public void DuplicateTest1()
        {
            string script = @"
namespace DupliNamespace
{
    public class DuplicateATest
    {
        private string Name=""t"";
        public string Show(string str)
        {
            return str + Name + typeof(Console).ToString();
        }
    }
}";
            var asm = script
                .GetAssemblyForUT(builder => builder
                    .UseNewLoadContext("duptest1")
                    .UseSmartMode()
                    .ConfigCompilerOption(opt => opt.WithLowerVersionsAssembly().WithCompilerFlag(CompilerBinderFlags.SuppressConstraintChecks)));
            var type1 = asm.GetTypeFromShortName("DuplicateATest");
            var methodInfo = asm.GetMethodFromShortName("DuplicateATest", "Show");

            Assert.Equal("strtSystem.Console", methodInfo.Invoke(Activator.CreateInstance(type1), new object[] { "str" }));

            asm = script
                .GetAssemblyForUT(builder => builder
                    .UseNewLoadContext("duptest1")
                    .UseSmartMode()
                    .ConfigCompilerOption(opt => opt.WithLowerVersionsAssembly().WithCompilerFlag(CompilerBinderFlags.SuppressConstraintChecks)));
            var type2 = asm.GetTypeFromShortName("DuplicateATest");
            methodInfo = asm.GetMethodFromShortName("DuplicateATest", "Show");

            Assert.Equal("strtSystem.Console", methodInfo.Invoke(Activator.CreateInstance(type2), new object[] { "str" }));
            Assert.True(type1 != type2);
            Assert.Equal(type1.GetDomain(), type2.GetDomain());
        }

        [Fact(DisplayName = "启用")]
        public void DuplicateTest2()
        {
            string script = @"
namespace DupliNamespace
{
    public class DuplicateATest
    {
        private string Name=""t"";
        public string Show(string str)
        {
            return str + Name + typeof(Console).ToString();
        }
    }
}";
            var asm = script
                .GetAssemblyForUT(builder => builder
                    .UseNewLoadContext("duptest1")
                    .UseSmartMode()
                    .ConfigCompilerOption(opt => opt.WithLowerVersionsAssembly().WithCompilerFlag(CompilerBinderFlags.IgnoreCorLibraryDuplicatedTypes)));
            var type1 = asm.GetTypeFromShortName("DuplicateATest");
            var methodInfo = asm.GetMethodFromShortName("DuplicateATest", "Show");

            Assert.Equal("strtSystem.Console", methodInfo.Invoke(Activator.CreateInstance(type1), new object[] { "str" }));

            asm = script
                .GetAssemblyForUT(builder => builder
                    .UseNewLoadContext("duptest1")
                    .UseSmartMode()
                    .ConfigCompilerOption(opt => opt.WithLowerVersionsAssembly().WithCompilerFlag(CompilerBinderFlags.IgnoreCorLibraryDuplicatedTypes)));
            var type2 = asm.GetTypeFromShortName("DuplicateATest");
            methodInfo = asm.GetMethodFromShortName("DuplicateATest", "Show");

            Assert.Equal("strtSystem.Console", methodInfo.Invoke(Activator.CreateInstance(type2), new object[] { "str" }));
            Assert.True(type1 != type2);
            Assert.Equal(type1.GetDomain(), type2.GetDomain());

        }
    }

    public class DuplicateATest
    {
        public string Show()
        {
            return typeof(Console).ToString();
        }
    }
}
