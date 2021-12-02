using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET50
{
    public class Test
    {
        public static string Name { get; set; }

        static Test()
        {
            Name = "old";
        }

        public void Show()
        {
            Console.WriteLine("old");
        }
    }
}
