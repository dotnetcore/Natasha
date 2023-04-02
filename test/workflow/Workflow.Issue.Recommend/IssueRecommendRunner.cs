using Github.NET.Sdk;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Workflow.Label
{
    [Trait("管道功能", "ISSUE")]
    public class IssueRecommendRunner
    {

        [Fact(DisplayName = "相似推荐")]
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
            if (!NMSGithubSdk.TryGetEnviromentValue(out string issueId, "ISSUE_ID", "${{ github.event.issue.node_id }}"))
            {
                Assert.Fail(issueId);
            }
            if (!NMSGithubSdk.TryGetEnviromentValue(out string issueTitle, "ISSUE_TITLE", "${{ github.event.issue.title }}"))
            {
                Assert.Fail(issueTitle);
            }
            var issueStatus = Environment.GetEnvironmentVariable("ISSUE_STATUS");
            bool? isOpen = null;
            if (issueStatus != null && issueStatus != "ALL")
            {
                if (issueStatus.Contains("OPEN"))
                {
                    isOpen = true;
                }
                else
                {
                    isOpen = false;
                }
            }

            NMSGithubSdk.SetGraphSecretByEnvKey("GITHUB_TOKEN");

            var error = await NMSGithubSdk.SetRecommendCompareWithIssuesAsync(
                  ownerName
                , repoName
                , issueId
                , issueTitle
                , isOpen
                , new List<(int count, double min, double max)> {
                    { (1,0.98,1.01) },
                    { (3,0.70,0.98) },
                    { (2,0.55,0.70) },
                    { (1,0.40,0.55) }
                },
                (recommends) =>
                {

                    StringBuilder comment = new StringBuilder();
                    if (recommends.Length > 0)
                    {
                        comment.AppendLine(@"|编号|相似度|ISSUE|");
                        comment.AppendLine(@"| :----: | :----: | :---------- |");
                        int index = 0;
                        foreach (var issue in recommends)
                        {
                            if (issue != null)
                            {
                                index += 1;
                                comment.AppendLine($"|{index}|{issue.Output}| [{issue.Title}]({issue.Url}) |");
                            }
                        }
                        comment.AppendLine();
                    }
                    else
                    {
                        comment.AppendLine("未检测到合适的 ISSUE 推荐给您。感谢您的反馈！");
                    }
                    comment.AppendLine("> 该条自动推荐信息来自于 nms-bot.");
                    return comment.ToString();
                });

            if (error != string.Empty)
            {
                Assert.Fail(error);
            }

        }
    }
}