using Github.NET.Sdk;
using Github.NET.Sdk.Model;
using Xunit;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Workflow.Label
{
    [Trait("管道功能", "LABEL")]
    public class LabelInitialize
    {

        [Fact(DisplayName = "标签初始化:PR")]
        public async Task Init()
        {
            
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
            var labels = SolutionHelper.GetSolutionInfo()?.GetAllLabels();
            if (labels != null && labels.Any())
            {
                var labelsWithBlockUser = labels.Append(new GithubLabelBase() { Name = "aaa-block-user", Color = "ff0000", Description = "该标签为屏蔽标签,打上该标签的 ISSUE 作者将被屏蔽." });
                var result = await NMSGithubSdk.ExpectLabelsCreateAndUpadateAsync(labelsWithBlockUser, repoId, ownerName, repoName);
                if (result != string.Empty)
                {
                    Assert.Fail(result);
                }
            }

        }
    }
}