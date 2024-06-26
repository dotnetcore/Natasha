namespace AllProxySample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NatashaDomain domain = new NatashaDomain("proxy");
            string file = "G:\\Project\\OpenSource\\Natasha\\samples\\HESample\\bin\\Debug\\net8.0\\HESample.dll";
            string dependencyDirectory = Path.GetDirectoryName(file)!;
            string[] dependencyFiles = Directory.GetFiles(dependencyDirectory, "*.dll", SearchOption.AllDirectories);
            AppContext.SetData("CurrentHEExeFilePath", file);
            var assembly = domain.LoadPluginWithAllDependency(file);
            foreach (var item in dependencyFiles)
            {
               domain.LoadPluginWithAllDependency(item);
            }
            var ep = assembly.EntryPoint!;
            ep.Invoke(null, [Array.Empty<string>()]);
        }
    }
}
