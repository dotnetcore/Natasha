using System;
using System.IO;

namespace MemAssembly.Compile.Output
{
    [Trait("基础编译(REF)", "输出")]
    public class SingleOutputTest : DomainPrepareBase
    {
        private readonly string _script = "public class A {}";
        private readonly string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "DynamicLibraryFolders");
        [Fact(DisplayName = "仅 DLL")]
        public void FileTest1()
        {
            string domainNmae = "single1";
            string domainPath = Path.Combine(_path, domainNmae);
            string dllPath = Path.Combine(domainPath, "output", "test1.dll");
            ClearFolder(domainPath);
            _script.GetAssemblyForUT(
                builder => builder
                .SetAssemblyName("fileut1")
                .UseNewLoadContext(domainNmae)
                .WithReleaseCompile()
                .SetDllFilePath(dllPath).ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode<object>()));
            Assert.True(File.Exists(dllPath));
            Assert.False(File.Exists(Path.Combine(domainPath, "test1.pdb")));
            Assert.False(File.Exists(Path.Combine(domainPath, "fileut1.pdb")));
            Assert.False(File.Exists(Path.Combine(domainPath, "test1.xml")));
            Assert.False(File.Exists(Path.Combine(domainPath, "fileut1.xml")));
        }
        [Fact(DisplayName = "仅 PDB")]
        public void FileTest2()
        {
            string domainNmae = "single2";
            string domainPath = Path.Combine(_path, domainNmae);
            string pdbPath = Path.Combine(domainPath, "output", "test1.pdb");
            ClearFolder(domainPath);
            _script.GetAssemblyForUT(
                builder => builder
                .SetAssemblyName("fileut1")
                .UseNewLoadContext(domainNmae)
                .SetPdbFilePath(pdbPath).ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode<object>()));
            Assert.True(File.Exists(pdbPath));
            Assert.False(File.Exists(Path.Combine(domainPath, "test1.dll")));
            Assert.False(File.Exists(Path.Combine(domainPath, "fileut1.dll")));
            Assert.False(File.Exists(Path.Combine(domainPath, "test1.xml")));
            Assert.False(File.Exists(Path.Combine(domainPath, "fileut1.xml")));
            Assert.False(File.Exists(Path.Combine(domainPath, "output", "test1.xml")));
            Assert.False(File.Exists(Path.Combine(domainPath, "output", "fileut1.xml")));
        }
        [Fact(DisplayName = "仅 XML")]
        public void FileTest3()
        {
            string domainNmae = "single3";
            string domainPath = Path.Combine(_path, domainNmae);
            string xmlPath = Path.Combine(domainPath, "output", "test1.xml");
            ClearFolder(domainPath);
            _script.GetAssemblyForUT(
                builder => builder
                .SetAssemblyName("fileut1")
                .UseNewLoadContext(domainNmae)
                .WithReleaseCompile()
                .SetCommentFilePath(xmlPath).ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode<object>()));
            Assert.True(File.Exists(xmlPath));
            Assert.False(File.Exists(Path.Combine(domainPath, "test1.dll")));
            Assert.False(File.Exists(Path.Combine(domainPath, "fileut1.dll")));
            Assert.False(File.Exists(Path.Combine(domainPath, "test1.pdb")));
            Assert.False(File.Exists(Path.Combine(domainPath, "fileut1.pdb")));
            Assert.False(File.Exists(Path.Combine(domainPath, "output", "test1.pdb")));
            Assert.False(File.Exists(Path.Combine(domainPath, "output", "fileut1.pdb")));
        }
        private void ClearFolder(string folder)
        {
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
        }
    }
}
