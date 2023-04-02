using Github.NET.Sdk;
using Xunit;

namespace Workflow.Label
{
    [Trait("管道功能", "LABEL")]
    public class LabelInitialize
    {

        [Fact(DisplayName = "标签初始化:PR")]
        public async Task Init()
        {
            var referencOwnerName = Environment.GetEnvironmentVariable("REFERENC_OWNER_NAME");
            var referencRepoName = Environment.GetEnvironmentVariable("REFERENC_REPO_NAME");
            var specialColor = Environment.GetEnvironmentVariable("REFERENC_REPO_NAME");
            if (specialColor == null)
            {
                specialColor = "68E0F8";
            }

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
            if (!NMSGithubSdk.TryGetEnviromentValue(out string repoId, "REPO_ID", "${{ github.event.repository.node_id }}"))
            {
                Assert.Fail(repoId);
            }


            NMSGithubSdk.SetGraphSecretByEnvKey("GITHUB_TOKEN");

            var prLabels = PRLabelerHelper.ScanLabelTempaltes();
            if (prLabels != null && prLabels.Count > 0)
            {
                var result = await NMSGithubSdk.ExpectLabelsCreateAsync(prLabels, repoId, ownerName, repoName, specialColor, referencOwnerName, referencRepoName);
                if (result != string.Empty)
                {
                    Assert.Fail(result);
                }
                else
                {
                    Assert.True(true);
                }
            }

        }
    }
}