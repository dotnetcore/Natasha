using Github.NET.Sdk.Model;

namespace Github.NET.Sdk
{
    public static class GithubSdk
    {
        public static readonly GithubProjectAPI Project;
        public static readonly GithubRepositoryAPI Repository;
        public static readonly GithubPullRequestAPI PullRequest;
        public static readonly GithubLabelAPI Label;
        public static readonly GithubIssueAPI Issue;
        public static readonly GithubPullRequestOrIssueAPI IssueOrPullRequest;


        static GithubSdk()
        {
            Project = new GithubProjectAPI();
            Repository = new GithubRepositoryAPI();
            PullRequest = new GithubPullRequestAPI();
            Label = new GithubLabelAPI();
            Issue = new GithubIssueAPI();
            IssueOrPullRequest = new GithubPullRequestOrIssueAPI();
        }

        public static void SetSecretTokenByEnvKey(string envKey = "GITHUB_TOKEN")
        {
            GithubGraphRequest.SetSecretByEnvKey(envKey);
        }
#if DEBUG
        public static void SetGraphSecret(string token)
        {
            GithubGraphRequest.SetSecret(token);
        }
#endif
    }

    public sealed class GithubPullRequestOrIssueAPI
    {
        public async ValueTask<(bool, string)> AddCommentAsync(string itemId, string body)
        {
            (var result, string error) = await GithubGraphRequest
               .Mutation()
               .Define("addComment", p => p
                   .WithParameter("subjectId", itemId)
                   .WithParameter("body", body)
                   )
               .Child("subject", e => e
                   .Child("id")).GraphResultAsync<GithubGraphReturn>();
            return (result?.Data?.AddComment?.Subject?.Id != null, error);
        }

        public async Task<(bool,string)> AddLabelAsync(string itemId,params string[] labelIds)
        {
            (var result, string error) = await GithubGraphRequest
               .Mutation()
               .Define("addLabelsToLabelable", p => p
                   .WithParameter("labelableId", itemId)
                   .WithParameter("labelIds", labelIds)
                   )
               .Child("labelable", e => e
                   .Child("__typename")).GraphResultAsync<GithubGraphReturn>();
            return (result?.Data?.AddLabelsToLabelable?.Labelable?.__typename != null, error);
        }
    }

    public sealed class GithubPullRequestAPI
    {
        public async Task<(GithubPullRequestFileConnections?, string)> GetFilesAsync(string owner, string repo, int number, int count, string? cursor = null)
        {
            (var result, string error) = await GithubGraphRequest
                .Query()
                .Define("repository", p => p.WithParameter("owner", owner).WithParameter("name", repo))
                .Child("pullRequest", p => p.WithParameter("number", number), e => e
                    .Child("files", p=> {
                        p.WithParameter("first", count);
                        if (cursor != null)
                        {
                            p.WithParameter("after", cursor);
                        }
                    }, e => e
                        .Child("nodes", e => e.Child("path", "additions", "deletions", "changeType"))
                        .Child("pageInfo", e => e.Child("endCursor", "hasNextPage"))
                        .Child("totalCount"))).GraphResultAsync<GithubGraphReturn>();
            return (result?.Data?.Repository?.PullRequest?.Files, error);
        }
    }
    public sealed class GithubIssueAPI
    {
        public async ValueTask<(bool, string)> DeleteAsync(string issueId)
        {
            //d4c5f9
            (var result, string error) = await GithubGraphRequest
                .Mutation()
                .Define("deleteIssue", p => p
                    .WithParameter("issueId", issueId)
                    )
                .Child("repository", e => e
                    .Child("id")).GraphResultAsync<GithubGraphReturn>();
            return (result?.Data?.DeleteIssue?.Repository?.Id != null, error);
        }
        public async Task<(GithubIssueConnections?, string)> GetsAsync(string owner, string repo, int count, bool? status = null, string? cursor = null)
        {
            (var result, string error) = await GithubGraphRequest
                .Query()
                .Define("repository", p => p.WithParameter("owner", owner).WithParameter("name", repo))
                .Child("issues", p => {
                    p.WithParameter("first", count);
                    if (status!=null)
                    {
                        if (status.Value)
                        {
                            p.WithParameter("states", "OPEN", false);
                        }
                        else
                        {
                            p.WithParameter("states", "CLOSED", false);
                        }
                    }
                    if (cursor!=null)
                    {
                        p.WithParameter("after", cursor);
                    }
                }, e => e
                    .Child("edges", e => e
                        .Child("node", e=>e
                            .Child("id","title","number","url")
                            .Child("author",e=>e
                                .Child("login")))
                        .Child("cursor"))
                    .Child("pageInfo", e=>e
                        .Child("endCursor","hasNextPage"))
                    .Child("totalCount")).GraphResultAsync<GithubGraphReturn>();
            return (result?.Data?.Repository?.Issues, error);
        }
    }
    public sealed class GithubLabelAPI
    {
        public async Task<(GithubLabel[]?,string)> GetsAsync(string owner, string repo)
        {
            (var result, string error) =await GithubGraphRequest
                .Query()
                .Define("repository", p => p.WithParameter("owner", owner).WithParameter("name", repo))
                .Child("labels", p => p.WithParameter("first", 100), e => e
                    .Child("nodes", e=>e
                        .Child("id", "name","color","description"))).GraphResultAsync<GithubGraphReturn>();
            return (result?.Data?.Repository?.Labels?.Nodes,error);
        }

