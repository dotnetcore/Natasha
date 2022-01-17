using PluginBase;

namespace PluginB
{
    public class PluginB : IPluginBase
    {
        public void ShowVersion()
        {
            Console.WriteLine("PluginB :" + typeof(Dapper.DefaultTypeMap).Assembly.GetName().Version);
        }
    }
}
