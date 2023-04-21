using Github.NMSAcion.NET.Sdk.Model;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Github.NET.Sdk
{
    public static class SolutionRecorder
    {
        public static readonly string NMSTemplateRoot;
        public static readonly string ConfigFilePath;

        static SolutionRecorder()
        {
            NMSTemplateRoot = Path.Combine(SolutionInfo.Root, ".github", "NMS_TEMPLATE");
            ConfigFilePath = Path.Combine(SolutionInfo.Root, ".github", "project.yml");
        }

        public static SolutionConfiguration GetNewestSolution()
        {
            return SolutionConfigurationExtension.LoadAndCreate();
        }
        public static void Save(SolutionConfiguration configuration)
        {
            var serializer = new SerializerBuilder()
                                 .WithNamingConvention(UnderscoredNamingConvention.Instance)
                                 .Build();
            var yaml = serializer.Serialize(configuration);
            File.WriteAllText(ConfigFilePath, yaml);
        }
    }
}
