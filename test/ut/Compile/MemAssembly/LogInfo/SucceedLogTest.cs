using MemAssembly.Log.Utils;

namespace MemAssembly.Log
{
    [Trait("基础编译(REF)", "日志")]
    public class SucceedLogTest : DomainPrepareBase
    {
        [Fact(DisplayName = "单脚本错误1")]
        public void CompareSucceedLog1()
        {
            string script = @"public class A{
public void Show(){
}
}";
            LogCheckHelper.JudgeLogError(script, "ee79d3e2b027491f93705a4098568bc9");
        }



        [Fact(DisplayName = "成功日志")]
        public void CompareSucceedLog2()
        {
            string code1 = @"public class A{
public string Name;
public string path;
public string Address;
}";
            string code2 = @"public class B{
public string Name;
public int Get(){
   return Name.Length;
}
}";
            LogCheckHelper.JudgeLogSuccess(code1, "2d79d3e2b027491f93705a4098578bcd", false, code2);
            
        }

    }



}
