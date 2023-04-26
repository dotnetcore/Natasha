using Github.NET.Sdk;
using Github.NET.Sdk.Model;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace Github.NMSAcion.NET.Sdk.Model
{
    public sealed class SolutionConfiguration
    {
        public HashSet<string> IgnoreProjects { get; set; } = new HashSet<string>();
        public IssueTemplateConfiguration[]? IssuesTemplateConfigs { get; set; }
        public ProjectGlobalConfiguration<ActionProjectConfiguration> Action { get; set; } = default!;
        public ProjectGlobalConfiguration<SamplesProjectConfiguration> Samples { get; set; } = default!;
        public ProjectGlobalConfiguration<SrcProjectConfiguration> Src { get; set; } = default!;
        public ProjectGlobalConfiguration<TestProjectConfiguration> Test { get; set; } = default!;
        public ProjectGlobalConfiguration<WorkflowProjectConfiguration> Workflow { get; set; } = default!;

    }
}
