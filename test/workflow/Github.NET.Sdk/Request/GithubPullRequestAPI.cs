using Github.NET.Sdk.Model;

namespace Github.NET.Sdk.Request
{
    public sealed class GithubPullRequestAPI
    {
        public async Task<(GithubPullRequestFileConnections?, string)> GetFilesAsync(string owner, string repo, int number, int count, string? cursor = null)
        {
            (var result, string error) = await GithubGraphRequest
                .Query()
                .Define("repository", p => p.WithParameter("owner", owner).WithParameter("name", repo))
                .Child("pullRequest", p => p.WithParameter("number", number), e => e
                    .Child("files", p => {
                        p.WithParameter("first", count);
                        if (cursor != null)
                        {
                            p.WithParameter("after", cursor);
                        }
                    }, e => e
                        .Child("nodes", e => e.Child("path", "additions", "deletions", "changeType"))
                        .Child("pageInfo", e => e.Child("endCursor", "hasNextPage"))
                        .Child("totalCount"))).GraphResultAsync<GithubGraphReturn>();
            return (result?.Data?.Repository?.PullRequest?.Files, error);
        }
    }
}
