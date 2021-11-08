using PluginBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatashaPluginSample
{
    public class MyAPTest : APTest
    {
        public override void Show()
        {
            Console.WriteLine("My AP");
        }
    }
}
