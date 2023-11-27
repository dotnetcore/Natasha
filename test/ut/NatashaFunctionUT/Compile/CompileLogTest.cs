using Natasha.CSharp.Extension.Inner;
using System;
using System.IO;
using Xunit;

namespace NatashaFunctionUT.Compile
{
    [Trait("基础功能测试", "编译")]
    public class CompileLogTest : DomainPrepare
    {
        [Fact(DisplayName = "错误日志1")]
        public void CompareErrorLog1()
        {
            string code = @"public class A{
public void Show(){
    return 1;
}
}";

            NatashaCompilationLog? log = null;
            try
            {
                AssemblyCSharpBuilder builder = new("ee79d3e2b027491f93705a4098568bc8");
                builder.Add(code);
                builder.CompileFailedEvent += (compilation, errors) =>
                {
                    log = compilation.GetNatashaLog();
                };
                builder.GetAssembly();
                Assert.Equal(1, 2);
            }
            catch (Exception ex)
            {

                CheckException(ex, log, "ee79d3e2b027491f93705a4098568bc8");

            }

        }

        private static void CheckException(Exception ex, NatashaCompilationLog? log, string fileName)
        {
            var logText = GetText(fileName);
            Assert.NotNull(log);
            Assert.True(ex is NatashaException);
            var logText2 = log!.ToString();
            Assert.Equal(logText.Split("\n").Length, logText2.Split("\n").Length);

            //if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            //{
            //    foreach (var item in log.Messages)
            //    {
            //        Assert.Contains(item.Code, logText);
            //        Assert.Contains(item.Message, logText);
            //    }
            //}
        }


        [Fact(DisplayName = "错误日志2")]
        public void CompareErrorLog2()
        {
            string code = @"using abc;
using bcd;
using bce;
public class A{
public string Name;
public Path path;
public string Address;
}";

            NatashaCompilationLog? log = null;
            try
            {
                AssemblyCSharpBuilder builder = new("ee79d3e2b027491f93705a4098578bcc");
                builder.Add(code);
                builder.ConfigCompilerOption(opt => opt.SetNullableCompile(Microsoft.CodeAnalysis.NullableContextOptions.Disable));
                builder.LogCompilationEvent += (logModel) =>
                {
                    log = logModel;

                };
                builder.GetAssembly();
                Assert.Equal(1, 2);
            }
            catch (Exception ex)
            {
                CheckException(ex, log, "ee79d3e2b027491f93705a4098578bcc");
            }

        }


        [Fact(DisplayName = "错误日志3")]
        public void CompareErrorLog3()
        {
            string code1 = @"public class A{
public string Name;
public Model path;
public string Address;
}";
            string code2 = @"public class A{
public string Name;
public int Get(){
   return age;
}
}";
            NatashaCompilationLog? log = null;
            try
            {
                AssemblyCSharpBuilder builder = new("ed79d3e2b027491f93705a4098578bcd");
                builder.Add(code1);
                builder.Add(code2);
                builder.ConfigCompilerOption(opt => opt.SetNullableCompile(Microsoft.CodeAnalysis.NullableContextOptions.Disable));
                builder.CompileFailedEvent += (compilation, errors) =>
                {
                    log = compilation.GetNatashaLog();
                };
                builder.GetAssembly();
                Assert.Equal(1, 2);
            }
            catch (Exception ex)
            {
                CheckException(ex, log, "ed79d3e2b027491f93705a4098578bcd");
            }

        }



        [Fact(DisplayName = "成功日志")]
        public void CompareSucceedLog()
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
            NatashaCompilationLog? log = null;
            AssemblyCSharpBuilder builder = new("2d79d3e2b027491f93705a4098578bcd");
            builder.Domain = DomainManagement.Random();
            builder.ConfigCompilerOption(opt => opt.SetNullableCompile(Microsoft.CodeAnalysis.NullableContextOptions.Disable));
            builder.Add(code1);
            builder.Add(code2);
            builder.CompileSucceedEvent += (compilation, assembly) =>
            {
                log = compilation.GetNatashaLog();
                Assert.NotNull(assembly);
            };
            builder.GetAssembly();

            var logText = GetText("2d79d3e2b027491f93705a4098578bcd");
            Assert.NotNull(log);
            Assert.Equal(logText.Split("\n").Length, log!.ToString().Split("\n").Length);

            //if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            //{
            //    foreach (var item in log.Messages)
            //    {
            //        Assert.Contains(item.Code, logText);
            //        Assert.Contains(item.Message, logText);
            //    }
            //}
        }

        private static string GetText(string fileName)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Compile", "LogFile", fileName + ".txt");
            return File.ReadAllText(path);
        }

    }



}
