using System;
using FlubuCore.Context;
using FlubuCore.Scripting;

namespace Build
{
    public class BuildScript : DefaultBuildScript
    {
        [FromArg("apiKey", "Nuget api key for publishing nuget package.")]
        public string NugetApiKey { get; set; }

        protected override void ConfigureBuildProperties(IBuildPropertiesContext context)
        {
            context.Properties.Set(BuildProps.ProductId, "Natasha");
            context.Properties.Set(BuildProps.SolutionFileName, "Natasha.sln");
            context.Properties.Set(BuildProps.BuildConfiguration, "Release");
        }

        protected override void ConfigureTargets(ITaskContext context)
        {
            var buildVersion = context.CreateTarget("build.version")
                .SetAsHidden()
                .SetDescription("Fetches build version from file.")
                .AddTask(x => x.FetchBuildVersionFromFileTask());

            var build = context.CreateTarget("Build")
                .SetDescription("Build's the solution.")
                .AddCoreTask(x => x.Restore())
                .AddCoreTask(x => x.UpdateNetCoreVersionTask("src/Natasha/Natasha.csproj"))
                .AddCoreTask(x => x.Build())
                .DependsOn(buildVersion);

            var runTests = context.CreateTarget("Run.Tests")
                .SetDescription("Run's all Natasha's tests.")
                .AddCoreTask(x => x.Test().Project("test/NatashaUT/NatashaUT.csproj").NoBuild())
                .AddCoreTask(x => x.Test().Project("test/NatashaBenchmark/NatashaBenchmark.csproj").NoBuild());

           var nugetPublish = context.CreateTarget("Nuget.Publish")
                .SetDescription("Packs and publishes nuget package.")
                .DependsOn(buildVersion)
                .AddCoreTask(x => x.Pack()
                    .Project("src/Natasha")
                    .IncludeSymbols()
                    .OutputDirectory("output"))
                .Do(PublishNuGetPackage);

           var rebuild = context.CreateTarget("Rebuild")
                .SetAsDefault()
                .DependsOn(buildVersion, build, runTests);

            context.CreateTarget("Rebuild.Server")
                .SetAsDefault()
                .DependsOn(rebuild)
                .DependsOn(nugetPublish);
        }

        private void PublishNuGetPackage(ITaskContext context)
        {
            var version = context.Properties.GetBuildVersion();
            var nugetVersion = version.ToString(3);

            context.CoreTasks().NugetPush($"output/DotNetCore.Natasha.{nugetVersion}.nupkg")
                .DoNotFailOnError((e) => { context.LogInfo($"Failed to publish Nuget package."); })
                .WithArguments("-s", "https://www.nuget.org/api/v2/package")
                .ApiKey(NugetApiKey)
                .Execute(context);
        }
    }
}
