using Github.NET.Sdk;
using Xunit;

namespace Workflow.Label
{
    [Trait("管道功能", "LABEL")]
    public class LabelInitialize
    {

        [Fact(DisplayName = "标签初始化:ISSUE模板")]
        public async Task Init()
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
            if (!NMSGithubSdk.TryGetEnviromentValue(out string repoId, "REPO_ID", "${{ github.event.repository.node_id }}"))
            {
                Assert.Fail(repoId);
            }


            NMSGithubSdk.SetGraphSecretByEnvKey("GITHUB_TOKEN");

            var issueLabels = IssueTemplateHelper.ScanLabelTempaltes();
            if (issueLabels != null && issueLabels.Count > 0)
            {
                Queue<string> randomColor = new();
                randomColor.Enqueue("fef2c0");
                randomColor.Enqueue("A44F86");
                randomColor.Enqueue("3707F0");
                randomColor.Enqueue("B3E8D6");
                randomColor.Enqueue("A0C4AF");
                randomColor.Enqueue("F82BBC");
                randomColor.Enqueue("31FA1C");
                randomColor.Enqueue("0ACB31");
                randomColor.Enqueue("FB5F95");
                randomColor.Enqueue("5D64FA");

                var result = await NMSGithubSdk.ExpectLabelsCreateAsync(issueLabels, repoId, ownerName, repoName, randomColor, referencOwnerName, referencRepoName);
                if (result != string.Empty)
                {
                    Assert.Fail(result);
                }
                else
                {
                    Assert.True(true);
                }
            }


            var error = await NMSGithubSdk.CreateLabelIfNotExist("aaa-block-user", repoId, ownerName, repoName, null, referencOwnerName, referencRepoName);
            if (error != string.Empty)
            {
                Assert.Fail(error);
            }
        }
    }
}