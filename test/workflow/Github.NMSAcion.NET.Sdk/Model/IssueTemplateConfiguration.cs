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
        public string TemplatePanelName { get; set; } = string.Empty;
        public string TemplateFileName { get; set; } = string.Empty;
        public string? TemplatePanelDescription { get; set; }
        public string? TemplatePRPrefix { get; set; }
        public GithubLabelBase[]? TemplatePRLabels { get; set; }
    }
}
