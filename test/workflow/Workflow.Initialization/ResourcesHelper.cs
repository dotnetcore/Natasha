using Github.NET.Sdk;
using Github.NET.Sdk.Model;
using System.Text.RegularExpressions;

internal static class ResourcesHelper
{
    public static readonly string CurrentProjectRoot;
    public static readonly string CurrentNMSTemplateRoot;
    public static readonly string CurrentISSUETemplateRoot;
    public static readonly string CurrentUTTestRoot;
    public static readonly string CurrentSamplesRoot;
    public static readonly string CurrentWorkflowRoot;
    public static readonly string CurrentSrcRoot;
    public static readonly string GlobalConfigFile;
    public static readonly string NMSConfigFilePath;
    public static readonly string IssueConfigFilePath;
    public static readonly string IssueTemplateConfigFilePath;
    public static readonly string GitConfigFilePath;
    public static readonly string UTTestConfigFilePath;
    public static readonly string UTTestTemplateFilePath;
    public static readonly string DependencyConfigFilePath;
    public static readonly string DependencyTemplateFilePath;
    public static readonly string CodecovConfigFilePath;
    public static readonly string CodecovTemplateFilePath;

    static ResourcesHelper()
    {
        CurrentProjectRoot = GetProjectRoot();
        NMSConfigFilePath = SolutionHelper.ConfigFilePath;

        CurrentSrcRoot = Path.Combine(CurrentProjectRoot, "src");
        CurrentUTTestRoot = Path.Combine(CurrentProjectRoot, "test", "ut");
        CurrentWorkflowRoot = Path.Combine(CurrentProjectRoot, "test", "workflow");
        CurrentSamplesRoot = Path.Combine(CurrentProjectRoot, "samples");
        CurrentNMSTemplateRoot = Path.Combine(CurrentProjectRoot, ".github", "NMS_TEMPLATE");
        CurrentISSUETemplateRoot = Path.Combine(CurrentProjectRoot, ".github", "ISSUE_TEMPLATE");
        GlobalConfigFile = Path.Combine(CurrentProjectRoot, "Directory.Build.props");


        UTTestConfigFilePath = Path.Combine(CurrentProjectRoot, ".github", "workflows", "pr_test.yml");
        CodecovConfigFilePath = Path.Combine(CurrentProjectRoot, ".github", "workflows", "codecov.yml");
        DependencyConfigFilePath = Path.Combine(CurrentProjectRoot, ".github", "dependabot.yml");
        IssueConfigFilePath = Path.Combine(CurrentISSUETemplateRoot, "config.yml");

        GitConfigFilePath = Path.Combine(CurrentProjectRoot, ".git", "config");

        IssueTemplateConfigFilePath = Path.Combine(CurrentNMSTemplateRoot, "config.yml.template");
        UTTestTemplateFilePath = Path.Combine(CurrentNMSTemplateRoot, "test.yml.template");
        DependencyTemplateFilePath = Path.Combine(CurrentNMSTemplateRoot, "dependency.yml.template");
        CodecovTemplateFilePath = Path.Combine(CurrentNMSTemplateRoot, "codecov.yml.template");
    }

    public static (string owner, string repo) GetCurrentRepository()
    {
        /*
         * [remote "origin"]
	        url = https://github.com/night-moon-studio/Template.git
         */
        Regex reg = new(".*http.*/(?<owner>.*?)/(?<repo>.*?)\\.git", RegexOptions.Singleline);
        var content = File.ReadAllText(GitConfigFilePath);
        var match = reg.Match(content);
        if (match.Success)
        {
            var owner = match.Groups["owner"].Value;
            var repo = match.Groups["repo"].Value;
            return (owner, repo);
        }
        return (string.Empty, string.Empty);
    }


