using Github.NET.Sdk.Request;

namespace Github.NET.Sdk
{
    public static class GithubSdk
    {
        public static readonly GithubProjectAPI Project;
        public static readonly GithubRepositoryAPI Repository;
        public static readonly GithubPullRequestAPI PullRequest;
        public static readonly GithubLabelAPI Label;
        public static readonly GithubIssueAPI Issue;
        public static readonly GithubPullRequestOrIssueAPI IssueOrPullRequest;


        static GithubSdk()
        {
            Project = new GithubProjectAPI();
            Repository = new GithubRepositoryAPI();
            PullRequest = new GithubPullRequestAPI();
            Label = new GithubLabelAPI();
            Issue = new GithubIssueAPI();
            IssueOrPullRequest = new GithubPullRequestOrIssueAPI();
        }

        public static void SetSecretTokenByEnvKey(string envKey = "GITHUB_TOKEN")
        {
            GithubGraphRequest.SetSecretByEnvKey(envKey);
        }
#if DEBUG
        public static void SetGraphSecret(string token)
        {
            GithubGraphRequest.SetSecret(token);
        }
#endif
    }
}
