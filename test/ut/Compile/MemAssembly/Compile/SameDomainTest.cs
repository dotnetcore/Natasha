using System;
using System.Linq;
using System.Threading;

namespace MemAssembly.Compile
{
    [Trait("基础编译(REF)", "同域编译")]
    public class SameDomainTest : CompilePrepareBase
    {
        [Fact(DisplayName = "同域复用")]
        public void DomainCompileTest()
        {
            var loadContext = DomainManagement.Random();
            AssemblyCSharpBuilder builder = new()
            {
                AssemblyName = "samedomainrepeate1",
                LoadContext = loadContext
            };
            builder.UseSmartMode();
            builder.Add("namespace TestA{ public class A {  public string Name = \"Hello\"; }}");
            var assemblyA = builder.GetAssembly();
            Assert.NotNull(assemblyA);
            Assert.True(builder.LoadContext.UsingRecorder.HasUsing("TestA"));
            Assert.NotNull(assemblyA.GetTypeFromShortName("A"));

            builder.Clear();
            builder.AssemblyName = "samedomainrepeate2";
            builder.Add("namespace TestB{ public class B {  public string Name = \" World!\"; }}");
            var assemblyB = builder.GetAssembly();
            Assert.NotNull(assemblyB);
            Assert.True(builder.LoadContext.UsingRecorder.HasUsing("TestB"));
            Assert.NotNull(assemblyB.GetTypeFromShortName("B"));

            builder.Clear();
            builder.AssemblyName = "samedomainrepeate3";
            builder.Add("public class C { public static string Show(){ return (new A()).Name+(new B()).Name; } }");
            var assemblyC = builder.GetAssembly();
            var func = assemblyC.GetDelegateFromShortName<Func<string>>("C","Show");
            Assert.Equal("Hello World!", func());

        }

        [Fact(DisplayName = "同域同名")]
        public void DomainCompileTest2()
        {
            var loadContext = DomainManagement.Random();
            AssemblyCSharpBuilder builder = new()
            {
                AssemblyName = "samedomainsamename1",
                LoadContext = loadContext
            };
            builder.UseSmartMode();
            builder.Add("namespace TestA{ public class A {  public string Name = \"Hello\"; }}");
            var assemblyA = builder.GetAssembly();
            Assert.NotNull(assemblyA);
            Assert.True(builder.LoadContext.UsingRecorder.HasUsing("TestA"));
            Assert.NotNull(assemblyA.GetTypeFromShortName("A"));

            builder.Clear();
            builder.AssemblyName = "samedomainsamename2";
            builder.Add("namespace TestB{ public class A {  public string Name = \" World!\"; }}");
            var assemblyB = builder.GetAssembly();
            Assert.NotNull(assemblyB);
            Assert.True(builder.LoadContext.UsingRecorder.HasUsing("TestB"));
            Assert.NotNull(assemblyB.GetTypeFromShortName("A"));

            builder.Clear();
            builder.AssemblyName = "samedomainsamename3";
            builder.Add("public class C { public static string Show(){ return (new TestA.A()).Name+(new TestB.A()).Name; } }");
            var assemblyC = builder.GetAssembly();
            var func = assemblyC.GetDelegateFromShortName<Func<string>>("C", "Show");
            Assert.Equal("Hello World!", func());

        }


        
    }
}
