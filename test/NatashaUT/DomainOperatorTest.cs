using Natasha;
using System;
using System.Runtime.CompilerServices;
using Xunit;

namespace NatashaUT
{

    [Trait("运算符API测试", "")]
    public class DomainOperatorTest
    {


        [Fact(DisplayName = "运算编译")]
        public void Test1()
        {
            var handler = DomainOperator.Instance 

                + "DomainOperatorTest1" 

                & @"public class  DomainTest1{
                        public string Name;
                        public DomainOperator Operator;
                }" | "W1233";

            var type = handler.GetType();
            Assert.Equal("DomainTest1",type.Name);

        }



        [Fact(DisplayName = "运算编译2")]
        public void Test2()
        {
            var handler = DomainOperator.Create("Tesst2Domain")
                & @"public class  DomainTest1{
                        public string Name;
                        public DomainOperator Operator;
                }";

            var type = handler.GetType();
            Assert.Equal("DomainTest1", type.Name);

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Test3()
        {
            var handler = DomainOperator.Instance

               + "DomainOperatorTest3"

               & @"public class  DomainTest1{
                        public string Name;
                        public DomainOperator Operator;
                }" | "W1233";

            var type = handler.GetType();
            Assert.Equal("DomainTest1", type.Name);
            DomainManagment.Get("DomainOperatorTest3").Dispose();
            //type.DisposeDomain();

        }

        [Fact(DisplayName = "卸载编译")]
        public void UnloadTest3()
        {
            Test3();
#if !NETCOREAPP2_2
            for (int i = 0; i < 10; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Assert.True(DomainManagment.IsDeleted("DomainOperatorTest3"));
#endif
        }
        public void Test4()
        {
            var handler = DomainOperator.Create("Tesst4Domain")
                & @"public class  DomainTest1{
                        public string Name;
                        public DomainOperator Operator;
                }";


            var type = handler.GetType();
            type.DisposeDomain();
            Assert.Equal("DomainTest1", type.Name);

        }

        [Fact(DisplayName = "卸载编译2")]
        public void UnloadTest4()
        {
            Test4();
#if !NETCOREAPP2_2
            for (int i = 0; i < 10; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Assert.True(DomainManagment.IsDeleted("Tesst4Domain"));
#endif
        }
    }
}
