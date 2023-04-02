using System.Text.RegularExpressions;

namespace Workflow.Label
{
    public static class IssueTemplateHelper
    {
        public static readonly string CurrentProjectRoot;
        public static readonly string IssueTemplateFolder;
        private static readonly Regex _labelPicker;
        static IssueTemplateHelper()
        {
            //labels: ["feedback"]
            CurrentProjectRoot = GetProjectRoot();
            IssueTemplateFolder = Path.Combine(CurrentProjectRoot, ".github", "ISSUE_TEMPLATE");
            _labelPicker = new Regex(@"labels:\s*\[\s*?""((?<result>.+?)""\s*,\s*"")*((?<result>.+?)"")?\s*\]", RegexOptions.Compiled);
        }

        public static HashSet<string>? ScanLabelTempaltes()
        {
            if (!Directory.Exists(IssueTemplateFolder))
            {
                return null;
            }
            var result = new HashSet<string>();
            var files = Directory.GetFiles(IssueTemplateFolder);
            for (int i = 0; i < files.Length; i += 1)
            {
                if (Path.GetFileName(files[i]) != "config.yml")
                {
                    var content = File.ReadAllText(files[i]);
                    var matches = _labelPicker.Matches(content);
                    if (matches != null && matches.Count>0)
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
            return File.Exists(Path.Combine(folder, "Directory.Build.props")) && Directory.GetFiles(folder).Any(item=>item.Contains(".sln"));
        }

    }
}
