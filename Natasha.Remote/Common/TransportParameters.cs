using System.Collections.Generic;

namespace Natasha.Remote
{
    /// <summary>
    /// 远程参数实体类
    /// </summary>
    public class TransportParameters
    {
        public string TypeName;
        public string MethodName;
        public Dictionary<string, string> Parameters;
        public TransportParameters()
        {
            Parameters = new Dictionary<string, string>();
        }
        public string this[string key]
        {
            get { return Parameters[key]; }
            set
            {
                Parameters[key] =value;
            }
        }
    }
}
