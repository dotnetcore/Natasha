using System;
using System.Runtime.CompilerServices;

namespace RefAssembly.Context
{
    [Trait("基础编译(REF)", "加载上下文")]
    public class LoadContextManagementTest : DomainPrepareBase
    {
        private WeakReference? _weakReference;
        [Fact(DisplayName = "随机")]
        public void LoadContextManagementTest1()
        {
            var context = DomainManagement.Random();
            Assert.NotNull(context);
            Assert.NotNull(context.Domain);
            Assert.NotNull(context.Domain.Name);
            Assert.Empty(context.Domain.Assemblies);
        }

        [Fact(DisplayName = "获取")]
        public void LoadContextManagementTest2()
        {
            var context = DomainManagement.Random();
            Assert.Equal(context, DomainManagement.Get(context.Domain.Name!));
            var context2 = DomainManagement.Create(context.Domain.Name!);
            Assert.Equal(context, context2);
            Assert.Equal(context.Domain, context2.Domain);
            Assert.Equal(context.Domain.Name, context2.Domain.Name);
        }

        [Fact(DisplayName = "卸载")]
        public void LoadContextManagementTest3()
        {
            CreateAndDisposeDomain("unloadtest1");
            Assert.True(_weakReference.IsAlive);
            Assert.NotNull(_weakReference.Target);
            for (int i = 0; i < 15; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Assert.True(DomainManagement.IsDeleted("unloadtest1"));
            Assert.Null(DomainManagement.Get("unloadtest1"));
            Assert.False(_weakReference.IsAlive);
            Assert.Null(_weakReference.Target);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void CreateAndDisposeDomain(string name)
        {
             var random = DomainManagement.Create(name);
            _weakReference = new WeakReference(random.Domain);
            random.Dispose();
        }
    }
}
