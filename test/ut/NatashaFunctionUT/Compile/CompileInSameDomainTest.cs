using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace NatashaFunctionUT.Compile
{
    [Trait("基础功能测试", "编译")]
    public class CompileInSameDomainTest : CompilerPrepareBase
    {
        [Fact(DisplayName = "[不同命名空间][不同程序集]同域编译")]
        public void DNDASDCompile()
        {

            var loadContext = DomainManagement.Random();
            var referenceCount = DomainManagement.Random().ReferenceRecorder.Count;


            AssemblyCSharpBuilder builder = new()
            {
                AssemblyName = "ASDASD1",
                LoadContext = loadContext
            };
            builder.UseSmartMode();
            Assert.NotEqual("Default", builder.Domain.Name);

            builder.Add("namespace TestA{ public class A {  public string Name = \"Hello\"; }}");
            var assemblyA = builder.GetAssembly();
            Assert.NotNull(assemblyA);
            Assert.True(builder.LoadContext.UsingRecorder.HasUsing("TestA"));

            builder.Clear();

            builder.AssemblyName = "ASDASD2";
            Assert.Equal(referenceCount, loadContext.ReferenceRecorder.Count - 1);
            builder.Add(@"namespace TestB 
{ 
    public class B 
    {  
        public B(){
            Name="" World!"";
        }
        public string Name; 
    }
}");

            var assemblyB = builder.GetAssembly();
            builder.WithCombineUsingCode(UsingLoadBehavior.WithCurrent);
            Assert.True(builder.LoadContext.UsingRecorder.HasUsing("TestB"));
            Assert.NotNull(assemblyB);

            Thread.Sleep(2000);
            builder.Clear();
            builder.AssemblyName = "ASDASD3";
            Assert.Equal(referenceCount, loadContext.ReferenceRecorder.Count - 2);
            builder.Add("public class C { public static string Show(){ return (new A()).Name+(new B()).Name; } }");
            var assemblyC = builder.GetAssembly();
            var type = assemblyC.GetTypes().Where(item => item.Name == "C").First();
            var methodInfo = type.GetMethod("Show");
            var result = (string)methodInfo!.Invoke(null, null)!;
            Assert.Equal("Hello World!", result);
        }


        [Fact(DisplayName = "[不同域]编译引用对比")]
        public void DDReferenceCompile()
        {

            AssemblyCSharpBuilder builder1 = new()
            {
                LoadContext = DomainManagement.Create("DiffDomainReferenceCompare1")
            };
            builder1.UseSmartMode();
            builder1.WithCombineUsingCode(UsingLoadBehavior.WithDefault);
            builder1.Add("namespace TestA{ public class A {  public string Name = \"Hello\"; public static NatashaUsingCache Get(){ return null;} }}");
            var assemblyA = builder1
                .WithCombineReferences(item => item.UseDefaultReferences())
                .GetAssembly();
            var typeA = assemblyA.GetTypes().Where(item => item.Name == "A").First();
            var objA = Activator.CreateInstance(typeA);
            Assert.NotNull(assemblyA);
            Assert.True(builder1.LoadContext.UsingRecorder.HasUsing("TestA"));

            AssemblyCSharpBuilder builder2 = new()
            {
                LoadContext = DomainManagement.Create("DiffDomainReferenceCompare2")
            };
            builder2.WithCombineUsingCode(UsingLoadBehavior.WithDefault);
            builder2.Add("namespace TestB{ public class A {  public string Name = \"Hello\";  public static NatashaUsingCache Get(){ return null;}  }}");
            var assemblyB = builder2
                .WithCombineReferences()
                .GetAssembly();
            var typeB = assemblyB.GetTypes().Where(item => item.Name == "A").First();
            var objB = Activator.CreateInstance(typeB);
            Assert.NotNull(assemblyB);
            Assert.True(builder2.LoadContext.UsingRecorder.HasUsing("TestB"));
            var domain1 = DomainManagement.Get("DiffDomainReferenceCompare1")!;
            var domain2 = DomainManagement.Get("DiffDomainReferenceCompare2")!;
            Assert.True(domain1.Domain.Assemblies.Any());
            Assert.Equal(domain1.Domain.Assemblies.Count(), domain2.Domain.Assemblies.Count());
            Assert.True(domain1.ReferenceRecorder.Count == domain2.ReferenceRecorder.Count);
        }
    }
}
