namespace AllProxySample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Run("PROXY");
        }
        public static void Run(string name)
        {
            NatashaDomain domain = new(name);
            string file = "G:\\Project\\OpenSource\\Jester\\src\\backend\\Jester.Api\\bin\\Debug\\net8.0\\Jester.Api.dll";
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
            Console.ReadKey();
        }
    }
}
