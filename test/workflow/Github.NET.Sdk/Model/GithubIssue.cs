namespace Github.NET.Sdk.Model
{
    public sealed class GithubIssueConnections
    {
        public GithubIssue[]? Nodes { get; set; }
        public GithubIssueEdge[]? Edges { get; set; }
        public GithubPageInfo? PageInfo { get; set; }
        public int TotalCount { get; set; }

    }

    public sealed class GithubIssueEdge
    {
        public GithubIssue Node { get; set; } = default!;
        public string? Cursor { get; set; }
    }

    public sealed class WrapperGithubIssue
    {
        public GithubIssue Node { get; set; } = default!;
    }
    public sealed class GithubIssue
    {
        public string Id { get; set; } = default!;
        public int Number { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Url { get; set; } = default!;
        public Author Author { get; set; } = default!;
    }

    public class Author
    {
        public string Login { get; set; } = string.Empty;
    }
}
