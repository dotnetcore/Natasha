using Github.NET.Sdk;
using Github.NET.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workflow.Runner
{
    [Trait("管道功能", "执行器")]
    public class Runner
    {
        [Fact(DisplayName = "标签屏蔽用户")]
        public async Task BlockUser()
        {
            if (NMSGithubSdk.JudgeCurrnetWorker("USER_BLOCK"))
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

                NMSGithubSdk.SetGraphSecretByEnvKey("GITHUB_TOKEN");
                NMSGithubSdk.SetApiSecretByEnKey("GITHUB_TOKEN");

                (var execResult, string error) = await NMSGithubSdk.HandleJunkIssueByLabelAsync(ownerName, repoName, "aaa-block-user");
                if (error != string.Empty)
                {
                    Assert.Fail(error);
                }
            }
        }

        [Fact(DisplayName = "相似 ISSUE 推荐")]
        public async Task RecommandIssue()
        {
            if (NMSGithubSdk.JudgeCurrnetWorker("ISSUE_RECOMMEND"))
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

        [Fact(DisplayName = "标签初始化")]
        public async Task InitLabels()
        {

            if (NMSGithubSdk.JudgeCurrnetWorker("LABEL_INIT"))
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
                var labels = SolutionRecorder.GetNewestSolution().GetAllLabels();
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

        [Fact(DisplayName = "归档 PR")]
        public async Task ArchivePR()
        {
            if (NMSGithubSdk.JudgeCurrnetWorker("PR_ARCHIVE"))
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
                var result = await NMSGithubSdk.AddPrToProjectAndSetStatusAsync(repoName, repoId, ownerName, ownerId, projectName, prId, "✅ Done", "Done", "已完成", "完成");
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


        [Fact(DisplayName = "PR 关联 ISSUE")]
        public async Task AssociationPR()
        {
            if (NMSGithubSdk.JudgeCurrnetWorker("PR_RECOMMEND"))
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

        [Fact(DisplayName = "PR 标记 Label")]
        public async Task LabelPR()
        {
            if (NMSGithubSdk.JudgeCurrnetWorker("PR_LABEL"))
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
                var solutionConfig = SolutionRecorder.GetNewestSolution();
                if (solutionConfig != null)
                {
                    (var result, string error) = await NMSGithubSdk.AddLabelToPRByFilesAsync(repoId, ownerName, repoName, prId, Convert.ToInt32(prNumber), solutionConfig.ToDictionary());
                    if (error != string.Empty)
                    {
                        Assert.Fail(error);
                    }
                }
                else
                {
                    Assert.Fail($"未找到 {SolutionRecorder.ConfigFilePath} 文件！");
                }
            }
        }

        [Fact(DisplayName = "计划归档")]
        public async Task ArchiveProject()
        {
            if (NMSGithubSdk.JudgeCurrnetWorker("PROJECT_ARCHIVE"))
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


        [Fact(DisplayName = "版本扫描")]
        public async Task ScanVersion()
        {
            if (NMSGithubSdk.JudgeCurrnetWorker("VERSION_SCANNER"))
            {
                bool isWriteToOutPut = false;
                var (version, log) = ChangeLogHelper.GetReleaseInfoFromFromFile(SolutionInfo.ChangeLogFile);
                if (!OperatingSystem.IsWindows())
                {
                    isWriteToOutPut = await CLIHelper.Output("RELEASE_VERSION", version);
                }
                Assert.True(isWriteToOutPut);
            }
        }
    }

}
