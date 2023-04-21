using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.NET.Sdk.Model
{
    public sealed class CSharpProject
    {
        public string Id { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;

        public string? PackageName { get; set; }

        public bool IsPackable { get; set; }

        public string RelativePath { get; set; } = string.Empty;

        public string RelativeFolder { get; set; } = string.Empty;

        public HashSet<string> TargetFramworks { get; set; } = new HashSet<string>();

    }
}
