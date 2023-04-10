using Github.NET.Sdk.Model;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Github.NET.Sdk
{
    public class SolutionHelper
    {
        public static readonly string CurrentProjectRoot;
        public static readonly string GlobalConfigFile;
        public static readonly string ConfigFilePath;


        static SolutionHelper()
        {
            CurrentProjectRoot = GetProjectRoot();
            GlobalConfigFile = Path.Combine(CurrentProjectRoot, "Directory.Build.props");
            ConfigFilePath = Path.Combine(CurrentProjectRoot, ".github", "NMS_TEMPLATE", "project.yml");
        }


        public static NMSSolutionInfo? GetSolutionInfo()
        {
            if (File.Exists(ConfigFilePath))
            {
                var content = File.ReadAllText(ConfigFilePath);
                if (!string.IsNullOrEmpty(content))
                {
                    var deserializer = new DeserializerBuilder()
                                               .WithNamingConvention(UnderscoredNamingConvention.Instance)  // see height_in_inches in sample yml 
                                               .Build();

                    //yml contains a string containing your YAML
                    return deserializer.Deserialize<NMSSolutionInfo>(content);
                }
            }
            return null;
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
        }

        private static bool CheckExistGlobalBuilderFile(string folder)
        {
            return File.Exists(Path.Combine(folder, "Directory.Build.props")) && Directory.GetFiles(folder).Any(item => item.Contains(".sln"));
        }
    }
}
