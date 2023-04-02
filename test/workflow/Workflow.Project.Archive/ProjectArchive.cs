using Github.NET.Sdk;
using Xunit;

namespace Workflow.Project.Archive
{
    [Trait("管道功能", "PROJECT")]
    public class ProjectArchive
    {
        [Fact(DisplayName = "归档")]
        public async Task Archive()
        {
            if(!NMSGithubSdk.TryGetTokenFromEnviroment(out string token, "GITHUB_TOKEN"))
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
            if (!NMSGithubSdk.TryGetEnviromentValue(out string projectName, "PROJECT_NAME", "${{ github.event.repository.name }}_VNext"))
            {
                Assert.Fail(projectName);
            }
            if (!NMSGithubSdk.TryGetEnviromentValue(out string projectArchiveName, "PROJECT_ARCHIVE_NAME", "${{ needs.prepare_check.outputs.releasePackString }}"))
            {
                Assert.Fail(projectArchiveName);
            }

            NMSGithubSdk.SetGraphSecretByEnvKey("GITHUB_TOKEN");
            var result = await NMSGithubSdk.ArchiveProjectAndCreateNewAsync(repoName, repoId, ownerName, ownerId, projectName, projectArchiveName);
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