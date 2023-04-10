namespace Github.NET.Sdk.Model
{

    public sealed class GithubProjetConnections
    {
        public GithubProject[]? Nodes { get; set; }
    }
    public sealed class GithubProject
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Number { get; set; } 
        public GithubProjectItemConnections? Items { get; set; }

        public ProjectV2SingleSelectField? Field { get; set; }
    }

    public sealed class WrapperProject
    {
        public GithubProject? ProjectV2 { get; set; }
    }
}
