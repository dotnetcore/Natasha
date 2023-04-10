namespace Github.NET.Sdk.Model
{
    public sealed class GithubProjectItemConnections
    {
        public GithubProjectItem[]? Nodes { get; set; }
    }
    public sealed class GithubProjectItem
    {

        public string Id { get; set; } = string.Empty;
        public GithubProjectPullRequestItem? Content { get; set; }
        public GithubProjectPullRequestItem? FieldValueByName { get; set; }

    }

    public sealed class WrapperGithubProjectItem
    {
        public GithubProjectItem? ProjectV2Item { get; set; }
        public GithubProjectItem? Item { get; set; }
    }

    public sealed class GithubProjectPullRequestItem
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}
