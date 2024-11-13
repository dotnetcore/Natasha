using Github.NET.Sdk.Model;

namespace Github.NET.Sdk.Request
{
    public sealed class GithubLabelAPI
    {
        public async Task<(GithubLabel[]?, string)> GetsAsync(string owner, string repo)
        {
            (var result, string error) = await GithubGraphRequest
                .Query()
                .Define("repository", p => p.WithParameter("owner", owner).WithParameter("name", repo))
                .Child("labels", p => p.WithParameter("first", 100), e => e
                    .Child("nodes", e => e
                        .Child("id", "name", "color", "description"))).GraphResultAsync<GithubGraphReturn>();
            return (result?.Data?.Repository?.Labels?.Nodes, error);
        }

        public async Task<(GithubLabel?, string)> GetByNameAsync(string owner, string repo, string labelName)
        {
            (var result, string error) = await GithubGraphRequest
                .Query()
                .Define("repository", p => p.WithParameter("owner", owner).WithParameter("name", repo))
                .Child("label", p => p.WithParameter("name", labelName), e => e
                    .Child("id")).GraphResultAsync<GithubGraphReturn>();
            return (result?.Data?.Repository?.Label, error);
        }

        public async Task<(GithubLabel?, string)> UpdateAsync(string labelId, string labelName, string labelColor, string labelDescription = "")
        {
            //d4c5f9
            (var result, string error) = await GithubGraphRequest
                .Mutation()
                .Define("updateLabel", p => p
                    .WithParameter("name", labelName)
                    .WithParameter("color", labelColor)
                    .WithParameter("id ", labelId)
                    .WithParameter("description", labelDescription)
                    )
                .Child("label", e => e
                    .Child("id")).GraphResultAsync<GithubGraphReturn>();
            return (result?.Data?.UpdateLabel?.Label, error);
        }

        public async Task<(GithubLabel?, string)> CreateAsync(string repoId, string labelName, string labelColor, string description = "")
        {
            //d4c5f9
            (var result, string error) = await GithubGraphRequest
                .Mutation()
                .Define("createLabel", p => p
                    .WithParameter("name", labelName)
                    .WithParameter("color", labelColor)
                    .WithParameter("repositoryId", repoId)
                    .WithParameter("description", description)
                    )
                .Child("label", e => e
                    .Child("id")).GraphResultAsync<GithubGraphReturn>();
            return (result?.Data?.CreateLabel?.Label, error);
        }
    }
}
