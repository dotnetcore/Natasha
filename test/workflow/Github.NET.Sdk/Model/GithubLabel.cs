using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Github.NET.Sdk.Model
{
    public sealed class GithubLabel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public GithubIssueConnections? Issues { get; set; }
    }

    public sealed class WrapperGithubLabel 
    {
        public GithubLabel? Label { get; set; }
    }

    public sealed class GithubLabelConnections
    {
        public GithubLabel[]? Nodes { get; set; }
    }

}
