using Github.NET.Sdk;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Workflow.PR.Label
{
    [Trait("管道功能", "PR")]
    public class LabelToPR
    {
        [Fact(DisplayName = "标记Label")]
        public async Task Label()
        {

            if (!NMSGithubSdk.TryGetTokenFromEnviroment(out string token, "GITHUB_TOKEN"))
            {
                Assert.Fail(token);
            }
            if (!NMSGithubSdk.TryGetEnviromentValue(out string repoName, "REPO_NAME", "${{ github.event.repository.name }}"))
            {
                Assert.Fail(repoName);
            }
            if (!NMSGithubSdk.TryGetEnviromentValue(out string repoId, "REPO_ID", "${{ github.event.repository.id }}"))
            {
                Assert.Fail(repoId);
            }
            if (!NMSGithubSdk.TryGetEnviromentValue(out string ownerName, "OWNER_NAME", "${{ github.repository_owner }}"))
            {
                Assert.Fail(ownerName);
            }
            if (!NMSGithubSdk.TryGetEnviromentValue(out string prNumber, "PR_NUM", "${{ github.event.pull_request.number }}"))
            {
                Assert.Fail(prNumber);
            }
            if (!NMSGithubSdk.TryGetEnviromentValue(out string prId, "PR_ID", "${{ github.event.pull_request.node_id }}"))
            {
                Assert.Fail(prId);
            }

            NMSGithubSdk.SetGraphSecretByEnvKey("GITHUB_TOKEN");
            var solutionConfig = SolutionHelper.GetSolutionInfo();
            if (solutionConfig != null)
            {
                (var result,string error) = await NMSGithubSdk.AddLabelToPRByFilesAsync(repoId, ownerName, repoName, prId, Convert.ToInt32(prNumber), solutionConfig.ToDictionary());
                if (error != string.Empty)
                {
                    Assert.Fail(error);
                } 
            }
            else
            {
                Assert.Fail($"未找到 {SolutionHelper.ConfigFilePath} 文件！");
            }
            
        }
    }
}