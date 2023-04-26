using Github.NET.Sdk;
using NuGet.Versioning;
using Publish.Helper;
using Solution.NET.Sdk.Model;
using System.Text;
using System.Text.RegularExpressions;

namespace Workflow.Nuget.Publish
{
    [Trait("管道功能", "NUGET")]
    public class PublishTest
    {
        [Fact(DisplayName = "打包检测")]
        public async Task Pass()
        {

            Regex versionReg = new Regex("(?<version>\\d+.\\d+.\\d+).*?");

            int pushCount = 0;
            bool isWriteEnv = true;

            var message = new StringBuilder();
            message.AppendLine();

            var (version, log) = ChangeLogHelper.GetReleaseInfoFromFromFile(SolutionInfo.ChangeLogFile);
            if (version != string.Empty && isWriteEnv)
            {
                var releaseInfo = ChangeLogHelper.GetReleasePackageInfo(log);
                if (releaseInfo == null)
                {
                    message.AppendLine("未获取到发布版本信息!");
                }
                else
                {

                    message.AppendLine($"CHANGELOG.md 扫描信息:");
                    var releasesDict = releaseInfo.ToDictionary(item => item.name, item => item.version);
                    foreach (var item in releasesDict)
                    {
                        message.AppendLine($"\t包名称: {item.Key} ,准备发布版本:{(string.IsNullOrEmpty(item.Value) ? "{空}" : item.Value)};");
                    }
                    message.AppendLine();
                    var projectCollection = SolutionInfo.GetCSProjectsByStartFolder("src");

                    if (projectCollection != null)
                    {
                        if (projectCollection.Projects.Count > 0)
                        {
                            var projects = projectCollection.Projects.Where(
                                item =>
                                {

                                    if (releasesDict.ContainsKey(item.PackageName))
                                    {
                                        item.PackageVersion = releasesDict[item.PackageName];
                                        ReWriteCsprojVersion(item);
                                        return true;
                                    }
                                    return false;
                                });

                            foreach (var project in projects)
                            {

                                var packageAble = project.IsPackable;
                                var packageName = project.PackageName;
                                var packageVersion = project.PackageVersion;

                                if (packageAble == true && packageName != null)
                                {
                                    if (!string.IsNullOrEmpty(packageVersion))
                                    {
                                        var latestVersion = await NugetHelper.GetLatestVersionAsync(packageName);
                                        if (latestVersion == null || NuGetVersion.Parse(packageVersion) > latestVersion)
                                        {
                                            var unixFilePath = project.RelativePath.Replace("\\", "/");
                                            //打包并检测该工程能否正常输出 NUGET 包
                                            var result = await NugetHelper.BuildAsync(unixFilePath) && await NugetHelper.PackAsync(unixFilePath);
                                            if (result)
                                            {
                                                pushCount += 1;
                                                Assert.True(result);
                                            }

                                        }
                                        else
                                        {
                                            message.AppendLine($"C#工程:包 {packageName} 的准发布版本 {packageVersion} 并不高于 NUGET 仓库中的 {latestVersion} 版本!");
                                        }
                                    }
                                    else
                                    {
                                        message.AppendLine($"CHANGELOG.md: 包 {packageName} 未包含版本信息!");
                                        message.AppendLine($"\t改正如:### {packageName} _ v1.0.0.0:");
                                    }
                                }
                                else
                                {
                                    if (packageName == null)
                                    {
                                        message.Append("C#工程:包 {空名} ");
                                    }
                                    else
                                    {
                                        message.Append($"C#工程:包 {packageName} ");
                                    }
                                    if (packageAble == true)
                                    {
                                        message.AppendLine("开启了打包功能.");
                                    }
                                    else
                                    {
                                        message.AppendLine("未开启打包功能(<IsPackable>true</IsPackable>).");
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        message.AppendLine($"未扫描到工程项目中存在正确的发包信息.");
                        message.AppendLine($"请检查 src/ 下的 csproj 文件以及 Directory.Build.props 文件是否正确填写包信息.");
                    }
                }
            }
            else
            {
                message.AppendLine("CHANGELOG.md: 未获取到 Release 发版信息!(## [x.x.x] - 2023-03-08)");
            }

            if (pushCount == 0)
            {
                Assert.Fail(message.ToString());
            }

            
            void ReWriteCsprojVersion(CSharpProject project)
            {
                string fileVersion = project.PackageVersion!;
                if (!Version.TryParse(project.PackageVersion, out _))
                {
                    var match = versionReg.Match(project.PackageVersion!);
                    {
                        fileVersion = match.Groups["version"].Value + ".0";
                    }
                }
                var csprojFilePath = Path.Combine(SolutionInfo.Root, project.RelativePath.Replace("\\", "/"));
                var content = File.ReadAllText(csprojFilePath);
                content = content.Replace("</PropertyGroup>", $"<Version>{project.PackageVersion!}</Version><FileVersion>{fileVersion}</FileVersion><AssemblyVersion>{fileVersion}</AssemblyVersion></PropertyGroup>");
                File.WriteAllText(csprojFilePath, content);
            }
        }
    }
}