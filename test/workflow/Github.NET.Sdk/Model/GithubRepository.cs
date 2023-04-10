

namespace Github.NET.Sdk.Model
{
    public sealed class GithubRepository
    {
        public string Id { get; set; } = string.Empty;

        public bool IsInOrganization { get; set; }

        public GithubRepositoryOwner? Owner { get; set; }

        public GithubProject? ProjectV2 { get; set; }

        public GithubProjetConnections? ProjectsV2 { get; set; }

        public GithubIssueConnections? Issues { get; set; }

        public GithubLabel? Label { get; set; }

        public GithubLabelConnections? Labels { get; set; }

        public GithubPullRequest? PullRequest { get; set; }

    }

    public sealed class WrapperGithubRepository
    {
        public GithubRepository? Repository { get; set; }
    }


}
