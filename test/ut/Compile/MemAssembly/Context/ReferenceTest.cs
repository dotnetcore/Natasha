using System;
using System.Linq;

namespace MemAssembly.Context
{
    [Trait("基础编译(REF)", "引用")]
    public class ReferenceTest : CompilePrepareBase
    {
        [Fact(DisplayName = "数量")]
        public void DDReferenceCompile()
        {
            AssemblyCSharpBuilder builder1 = new();
            var assemblyA = builder1
                .UseNewLoadContext("DiffDomainReferenceCompare1")
                .UseSmartMode()
                .WithCombineUsingCode(UsingLoadBehavior.WithDefault)
                .Add("namespace TestA{ public class A {  public string Name = \"Hello\"; public static NatashaUsingCache Get(){ return null;} }}")
                .GetAssembly();

            var typeA = assemblyA.GetTypes().Where(item => item.Name == "A").First();
            var objA = Activator.CreateInstance(typeA);

            Assert.NotNull(assemblyA);
            Assert.True(builder1.LoadContext.UsingRecorder.HasUsing("TestA"));


            AssemblyCSharpBuilder builder2 = new();
            var assemblyB = builder2
                .UseNewLoadContext("DiffDomainReferenceCompare2")
                .UseSmartMode()
                .WithCombineUsingCode(UsingLoadBehavior.WithDefault)
                .Add("namespace TestB{ public class A {  public string Name = \"Hello\";  public static NatashaUsingCache Get(){ return null;}  }}")
                .GetAssembly();

            var typeB = assemblyB.GetTypes().Where(item => item.Name == "A").First();
            var objB = Activator.CreateInstance(typeB);

            Assert.NotNull(assemblyB);
            Assert.True(builder2.LoadContext.UsingRecorder.HasUsing("TestB"));

            var domain1 = DomainManagement.Get("DiffDomainReferenceCompare1")!;
            var domain2 = DomainManagement.Get("DiffDomainReferenceCompare2")!;
            Assert.NotEmpty(domain1.Domain.Assemblies);
            Assert.Equal(domain1.Domain.Assemblies.Count(), domain2.Domain.Assemblies.Count());
            Assert.True(domain1.ReferenceRecorder.Count == domain2.ReferenceRecorder.Count);
        }
    }
}
