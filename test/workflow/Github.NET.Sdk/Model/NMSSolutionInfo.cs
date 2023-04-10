namespace Github.NET.Sdk.Model
{

    public sealed class NMSSolutionInfo
    {
        public HashSet<string>? IgnoreProjects { get; set; }
        public NMSIssueTemplateLabelsConfig[]? IssuesTemplateLabelConfigs { get; set; }
        public WrapperNMSProjectInfo<NMSActionProjectInfo>? Action { get; set; }
        public WrapperNMSProjectInfo<NMSSamplesProjectInfo>? Samples { get; set; }
        public WrapperNMSProjectInfo<NMSSrcProjectInfo>? Src { get; set; }
        public WrapperNMSProjectInfo<NMSTestProjectInfo>? Test { get; set; }
        public WrapperNMSProjectInfo<NMSWorkflowProjectInfo>? Workflow { get; set; }
        public void UpdateFrom(NMSSolutionInfo oldInfo)
        {
            if (IssuesTemplateLabelConfigs != null)
            {
                if (oldInfo.IssuesTemplateLabelConfigs != null)
                {
                    var issuesTemplate = oldInfo.IssuesTemplateLabelConfigs.ToDictionary(item => item.TemplateFileName, item => item);
                    for (int i = 0; i < IssuesTemplateLabelConfigs.Length; i += 1)
                    {
                        var key = IssuesTemplateLabelConfigs[i].TemplateFileName;
                        if (issuesTemplate.ContainsKey(key))
                        {
                            IssuesTemplateLabelConfigs[i] = issuesTemplate[key];
                        }
                    }
                }
            }
            Action = oldInfo.Action;
            Src = PickConfigNode(Src, oldInfo.Src);
            Test = PickConfigNode(Test, oldInfo.Test);
            Samples = PickConfigNode(Samples, oldInfo.Samples);
            Workflow = PickConfigNode(Workflow, oldInfo.Workflow);
            static WrapperNMSProjectInfo<T>? PickConfigNode<T>(WrapperNMSProjectInfo<T>? current, WrapperNMSProjectInfo<T>? old) where T : NMSProjectInfo, new()
            {

                if (old != null)
                {
                    if (current == null)
                    {
                        current = new WrapperNMSProjectInfo<T>();
                    }
                    current.GlobalLabels = old.GlobalLabels;
                    PickOldNode(current.Projects, old.Projects);
                }
                return current;
                static void PickOldNode(List<T>? current, List<T>? old)
                {
                    if (current != null && old != null)
                    {
                        Dictionary<string, T> srcStore = old.ToDictionary(item => item.ProjectFolder, item => item);
                        for (int i = 0; i < current.Count; i += 1)
                        {
                            var temp = current[i];
                            if (srcStore.ContainsKey(temp.ProjectFolder))
                            {
                                current[i] = srcStore[temp.ProjectFolder];
                            }
                        }
                    }
                }
            }
        }
        public IEnumerable<GithubLabelBase> GetAllLabels()
        {
            Dictionary<string, GithubLabelBase> cache = new();
            if (IssuesTemplateLabelConfigs != null)
            {
                for (int i = 0; i < IssuesTemplateLabelConfigs.Length; i += 1)
                {
                    var labels = IssuesTemplateLabelConfigs[i].Labels;
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
            FillLabelCache(Action);
            FillLabelCache(Src);
            FillLabelCache(Samples);
            FillLabelCache(Test);
            FillLabelCache(Workflow);
            void FillLabelCache<T>(WrapperNMSProjectInfo<T>? wrapper) where T : NMSProjectInfo, new()
            {
                if (wrapper != null)
                {
                    AddListToCache(wrapper.GlobalLabels);
                    if (wrapper.Projects != null)
                    {
                        foreach (var item in wrapper.Projects)
                        {
                            AddListToCache(item.Labels);
                            if (item.DependencyConfig != null)
                            {
                                AddListToCache(item.DependencyConfig.Labels);
                            }
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
            return cache.Values;
        }
        public Dictionary<string, IEnumerable<GithubLabelBase>> ToDictionary()
        {
            Dictionary<string, IEnumerable<GithubLabelBase>> cache = new();
            FileMapperToDictionary(Src);
            FileMapperToDictionary(Test);
            FileMapperToDictionary(Samples);
            FileMapperToDictionary(Action);
            FileMapperToDictionary(Workflow);
            return cache;

            void FileMapperToDictionary<T>(WrapperNMSProjectInfo<T>? node) where T : NMSProjectInfo, new()
            {
                if (node != null)
                {
                    if (node.FoldedProjects!=null && node.GlobalLabels!=null)
                    {
                        var list = new List<GithubLabelBase>(node.GlobalLabels);
                        foreach (var item in node.FoldedProjects)
                        {
                            cache[item] = list;
                        }
                    }
                    if (node.Projects!=null)
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
        }


        public void ReBuildIgnoreProjects()
        {

            IgnoreProjects ??= new HashSet<string>();
            ScannIgnoreProjects(Src);
            ScannIgnoreProjects(Test);
            ScannIgnoreProjects(Samples);
            ScannIgnoreProjects(Workflow);

            void ScannIgnoreProjects<T>(WrapperNMSProjectInfo<T>? node) where T : NMSProjectInfo, new()
            {
                if (node != null && node.Projects != null)
                {
                    IgnoreProjects!.UnionWith(node.Projects.Where(item => item.IsIgnored == true).Select(item => item.ProjectFolder));
                }
            }
        }

        public void ReBuildFoldProjects()
        {
            ScannFoldedProjects(Src);
            ScannFoldedProjects(Test);
            ScannFoldedProjects(Samples);
            ScannFoldedProjects(Workflow);

            static void ScannFoldedProjects<T>(WrapperNMSProjectInfo<T>? node) where T : NMSProjectInfo, new()
            {
                if (node!=null)
                {
                    node.FoldedProjects ??= new();
                    if (node.Projects != null)
                    {
                        node.FoldedProjects!.UnionWith(node.Projects.Where(item => item.IsFolded == true).Select(item => item.ProjectFolder));
                    }
                }
            }
        }

    }

    public sealed class NMSIssueTemplateLabelsConfig
    {
        public string TemplateName { get; set; } = string.Empty;
        public string TemplateFileName { get; set; } = string.Empty;
        public string? TitlePrefix { get; set; }
        public GithubLabelBase[]? Labels { get; set; }
    }

    public sealed class WrapperNMSProjectInfo<T> where T : NMSProjectInfo, new()
    {
        public HashSet<string>? FoldedProjects { get; set; }
        public GithubLabelBase[]? GlobalLabels { get; set; }
        public List<T>? Projects { get; set; }
    }

    public sealed class NMSTestProjectInfo : NMSProjectInfo
    {
        public bool TriggerPullRequestTest { get; set; }
        public bool TriggerCodecov { get; set; }
    }

    public sealed class NMSWorkflowProjectInfo : NMSProjectInfo
    {

    }
    public sealed class NMSSamplesProjectInfo : NMSProjectInfo
    {

    }

    public sealed class NMSSrcProjectInfo : NMSProjectInfo
    {

    }
    public sealed class NMSActionProjectInfo : NMSProjectInfo
    {

    }

    public class NMSProjectInfo
    {
        public bool IsIgnored { get; set; }

        public bool IsFolded { get; set; }

        public string ProjectName { get; set; } = string.Empty;

        public string ProjectFolder { get; set; } = string.Empty;

        public GithubLabelBase[]? Labels { get; set; }

        public NMSDependencyConfig? DependencyConfig { get; set; }

    }

    public sealed class NMSDependencyConfig
    {
        public string Type { get; set; } = PackageType.Nuget;
        public string DependencyUpdateInterval { get; set; } = PackageCheckInterval.EveryWorkDay;
        public string? CommitPrefix { get; set; } = "[DEPENDENCY]";
        public string? SpecialTime { get; set; }
        public string? SpecialTimeZone { get; set; }
        public GithubLabelBase[]? Labels { get; set; }
        public NMSDependencyInfo[]? Ignore { get; set; }
    }

    public sealed class NMSDependencyInfo
    {
        public string Name { get; set; } = string.Empty;

        public string[]? Versions { get; set; }

        public string[]? VersionsType { get; set; }
    }

    public sealed class PackageType
    {
        public const string GithubAction = "github-actions";
        public const string Nuget = "nuget";
        public const string Pip = "pip";
        public const string Npm = "npm";
        public const string Composer = "composer";
        public const string Docker = "docker";
    }

    public sealed class PackageCheckInterval
    {
        public const string EveryWorkDay = "daily";
        public const string EveryWeek = "weekly";
        public const string EveryMonth = "monthly";

    }

    /// <summary>
    /// https://en.wikipedia.org/wiki/List_of_tz_database_time_zones
    /// </summary>
    public sealed class PackageTriggerTimeZone
    {
        public const string HaErBing = "Asia/Harbin";
        public const string ChongQing = "Asia/Chongqing";
        public const string XiangGang = "Asia/Hong_Kong";
        public const string Tokyo = "Asia/Tokyo";

    }


}
