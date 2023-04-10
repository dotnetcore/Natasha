using System.Text.RegularExpressions;

namespace Workflow.Label
{
    public static class PRLabelerHelper
    {
        public static readonly string CurrentProjectRoot;
        public static readonly string LaberYMLFile;
        private static readonly Regex _labelPicker;
        static PRLabelerHelper()
        {
            //labels: ["feedback"]
            CurrentProjectRoot = GetProjectRoot();
            LaberYMLFile = Path.Combine(CurrentProjectRoot, ".github", "labeler.yml");
            _labelPicker = new Regex("(?<result>.*?):\\s*", RegexOptions.Compiled);
        }

        public static HashSet<string>? ScanLabelTempaltes()
        {
            if (!File.Exists(LaberYMLFile))
            {
                return null;
            }
            var result = new HashSet<string>();
            var content = File.ReadAllText(LaberYMLFile);
            var matches = _labelPicker.Matches(content);
            if (matches != null && matches.Count > 0)
            {
                for (int j = 0; j < matches.Count; j++)
                {
                    var groups = matches[j].Groups["result"].Captures;
                    for (int z = 0; z < groups.Count; z++)
                    {
                        result.Add(groups[z].Value.ToLowerInvariant());
                    }
                }
            }
            return result;
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
