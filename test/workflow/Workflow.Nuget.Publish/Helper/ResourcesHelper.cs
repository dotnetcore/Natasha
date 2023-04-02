namespace Publish.Helper
{
    public static class ResourcesHelper
    {
        public static readonly string CurrentProjectRoot;
        public static readonly string CurrentSrcFolder;
        public static readonly string GlobalConfigFile;
        public static readonly string ChangeLogFile;

        static ResourcesHelper()
        {
            CurrentProjectRoot = GetProjectRoot();
            CurrentSrcFolder = Path.Combine(CurrentProjectRoot, "src");//, "*.csproj", SearchOption.AllDirectories);
            GlobalConfigFile = Path.Combine(CurrentProjectRoot, "Directory.Build.props");
            ChangeLogFile = Path.Combine(CurrentProjectRoot, "CHANGELOG.md");
        }


        public static string GetProjectRoot()
        {
            var currentFolder = AppDomain.CurrentDomain.BaseDirectory;
            while (!CheckExistGlobalBuilderFile(currentFolder))
            {
                var parentFolder = Directory.GetParent(currentFolder);
                if (parentFolder != null)
                {
                    currentFolder = parentFolder.FullName;
                }
                else
                {
                    return string.Empty;
                }
            }
            return currentFolder;
        }

        private static bool CheckExistGlobalBuilderFile(string folder)
        {
            return File.Exists(Path.Combine(folder, "Directory.Build.props")) && Directory.GetFiles(folder).Any(item => item.Contains(".sln"));
        }

    }
}
