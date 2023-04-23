
using Solution.NET.Sdk.Model;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Github.NET.Sdk
{
    public static class SolutionInfo
    {
        public static readonly string Root;
        public static readonly string CurrentSrcProject;
        public static readonly string SolutionFilePath;
        public static readonly string ChangeLogFile;
        private static readonly Regex _propertyGroupRegex;

        static SolutionInfo()
        {
            Root = GetProjectRoot();
            ChangeLogFile = Path.Combine(Root, "CHANGELOG.md");
            CurrentSrcProject = Path.Combine(Root, "src");
            _propertyGroupRegex = new Regex("(?<result><PropertyGroup.*?>.*?</PropertyGroup>)", RegexOptions.Singleline | RegexOptions.Compiled);
            SolutionFilePath = Directory.GetFiles(Root, "*.sln")[0];
        }


        public static CSharpProjectCollection GetCSProjectsByStartFolder(string folderPrefix)
        {
            var result = new CSharpProjectCollection();
           
            var globalNode = new CSharpProject();
            globalNode.ProjectName = "Directory.Build.props";
            globalNode.RelativePath = Path.Combine(CurrentSrcProject, "Directory.Build.props");
            globalNode.RelativeFolder = folderPrefix;
            globalNode.PackageName = "Directory.Build.props";
            var globalGroupNode = InternalDeserializeFromFile<PropertyGroup>(globalNode.RelativePath);
            globalGroupNode?.UpdateToCSProject(globalNode);
            result.GlobalConfig = globalNode;

            Regex regex = new Regex($"\"(?<name>[\\w\\.\\-_]+?)\",\\s*?\"(?<path>{folderPrefix.Replace("\\","\\\\")}(\\\\[\\w\\.\\-_]+)*?\\.csproj?)\",\\s*?\"{{(?<id>.*?)}}\".*?EndProject", RegexOptions.Singleline);
            var slnContent = File.ReadAllText(SolutionFilePath);
            var matches = regex.Matches(slnContent);
            if (matches.Count>0)
            {
                result.Projects = new List<CSharpProject>();
                for (int i = 0; i < matches.Count; i++)
                {
                    var match = matches[i];
                    if (match.Success)
                    {
                        var csProjectNode = new CSharpProject();
                        csProjectNode.ProjectName = match.Groups["name"].Value;
                        csProjectNode.RelativePath = match.Groups["path"].Value;
                        csProjectNode.RelativeFolder = Path.GetDirectoryName(csProjectNode.RelativePath)!.Replace("\\", "/");
                        csProjectNode.Id = match.Groups["id"].Value;
                        var groupNode = InternalDeserializeFromFile<PropertyGroup>(Path.Combine(Root, csProjectNode.RelativePath.Replace("\\", "/")));
                        if (groupNode!=null)
                        {
                            if (globalGroupNode != null)
                            {
                                groupNode.SupplementFrom(globalGroupNode);
                            }
                            groupNode.UpdateToCSProject(csProjectNode);
                        }
                        result.Projects.Add(csProjectNode);
                    }
                }
            }
            return result;

        }


        private static T? InternalDeserializeFromFile<T>(string file)
        {
            return InternalDeserializeFromContent<T>(File.ReadAllText(file));
        }

        private static T? InternalDeserializeFromContent<T>(string content)
        {
            var rootMatch = _propertyGroupRegex.Match(content);
            if (rootMatch.Success)
            {
                var propertyGroupContent = rootMatch.Groups["result"].Value;
                var mySerializer = new XmlSerializer(typeof(T));
                using var myFileStream = new MemoryStream(Encoding.UTF8.GetBytes(propertyGroupContent));
                return (T?)mySerializer.Deserialize(myFileStream);
            }
            return default;
        }


        private static string GetProjectRoot()
        {
            var currentFolder = AppDomain.CurrentDomain.BaseDirectory;
            while (!CheckExistGlobalBuilderFile(currentFolder))
            {
                var parentFolder = Directory.GetParent(currentFolder);
                if (parentFolder != null)
                {
                    currentFolder = parentFolder.FullName;
                }
                else
                {
                    return string.Empty;
                }
            }
           
            return currentFolder;

            static bool CheckExistGlobalBuilderFile(string folder)
            {
                return File.Exists(Path.Combine(folder, "CHANGELOG.md")) && Directory.GetFiles(folder,"*.sln").Length >0;
            }
        }

    }
}
