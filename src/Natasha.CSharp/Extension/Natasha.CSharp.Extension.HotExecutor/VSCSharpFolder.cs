using System.Diagnostics;

namespace Natasha.CSharp.Extension.HotExecutor
{
    public static class VSCSharpFolder
    {
        public static readonly string MainCsprojPath;
        public static readonly string BinPath;
        public static readonly string ObjPath;
        public static readonly string DebugPath;
        public static readonly string ReleasePath;
        public static readonly string SlnPath;
        public static readonly string ExecuteName;
        public static readonly string ExecutePath;
        public static readonly string CSProjFilePath;
        static VSCSharpFolder()
        {
            var currentExeFilePath = Process.GetCurrentProcess().MainModule.FileName;
            ExecutePath = new FileInfo(currentExeFilePath).Directory!.FullName;
            var currentDirectoryInfo = new DirectoryInfo(ExecutePath);
            MainCsprojPath = FindFileDirectory(currentDirectoryInfo,"*.csproj");
            var files = Directory.GetFiles(MainCsprojPath,"*.csproj");
            CSProjFilePath = files[0];
            BinPath = Path.Combine(MainCsprojPath, "bin");
            ObjPath = Path.Combine(MainCsprojPath, "obj");
            DebugPath = Path.Combine(BinPath, "Debug");
            ReleasePath = Path.Combine(BinPath, "Release");
            ExecuteName = Path.GetFileName(currentExeFilePath);
            SlnPath = FindFileDirectory(currentDirectoryInfo,"*.sln");
        }

        static string FindFileDirectory(DirectoryInfo csprojDirectory, string suffix)
        {
            DirectoryInfo directory = new(csprojDirectory.FullName);
            while (directory != directory.Root)
            {
                if (directory.GetFiles(suffix).Length > 0)
                {
                    return directory.FullName;
                }
                directory = directory.Parent;
            }
            throw new Exception("没有找到 sln 根文件！");
        }
    }
}
