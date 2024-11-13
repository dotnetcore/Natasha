using System;
using System.IO;

namespace NatashaFunctionUT.Domain.Plugin
{

    public class PluginPrepare : DomainPrepare
    {
        protected readonly string _basePath1;
        protected readonly string _basePath2;
        public PluginPrepare()
        {

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
