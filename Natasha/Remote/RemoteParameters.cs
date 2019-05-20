using System.Collections.Generic;

namespace Natasha.Remote
{
    public class RemoteParameters
    {
        public string TypeName;
        public string MethodName;
        public Dictionary<string, string> Parameters;
        public RemoteParameters()
        {
            Parameters = new Dictionary<string, string>();
        }
        public string this[string key]
        {
            get { return Parameters[key]; }
            set
            {
                Parameters[key] = value;
            }
        }
    }
}
