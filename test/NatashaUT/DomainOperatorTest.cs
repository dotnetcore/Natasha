using Natasha;
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
    }
}
