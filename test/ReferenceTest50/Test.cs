using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceTest50
{
    public record Test
    {
        public string Name { get; set; }

        public Test Create2()
        {
            Name = "a";
            return this;
        }
        public Test Create()
        {
            return this with { Name = "a" };
        }
    }
}
