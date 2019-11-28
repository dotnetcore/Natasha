using Natasha;
using System;
using System.Collections.Generic;
using System.Text;
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
                }"  | "System" | typeof(DomainOperator);

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
                }" | "System" | typeof(DomainOperator);

            var type = handler.GetType();
            Assert.Equal("DomainTest1", type.Name);
        }
    }
}
