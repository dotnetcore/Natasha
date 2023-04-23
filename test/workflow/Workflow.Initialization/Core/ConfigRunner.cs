using Github.NET.Sdk;
using Github.NMSAcion.NET.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Workflow.Initialization.Core
{
    internal static class ConfigRunner
    {
        public static readonly string NMSTemplateRoot;
        public static readonly string IssueTemplateRoot;
        public static readonly string WorkflowRoot;
        public static readonly string ConfigFilePath;

        static ConfigRunner()
        {
            NMSTemplateRoot = Path.Combine(SolutionInfo.Root, ".github", "NMS_TEMPLATE");
            IssueTemplateRoot = Path.Combine(SolutionInfo.Root, ".github", "ISSUE_TEMPLATE");
            WorkflowRoot = Path.Combine(SolutionInfo.Root, ".github", "workflows");
            ConfigFilePath = Path.Combine(SolutionInfo.Root, ".github", "project.yml");
        }

        /// <summary>
        /// 配置ISSUE模板中 DISSCUSSION 部分
        /// </summary>
        public static void UpdateIssueTemplateConfig()
        {
            var issueConfigContent = File.ReadAllText(Path.Combine(NMSTemplateRoot, "config.yml.template"));
            (string owner, string repo) = ResourcesHelper.GetCurrentRepository();
            var content = issueConfigContent.Replace("${{owner_name/repo_name}}", $"{owner}/{repo}");
            File.WriteAllText(Path.Combine(NMSTemplateRoot, "config.yml"), content);
        }

        /// <summary>
        /// 更新 ISSUE 配置
        /// </summary>
        /// <param name="solutionInfo"></param>
        internal static void UpdateIssueTempalate(SolutionConfiguration solutionInfo)
        {
            if (solutionInfo.IssuesTemplateConfigs != null)
            {
                foreach (var item in solutionInfo.IssuesTemplateConfigs)
                {
                    var templatePath = Path.Combine(NMSTemplateRoot, $"{item.FileName}.issue.template");
                    if (File.Exists(templatePath))
                    {
                        var configPath = Path.Combine(IssueTemplateRoot, $"{item.FileName}.yml");
                        var content = File.ReadAllText(templatePath);
                        content = content.Replace("${{name}}", $"name:{item.PanelName}");
                        if (item.PanelDescription == null)
                        {
                            content = content.Replace("${{description}}", "");
                        }
                        else
                        {
                            content = content.Replace("${{description}}", $"description: {item.PanelDescription}");
                        }
                        if (item.PullRequestPrefix == null)
                        {
                            content = content.Replace("${{title}}", "");
                        }
                        else
                        {
                            content = content.Replace("${{title}}", $"title: \"{item.PullRequestPrefix}\"");
                        }
                        if (item.PullRequestLabels == null)
                        {
                            File.WriteAllText(configPath, content.Replace("${{labels}}", ""));
                        }
                        else
                        {
                            File.WriteAllText(configPath, content.Replace("${{labels}}", $"labels: [\"{string.Join("\",\"", item.PullRequestLabels.Select(item => item.Name))}\"]"));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 更新 UT测试 配置
        /// </summary>
        /// <param name="solutionInfo"></param>
        internal static void UpdateUnitTestYML(SolutionConfiguration solutionInfo)
        {
            var ymlFile = Path.Combine(WorkflowRoot, "pr_test.yml");
            if (solutionInfo.Test.Projects != null)
            {

                var templateFile = Path.Combine(NMSTemplateRoot, "test.yml.template");
                StringBuilder utString = new StringBuilder();
                foreach (var project in solutionInfo.Test.Projects)
                {
                    if (project.TriggerPullRequestTest)
                    {
                        utString.AppendLine(GetUTTestTaskString(project.ProjectName, project.ProjectFolder));
                    }
                }
                if (utString.Length > 0)
                {
                    var content = File.ReadAllText(templateFile);
                    File.WriteAllText(ymlFile, content.Replace("${{nms.tests}}", utString.ToString()));
                    return;
                }

            }
            if (File.Exists(ymlFile))
            {
                File.Delete(ymlFile);
            }

            static string GetUTTestTaskString(string projectName, string projectFolder)
            {
                return $"    - name: 🚦 {projectName} UT Test\r\n      run: dotnet test './{projectFolder}' --nologo -c Release";
            }
        }

        /// <summary>
        /// 更新 依赖检测 配置
        /// </summary>
        /// <param name="solutionInfo"></param>
        internal static void UpdateDependencyYML(SolutionConfiguration solutionInfo)
        {

            var ymlFile = Path.Combine(SolutionInfo.Root, ".github", "dependabot.yml");
            List<CSProjectConfigurationBase> projectInfos = new();
            FillList(solutionInfo.Src);
            FillList(solutionInfo.Test);
            FillList(solutionInfo.Samples);
            FillList(solutionInfo.Workflow);
            FillList(solutionInfo.Action);

            StringBuilder deString = new StringBuilder();
            foreach (var item in projectInfos)
            {
                if (item.DependencyConfig != null)
                {
                    deString.AppendLine(GetDependencyString(item.DependencyConfig, item.ProjectFolder));
                }
            }
            if (deString.Length > 0)
            {
                var content = File.ReadAllText(Path.Combine(NMSTemplateRoot, "dependency.yml.template"));
                File.WriteAllText(ymlFile, content.Replace("${{nms.denpendency}}", deString.ToString()));
            }
            else if (File.Exists(ymlFile))
            {
                File.Delete(ymlFile);
            }


            void FillList<T>(ProjectGlobalConfiguration<T> node) where T : CSProjectConfigurationBase, new()
            {
                if (node.Projects != null)
                {
                    projectInfos.AddRange(node.Projects);
                }
            }

            static string GetDependencyString(DependencyConfiguration? config, string directory)
            {
                if (config != null)
                {
                    StringBuilder result = new StringBuilder();
                    result.AppendLine($"  - package-ecosystem: \"{config.Type}\"");
                    result.AppendLine($"    directory: \"{directory}\"");
                    result.AppendLine($"    schedule:");
                    result.AppendLine($"      interval: \"{config.Interval}\"");
                    if (config.SpecialTime != null)
                    {
                        result.AppendLine($"      time: \"{config.SpecialTime}\"");
                    }
                    if (config.SpecialTimeZone != null)
                    {
                        result.AppendLine($"      timezone: \"{config.SpecialTimeZone}\"");
                    }
                    if (config.CommitPrefix != null)
                    {
                        result.AppendLine("    commit-message:");
                        result.AppendLine($"      prefix: \"{config.CommitPrefix}\"");
                    }
                    if (config.Labels != null)
                    {
                        result.AppendLine("    labels:");
                        foreach (var item in config.Labels)
                        {
                            result.AppendLine($"      - \"{item.Name}\"");
                        }
                    }
                    if (config.Ignore != null)
                    {
                        result.AppendLine("    ignore:");
                        foreach (var item in config.Ignore)
                        {
                            result.AppendLine($"      - dependency-name: \"{item.Name}\"");
                            if (item.Versions != null)
                            {
                                result.AppendLine($"        versions: [\"{string.Join("\\\",\\\"", item.Versions)}\"]");
                            }
                            if (item.VersionsType != null)
                            {
                                result.AppendLine($"        update-types: [\"{string.Join("\\\",\\\"", item.VersionsType)}\"]");
                            }
                        }
                    }

                    return result.ToString();
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// 更新 覆盖率上传 配置
        /// </summary>
        /// <param name="solutionInfo"></param>
        internal static void UpdateCodecovYML(SolutionConfiguration solutionInfo)
        {
            /*
             
    - name: 🚦 Check & Pack Codecov
      run: dotnet test "./test/ut/UnitTestProject/UnitTestProject.csproj" --nologo -f net6.0 -c Release --collect:"XPlat Code Coverage" --results-directory "./coverage/"
            */

            var ymlFile = Path.Combine(WorkflowRoot, "codecov.yml");
            StringBuilder codecovResult = new StringBuilder();

            if (solutionInfo.Test.Projects != null)
            {
                foreach (var project in solutionInfo.Test.Projects)
                {
                    if (project.TriggerCodecov)
                    {
                        codecovResult.AppendLine($"    - name: 🚦 Check & Pack Codecov - {project.ProjectName}");
                        codecovResult.AppendLine($"      run: dotnet test \"./{project.ProjectFolder}/{project.ProjectName}.csproj\" --nologo -f net6.0 -c Release --collect:\"XPlat Code Coverage\" --results-directory \"./coverage/\"");
                    }
                }

                if (codecovResult.Length > 0)
                {
                    var content = File.ReadAllText(Path.Combine(NMSTemplateRoot, "codecov.yml.template"));
                    File.WriteAllText(ymlFile, content.Replace("${{nms.codecov}}", codecovResult.ToString()));
                    return;
                }
            }

            if (File.Exists(ymlFile))
            {
                File.Delete(ymlFile);
            }
        }


        /// <summary>
        /// 更新 using覆盖 配置
        /// </summary>
        /// <param name="solutionInfo"></param>
        internal static void GenUsings(SolutionConfiguration solutionInfo)
        {
            if (solutionInfo.Src.Projects == null)
            {
                return;
            }
            var collection = SolutionInfo.GetCSProjectsByStartFolder("src");
            var projectMapper = collection.Projects.ToDictionary(item => item.Id, item => item);
            Regex usingReg = new Regex("namespace (?<using>.*?)[;{].*?public[^\\r\\n]*?(class|struct|enum|interface|record).*?{", RegexOptions.Singleline | RegexOptions.Compiled);
            Regex buildReg = new Regex("<ItemGroup>[\\s]*?<None Include=\"Targets\\\\Project.Usings.targets\".*?</ItemGroup>", RegexOptions.Singleline | RegexOptions.Compiled);

            foreach (var project in solutionInfo.Src.Projects)
            {
                if (project.UsingOutput != null && project.UsingOutput.Enable)
                {

                    var folder = Path.Combine(SolutionInfo.Root, project.ProjectFolder);
                    var files = Directory.GetFiles(folder, "*.cs", SearchOption.AllDirectories);
                    if (files == null)
                    {
                        continue;
                    }


                    HashSet<string> publicUsings = new HashSet<string>();
                    foreach (var file in files)
                    {
                        var matches = usingReg.Matches(File.ReadAllText(file));
                        if (matches.Count > 0)
                        {
                            for (int i = 0; i < matches.Count; i++)
                            {
                                var match = matches[i];
                                if (match.Success)
                                {
                                    var captures = match.Groups["using"].Captures;
                                    for (int j = 0; j < captures.Count; j++)
                                    {
                                        publicUsings.Add(captures[j].Value.Trim());
                                    }
                                }
                            }
                        }
                    }


                    var tagetFolder = Path.Combine(folder, "Targets");
                    if (!Directory.Exists(tagetFolder))
                    {
                        Directory.CreateDirectory(tagetFolder);
                    }

                    StringBuilder usingBuilder = new StringBuilder();
                    StringBuilder projectBuilder = new StringBuilder();
                    var nodeInfo = projectMapper[project.Id];
                    string targetFilePath = Path.Combine(tagetFolder, "Project.Usings.targets");
                    var csProjFilePath = Path.Combine(SolutionInfo.Root, nodeInfo.RelativePath);
                   
                    if (publicUsings.Count == 0)
                    {
                        if (File.Exists(targetFilePath))
                        {
                            File.Delete(targetFilePath);
                        }
                        var delCsprojContent = File.ReadAllText(csProjFilePath);
                        var delCsprojMatch = buildReg.Match(delCsprojContent);
                        if (delCsprojMatch.Success)
                        {
                            var delContent = buildReg
                                .Replace(delCsprojContent, "")
                                .Replace("\r\n</Project>", "</Project>")
                                .Replace("\t</Project>", "</Project>");
                            File.WriteAllText(csProjFilePath, delContent);
                        }
                        
                        continue;
                    }
                    else
                    {
                        usingBuilder.AppendLine("<Project>");
                        usingBuilder.AppendLine("\t<ItemGroup Condition=\"'$(ImplicitUsings)' == 'enable' or '$(ImplicitUsings)' == 'true'\">");
                        if (project.UsingOutput.Ignores != null)
                        {
                            publicUsings.ExceptWith(project.UsingOutput.Ignores);
                        }
                        foreach (var item in publicUsings)
                        {
                            usingBuilder.AppendLine($"\t\t<Using Include=\"{item}\" />");
                        }
                        usingBuilder.AppendLine("\t</ItemGroup>");
                        usingBuilder.Append("</Project>");


                        projectBuilder.AppendLine($"<ItemGroup>");

                        foreach (var item in nodeInfo.TargetFramworks)
                        {
                            projectBuilder.AppendLine($"\t\t<None Include=\"Targets\\Project.Usings.targets\" Pack=\"true\" PackagePath=\"build\\{item}\\{nodeInfo.PackageName}.targets\" />");
                            projectBuilder.AppendLine($"\t\t<None Include=\"Targets\\Project.Usings.targets\" Pack=\"true\" PackagePath=\"buildTransitive\\{item}\\{nodeInfo.PackageName}.targets\" />");
                            projectBuilder.AppendLine($"\t\t<None Include=\"Targets\\Project.Usings.targets\" Pack=\"true\" PackagePath=\"buildMultiTargeting\\{item}\\{nodeInfo.PackageName}.targets\" />");
                        }
                        projectBuilder.Append($"\t</ItemGroup>");

                    }

                    File.WriteAllText(targetFilePath, usingBuilder.ToString());
                    var csprojContent = File.ReadAllText(csProjFilePath);
                    var csprojMatch = buildReg.Match(csprojContent);
                    var content = string.Empty;
                    if (csprojMatch.Success)
                    {
                        content = buildReg.Replace(csprojContent, projectBuilder.ToString());
                    }
                    else if (projectBuilder.Length > 0)
                    {
                        projectBuilder.Append("\r\n</Project>");
                        content = csprojContent.Replace("</Project>", "\t" + projectBuilder.ToString());
                    }
                    File.WriteAllText(csProjFilePath, content);


                }
            }
        }

        /// <summary>
        /// 更新 PackgeId
        /// </summary>
        /// <param name="solutionInfo"></param>
        internal static void GenPackageId(SolutionConfiguration solutionInfo)
        {
            if (solutionInfo.Src.Projects == null)
            {
                return;
            }
            var collection = SolutionInfo.GetCSProjectsByStartFolder("src");
            var projectMapper = collection.Projects.ToDictionary(item => item.Id, item => item);

            Regex buildReg = new Regex("<PackageId>.*?</PackageId>", RegexOptions.Singleline | RegexOptions.Compiled);
            foreach (var project in solutionInfo.Src.Projects)
            {

                var nodeInfo = projectMapper[project.Id];
                var file = Path.Combine(SolutionInfo.Root, nodeInfo.RelativePath);
                var csprojContent = File.ReadAllText(file);
                var match = buildReg.Match(csprojContent);
                var newPackageId = $"<PackageId>{project.PackageName}</PackageId>";
                var content = string.Empty;
                if (match.Success)
                {
                    content = buildReg.Replace(csprojContent, newPackageId);
                }
                else
                {
                    newPackageId += "\r\n\t</PropertyGroup>";
                    content = csprojContent.Replace("</PropertyGroup>", "\t" + newPackageId);
                }
                File.WriteAllText(file, content);

            }
        }
    }
}
