using Github.NET.Sdk;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Workflow.PR.Association
{
    [Trait("管道功能", "PR")]
    public class PRAssociationRunner
    {
        [Fact(DisplayName = "关联 ISSUE")]
        public async Task Association()
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
            if (!NMSGithubSdk.TryGetEnviromentValue(out string prId, "PR_ID", "${{ github.event.pull_request.node_id }}"))
            {
                Assert.Fail(prId);
            }
            if (!NMSGithubSdk.TryGetEnviromentValue(out string prTitle, "PR_TITLE", "${{ github.event.pull_request.title }}"))
            {
                Assert.Fail(prTitle);
            }
            if (!NMSGithubSdk.TryGetEnviromentValue(out string minSimilarityScore, "MIN_SIMILAR_SCORE", "0.98"))
            {
                Assert.Fail(minSimilarityScore);
            }
            if (!NMSGithubSdk.TryGetEnviromentValue(out string maxSimilarityScore, "MAX_SIMILAR_SCORE", "1.00"))
            {
                Assert.Fail(maxSimilarityScore);
            }
            if (!NMSGithubSdk.TryGetEnviromentValue(out string pickCount, "PICK_COUNT", "3"))
            {
                Assert.Fail(pickCount);
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

            int count = Convert.ToInt32(pickCount);
            double min = Convert.ToDouble(minSimilarityScore);
            double max = Convert.ToDouble(maxSimilarityScore);
            NMSGithubSdk.SetGraphSecretByEnvKey("GITHUB_TOKEN");
            var error = await NMSGithubSdk.SetRecommendCompareWithIssuesAsync(
                  ownerName
                , repoName
                , prId
                , prTitle
                , isOpen
                , new List<(int count, double min, double max)> {
                    { ( count, min,  max) }
                },
                (recommends) =>
                {

                    StringBuilder comment = new();
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
                        comment.AppendLine($"未检测到合适的 ISSUE 推荐给您。感谢您的反馈！");
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