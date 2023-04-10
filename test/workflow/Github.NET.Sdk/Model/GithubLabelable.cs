using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Github.NET.Sdk.Model
{
    public sealed class GithubLabelable
    {
        public string __typename { get; set; } = string.Empty;
    }
    public sealed class WrapperGithubLabelable
    {
        public GithubLabelable? Labelable { get; set; }
    }
}
