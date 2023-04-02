using Github.NET.Sdk;
using System.Threading.Tasks;
using Xunit;

namespace Workflow.PR.Archive
{
    [Trait("管道功能", "PR")]
    public class PRToProject
    {
        [Fact(DisplayName = "归档")]
        public async Task Archive()
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
            if (!NMSGithubSdk.TryGetEnviromentValue(out string ownerId, "OWNER_ID", "${{ github.event.repository.owner.node_id }}"))
            {
                Assert.Fail(ownerId);
            }
            if (!NMSGithubSdk.TryGetEnviromentValue(out string projectName, "PROJECT_NAME", "myProjectName"))
            {
                Assert.Fail(projectName);
            }
            if (!NMSGithubSdk.TryGetEnviromentValue(out string prId, "PR_ID", "${{ github.event.pull_request.node_id }}"))
            {
                Assert.Fail(prId);
            }

            NMSGithubSdk.SetGraphSecretByEnvKey("GITHUB_TOKEN");
            var result = await NMSGithubSdk.AddPrToProjectAndSetStatusAsync(repoName, repoId, ownerName, ownerId, projectName, prId, "Done", "已完成", "完成");
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