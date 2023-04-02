using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Github.NET.Sdk.Model
{
    public sealed class GithubPageInfo
    {
        public string EndCursor { get; set; } = string.Empty;
        public bool HasNextPage { get; set; }
    }
}
