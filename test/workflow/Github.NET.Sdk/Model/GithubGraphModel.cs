using Github.NET.Sdk.Model;

namespace Github.NET.Sdk
{
    public sealed class GithubGraphReturn
    {
        public GithubData? Data { get; set; }
    }
    public sealed class GithubData
    {
        public GithubRepository? Repository { get; set; }
        public WrapperGithubRepository? DeleteIssue { get; set; }
        public WrapperProject? CreateProjectV2 { get; set; }
        public WrapperProject? UpdateProjectV2 { get; set; }
        public WrapperGithubProjectItem? UpdateProjectV2ItemFieldValue { get; set; }
        public WrapperGithubProjectItem? AddProjectV2ItemById { get; set; }
        public WrapperGithubLabel? CreateLabel { get; set; }
        public WrapperGithubLabel? UpdateLabel { get; set; }
        public WrapperGithubComment? AddComment { get; set; }
        public WrapperGithubLabelable? AddLabelsToLabelable { get; set; }
    }


}
