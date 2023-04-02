using Xunit;

namespace NatashaFunctionUT.Special
{
    [Trait("高级API功能测试", "其他")]
    public class CS0104Test : DomainPrepare
    {
        [Fact(DisplayName = "普通二义性测试")]
        public void Test1()
        {
            var func = NDelegate.RandomDomain()
                .WithCS0104Handler()
                .ConfigUsing("Cs0104Model2")
                .Func<object>("Cs0104Model a = new Cs0104Model(); return a;");
            var result = func();
            Assert.Equal("Cs0104Model2",result.GetType().Namespace);
        }
        

        [Fact(DisplayName = "泛型二义性测试")]
        public void Test2()
        {
            var func = NDelegate.RandomDomain()
                .WithCS0104Handler()
                .ConfigUsing("Cs0104Model1")
                .Func<object>("CS0104GModel<int> a = new CS0104GModel<int>(); return a;");
            var result = func();
            Assert.Equal("Cs0104Model1", result.GetType().Namespace);
        }

        [Fact(DisplayName = "特殊二义性测试")]
        public void Test3()
        {

            var func = NDelegate.RandomDomain()
                .WithCS0104Handler()
                .ConfigUsing("Cs0104Model1")
                .Func<Cs0104Model1.String>("String a = new String(Cs0104Model.A); return a;");
            var result = func();
            Assert.Equal("Cs0104Model1", result.GetType().Namespace);
        }
    }
}
