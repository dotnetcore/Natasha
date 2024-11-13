using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Github.NET.Sdk.Request
{
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

        public async Task<(bool, string)> AddLabelAsync(string itemId, params string[] labelIds)
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
}
