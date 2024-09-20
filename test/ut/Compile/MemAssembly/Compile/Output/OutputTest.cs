using System;
using System.IO;
using System.Reflection;

namespace MemAssembly.Compile.Output
{
    [Trait("基础编译(REF)", "输出")]
    public class OutputTest : DomainPrepareBase
    {
        private (string,string,string) ClearFile(string name, string domain)
        {
            var dll = Path.Combine(_path,domain, name+".dll");
            if (File.Exists(dll))
            {
                File.Delete(dll);
            }
            var pdb = Path.Combine(_path,domain, name + ".pdb");
            if (File.Exists(pdb))
            {
                File.Delete(pdb);
            }
            var xml = Path.Combine(_path,domain, name + ".xml");
            if (File.Exists(xml))
            {
                File.Delete(xml);
            }
            return (dll, pdb, xml);
        }
        private readonly string _script = @"//1
public class A {}";
        private readonly string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "DynamicLibraryFolders");
        [Fact(DisplayName = "Natasha文件输出")]
        public void FileTest1()
        {
            string domainName = "folder1";
            (string dll, string pdb, string xml) = ClearFile("fileut1", domainName);
            string domainPath = Path.Combine(_path, domainName);
            ClearFolder(domainPath);
            _script.GetAssemblyForUT(
                builder => builder
                .SetAssemblyName("fileut1")
                .UseNewLoadContext(domainName)
                .WithFileOutput().ConfigLoadContext(ctx=>ctx.AddReferenceAndUsingCode<object>()));
            
            Assert.True(File.Exists(dll));
            Assert.True(File.Exists(pdb));
            Assert.True(File.Exists(xml));
        }

        [Fact(DisplayName = "指定输出文件夹")]
        public void FileTest2()
        {
            string domainName = "folder2";
            string domainPath = Path.Combine(_path, domainName);
            (string dll, string pdb, string xml) = ClearFile("fileut1", domainName);
            ClearFolder(domainPath);
            _script.GetAssemblyForUT(
                builder => builder
                .SetAssemblyName("fileut1")
                .UseNewLoadContext(domainName)
                .WithFileOutput(domainPath).ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode<object>()));
            Assert.True(File.Exists(dll));
            Assert.True(File.Exists(pdb));
            Assert.True(File.Exists(xml));
        }

        [Fact(DisplayName = "指定输出DLL")]
        public void FileTest3()
        {
            string domainName = "folder3";
            string domainPath = Path.Combine(_path, domainName);
            string dllPath = Path.Combine(domainPath,"output", "test1.dll");
            (string dll, string pdb, string xml) = ClearFile("fileut1", domainName);
            ClearFolder(domainPath);
            _script.GetAssemblyForUT(
                builder => builder
                .SetAssemblyName("fileut1")
                .UseNewLoadContext(domainName)
                .WithReleaseCompile()
                .WithFileOutput(dllPath,null).ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode<object>()));
            Assert.True(File.Exists(dllPath));
            Assert.False(File.Exists(pdb));
            Assert.False(File.Exists(Path.Combine(domainPath, "test1.pdb")));
            Assert.False(File.Exists(xml));
            Assert.False(File.Exists(Path.Combine(domainPath, "test1.xml")));
            Assert.False(File.Exists(Path.Combine(domainPath, "test1.dll")));
            Assert.False(File.Exists(dll));
            Assert.False(File.Exists(Path.Combine(domainPath, "output", "test1.pdb")));
            Assert.False(File.Exists(Path.Combine(domainPath, "output", "test1.xml")));
            Assert.False(File.Exists(Path.Combine(domainPath, "output", "fileut1.pdb")));
            Assert.False(File.Exists(Path.Combine(domainPath, "output", "fileut1.xml")));
        }

        [Fact(DisplayName = "指定输出PDB")]
        public void FileTest4()
        {
            string domainName = "folder4";
            string domainPath = Path.Combine(_path, domainName);
            (string dll, string pdb, string xml) = ClearFile("fileut1", domainName);
            string dllPath = Path.Combine(domainPath, "output", "test1.dll");
            string pdbPath = Path.Combine(domainPath, "output", "test1.pdb");
            ClearFolder(domainPath);
            _script.GetAssemblyForUT(
                builder => builder
                .SetAssemblyName("fileut1")
                .WithReleaseCompile()
                .UseNewLoadContext(domainName)
                .WithFileOutput(dllPath, pdbPath, null).ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode<object>()));
            Assert.True(File.Exists(dllPath));
            Assert.False(File.Exists(pdbPath));
            Assert.False(File.Exists(Path.Combine(domainPath, "test1.dll")));
            Assert.False(File.Exists(dll));
            Assert.False(File.Exists(Path.Combine(domainPath, "test1.xml")));
            Assert.False(File.Exists(xml));
            Assert.False(File.Exists(Path.Combine(domainPath, "output", "test1.xml")));
            Assert.False(File.Exists(Path.Combine(domainPath, "output", "fileut1.xml")));
        }


        [Fact(DisplayName = "指定输出XML")]
        public void FileTest5()
        {
            string domainName = "folder5";
            string domainPath = Path.Combine(_path, domainName);
            string dllPath = Path.Combine(domainPath, "output", "test1.dll");
            string xmlPath = Path.Combine(domainPath, "output", "test1.xml");
            ClearFolder(domainPath);
            _script.GetAssemblyForUT(
                builder => builder
                .SetAssemblyName("fileut1")
                .UseNewLoadContext(domainName)
                .WithReleaseCompile()
                .WithFileOutput(dllPath, null, xmlPath).ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode<object>()));
            Assert.True(File.Exists(dllPath));
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
