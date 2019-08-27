using Natasha;
using Natasha.Operator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace NatashaUT
{

    [Trait("程序集编译测试", "插件")]
    public class AssemblyPluginTest
    {
#if !NETCOREAPP2_2


        [Fact(DisplayName = "插件编译协作")]
        public void Test1()
        {

            Assert.False(GetResult());
            for (int i = 0; i < 6; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Assert.True(DomainManagment.IsDeleted("TempDomain11"));

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool GetResult()
        {
            bool result=false;
            //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "ClassLibrary1.dll");
            string path = @"I:\Vs2017Test\SelfTestNatasha\ClassLibrary1\bin\Debug\netstandard2.0\ClassLibrary1.dll";
            using (DomainManagment.CreateAndLock("TempDomain11"))
            {

                var domain = DomainManagment.CurrentDomain;
                var assemebly = domain.LoadFile(path);
                var action = FastMethodOperator.New
                   .Using(assemebly)
                   .MethodBody(@"Class1 a = new Class1();return  a.Show();")
                   .Complie<Func<bool>>();
                result = action();
                domain.Dispose();
                domain.Unload();
            }
            return result;
        }

#endif

    }
}
