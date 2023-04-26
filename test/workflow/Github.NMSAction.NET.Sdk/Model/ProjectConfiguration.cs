using Github.NET.Sdk.Model;

namespace Github.NMSAcion.NET.Sdk.Model
{
    public class CSProjectConfigurationBase
    {
        public string Id { get; set; } = string.Empty;

        public bool IsIgnored { get; set; }

        public bool IsFolded { get; set; }

        public string RelativePath { get; set; } = string.Empty;

        public string ProjectName { get; set; } = string.Empty;

        public string PackageName { get; set; } = string.Empty;

        public string ProjectFolder { get; set; } = string.Empty;

        public GithubLabelBase[]? Labels { get; set; }

        public DependencyConfiguration? DependencyConfig { get; set; }
    }

    public sealed class TestProjectConfiguration : CSProjectConfigurationBase
    {
        public bool TriggerPullRequestTest { get; set; }
        public bool TriggerCodecov { get; set; }
    }

    public sealed class WorkflowProjectConfiguration : CSProjectConfigurationBase
    {

    }
    public sealed class SamplesProjectConfiguration : CSProjectConfigurationBase
    {

    }

    public sealed class SrcProjectConfiguration : CSProjectConfigurationBase
    {
        public GlobalUsingConfiguration? UsingOutput { get; set; }
    }
    public sealed class ActionProjectConfiguration : CSProjectConfigurationBase
    {

    }

}
