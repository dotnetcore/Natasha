using Xunit;

namespace NatashaFunctionUT.Special
{
    [Trait("高级API功能测试", "其他")]
    public class PrivateTest : DomainPrepare
    {

        [Fact(DisplayName = "私有成员调用")]
        public void Test()
        {
            PrivateMemberClassModel test = new PrivateMemberClassModel();
            var action = NDelegate
                .RandomDomain()
                .ConfigClass(item=>item.AllowPrivate<PrivateMemberClassModel>())
                .Action<PrivateMemberClassModel>("obj.publicA = 2;obj.internalA = 3; obj.privateA=1;");
            action(test);
#if DEBUG
        Assert.Equal(4, test.GetD);
#endif

        }


        [Fact(DisplayName = "私有成员调用2")]
        public void Test1()
        {

            PrivateMemberClassModel test = new PrivateMemberClassModel();
            var action = NDelegate
                .RandomDomain()
                .ConfigClass(item => item.AllowPrivate<PrivateMemberClassModel>().SkipInit())
                .Action<PrivateMemberClassModel>("obj.publicA = 2;obj.internalA = 3; obj.privateA=1;");
            action(test);
#if DEBUG
            Assert.Equal(4, test.GetD);
#endif
        }


    }


    public class PrivateMemberClassModel
    {
        public int publicA { get; set; }
        protected int protectedA { get; set; }
        internal int internalA { get; set; }
        private int privateA { get; set; }
        protected internal int E { get; set; }
        internal protected int F { get; set; }
        public int GetD { get { return privateA + internalA; } }
    }
}
