namespace Github.NET.Sdk.Model
{
    public sealed class GithubComment
    {
        public string Id { get; set; } = string.Empty;
    }

    public sealed class WrapperGithubComment
    {
        public GithubComment? Subject { get; set; }
    }
}
