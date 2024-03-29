﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace NatashaFunctionUT.Compile
{
    [Trait("基础功能测试", "编译")]
    public class CompileUnitTest : CompilerPrepareBase
    {

        [Fact(DisplayName = "基础编译测试 - 程序集")]
        public void TestAssembly()
        {
            string code = @"public class A{public string Name=""HelloWorld"";}";

            AssemblyCSharpBuilder builder = new();
            builder.UseRandomDomain();
            builder.UseSmartMode();
            builder.Add(code);

            var assembly = builder.GetAssembly();
            Assert.NotNull(assembly);

            var type = assembly.GetTypes().Where(item => item.Name == "A").First();
            var info = type.GetField("Name");

            Assert.Equal("HelloWorld", info!.GetValue(Activator.CreateInstance(type)));
            Assert.NotEqual("Default", builder.Domain.Name);

        }

        [Fact(DisplayName = "基础编译测试 - 类型")]
        public void TestType()
        {
            string code = @"public class A{public string Name=""HelloWorld"";}";

            AssemblyCSharpBuilder builder = new();
            builder.UseRandomDomain();
            builder.UseSmartMode();
            builder.Add(code);

            var assembly = builder.GetAssembly();
            var type = assembly.GetTypeFromShortName("A");
            Assert.Equal("A", type.Name);

            var info = type.GetField("Name");
            Assert.Equal("HelloWorld", info!.GetValue(Activator.CreateInstance(type)));


            Assert.NotEqual("Default", builder.Domain.Name);
        }


        [Fact(DisplayName = "基础编译测试 - 委托")]
        public void TestDelegate()
        {
            string code = @"public class A{public string Name=""HelloWorld""; public static string Get(){  return (new A()).Name; }}";

            AssemblyCSharpBuilder builder = new("compileUntiTestFoDelegateAssembly");
            builder.UseNewDomain("compileUntiTestFoDelegate");
            builder.UseSmartMode();
            builder.Add(code);

            var assembly = builder.GetAssembly();
            var func = assembly.GetDelegateFromShortName<Func<string>>("A", "Get");
            Assert.Equal("compileUntiTestFoDelegateAssembly", func.Method.Module.Assembly.GetName().Name!);
            Assert.Equal("HelloWorld", func());
            Assert.Equal("compileUntiTestFoDelegate", builder.Domain.Name);
        }

    }
}
