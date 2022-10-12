using PluginBase;

namespace PluginA
{
    public class PluginA : IPluginBase
    {
        public void ShowVersion()
        {
            Console.WriteLine("PluginA :" + typeof(Dapper.DefaultTypeMap).Assembly.GetName().Version);
        }
    }
}
