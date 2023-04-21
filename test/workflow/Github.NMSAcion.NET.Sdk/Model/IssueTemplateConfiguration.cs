using Github.NET.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Github.NMSAcion.NET.Sdk.Model
{
    public sealed class IssueTemplateConfiguration
    {
        public string PanelName { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string? PanelDescription { get; set; }
        public string? PullRequestPrefix { get; set; }
        public GithubLabelBase[]? PullRequestLabels { get; set; }
    }
}