        public async Task<(GithubLabel?,string)> GetByNameAsync(string owner, string repo, string labelName)
        {
            (var result, string error) =await GithubGraphRequest
                .Query()
                .Define("repository", p => p.WithParameter("owner", owner).WithParameter("name", repo))
                .Child("label", p=>p.WithParameter("name", labelName),e=>e
                    .Child("id")).GraphResultAsync<GithubGraphReturn>();
            return (result?.Data?.Repository?.Label, error);
        }

        public async Task<(GithubLabel?, string)> UpdateAsync(string labelId, string labelName, string labelColor, string labelDescription = "")
        {
            //d4c5f9
            (var result, string error) = await GithubGraphRequest
                .Mutation()
                .Define("updateLabel", p => p
                    .WithParameter("name", labelName)
                    .WithParameter("color", labelColor)
                    .WithParameter("id ", labelId)
                    .WithParameter("description", labelDescription)
                    )
                .Child("label", e => e
                    .Child("id")).GraphResultAsync<GithubGraphReturn>();
            return (result?.Data?.UpdateLabel?.Label, error);
        }

        public async Task<(GithubLabel?, string)> CreateAsync(string repoId, string labelName, string labelColor, string description = "")
        {
            //d4c5f9
            (var result, string error) =await GithubGraphRequest
                .Mutation()
                .Define("createLabel", p => p
                    .WithParameter("name", labelName)
                    .WithParameter("color", labelColor)
                    .WithParameter("repositoryId",repoId)
                    .WithParameter("description", description)
                    )
                .Child("label", e => e
                    .Child("id")).GraphResultAsync<GithubGraphReturn>();
            return (result?.Data?.CreateLabel?.Label, error);
        }
    }
    public sealed class GithubRepositoryAPI
    {
        public async Task<(GithubRepository?, string)> GetAsync(string owner, string repo)
        {
            (var result, string error) =await GithubGraphRequest
                .Query()
                .Define("repository", p => p.WithParameter("owner", owner).WithParameter("name", repo))
                .Child("id", "isInOrganization")
                .Child("owner", e => e
                    .Child("id")).GraphResultAsync<GithubGraphReturn>();

            return (result?.Data?.Repository, error);
        }
    }
    public sealed class GithubProjectAPI
    {

