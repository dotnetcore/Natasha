using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NatashaUT.BuilderUT
{
    public class AssemblyBuilderTest : PrepareTest
    {
        [Fact(DisplayName = "字符串编译1")]
        public void TestClass()
        {
            AssemblyCSharpBuilder assemblyCSharpBuilder = new AssemblyCSharpBuilder();
            assemblyCSharpBuilder.UseNatashaFileOut();
            assemblyCSharpBuilder.Domain = DomainManagement.Random;
            assemblyCSharpBuilder.Add(@"public class A {   
    /// <summary>
    /// 注释
    /// </summary>
    public string Name{get;set;}
} ");
            var assembly = assemblyCSharpBuilder.GetAssembly();
            Assert.NotNull(assembly);
        }
        
    }
}
