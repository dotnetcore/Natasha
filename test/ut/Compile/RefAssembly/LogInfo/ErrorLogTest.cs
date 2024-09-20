using RefAssembly.Log.Utils;

namespace RefAssembly.Log
{
    [Trait("基础编译(REF)", "日志")]
    public class ErrorLogTest : DomainPrepareBase
    {
        [Fact(DisplayName = "单脚本错误1")]
        public void CompareErrorLog1()
        {
            string script = @"public class A{
public void Show(){
    return 1;
}
}";
            LogCheckHelper.JudgeLogError(script, "ee79d3e2b027491f93705a4098568bc8");
        }



        [Fact(DisplayName = "单脚本错误2")]
        public void CompareErrorLog2()
        {
            string script = @"using abc;
using bcd;
using bce;
public class A{
public string Name;
public Path path;
public string Address;
}";
            LogCheckHelper.JudgeLogError(script, "ee79d3e2b027491f93705a4098578bcc", false);
        }


        [Fact(DisplayName = "多脚本错误")]
        public void CompareErrorLog3()
        {
            string script1 = @"public class A{
public string Name;
public Model path;
public string Address;
}";
            string script2 = @"public class A{
public string Name;
public int Get(){
   return age;
}
}";
            LogCheckHelper.JudgeLogError(script1, "ed79d3e2b027491f93705a4098578bcd", false, script2);
        }

    }



}
