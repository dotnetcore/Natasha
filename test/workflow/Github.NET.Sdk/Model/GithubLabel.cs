namespace Github.NET.Sdk.Model
{
    public class GithubLabelBase
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Color { get; set; } = string.Empty;
    }
    public sealed class GithubLabel : GithubLabelBase
    {
        public string Id { get; set; } = string.Empty;
        public GithubIssueConnections? Issues { get; set; }
    }

    public sealed class WrapperGithubLabel 
    {
        public GithubLabel? Label { get; set; }
    }

    public sealed class GithubLabelConnections
    {
        public GithubLabel[]? Nodes { get; set; }
    }

}
