﻿using System.Diagnostics;
using System.Xml;

namespace Natasha.CSharp.HotExecutor.Utils
{
    public static class VSCSProjectInfoHelper
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
        public static readonly HashSet<string> ExpectFiles;
        public static readonly bool EnableImplicitUsings;
        public static IEnumerable<string>? MainAssemblyUsings;
        public static readonly string HEOutputPath;
        static VSCSProjectInfoHelper()
        {
            ExpectFiles = [];
            var currentExeFilePath = Process.GetCurrentProcess().MainModule.FileName;
            var filePathWrapper = AppContext.GetData("CurrentHEExeFilePath");
            if (filePathWrapper != null)
            {
                currentExeFilePath = (string)filePathWrapper;
            }
            ExecutePath = new FileInfo(currentExeFilePath).Directory!.FullName;
            var currentDirectoryInfo = new DirectoryInfo(ExecutePath);
            MainCsprojPath = FindFileDirectory(currentDirectoryInfo,"*.csproj");
            HEOutputPath = Path.Combine(MainCsprojPath, "HEOutput");
            if (!Directory.Exists(HEOutputPath))
            {
                Directory.CreateDirectory(HEOutputPath);
            }
            var files = Directory.GetFiles(MainCsprojPath,"*.csproj");
            CSProjFilePath = files[0];
            BinPath = Path.Combine(MainCsprojPath, "bin");
            ObjPath = Path.Combine(MainCsprojPath, "obj");
            DebugPath = Path.Combine(BinPath, "Debug");
            ReleasePath = Path.Combine(BinPath, "Release");
            ExecuteName = Path.GetFileName(currentExeFilePath);
            SlnPath = FindFileDirectory(currentDirectoryInfo,"*.sln");

            XmlDocument doc = new XmlDocument();
            doc.Load(CSProjFilePath);


            XmlNode implicitUsingsNode = doc.SelectSingleNode("//ImplicitUsings");

            if (implicitUsingsNode != null)
            {
                string value = implicitUsingsNode.InnerText.Trim().ToLower();
                if (value == "enable" || value == "true")
                {
                    EnableImplicitUsings = true;
                }
            }
            //EnableImplicitUsings = false;
            XmlNodeList itemGroupNodes = doc.SelectNodes("//ItemGroup");

            foreach (XmlNode itemGroupNode in itemGroupNodes)
            {
                XmlNodeList compileNodes = itemGroupNode.SelectNodes("Compile[@Remove]");

                foreach (XmlNode compileNode in compileNodes)
                {
                    string removedFile = compileNode.Attributes["Remove"].Value;
                    if (removedFile.EndsWith(".cs"))
                    {
                        ExpectFiles.Add(Path.Combine(MainCsprojPath,removedFile));
                    }
                }
            }
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

        public static bool CheckFileAvailiable(string file)
        {
            if (ExpectFiles.Contains(file))
            {
                return false;
            }
            return CheckFileAvivaliable(file, BinPath, ObjPath);
        }

        public static bool IsXamlAndResxFile(string file)
        {
            return file.EndsWith(".xaml") || file.EndsWith(".resx");
        }
        public static bool CheckFileAvivaliable(string file, string binPath, string objPath)
        {
            if (file.EndsWith(".cs"))
            {
                if (file.StartsWith(binPath))
                {
                    return false;
                }
                if (!file.EndsWith(".g.cs") && file.StartsWith(ObjPath))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public static void SetMainUsing(IEnumerable<string> mainUsings)
        {
            MainAssemblyUsings = mainUsings;
        }

    }
}
