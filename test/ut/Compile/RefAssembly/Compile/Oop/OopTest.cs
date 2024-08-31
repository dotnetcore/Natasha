namespace RefAssembly.Compile.Oop
{
    [Trait("基础编译(REF)", "普通结构")]
    public class OopTest : DomainPrepareBase
    {
        [Fact(DisplayName = "类")]
        public void ClassTest()
        {
            OopCheckHelper.SimpleCheckWrapper("class", true);
            OopCheckHelper.SimpleCheckWrapper("class", false);
        }

        [Fact(DisplayName = "结构体")]
        public void StructTest()
        {
            OopCheckHelper.SimpleCheckWrapper("struct", true);
            OopCheckHelper.SimpleCheckWrapper("struct", false);
        }

        [Fact(DisplayName = "枚举")]
        public void WithNamespaceComplexTest()
        {
            OopCheckHelper.SimpleCheckWrapper("enum", true);
            OopCheckHelper.SimpleCheckWrapper("enum", false);
        }

        [Fact(DisplayName = "接口")]
        public void InterfaceTest()
        {
            OopCheckHelper.SimpleCheckWrapper("interface", true);
            OopCheckHelper.SimpleCheckWrapper("interface", false);
        }

        [Fact(DisplayName = "记录")]
        public void RecordTest()
        {
            OopCheckHelper.SimpleCheckWrapper("record", true);
            OopCheckHelper.SimpleCheckWrapper("record", false);
        }    
    }
}
