using Natasha.CSharp;
using Xunit;

namespace NatashaUT
{
    [Trait("CS0104", "")]
    public class CS0104Test : PrepareTest
    {
        [Fact(DisplayName = "普通二义性测试")]
        public void Test1()
        {
            var func = NDelegate.RandomDomain().Func<object>("Cs0104Model a = new Cs0104Model(); return a;", "Cs0104Model1");
            var result = func();
            Assert.Equal("Cs0104Model1",result.GetType().Namespace);
        }
        

        [Fact(DisplayName = "泛型二义性测试")]
        public void Test2()
        {
            var func = NDelegate.RandomDomain().Func<object>("CS0104GModel<int> a = new CS0104GModel<int>(); return a;", "Cs0104Model1");
            var result = func();
            Assert.Equal("Cs0104Model1", result.GetType().Namespace);
        }

        [Fact(DisplayName = "特殊二义性测试")]
        public void Test3()
        {

            var func = NDelegate.RandomDomain().Func<Cs0104Model1.String>("String a = new String(Cs0104Model.A); return a;", "Cs0104Model1");
            var result = func();
            Assert.Equal("Cs0104Model1", result.GetType().Namespace);
        }
    }
}
