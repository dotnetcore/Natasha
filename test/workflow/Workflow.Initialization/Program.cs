using Github.NET.Sdk.Model;
using Microsoft.VisualBasic;
using System;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Workflow.Template.Initialization
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UpdateIssueTemplateConfig();
            var newSolutionInfo = GetAndUpdateInfo();
            UpdateIssueTempalate(newSolutionInfo);
            UpdateUnitTestYML(newSolutionInfo);
            UpdateDependencyYML(newSolutionInfo);
            UpdateCodecovYML(newSolutionInfo);
        }


        private static void UpdateIssueTemplateConfig()
        {
            var issueConfigContent = File.ReadAllText(ResourcesHelper.IssueTemplateConfigFilePath);
            (string owner, string repo) = ResourcesHelper.GetCurrentRepository();
            var content = issueConfigContent.Replace("${{owner_name/repo_name}}", $"{owner}/{repo}");
            File.WriteAllText(ResourcesHelper.IssueConfigFilePath, content);

        }

        private static NMSSolutionInfo GetAndUpdateInfo()
        {
            var solution = ResourcesHelper.GetSolutionInfo();
            if (File.Exists(ResourcesHelper.NMSConfigFilePath))
            {
                string content = File.ReadAllText(ResourcesHelper.NMSConfigFilePath);
                if (!string.IsNullOrEmpty(content))
                {
                    var deserializer = new DeserializerBuilder()
                                           .WithNamingConvention(UnderscoredNamingConvention.Instance)  // see height_in_inches in sample yml 
                                           .Build();

                    //yml contains a string containing your YAML
                    var oldSolution = deserializer.Deserialize<NMSSolutionInfo>(content);
                    solution.UpdateFrom(oldSolution);
                }

            }

            var serializer = new SerializerBuilder()
                                 .WithNamingConvention(UnderscoredNamingConvention.Instance)
                                 .Build();
            var yaml = serializer.Serialize(solution);
            File.WriteAllText(ResourcesHelper.NMSConfigFilePath, yaml);
            return solution;
        }


        private static void UpdateUnitTestYML(NMSSolutionInfo solutionInfo)
        {

            if (solutionInfo.Test != null && solutionInfo.Test.Projects != null)
            {
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
                    var content = File.ReadAllText(ResourcesHelper.UTTestTemplateFilePath);
                    File.WriteAllText(ResourcesHelper.UTTestConfigFilePath, content.Replace("${{nms.tests}}", utString.ToString()));
                    return;
                }

            }
            if (File.Exists(ResourcesHelper.UTTestConfigFilePath))
            {
                File.Delete(ResourcesHelper.UTTestConfigFilePath);
            }

            static string GetUTTestTaskString(string projectName, string projectFolder)
            {
                return $"    - name: 🚦 {projectName} UT Test\r\n      run: dotnet test './{projectFolder}' --nologo -c Release";
            }
        }


        private static void UpdateDependencyYML(NMSSolutionInfo solutionInfo)
        {
            List<NMSProjectInfo> projectInfos = new();
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
                var content = File.ReadAllText(ResourcesHelper.DependencyTemplateFilePath);
                File.WriteAllText(ResourcesHelper.DependencyConfigFilePath, content.Replace("${{nms.denpendency}}", deString.ToString()));
            }
            else if (File.Exists(ResourcesHelper.DependencyConfigFilePath))
            {
                File.Delete(ResourcesHelper.DependencyConfigFilePath);
            }



            void FillList<T>(WrapperNMSProjectInfo<T>? node) where T : NMSProjectInfo, new()
            {
                if (node != null && node.Projects != null)
                {
                    projectInfos.AddRange(node.Projects);
                }
            }

            static string GetDependencyString(NMSDependencyConfig? config, string directory)
            {
                if (config != null)
                {
                    StringBuilder result = new StringBuilder();
                    result.AppendLine($"  - package-ecosystem: \"{config.Type}\"");
                    result.AppendLine($"    directory: \"{directory}\"");
                    result.AppendLine($"    schedule:");
                    result.AppendLine($"      interval: \"{config.DependencyUpdateInterval}\"");
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
                            if (item.Versions!=null)
                            {
                                result.AppendLine($"        versions: [\"{string.Join("\\\",\\\"", item.Versions)}\"]");
                            }
                            if (item.VersionsType!=null)
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


        private static void UpdateCodecovYML(NMSSolutionInfo solutionInfo)
        {
            /*
             
    - name: 🚦 Check & Pack Codecov
      run: dotnet test "./test/ut/UnitTestProject/UnitTestProject.csproj" --nologo -f net6.0 -c Release --collect:"XPlat Code Coverage" --results-directory "./coverage/"
            */
            StringBuilder codecovResult = new StringBuilder();
            if (solutionInfo.Test!=null)
            {
                if (solutionInfo.Test.Projects!=null)
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
                        var content = File.ReadAllText(ResourcesHelper.CodecovTemplateFilePath);
                        File.WriteAllText(ResourcesHelper.CodecovConfigFilePath, content.Replace("${{nms.codecov}}", codecovResult.ToString()));
                        return;
                    }
                }

            }
            if (File.Exists(ResourcesHelper.CodecovConfigFilePath))
            {
                File.Delete(ResourcesHelper.CodecovConfigFilePath);
            }
        }


        private static void UpdateIssueTempalate(NMSSolutionInfo solutionInfo)
        {
            if (solutionInfo.IssuesTemplateLabelConfigs!=null)
            {
                foreach (var item in solutionInfo.IssuesTemplateLabelConfigs)
                {
                    var templatePath = Path.Combine(ResourcesHelper.CurrentNMSTemplateRoot, $"{item.TemplateFileName}.issue.template");
                    var configPath = Path.Combine(ResourcesHelper.CurrentISSUETemplateRoot, $"{item.TemplateFileName}.yml");
                    if (File.Exists(templatePath))
                    {
                        var content = File.ReadAllText(templatePath);
                        content = content.Replace("${{name}}", $"name:{item.TemplateName}");
                        if (item.TitlePrefix == null)
                        {
                            content = content.Replace("${{title}}", "");
                        }
                        else
                        {
                            content = content.Replace("${{title}}", $"title: \"{ item.TitlePrefix}\"");
                        }
                        if (item.Labels==null)
                        {
                            File.WriteAllText(configPath, content.Replace("${{labels}}", ""));
                        }
                        else
                        {
                            File.WriteAllText(configPath, content.Replace("${{labels}}", $"labels: [\"{string.Join("\",\"",item.Labels.Select(item=>item.Name))}\"]"));
                        }
                    }
                }
            }
        }
    }
}