        public async ValueTask<(bool, string)> ExistAsync(string ownerName, string repoName, string projectName, bool? isOpen = null)
        {
            var exist = false;
            string error = string.Empty;
            if (isOpen.HasValue)
            {
                if (isOpen.Value)
                {
                    (var result, error) =await GithubGraphRequest
                    .Query()
                    .Define("repository", p => p.WithParameter("owner", $"{ownerName}").WithParameter("name", $"{repoName}"))
                    .Child("projectsV2", p => p.WithParameter("first", 1).WithParameter("query", projectName), e => e
                        .Child("nodes", e => e
                            .Child("id", "title", "number"))).GraphResultAsync<GithubGraphReturn>();

                    var nodes = result?.Data?.Repository?.ProjectsV2?.Nodes;
                    if (nodes != null)
                    {
                        exist = nodes.Length > 0;
                    }

                }
                else
                {
                    (var result, error) =await GithubGraphRequest
                   .Query()
                   .Define("repository", p => p.WithParameter("owner", $"{ownerName}").WithParameter("name", $"{repoName}"))
                   .Child("projectsV2", p => p.WithParameter("first", 100).WithParameter("orderBy", "{direction:DESC,field:UPDATED_AT}", false), e => e
                       .Child("nodes", e => e
                           .Child("id", "title", "number"))).GraphResultAsync<GithubGraphReturn>();

                    var projects = result?.Data?.Repository?.ProjectsV2?.Nodes;
                    if (projects != null && projects.Length > 0)
                    {
                        exist = projects.Any(p => p.Title == projectName);
                    }
                }
            }
            else
            {

                (var result, error) =await GithubGraphRequest
                    .Query()
                    .Define("repository", p => p.WithParameter("owner", $"{ownerName}").WithParameter("name", $"{repoName}"))
                    .Child("projectsV2", p => p.WithParameter("first", 1).WithParameter("query", projectName), e => e
                        .Child("nodes", e => e
                            .Child("id", "title", "number"))).GraphResultAsync<GithubGraphReturn>();

                var projects = result?.Data?.Repository?.ProjectsV2?.Nodes;
                if (projects != null && projects.Length > 0)
                {
                    exist = projects.Any(p => p.Title == projectName);
                }
                if (!exist)
                {
                    (result, error) = await GithubGraphRequest
                    .Query()
                    .Define("repository", p => p.WithParameter("owner", $"{ownerName}").WithParameter("name", $"{repoName}"))
                    .Child("projectsV2", p => p.WithParameter("first", 100).WithParameter("orderBy", "{direction:DESC,field:UPDATED_AT}", false), e => e
                        .Child("nodes", e => e
                            .Child("id", "title", "number"))).GraphResultAsync<GithubGraphReturn>();

                    projects = result?.Data?.Repository?.ProjectsV2?.Nodes;
                    if (projects != null && projects.Length > 0)
                    {
                        exist = projects.Any(p => p.Title == projectName);
                    }
                }

            }
            return (exist, error);

        }
        public async Task<(GithubProject?, string)> GetAsync(string ownerName, string repoName, string projectName)
        {
            (var result, string error) =await GithubGraphRequest
                .Query()
                .Define("repository", p => p.WithParameter("owner", $"{ownerName}").WithParameter("name", $"{repoName}"))
                    .Child("projectsV2", p => p.WithParameter("first", 1).WithParameter("query", projectName), e => e
                            .Child("nodes", e => e
                                 .Child("id", "title", "number")
                                 .Child("field", p => p.WithParameter("name", "Status"), e => e
                                       .ChildWithStrongType("ProjectV2SingleSelectField", e => e
                                           .Child("id")
                                           .Child("options", e => e.Child("id", "name"))))
                                .Child("items", p => p.WithParameter("last", 100), e => e
                                         .Child("nodes", e => e
                                              .Child("id")
                                              .Child("content", e => e
                                                   .ChildWithStrongType("PullRequest", e => e
                                                       .Child("id"))))))).GraphResultAsync<GithubGraphReturn>();

            var projects = result?.Data?.Repository?.ProjectsV2;
            if (projects != null && projects.Nodes != null && projects.Nodes.Length > 0)
            {
                var project = projects.Nodes[0];
                if (project.Title == projectName)
                {
                    return (project, error);
                }
            }
            return (null, error);

        }
        public async Task<(GithubProject?, string)> CreateAsync(string ownerId, string repoId, string projectName)
        {
            (var result, string error) =await GithubGraphRequest
                .Mutation()
                .Define("createProjectV2", p => p
                     .WithParameter("ownerId", ownerId)
                     .WithParameter("repositoryId", repoId)
                     .WithParameter("title", projectName))
                .Child("projectV2", e => e.Child("id", "number"))
                .GraphResultAsync<GithubGraphReturn>();
            return (result?.Data?.CreateProjectV2?.ProjectV2, error);
        }
        public async ValueTask<(bool, string)> UpdateAsync(string projectId, bool? closed = null, bool? visiable = null, string? readme = null, string? shortDescription = null, string? title = null)
        {
            (var result, string error) =await GithubGraphRequest
                .Mutation()
                .Define("updateProjectV2", p =>
                {
                    p.WithParameter("projectId", projectId);
                    if (closed != null)
                    {
                        p.WithParameter("closed", closed.Value);
                    }
                    if (visiable != null)
                    {
                        p.WithParameter("public", visiable.Value);
                    }
                    if (title != null)
                    {
                        p.WithParameter("title", title);
                    }
                    if (shortDescription != null)
                    {
                        p.WithParameter("shortDescription", shortDescription);
                    }
                    if (readme != null)
                    {
                        p.WithParameter("readme", readme);
                    }
                })
                .Child("projectV2", e => e.Child("id", "number"))
                .GraphResultAsync<GithubGraphReturn>();
            var id = result?.Data?.UpdateProjectV2?.ProjectV2?.Id;
            return (id != null && id != string.Empty,error);
        }

        public async ValueTask<(bool, string)> UpdateItemStatusAsync(string projectId, string itemId, string fieldId, ProjectV2SingleSelectOptions value)
        {
            (var result, string error) =await GithubGraphRequest
               .Mutation()
               .Define("updateProjectV2ItemFieldValue", p => p
                   .WithParameter("projectId", projectId)
                   .WithParameter("itemId", itemId)
                   .WithParameter("fieldId", fieldId)
                   .WithParameter("value", $"{{singleSelectOptionId: \\\"{value.Id}\\\"}}", false)
                )
               .Child("projectV2Item", e => e
                   .Child("id")
                   .Child("fieldValueByName", p => p.WithParameter("name", "Status"), e => e
                    .ChildWithStrongType("ProjectV2ItemFieldSingleSelectValue", e=>e
                        .Child("id", "name"))
                )).GraphResultAsync<GithubGraphReturn>();
            var id = result?.Data?.UpdateProjectV2ItemFieldValue?.ProjectV2Item?.FieldValueByName?.Name;
            return (id!=null && id == value.Name, error);
        }

        public async Task<(GithubProjectItem?, string)> AddItemAsync(string projectId, string contentId)
        {

            (var result, string error) =await GithubGraphRequest
               .Mutation()
               .Define("addProjectV2ItemById", p => p
                   .WithParameter("projectId", projectId)
                   .WithParameter("contentId", contentId)
                )
               .Child("item", e => e
                   .Child("id")
                ).GraphResultAsync<GithubGraphReturn>();

            return (result?.Data?.AddProjectV2ItemById?.Item, error);

        }
    }
}
