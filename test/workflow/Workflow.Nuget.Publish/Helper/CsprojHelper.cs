using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Publish.Helper
{
    internal static class CSProjHelper
    {

        public static readonly Project? GlobalNode;

        private static readonly Regex _propertyGroupRegex;
        private static readonly Regex _packgeIdRegex;
        private static readonly Regex _packgeVersionRegex;
        private static readonly Regex _packgeAbleRegex;
        static CSProjHelper()
        {

            _propertyGroupRegex = new Regex("(?<result><PropertyGroup.*?>.*?</PropertyGroup>)", RegexOptions.Singleline | RegexOptions.Compiled);
            _packgeIdRegex = new Regex("<PackageId>(?<result>.*?)</PackageId>", RegexOptions.Singleline | RegexOptions.Compiled);
            _packgeVersionRegex = new Regex("<Version>(?<result>.*?)</Version>", RegexOptions.Singleline | RegexOptions.Compiled);
            _packgeAbleRegex = new Regex("<IsPackable>(?<result>.*?)</IsPackable>", RegexOptions.Singleline | RegexOptions.Compiled);
            if (File.Exists(ResourcesHelper.GlobalConfigFile))
            {
                var content = File.ReadAllText(ResourcesHelper.GlobalConfigFile);
                var match = _propertyGroupRegex.Match(content);
                if (match.Success)
                {
                    var propertyContent = match.Groups["result"].ToString();
                    var propretyNode = InternalDeserializeFromContent<PropertyGroup>(propertyContent);
                    if (propretyNode == null)
                    {
                        propretyNode = new PropertyGroup();
                    }
                    GlobalNode = new Project
                    {
                        PropertyGroup = propretyNode
                    };
                }

            }

        }





        public static IEnumerable<(string, Project?)> GetProjectsFromSrc()
        {
            return GetProjectsFromFolder(ResourcesHelper.CurrentSrcFolder);
        }


        public static IEnumerable<(string, Project?)> GetProjectsFromFolder(string folder)
        {
            var csprojFiles = Directory.GetFiles(folder, "*.csproj", SearchOption.AllDirectories);
            return csprojFiles.Select(item =>
            {
                var project = InternalDeserializeFromFile<Project>(item);
                if (GlobalNode != null)
                {
                    if (!project!.PropertyGroup.IsPackable.HasValue)
                    {
                        project!.PropertyGroup.IsPackable = GlobalNode.PropertyGroup.IsPackable;
                    }
                    if (project!.PropertyGroup.Version == null)
                    {
                        project!.PropertyGroup.Version = GlobalNode.PropertyGroup.Version;
                    }
                    if (project!.PropertyGroup.PackageId == null)
                    {
                        project!.PropertyGroup.PackageId = GlobalNode.PropertyGroup.PackageId;
                    }
                }
                return (item, project);
            });
        }


        private static T? InternalDeserializeFromContent<T>(string content)
        {

            var mySerializer = new XmlSerializer(typeof(T));
            using var myFileStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            return (T?)mySerializer.Deserialize(myFileStream);

        }
        private static T? InternalDeserializeFromFile<T>(string file)
        {

            var mySerializer = new XmlSerializer(typeof(T));
            using var myFileStream = new FileStream(file, FileMode.Open);
            return (T?)mySerializer.Deserialize(myFileStream);

        }

    }



    public class Project
    {
        public PropertyGroup PropertyGroup { get; set; } = default!;
    }

    public class PropertyGroup
    {
        public bool? IsPackable { get; set; }
        public string? PackageId { get; set; }
        public string? Version { get; set; }
    }
}
