namespace Solution.NET.Sdk.Model
{
    public sealed class PropertyGroup
    {
        public string? TargetFrameworks { get; set; }
        public string? TargetFramework { get; set; }
        public string? PackageId { get; set; }
        public bool? IsPackable { get; set; }
        public string? Version { get; set; }

        public void SupplementFrom(PropertyGroup globalNode)
        {
            TargetFramework ??= globalNode.TargetFramework;
            TargetFrameworks ??= globalNode.TargetFrameworks;
            PackageId ??= globalNode.PackageId;
            IsPackable ??= globalNode.IsPackable;
            Version ??= globalNode.Version;
        }
        public void UpdateToCSProject(CSharpProject project)
        {
            project.PackageName = this.PackageId;
            project.TargetFramworks = new HashSet<string>();
            if (TargetFramework!=null)
            {
                project.TargetFramworks.Add(TargetFramework);
            }
            else if(TargetFrameworks!=null)
            {
                var targets = TargetFrameworks.Split(";", StringSplitOptions.RemoveEmptyEntries);
                foreach (var targetFramework in targets) { project.TargetFramworks.Add(targetFramework); }
               
            }
            if (project.PackageName == string.Empty)
            {
                project.PackageName = project.ProjectName;
            }
            if (IsPackable.HasValue)
            {
                project.IsPackable = IsPackable.Value;
            }

        }
    }
}
