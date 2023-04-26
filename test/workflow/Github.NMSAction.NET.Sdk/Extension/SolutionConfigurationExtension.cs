using Github.NET.Sdk;
using Github.NET.Sdk.Model;
using Github.NMSAcion.NET.Sdk.Model;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


public static class SolutionConfigurationExtension
{
    public static SolutionConfiguration? LoadFromYml()
    {
        if (File.Exists(SolutionRecorder.ConfigFilePath))
        {
            var content = File.ReadAllText(SolutionRecorder.ConfigFilePath);
            if (!string.IsNullOrEmpty(content))
            {
                var deserializer = new DeserializerBuilder()
                                           .WithNamingConvention(UnderscoredNamingConvention.Instance)  // see height_in_inches in sample yml 
                                           .Build();

                //yml contains a string containing your YAML
                return deserializer.Deserialize<SolutionConfiguration>(content);
            }
        }
        return null;
    }
    public static SolutionConfiguration LoadAndCreate()
    {
        var solutionInfo = new SolutionConfiguration();
        solutionInfo.Src = new();
        solutionInfo.Test = new();
        solutionInfo.Action = new();
        solutionInfo.Samples = new();
        solutionInfo.Workflow = new();

        HashSet<string> ignoreProjects;
        var oldSolution = LoadFromYml();
        if (oldSolution != null)
        {

            oldSolution.ReBuildIgnoreProjects();
            ignoreProjects = new HashSet<string>(oldSolution.IgnoreProjects);
            solutionInfo.IgnoreProjects = oldSolution.IgnoreProjects;
            oldSolution.ReBuildFoldProjects();
            CombineFoldedProjects(solutionInfo.Src, oldSolution.Src, ignoreProjects);
            CombineFoldedProjects(solutionInfo.Test, oldSolution.Test, ignoreProjects);
            CombineFoldedProjects(solutionInfo.Workflow, oldSolution.Workflow, ignoreProjects);
            CombineFoldedProjects(solutionInfo.Samples, oldSolution.Samples, ignoreProjects);
        }
        else
        {
            ignoreProjects = new();
        }

        solutionInfo.Action.Projects = new List<ActionProjectConfiguration>()
            {
                new ActionProjectConfiguration()
                {
                     ProjectFolder = ".github",
                     Labels = new GithubLabelBase[] { new GithubLabelBase() { Name="pr_action", Color= "68E0F8", Description="The pr will be marked with pr_action label." } },
                     DependencyConfig = new DependencyConfiguration()
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
        solutionInfo.Src.Projects = GetProjects<SrcProjectConfiguration>("src", ignoreProjects);
        solutionInfo.Test.Projects = GetProjects<TestProjectConfiguration>("test\\ut", ignoreProjects, 3);
        solutionInfo.Samples.Projects = GetProjects<SamplesProjectConfiguration>("samples", ignoreProjects);
        solutionInfo.Workflow.Projects = GetProjects<WorkflowProjectConfiguration>("test\\workflow", ignoreProjects);

        var files = Directory.GetFiles(SolutionRecorder.NMSTemplateRoot, "*.issue.template");
        if (files.Length > 0)
        {
            solutionInfo.IssuesTemplateConfigs = new IssueTemplateConfiguration[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                var fileName = Path.GetFileNameWithoutExtension(files[i]);
                fileName = fileName.Substring(0, fileName.Length - 6);
                solutionInfo.IssuesTemplateConfigs[i] = new IssueTemplateConfiguration() { FileName = fileName };
            }
        }
        if (oldSolution != null)
        {
            UpdateFrom(solutionInfo, oldSolution);
        }
        return solutionInfo;

        static void CombineFoldedProjects<T>(ProjectGlobalConfiguration<T> current, ProjectGlobalConfiguration<T> old, HashSet<string> totalFolders) where T : CSProjectConfigurationBase, new()
        {
            if (old.FoldedProjects != null)
            {
                totalFolders.UnionWith(old.FoldedProjects);
                current.FoldedProjects = old.FoldedProjects;
            }
        }
    }
    public static List<T> GetProjects<T>(string folder, HashSet<string> unExpectProjects, int deepth = 0) where T : CSProjectConfigurationBase, new()
    {
        List<T> nmsProjects = new();
        var collection = SolutionInfo.GetCSProjectsByStartFolder(folder);
        var projectNodes = collection
            .Projects
            .Where(project =>
            {
                var unixPath = project.RelativePath.Replace("\\", "/");
                var shut = !unExpectProjects.Contains(unixPath);
                if (deepth!=0)
                {
                    shut = shut && unixPath.Split('/').Length == deepth + 1;
                }
                return shut;
            }

            )
            .Select(project =>
            {
                return new T()
                {
                    RelativePath = project.RelativePath.Replace("\\", "/"),
                    ProjectName = project.ProjectName,
                    PackageName = project.PackageName == string.Empty ? project.ProjectName : project.PackageName,
                    Id = project.Id,
                    ProjectFolder = project.RelativeFolder
                };
            });

        nmsProjects.AddRange(projectNodes);
        return nmsProjects;
    }
    public static void UpdateFrom(this SolutionConfiguration current, SolutionConfiguration old)
    {
        if (current.IssuesTemplateConfigs != null && old.IssuesTemplateConfigs != null)
        {
            var issuesTemplateMapper = old.IssuesTemplateConfigs.ToDictionary(item => item.FileName, item => item);
            for (int i = 0; i < current.IssuesTemplateConfigs.Length; i += 1)
            {
                var key = current.IssuesTemplateConfigs[i].FileName;
                if (issuesTemplateMapper.ContainsKey(key))
                {
                    current.IssuesTemplateConfigs[i] = issuesTemplateMapper[key];
                }
            }
        }

        current.Action = old.Action;
        current.Src = PickConfigNode(current.Src, old.Src);
        current.Test = PickConfigNode(current.Test, old.Test);
        current.Samples = PickConfigNode(current.Samples, old.Samples);
        current.Workflow = PickConfigNode(current.Workflow, old.Workflow);
        static ProjectGlobalConfiguration<T> PickConfigNode<T>(ProjectGlobalConfiguration<T> current, ProjectGlobalConfiguration<T> old) where T : CSProjectConfigurationBase, new()
        {
            current.GlobalLabels = old.GlobalLabels;
            PickOldNode(current.Projects, old.Projects);
            return current;

            static void PickOldNode(List<T>? current, List<T>? old)
            {
                if (current != null && old != null)
                {
                    Dictionary<string, T> oldMapper = old.ToDictionary(item => item.Id, item => item);
                    for (int i = 0; i < current.Count; i += 1)
                    {
                        var temp = current[i];
                        if (oldMapper.ContainsKey(temp.Id))
                        {
                            current[i] = oldMapper[temp.Id];
                        }
                    }
                }
            }
        }
    }
    public static IEnumerable<GithubLabelBase> GetAllLabels(this SolutionConfiguration current)
    {
        Dictionary<string, GithubLabelBase> cache = new();
        if (current.IssuesTemplateConfigs != null)
        {
            for (int i = 0; i < current.IssuesTemplateConfigs.Length; i += 1)
            {
                var labels = current.IssuesTemplateConfigs[i].PullRequestLabels;
                if (labels != null)
                {
                    foreach (var label in labels)
                    {
                        if (!cache.ContainsKey(label.Name))
                        {
                            cache[label.Name] = label;
                        }
                    }
                }
            }
        }
        FillLabelCache(current.Action);
        FillLabelCache(current.Src);
        FillLabelCache(current.Samples);
        FillLabelCache(current.Test);
        FillLabelCache(current.Workflow);
        return cache.Values;
        void FillLabelCache<T>(ProjectGlobalConfiguration<T> wrapper) where T : CSProjectConfigurationBase, new()
        {
            AddListToCache(wrapper.GlobalLabels);
            if (wrapper.Projects != null)
            {
                foreach (var item in wrapper.Projects)
                {
                    AddListToCache(item.Labels);
                    AddListToCache(item.DependencyConfig?.Labels);
                }
            }
        }
        void AddListToCache(IEnumerable<GithubLabelBase>? githubLabels)
        {
            if (githubLabels != null)
            {
                foreach (var item in githubLabels)
                {
                    if (!cache.ContainsKey(item.Name))
                    {
                        cache[item.Name] = item;
                    }
                }
            }

        }

    }
    public static Dictionary<string, IEnumerable<GithubLabelBase>> ToDictionary(this SolutionConfiguration solution)
    {
        Dictionary<string, IEnumerable<GithubLabelBase>> cache = new();
        FileMapperToDictionary(solution.Src);
        FileMapperToDictionary(solution.Test);
        FileMapperToDictionary(solution.Samples);
        FileMapperToDictionary(solution.Action);
        FileMapperToDictionary(solution.Workflow);
        return cache;

        void FileMapperToDictionary<T>(ProjectGlobalConfiguration<T> node) where T : CSProjectConfigurationBase, new()
        {

            if (node.FoldedProjects != null && node.GlobalLabels != null)
            {
                var list = new List<GithubLabelBase>(node.GlobalLabels);
                foreach (var item in node.FoldedProjects)
                {
                    cache[item] = list;
                }
            }
            if (node.Projects != null)
            {
                foreach (var item in node.Projects)
                {
                    var labelList = new List<GithubLabelBase>();
                    if (node.GlobalLabels != null)
                    {
                        labelList.AddRange(node.GlobalLabels);
                    }
                    if (item.Labels != null)
                    {
                        labelList.AddRange(item.Labels);
                    }
                    cache[item.ProjectFolder] = labelList;
                }
            }
        }
    }
    public static void ReBuildIgnoreProjects(this SolutionConfiguration solution)
    {

        solution.IgnoreProjects ??= new HashSet<string>();
        ScannIgnoreProjects(solution.Src);
        ScannIgnoreProjects(solution.Test);
        ScannIgnoreProjects(solution.Samples);
        ScannIgnoreProjects(solution.Workflow);

        void ScannIgnoreProjects<T>(ProjectGlobalConfiguration<T> node) where T : CSProjectConfigurationBase, new()
        {
            if (node.Projects != null)
            {
                solution.IgnoreProjects!.UnionWith(node.Projects.Where(item => item.IsIgnored == true).Select(item => item.RelativePath));
            }
        }
    }
    public static void ReBuildFoldProjects(this SolutionConfiguration solution)
    {
        ScannFoldedProjects(solution.Src);
        ScannFoldedProjects(solution.Test);
        ScannFoldedProjects(solution.Samples);
        ScannFoldedProjects(solution.Workflow);

        static void ScannFoldedProjects<T>(ProjectGlobalConfiguration<T> node) where T : CSProjectConfigurationBase, new()
        {

            node.FoldedProjects ??= new();
            if (node.Projects != null)
            {
                node.FoldedProjects!.UnionWith(node.Projects.Where(item => item.IsFolded == true).Select(item => item.RelativePath));
            }
        }
    }

}
