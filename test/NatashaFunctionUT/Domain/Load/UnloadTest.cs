using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NatashaFunctionUT.Domain.Load
{
    [Trait("基础功能测试", "域")]
    public class UnloadTest
    {
       
        [MethodImpl(MethodImplOptions.NoInlining)]
        public string CreateAndUnload()
        {
            NatashaDomain? domain = default;
            using (DomainManagement.Create("au_test").CreateScope())
            {
                domain = DomainManagement.CurrentDomain;
                Assert.Equal("au_test", domain.Name);
            }
            domain.Dispose();
            return "au_test";
        }

        [Fact(DisplayName = "域的创建与卸载")]
        public void ContextAndUnload()
        {

            var name = CreateAndUnload();
            for (int i = 0; i < 6; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Assert.True(DomainManagement.IsDeleted(name));

        }
    }
}
