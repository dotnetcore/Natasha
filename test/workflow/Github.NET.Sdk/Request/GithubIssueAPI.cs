using Github.NET.Sdk.Model;

namespace Github.NET.Sdk.Request
{
    public sealed class GithubIssueAPI
    {
        public async ValueTask<(bool, string)> DeleteAsync(string issueId)
        {
            //d4c5f9
            (var result, string error) = await GithubGraphRequest
                .Mutation()
                .Define("deleteIssue", p => p
                    .WithParameter("issueId", issueId)
                    )
                .Child("repository", e => e
                    .Child("id")).GraphResultAsync<GithubGraphReturn>();
            return (result?.Data?.DeleteIssue?.Repository?.Id != null, error);
        }
        public async Task<(GithubIssueConnections?, string)> GetsAsync(string owner, string repo, int count, bool? status = null, string? cursor = null)
        {
            (var result, string error) = await GithubGraphRequest
                .Query()
                .Define("repository", p => p.WithParameter("owner", owner).WithParameter("name", repo))
                .Child("issues", p => {
                    p.WithParameter("first", count);
                    if (status != null)
                    {
                        if (status.Value)
                        {
                            p.WithParameter("states", "OPEN", false);
                        }
                        else
                        {
                            p.WithParameter("states", "CLOSED", false);
                        }
                    }
                    if (cursor != null)
                    {
                        p.WithParameter("after", cursor);
                    }
                }, e => e
                    .Child("edges", e => e
                        .Child("node", e => e
                            .Child("id", "title", "number", "url")
                            .Child("author", e => e
                                .Child("login")))
                        .Child("cursor"))
                    .Child("pageInfo", e => e
                        .Child("endCursor", "hasNextPage"))
                    .Child("totalCount")).GraphResultAsync<GithubGraphReturn>();
            return (result?.Data?.Repository?.Issues, error);
        }
    }
}
