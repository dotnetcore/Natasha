using Newtonsoft.Json;
using PluginBase;

namespace Plugin1
{
    
    public class P1 : IPluginBase
    {
        public string PluginMethod1()
        {
            return $"Json:{typeof(JsonConvert).Assembly.GetName().Version};Dapper:{typeof(Dapper.SqlMapper).Assembly.GetName().Version};IPluginBase:{new PluginModel().PluginVersion};Self:{this.GetType().Assembly.GetName().Version}";
        }

        public string PluginMethod2(PluginModel pluginModel)
        {
            pluginModel = new PluginModel();
            return pluginModel.PluginName + pluginModel.PluginVersion;
        }
    }
}
