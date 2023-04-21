using Github.NET.Sdk.Model;

namespace Github.NMSAcion.NET.Sdk.Model
{
    public sealed class DependencyConfiguration
    {
        public string Type { get; set; } = PackageType.Nuget;
        public string Interval { get; set; } = PackageCheckInterval.EveryWorkDay;
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
