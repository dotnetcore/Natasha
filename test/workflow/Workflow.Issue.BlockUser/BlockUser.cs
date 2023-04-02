using Github.NET.Sdk;
using Xunit;

namespace Workflow.Label
{
    [Trait("管道功能", "LABEL")]
    public class BlockUser
    {

        [Fact(DisplayName = "标签屏蔽用户")]
        public async Task Block()
        {
            var referencOwnerName = Environment.GetEnvironmentVariable("REFERENC_OWNER_NAME");
            var referencRepoName = Environment.GetEnvironmentVariable("REFERENC_REPO_NAME");

            if (!NMSGithubSdk.TryGetTokenFromEnviroment(out string token, "GITHUB_TOKEN"))
            {
                Assert.Fail(token);
            }
            if (!NMSGithubSdk.TryGetEnviromentValue(out string repoName, "REPO_NAME", "${{ github.event.repository.name }}"))
            {
                Assert.Fail(repoName);
            }
            if (!NMSGithubSdk.TryGetEnviromentValue(out string ownerName, "OWNER_NAME", "${{ github.repository_owner }}"))
            {
                Assert.Fail(ownerName);
            }

            NMSGithubSdk.SetGraphSecretByEnvKey("GITHUB_TOKEN");
            NMSGithubSdk.SetApiSecretByEnKey("GITHUB_TOKEN");

            (var execResult, string error) = await NMSGithubSdk.HandleJunkIssueByLabelAsync(ownerName, repoName, "aaa-block-user");
            if (error != string.Empty)
            {
                Assert.Fail(error);
            }
        }
    }
}