using Github.NET.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Github.NMSAcion.NET.Sdk.Model
{
    public sealed class ProjectGlobalConfiguration<T> where T : CSProjectConfigurationBase, new()
    {
        public HashSet<string>? FoldedProjects { get; set; }
        public GithubLabelBase[]? GlobalLabels { get; set; }
        public List<T>? Projects { get; set; }
    }
}
