using Natasha;
using System;
using System.IO;
using Xunit;

namespace NatashaUT
{

    [Trait("程序集编译测试", "文件")]
    public class AssemblyFileTest
    {
        [Fact(DisplayName = "文件部分类编译")]
        public void Test1()
        {

            string path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "File",  "TextFile1.txt");
            string path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "File", "TestFileModel.cs");
            
            
            NAssembly assembly = new NAssembly("AsmTestFile");
            assembly.AddFile(path1);
            assembly.AddFile(path2);


            var result = assembly.Complier();
            Assert.NotNull(result);


            var type = assembly.GetType("TestFileModel");
            Assert.NotNull(type);
            Assert.Equal("TestFileModel", type.Name);


            var @delegate = NFunc<string>.Delegate("return new TestFileModel().Name;", result);
            Assert.Equal("aaa",@delegate());
        }
    }
}
