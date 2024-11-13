using Github.NET.Sdk.Model;

namespace Github.NET.Sdk.Request
{
    public sealed class GithubRepositoryAPI
    {
        public async Task<(GithubRepository?, string)> GetAsync(string owner, string repo)
        {
            (var result, string error) = await GithubGraphRequest
                .Query()
                .Define("repository", p => p.WithParameter("owner", owner).WithParameter("name", repo))
                .Child("id", "isInOrganization")
                .Child("owner", e => e
                    .Child("id")).GraphResultAsync<GithubGraphReturn>();

            return (result?.Data?.Repository, error);
        }
    }
}