    public static NMSSolutionInfo GetSolutionInfo()
    {
        HashSet<string> ignoreProjects;
        var oldSolution = SolutionHelper.GetSolutionInfo();
        if (oldSolution!=null)
        {
            oldSolution.ReBuildIgnoreProjects();
            oldSolution.ReBuildFoldProjects();
            ignoreProjects = new HashSet<string>(oldSolution.IgnoreProjects!);
        }
        else
        {
            ignoreProjects = new();
        }

        var solutionInfo = new NMSSolutionInfo();
        var files = Directory.GetFiles(CurrentNMSTemplateRoot,"*.issue.template");
        if (files.Length > 0)
        {
            solutionInfo.IssuesTemplateConfigs = new NMSIssueTemplateConfig[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                var fileName = Path.GetFileNameWithoutExtension(files[i]);
                fileName = fileName.Substring(0, fileName.Length - 6);
                solutionInfo.IssuesTemplateConfigs[i] = new NMSIssueTemplateConfig() { TemplateFileName = fileName };
            }
        }

        solutionInfo.Src = new();
        solutionInfo.Test = new();
        solutionInfo.Action = new();
        solutionInfo.Samples = new();
        solutionInfo.Workflow = new();
        if (oldSolution != null)
        {
            if (oldSolution.IgnoreProjects != null)
            {
                solutionInfo.IgnoreProjects = oldSolution.IgnoreProjects;
            }
            if (oldSolution.Src != null && oldSolution.Src.FoldedProjects != null)
            {
                ignoreProjects.UnionWith(oldSolution.Src.FoldedProjects);
                solutionInfo.Src.FoldedProjects = oldSolution.Src.FoldedProjects;
            }
            if (oldSolution.Test != null && oldSolution.Test.FoldedProjects != null)
            {
                ignoreProjects.UnionWith(oldSolution.Test.FoldedProjects);
                solutionInfo.Test.FoldedProjects = oldSolution.Test.FoldedProjects;
            }
            if (oldSolution.Workflow != null && oldSolution.Workflow.FoldedProjects != null)
            {
                ignoreProjects.UnionWith(oldSolution.Workflow.FoldedProjects);
                solutionInfo.Workflow.FoldedProjects = oldSolution.Workflow.FoldedProjects;
            }
            if (oldSolution.Samples != null && oldSolution.Samples.FoldedProjects != null)
            {
                ignoreProjects.UnionWith(oldSolution.Samples.FoldedProjects);
                solutionInfo.Samples.FoldedProjects = oldSolution.Samples.FoldedProjects;
            }
        }

        solutionInfo.Action.Projects = new List<NMSActionProjectInfo>()
        {
            new NMSActionProjectInfo()
            {
                 ProjectFolder = ".github",
                 Labels = new GithubLabelBase[] { new GithubLabelBase() { Name="pr_action", Color= "68E0F8", Description="The pr will be marked with pr_action label." } },
                 DependencyConfig = new NMSDependencyConfig()
                 {
                      Type = PackageType.GithubAction,
                      Interval = PackageCheckInterval.EveryMonth,
                      SpecialTime = "05:00",
                      SpecialTimeZone = PackageTriggerTimeZone.HaErBing,
                      CommitPrefix ="[ACTION UPDATE]",
                       Labels= new GithubLabelBase[]
                       {
                           new GithubLabelBase()
                           {
                                Name = "dependencies",
                                Color = "68E0F8",
                                Description = "有依赖需要升级"
                           }
                       }
                 },
                 ProjectName = "Action"
            }
        };
        solutionInfo.Src.Projects = GetProjects<NMSSrcProjectInfo>(CurrentSrcRoot, ignoreProjects);
        solutionInfo.Test.Projects = GetProjects<NMSTestProjectInfo>(CurrentUTTestRoot, ignoreProjects, 1);
        solutionInfo.Samples.Projects = GetProjects<NMSSamplesProjectInfo>(CurrentSamplesRoot, ignoreProjects);
        solutionInfo.Workflow.Projects = GetProjects<NMSWorkflowProjectInfo>(CurrentWorkflowRoot, ignoreProjects);
        return solutionInfo;


    }


    public static List<T> GetProjects<T>(string folder, HashSet<string> unExpectProjects, int deepth = 0, bool searchAllDirectories = true) where T : NMSProjectInfo, new()
    {
        if (deepth != 0)
        {
            List<T> nmsProjects = new();
            var csprojFiles = Directory.GetFiles(folder, "*.csproj", SearchOption.TopDirectoryOnly);
            if (csprojFiles != null)
            {
                nmsProjects.AddRange(csprojFiles.Select(item =>
                {
                    var relativeFolder = Path.GetDirectoryName(item)!.Replace(CurrentProjectRoot, "").Replace("\\", "/");
                    if (relativeFolder.StartsWith("/"))
                    {
                        relativeFolder = relativeFolder[1..];
                    }
                    return new T()
                    {

                        ProjectName = Path.GetFileNameWithoutExtension(item),
                        ProjectFolder = relativeFolder

                    };
                }).Where(item => !unExpectProjects.Contains(item.ProjectFolder)));
            }
            var folders = Directory.GetDirectories(folder);
            foreach (var item in folders)
            {
                nmsProjects.AddRange(GetProjects<T>(item, unExpectProjects, deepth - 1));
            }
            return nmsProjects;
        }
        else
        {
            var csprojFiles = Directory.GetFiles(folder, "*.csproj", searchAllDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            if (csprojFiles != null)
            {
                return csprojFiles.Select(item =>
                {
                    var relativeFolder = Path.GetDirectoryName(item)!.Replace(CurrentProjectRoot, "").Replace("\\", "/");
                    if (relativeFolder.StartsWith("/"))
                    {
                        relativeFolder = relativeFolder[1..];
                    }
                    return new T()
                    {
                        ProjectName = Path.GetFileNameWithoutExtension(item),
                        ProjectFolder = relativeFolder,
                    };
                }).Where(item => !unExpectProjects.Contains(item.ProjectFolder)).ToList();
            }
            return new List<T>();
        }
    }

    private static string GetProjectRoot()
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

