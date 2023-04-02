using System;

namespace PluginBase
{
    public interface IPluginBase
    {
        public string PluginMethod1();
    }


    public class PluginModel 
    { 
        public string PluginName { get; set; }

        public string PluginVersion { get; set; } = "1.0.0.0";

        public string PluginDescription { get; set; }

        public DateTime CreateTime { get; set; }
    }

}
