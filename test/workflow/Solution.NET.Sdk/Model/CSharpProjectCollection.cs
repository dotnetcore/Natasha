using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.NET.Sdk.Model
{
    public sealed class CSharpProjectCollection
    {
        public CSharpProject? GlobalConfig { get; set; }

        public List<CSharpProject> Projects { get; set; } = new List<CSharpProject>();

    }
}
