using PluginBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatashaPluginSample
{
    public class MyIPTest : IPTest
    {
        public void Show()
        {
            Console.WriteLine("My IP");
        }
    }
}
