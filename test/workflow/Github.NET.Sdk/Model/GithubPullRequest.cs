namespace Github.NET.Sdk.Model
{
    public sealed class GithubPullRequest
    {
        public GithubPullRequestFileConnections? Files { get; set; }
    }
    public sealed class GithubPullRequestFileConnections
    {
        public GithubPullRequestFile[]? Nodes { get; set; }
        public GithubPageInfo? PageInfo { get; set; }
    }
    public sealed class GithubPullRequestFile
    {
        public string Path { get; set; } = string.Empty;
        public string ViewerViewedState { get; set; } = string.Empty;
        public int Additions { get; set; }
        public int Deletions { get; set; }  
        public string ChangeType { get; set; } = string.Empty;
    }
}
