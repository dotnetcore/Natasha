namespace MemAssembly.Compile.Oop
{
    [Trait("基础编译(REF)", "泛型结构")]
    public class OopTTest : DomainPrepareBase
    {

        [Fact(DisplayName = "泛型类")]
        public void GenericClassTest()
        {
            OopCheckHelper.SimpleCheckWrapper("class", true, true);
            OopCheckHelper.SimpleCheckWrapper("class", false, true);
        }

        [Fact(DisplayName = "泛型结构体")]
        public void GenericStructTest()
        {
            OopCheckHelper.SimpleCheckWrapper("struct", true, true);
            OopCheckHelper.SimpleCheckWrapper("struct", false, true);
        }

        [Fact(DisplayName = "泛型接口")]
        public void GenericInterfaceTest()
        {
            OopCheckHelper.SimpleCheckWrapper("interface", true, true);
            OopCheckHelper.SimpleCheckWrapper("interface", false, true);
        }

        [Fact(DisplayName = "泛型记录")]
        public void GenericRecordTest()
        {
            OopCheckHelper.SimpleCheckWrapper("record", true, true);
            OopCheckHelper.SimpleCheckWrapper("record", false, true);
        }
    }
}
