using Github.NET.Sdk.Model;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Github.NET.Sdk
{
    public static class NMSGithubSdk
    {
        private static readonly HttpClient _webApi;

        public static void SetApiSecretByEnKey(string envKey = "GITHUB_TOKEN")
        {
            var key = Environment.GetEnvironmentVariable(envKey);
            _webApi.DefaultRequestHeaders.Add("Authorization", $"Bearer {key}");
        }

        public static void SetGraphSecretByEnvKey(string envKey = "GITHUB_TOKEN")
        {
            GithubGraphRequest.SetSecretByEnvKey(envKey);
        }
#if DEBUG
        public static void SetGraphSecret(string token)
        {
            GithubGraphRequest.SetSecret(token);
        }
        public static void SetApiSecret(string token)
        {
            _webApi.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
#endif


        public static bool TryGetTokenFromEnviroment(out string result, string keyName)
        {
            return TryGetEnviromentValue(out result, keyName, "{your token}", $"(参考:https://docs.github.com/zh/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token {Environment.NewLine} 拿到 Token 需要配置, 在你的仓库配置 Setting -> Secrets and variables -> Action 创建安全变量.");
        }
        public static bool TryGetEnviromentValue(out string result, string keyName, string valueDefine, string? details = null)
        {
            var value = Environment.GetEnvironmentVariable(keyName);
            if (value == null)
            {
                result = $"环境变量中未检测到键为 {keyName} 的值.(请在运行作业或工作流中添加 \"env: {keyName}:{valueDefine}\"){Environment.NewLine}";
                if (details != null)
                {
                    result += details;
                }
                return false;
            }
            else
            {
                result = value;
                return true;
            }
        }


        private static readonly GithubProjectAPI _projectReuqest;
        private static readonly GithubIssueAPI _issueRequest;

        static NMSGithubSdk()
        {
            _projectReuqest = GithubSdk.Project;
            _issueRequest = GithubSdk.Issue;
            _webApi = new HttpClient();
            _webApi.BaseAddress = new Uri("https://api.github.com/");
            _webApi.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
            _webApi.DefaultRequestHeaders.Add("User-Agent", "Awesome-Octocat-App");
            _webApi.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");

        }

        public static async Task<string> SetRecommendCompareWithIssuesAsync(string ownerName, string repoName, string currentItemId, string sourceTitle, bool? isOpen, List<(int count, double min, double max)> pickInfo, Func<RecommendResultModel[], string> commentAction)
        {

            (var issues, string error) = await GetAllIssuesAsync(ownerName, repoName, isOpen, currentItemId);

            if (issues != null)
            {
                (var recommends, error) = await RecommendHelper.RecommendAsync(sourceTitle, issues.Select(item => new RecommendInfoModel { Id = item.Id, Number = item.Number, Title = item.Title, Url = item.Url }), pickInfo);
                if (recommends != null)
                {
                    var comment = commentAction(recommends);
                    (var commentResult, error) = await GithubSdk.IssueOrPullRequest.AddCommentAsync(currentItemId, comment);
                    if (!commentResult)
                    {
                        return $"提交评论 { comment} 时出错：{error}";
                    }
                    return string.Empty;
                }
            }
            return error;

        }


        public static async Task<(IEnumerable<GithubIssue>?,string)> GetAllIssuesAsync(string ownerName, string repoName, bool? status = null, params string[] expectIds)
        {
            List<GithubIssue> result = new();
            var expectSets = new HashSet<string>(expectIds);
            (var connections, string error) = await GithubSdk.Issue.GetsAsync(ownerName, repoName, 100, status);
            if (connections != null)
            {
                var pageInfo = connections.PageInfo;
                if (pageInfo!=null)
                {
                    var edges = connections.Edges?.Where(item => !expectSets.Contains(item.Node.Id))?.Select(item => item.Node);
                    if (edges != null && edges.Any())
                    {
                        result.AddRange(edges);
                    }
                    while (pageInfo.HasNextPage)
                    {
                        (connections, error) = await GithubSdk.Issue.GetsAsync(ownerName, repoName, 100, status, pageInfo.EndCursor);
                        if (connections==null)
                        {
                            return (null, error);
                        }

                        pageInfo = connections.PageInfo;
                        if (pageInfo == null)
                        {
                            return (null, error);
                        }

                        edges = connections.Edges?.Where(item => !expectSets.Contains(item.Node.Id))?.Select(item => item.Node);
                        if (edges != null && edges.Any())
                        {
                            result.AddRange(edges);
                        }
                    }
                    return (result, string.Empty);
                }
            }
            return (null, error);
        }

        public static void SetSecretToken(string envKey = "GITHUB_TOKEN")
        {
            GithubGraphRequest.SetSecretByEnvKey(envKey);
        }


        public static async Task<(GithubProject?, string)> CreateVNextProject(string ownerId, string repoId, string projectName)
        {
            (var projectVNext, string error) = await _projectReuqest.CreateAsync(ownerId!, repoId!, projectName);
            if (projectVNext == null)
            {
                return (projectVNext, "创建计划失败!" + error);
            }
            (var updateResult, error) = await _projectReuqest.UpdateAsync(projectVNext.Id, visiable: true, shortDescription: $"Archive project created at {DateTime.Now:yyyy-MM-dd HH:mm:ss} by nmsbot.");
            if (!updateResult)
            {
                return (projectVNext, "更新 VNext 计划状态失败" + error);
            }
            return (projectVNext, string.Empty);
        }


        public static async Task<string> AddPrToProjectAndSetStatusAsync(string repoName, string repoId, string ownerName, string ownerId, string vNextName, string prId, params string[] statusNames)
        {

            (var projectVNext, string error) = await _projectReuqest.GetAsync(ownerName!, repoName!, vNextName);
            if (projectVNext == null)
            {
                (projectVNext, error) = await CreateVNextProject(ownerId!, repoId!, vNextName);
                if (error != string.Empty)
                {
                    return error;
                }
            }
            if (projectVNext != null)
            {

                (var item, error) = await _projectReuqest.AddItemAsync(projectVNext.Id, prId!);
                if (item != null)
                {
                    var field = projectVNext.Field;
                    if (field == null)
                    {
                        //await Task.Delay(1000);
                        (projectVNext, error) = await _projectReuqest.GetAsync(ownerName!, repoName!, vNextName);
                        field = projectVNext!.Field;
                    }
                    if (field != null)
                    {

                        var value = field.Options?.First(item => statusNames.Contains(item.Name));
                        if (value != null)
                        {
                            var archiveResult = await _projectReuqest.UpdateItemStatusAsync(
                               projectVNext.Id,
                               item.Id,
                               projectVNext.Field!.Id,
                               value);

                            return string.Empty;
                        }
                        else
                        {
                            return $"未找到 VNext 计划中的 Status 字段未找到值：{string.Join(',', statusNames)}！{error}";
                        }

                    }
                    else
                    {
                        return $"未找到 VNext 计划中有 Status 字段！{error}";
                    }

                }
                else
                {
                    return $"PR 归档失败！{error}";
                }
            }
            else
            {
                return $"创建 VNext 计划不成功，PR 未归档！{error}";
            }
        }


        public static async Task<string> ArchiveProjectAndCreateNewAsync(string repoName, string repoId, string ownerName, string ownerId, string vNextName, string archiveName)
        {

            (var existArchiveProject, _) = await _projectReuqest.ExistAsync(ownerName, repoName, archiveName);
            if (!existArchiveProject)
            {
                (var projectVNext, _) = await _projectReuqest.GetAsync(ownerName!, repoName!, vNextName);
                if (projectVNext != null)
                {
                    (var updateResult, string error) = await _projectReuqest.UpdateAsync(projectVNext.Id, closed: true, title: archiveName);
                    if (!updateResult)
                    {
                        return $"归档 {vNextName} 计划状态失败!{error}";
                    }
                    else
                    {
                        (_, error) = await CreateVNextProject(ownerId!, repoId!, vNextName);
                        if (error != string.Empty)
                        {
                            return error;
                        }
                    }
                }
            }
            return string.Empty;
        }

        public static async Task<string> ExpectLabelsCreateAsync(HashSet<string> expectLabels, string repoId, string ownerName, string repoName, Queue<string>? randomColor = null, string? referencOwnerName = null, string? referencRepoName = null)
        {
            (var myLabels, string error) = await GithubSdk.Label.GetsAsync(ownerName, repoName);
            if (myLabels != null)
            {
                expectLabels.ExceptWith(myLabels!.Select(item => item.Name));
                if (expectLabels.Count > 0)
                {

                    Dictionary<string, GithubLabel> referecLabelMap = new();
                    if (referencOwnerName != null && referencRepoName != null)
                    {
                        (var labels, string getError) = await GithubSdk.Label.GetsAsync(referencOwnerName, referencRepoName);
                        if (labels != null)
                        {
                            foreach (var item in labels)
                            {
                                referecLabelMap[item.Name.ToLowerInvariant()] = item;
                            }
                        }
                        else
                        {
                            return $"参考库的Labels是否存在异常? {getError}";
                        }
                    }

                    foreach (var newLable in expectLabels)
                    {
                        var color = Environment.GetEnvironmentVariable($"{newLable.ToUpperInvariant()}_LABEL_COLOR");
                        var description = Environment.GetEnvironmentVariable($"{newLable.ToUpperInvariant()}_LABEL_DESCRIPTION");
                        if (color == null)
                        {
                            if (referecLabelMap.ContainsKey(newLable))
                            {
                                color = referecLabelMap[newLable].Color;
                            }
                            else if (randomColor != null && randomColor.Count > 0)
                            {
                                color = randomColor.Dequeue();
                            }
                            else
                            {
                                return "创建标签失败,颜色池已耗尽,更多的颜色建议在环境变量中添加:如 bug => env:  BUG_LABEL_COLOR: #xxx";
                            }
                        }
                        if (description == null)
                        {
                            if (referecLabelMap.ContainsKey(newLable))
                            {
                                description = referecLabelMap[newLable].Color;
                            }
                            else
                            {
                                description = $"[{newLable}] made by nmsbot.";
                            }
                        }


                        (var result, error) = await GithubSdk.Label.CreateAsync(repoId, newLable, color, description);
                        if (!result)
                        {
                            return $"API 创建 #{color} 颜色的 <{newLable}> 标签失败! {error}";
                        }
                    }

                }
            }
            return string.Empty;
        }
        public static async Task<string> ExpectLabelsCreateAsync(HashSet<string> expectLabels, string repoId, string ownerName, string repoName, string? specialColor = null, string? referencOwnerName = null, string? referencRepoName = null)
        {
            (var myLabels, string error) = await GithubSdk.Label.GetsAsync(ownerName, repoName);
            if (myLabels != null)
            {
                expectLabels.ExceptWith(myLabels!.Select(item => item.Name));
                if (expectLabels.Count > 0)
                {


                    Dictionary<string, GithubLabel> referecLabelMap = new();
                    if (referencOwnerName != null && referencRepoName != null)
                    {
                        (var labels, string getError) = await GithubSdk.Label.GetsAsync(referencOwnerName, referencRepoName);
                        if (labels != null)
                        {
                            foreach (var item in labels)
                            {
                                referecLabelMap[item.Name.ToLowerInvariant()] = item;
                            }
                        }
                        else
                        {
                            return $"参考库的Labels是否存在异常? {getError}";
                        }
                    }

                    foreach (var newLable in expectLabels)
                    {
                        var color = Environment.GetEnvironmentVariable($"{newLable.ToUpperInvariant()}_LABEL_COLOR");
                        var description = Environment.GetEnvironmentVariable($"{newLable.ToUpperInvariant()}_LABEL_DESCRIPTION");
                        if (color == null)
                        {
                            if (referecLabelMap.ContainsKey(newLable))
                            {
                                color = referecLabelMap[newLable].Color;
                            }
                            else if (specialColor != null)
                            {
                                color = specialColor;
                            }
                            else
                            {
                                return "创建标签失败,颜色池已耗尽,更多的颜色建议在环境变量中添加:如 bug => env:  BUG_LABEL_COLOR: #xxx";
                            }
                        }
                        if (description == null)
                        {
                            if (referecLabelMap.ContainsKey(newLable))
                            {
                                description = referecLabelMap[newLable].Color;
                            }
                            else
                            {
                                description = $"[{newLable}] made by nmsbot.";
                            }
                        }

                        (var result, error) = await GithubSdk.Label.CreateAsync(repoId, newLable, color, description);
                        if (!result)
                        {
                            return $"API 创建 #{color} 颜色的 <{newLable}> 标签失败! {error}";
                        }
                    }

                }
            }
            return string.Empty;
        }

        public static async Task<string> CreateLabelIfNotExist(string newLabelName, string repoId, string ownerName, string repoName, string? specialColor = null, string? referencOwnerName = null, string? referencRepoName = null)
        {
            (var myLabels, string error) = await GithubSdk.Label.GetsAsync(ownerName, repoName);
            if (myLabels != null)
            {

                if (myLabels.Select(item => item.Name).Contains(newLabelName))
                {
                    return string.Empty;
                }

                Dictionary<string, GithubLabel> referecLabelMap = new();
                if (referencOwnerName != null && referencRepoName != null)
                {
                    (var labels, string getError) = await GithubSdk.Label.GetsAsync(referencOwnerName, referencRepoName);
                    if (labels != null)
                    {
                        foreach (var item in labels)
                        {
                            referecLabelMap[item.Name.ToLowerInvariant()] = item;
                        }
                    }
                    else
                    {
                        return $"参考库的Labels是否存在异常? {getError}";
                    }
                }

                var color = Environment.GetEnvironmentVariable($"{newLabelName.ToUpperInvariant()}_LABEL_COLOR");
                var description = Environment.GetEnvironmentVariable($"{newLabelName.ToUpperInvariant()}_LABEL_DESCRIPTION");
                if (color == null)
                {
                    if (referecLabelMap.ContainsKey(newLabelName))
                    {
                        color = referecLabelMap[newLabelName].Color;
                    }
                    else if (specialColor != null)
                    {
                        color = specialColor;
                    }
                    else
                    {
                        return "创建标签失败,颜色池已耗尽,更多的颜色建议在环境变量中添加:如 bug => env:  BUG_LABEL_COLOR: #xxx";
                    }
                }
                if (description == null)
                {
                    if (referecLabelMap.ContainsKey(newLabelName))
                    {
                        description = referecLabelMap[newLabelName].Color;
                    }
                    else
                    {
                        description = $"[{newLabelName}] made by nmsbot.";
                    }
                }


                (var result, error) = await GithubSdk.Label.CreateAsync(repoId, newLabelName, color, description);
                if (!result)
                {
                    return $"API 创建 #{color} 颜色的 <{newLabelName}> 标签失败! {error}";
                }

            }
            return string.Empty;
        }

        public static async Task<(GithubIssue[]?, bool?, string)> GetIssueAuthorsByLabelAsync(string ownerName, string repoName, string labelName)
        {
            (var result, string error) = await GithubGraphRequest
                .Query()
                .Define("repository", p => p.WithParameter("owner", ownerName).WithParameter("name", repoName))
                    .Child("isInOrganization")
                    .Child("label", p => p.WithParameter("name", labelName), e => e
                            .Child("issues", p => p.WithParameter("first", 100), e => e
                                .Child("nodes", e => e
                                    .Child("id")
                                    .Child("author", e => e
                                        .Child("login"))))).GraphResultAsync<GithubGraphReturn>();

            var issues = result?.Data?.Repository?.Label?.Issues?.Nodes;
            if (issues != null && issues.Length > 0)
            {
                return (issues, result!.Data!.Repository!.IsInOrganization, string.Empty);
            }
            return (null, null, error);
        }

        public static async Task<(bool, string)> HandleJunkIssueByLabelAsync(string ownerName, string repoName, string labelName)
        {
            (var issues, var isInOrg, var error) = await GetIssueAuthorsByLabelAsync(ownerName, repoName, labelName);
            if (issues != null && issues.Length > 0)
            {
                Dictionary<string, IEnumerable<GithubIssue>> cache = issues.ToDictionary(issue => issue.Author.Login, issue => issues.Where(item => item.Author.Login == issue.Author.Login)); ;

                if (isInOrg!.Value)
                {
                    foreach ((var blockName,var values) in cache)
                    {
                        var result = await BlockUserByOrgNameAsync(blockName, ownerName);
                        if (!result.Item1)
                        {
                            return result;
                        }
                        foreach (var issue in values)
                        {
                            result =await _issueRequest.DeleteAsync(issue.Id);
                            if (!result.Item1)
                            {
                                return result;
                            }
                        }
                    }
                }
                else
                {
                    foreach ((var blockName, var values) in cache)
                    {
                        var result = await BlockUserByUserNameAsync(blockName);
                        if (!result.Item1)
                        {
                            return result;
                        }
                        foreach (var issue in values)
                        {
                            result = await _issueRequest.DeleteAsync(issue.Id);
                            if (!result.Item1)
                            {
                                return result;
                            }
                        }
                    }
                }
            }

            return (false, error);
        }
        public static async ValueTask<(bool, string)> BlockUserByOrgNameAsync(string blockName, string orgName)
        {
            /*
                https://docs.github.com/zh/rest/orgs/blocking?apiVersion=2022-11-28#check-if-a-user-is-blocked-by-an-organization
             */
            var response = await _webApi.GetAsync($"/orgs/{orgName}/blocks/{blockName}");
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return (true, string.Empty);
            }
            else
            {
                response = await _webApi.PutAsync($"/orgs/{orgName}/blocks/{blockName}", null);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return (true, string.Empty);
                }
                return (false, await response.Content.ReadAsStringAsync());
            }
        }
        public static async ValueTask<(bool, string)> BlockUserByUserNameAsync(string blockName)
        {
            /*
                https://docs.github.com/zh/rest/users/blocking?apiVersion=2022-11-28#check-if-a-user-is-blocked-by-the-authenticated-user
             */
            var response = await _webApi.GetAsync($"/user/blocks/{blockName}");
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return (true, string.Empty);
            }
            else
            {
                response = await _webApi.PutAsync($"/user/blocks/{blockName}", null);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return (true, string.Empty);
                }
                return (false, await response.Content.ReadAsStringAsync());
            }
        }
    }
}
