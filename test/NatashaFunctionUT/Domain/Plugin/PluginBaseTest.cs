using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NatashaFunctionUT.Domain.Plugin
{

    public class PluginBaseTest
    {
        protected readonly string _runtimeVersion;
        protected readonly string _basePath1;
        protected readonly string _basePath2;
        public PluginBaseTest()
        {
           
#if NETCOREAPP3_1
            _runtimeVersion = "netcoreapp3.1";
#elif NET5_0
             _runtimeVersion = "net5.0";
#elif NET6_0_OR_GREATER
            _runtimeVersion = "net6.0";
#endif
            _basePath1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Domain", "Plugin", this.GetType().Name.Replace("Test", ""), "1", _runtimeVersion);
            _basePath2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Domain", "Plugin", this.GetType().Name.Replace("Test", ""), "2", _runtimeVersion);

        }

        public string PathCombine1(string fileName)
        {
            return Path.Combine(_basePath1, fileName);
        }
        public string PathCombine2(string fileName)
        {
            return Path.Combine(_basePath2, fileName);
        }
    }
}
