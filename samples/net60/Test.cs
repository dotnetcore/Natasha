using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net60
{
    internal class Test<T> where T : Test2?
    {

    }

    public class Test2
    {
        public void Show(string? name, string? bye, string? bye1, string bye2)
        {
            //return string.Empty;
        }

        public string? Show2(string name, string? bye, string? bye1, string byt2)
        {
            return string.Empty;
        }
        public string Show3(string? name, string bye, string bye1, string? byt2)
        {
            return string.Empty;
        }
        public void Show4(string? name)
        {
            //return string.Empty;
        }

        public void Show5(string? name,string bye, string? byte1)
        {
            //return string.Empty;
        }
    }
}
